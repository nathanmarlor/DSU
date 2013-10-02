namespace dealstealunreal.com.Data.Interfaces
{
    using System;
    using dealstealunreal.com.Models.Sessions;
    using System.Collections.Generic;

    public interface ISessionDataAccess
    {
        void SaveSession(Session session);

        void DeleteSession(Guid sessionId);

        void UpdateSessionTime(Guid sessionId, DateTime dateTime);

        IList<Session> GetAllSessions();
    }
}