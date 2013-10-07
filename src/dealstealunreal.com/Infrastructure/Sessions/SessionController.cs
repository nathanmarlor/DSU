
namespace dealstealunreal.com.Infrastructure.Sessions
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Web;
    using System.Web.Security;
    using Data.Interfaces;
    using dealstealunreal.com.Infrastructure.Security.Interfaces;
    using dealstealunreal.com.Models.Sessions;
    using Exceptions;
    using Interfaces;
    using Models.User;

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

            FormsAuthentication.SetAuthCookie(username, rememberMe);

            Session session = new Session()
                                  {
                                      SessionId = Guid.NewGuid(),
                                      Username = user.UserName,
                                      LastUpdated = DateTime.Now,
                                      RememberMe = rememberMe
                                  };

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

        public Session GetCurrentUser()
        {
            locker.EnterReadLock();

            try
            {
                Guid sessionId = GetSessionId();

                sessionDataAccess.UpdateSessionTime(sessionId, DateTime.Now);

                sessions[sessionId].LastUpdated = DateTime.Now;

                FormsAuthentication.SetAuthCookie(sessions[sessionId].Username, sessions[sessionId].RememberMe);

                return sessions[sessionId];
            }
            finally
            {
                locker.ExitReadLock();
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

        private Guid GetSessionId()
        {
            var sessionString = HttpContext.Current.Session["sessionId"] ?? Guid.Empty;

            Guid sessionGuid = Guid.Parse(sessionString.ToString());

            if (!sessions.ContainsKey(sessionGuid))
            {
                // TODO: Log, this is not a session we know about!
                throw new InvalidSessionException();
            }

            return sessionGuid;
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