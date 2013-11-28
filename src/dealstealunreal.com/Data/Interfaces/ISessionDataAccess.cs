namespace dealstealunreal.com.Data.Interfaces
{
    using System;
    using System.Collections.Generic;
    using Models.Sessions;

    /// <summary>
    /// Interface for session data access
    /// </summary>
    public interface ISessionDataAccess
    {
        /// <summary>
        /// Save session
        /// </summary>
        /// <param name="session">Session Id</param>
        void SaveSession(Session session);

        /// <summary>
        /// Delete session
        /// </summary>
        /// <param name="sessionId">Session Id</param>
        void DeleteSession(Guid sessionId);

        /// <summary>
        /// Update session time
        /// </summary>
        /// <param name="sessionId">Session Id</param>
        /// <param name="dateTime">Time</param>
        void UpdateSessionTime(Guid sessionId, DateTime dateTime);

        /// <summary>
        /// Get all sessions
        /// </summary>
        /// <returns>List of sessions</returns>
        IList<Session> GetAllSessions();
    }
}