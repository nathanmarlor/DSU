namespace dealstealunreal.com.Infrastructure.Sessions
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Web;
    using System.Web.Security;
    using Data.Interfaces;
    using Exceptions;
    using Interfaces;
    using Models.Sessions;
    using Models.User;
    using Ninject.Extensions.Logging;
    using Security.Interfaces;

    public class SessionController : ISessionController
    {
        private readonly ILogger log;
        private readonly IMemberDataAccess memberDataAccess;
        private readonly ISessionDataAccess sessionDataAccess;
        private readonly IHash hash;
        private readonly Dictionary<Guid, Session> sessions;
        private readonly TimeSpan timeout;
        private readonly ReaderWriterLockSlim locker;

        public SessionController(ILogger log, IMemberDataAccess memberDataAccess, ISessionDataAccess sessionDataAccess, IHash hash, TimeSpan timeout)
        {
            this.log = log;
            this.memberDataAccess = memberDataAccess;
            this.sessionDataAccess = sessionDataAccess;
            this.hash = hash;
            this.timeout = timeout;
            sessions = new Dictionary<Guid, Session>();
            locker = new ReaderWriterLockSlim();
        }

        public bool Logon(string username, string password, bool rememberMe)
        {
            User user;

            log.Trace("Attempting to log on user {0} with remember me: {1}", username, rememberMe);

            try
            {
                user = memberDataAccess.GetUser(username);
            }
            catch (MemberDatabaseException)
            {
                log.Debug("Invalid user specified {0}", username);
                return false;
            }

            if (hash.HashString(password) != user.Password.Trim())
            {
                log.Warn("Passwords do not match for user {0}", username);
                return false;
            }

            Session session = new Session
                                  {
                                      SessionId = Guid.NewGuid(),
                                      Username = user.UserName,
                                      LastUpdated = DateTime.Now,
                                      RememberMe = rememberMe
                                  };

            log.Trace("Setting new session cookie for user {0}", username);

            var cookie = FormsAuthentication.GetAuthCookie("DealStealUnreal", session.RememberMe);

            cookie.Expires = DateTime.Now.AddYears(10);

            var ticket = FormsAuthentication.Decrypt(cookie.Value);

            var newTicket = new FormsAuthenticationTicket(ticket.Version, ticket.Name, ticket.IssueDate, ticket.Expiration, ticket.IsPersistent, session.SessionId.ToString());

            cookie.Value = FormsAuthentication.Encrypt(newTicket);

            HttpContext.Current.Response.Cookies.Add(cookie);

            HttpContext.Current.Session["sessionId"] = session.SessionId;

            sessionDataAccess.SaveSession(session);

            locker.EnterWriteLock();

            try
            {
                sessions[session.SessionId] = session;
            }
            finally
            {
                locker.ExitWriteLock();
            }

            return true;
        }

        public void Logoff()
        {
            locker.EnterWriteLock();

            try
            {
                Guid sessionId = GetSessionId();

                log.Debug("Logging off user with session id: {0}", sessionId);

                sessions.Remove(sessionId);

                HttpContext.Current.Response.Cookies.Remove(FormsAuthentication.FormsCookieName);

                sessionDataAccess.DeleteSession(sessionId);
            }
            catch (InvalidSessionException)
            {
                log.Trace("Attempted to log off a user without a session");
            }
            finally
            {
                locker.ExitWriteLock();
            }
        }

        public void PruneSessions()
        {
            this.Load();

            while (true)
            {
                locker.EnterWriteLock();

                Dictionary<Guid, Session> tempSessions = new Dictionary<Guid, Session>(sessions);

                try
                {
                    foreach (var session in tempSessions)
                    {
                        TimeSpan duration = DateTime.Now - session.Value.LastUpdated;

                        if (duration > timeout && !session.Value.RememberMe)
                        {
                            log.Info("Pruning session for user: {0}", session.Value.Username);

                            sessions.Remove(session.Key);

                            sessionDataAccess.DeleteSession(session.Key);
                        }
                    }
                }
                finally
                {
                    locker.ExitWriteLock();
                }

                Thread.Sleep(1000);
            }
        }

        public Session GetCurrentUsersSession()
        {
            locker.EnterReadLock();

            try
            {
                Guid sessionId = GetSessionId();

                sessionDataAccess.UpdateSessionTime(sessionId, DateTime.Now);

                sessions[sessionId].LastUpdated = DateTime.Now;

                return sessions[sessionId];
            }
            finally
            {
                locker.ExitReadLock();
            }
        }

        private Guid GetSessionId()
        {
            var sessionString = HttpContext.Current.Session["sessionId"] ?? Guid.Empty;

            Guid sessionGuid = Guid.Parse(sessionString.ToString());

            if (sessions.ContainsKey(sessionGuid))
            {
                log.Trace("Session {0} was present and valid", sessionGuid);
                return sessionGuid;
            }

            var httpCookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (httpCookie != null)
            {
                var ticket = FormsAuthentication.Decrypt(httpCookie.Value);
                if (ticket != null)
                {
                    var guid = Guid.Parse(ticket.UserData);
                    HttpContext.Current.Session["sessionId"] = guid;
                    log.Trace("Found session id from cookie: {0}", guid);
                    return guid;
                }
            }

            throw new InvalidSessionException();
        }

        private void Load()
        {
            IList<Session> allSessions = new List<Session>();

            try
            {
                allSessions = sessionDataAccess.GetAllSessions();
            }
            catch (SessionDatabaseException)
            {
                log.Warn("Could not load all active sessions from the database");
            }

            locker.EnterWriteLock();

            try
            {
                foreach (var session in allSessions)
                {
                    log.Trace("Loaded session for user: {0}", session.Username);
                    sessions.Add(session.SessionId, session);
                }
            }
            finally
            {
                locker.ExitWriteLock();
            }
        }
    }
}