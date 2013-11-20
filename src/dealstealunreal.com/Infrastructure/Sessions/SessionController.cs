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
    using Security.Interfaces;

    public class SessionController : ISessionController
    {
        private readonly IMemberDataAccess memberDataAccess;
        private readonly ISessionDataAccess sessionDataAccess;
        private readonly IHash hash;
        private readonly Dictionary<Guid, Session> sessions;
        private TimeSpan timeout;
        private ReaderWriterLockSlim locker;

        public SessionController(IMemberDataAccess memberDataAccess, ISessionDataAccess sessionDataAccess, IHash hash, TimeSpan timeout)
        {
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

            try
            {
                user = memberDataAccess.GetUser(username);
            }
            catch (MemberDatabaseException)
            {
                return false;
            }

            if (hash.HashString(password) != user.Password.Trim())
            {
                return false;
            }

            Session session = new Session
                                  {
                                      SessionId = Guid.NewGuid(),
                                      Username = user.UserName,
                                      LastUpdated = DateTime.Now,
                                      RememberMe = rememberMe
                                  };

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

                sessions.Remove(sessionId);

                HttpContext.Current.Response.Cookies.Remove(FormsAuthentication.FormsCookieName);

                sessionDataAccess.DeleteSession(sessionId);
            }
            catch (InvalidSessionException)
            {
                // TODO: log!
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
                //TODO: Log this - then keep sessions in memory
            }

            locker.EnterWriteLock();

            try
            {
                foreach (var session in allSessions)
                {
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