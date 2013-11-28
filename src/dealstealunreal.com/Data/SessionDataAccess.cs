namespace dealstealunreal.com.Data
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data.SqlClient;
    using Exceptions;
    using Interfaces;
    using Models.Sessions;
    using Ninject.Extensions.Logging;

    public class SessionDataAccess : ISessionDataAccess
    {
        private const string SaveSessionQuery = "IF EXISTS (Select * from Sessions where Username = @userName) UPDATE Sessions set SessionId = @sessionId, LastUpdatedTime = @lastUpdatedTime, RememberMe = @rememberMe where Username = @userName ELSE INSERT INTO Sessions (SessionId, Username, LastUpdatedTime, RememberMe) VALUES (@sessionId, @userName, @lastUpdatedTime, @rememberMe)";
        private const string DeleteSessionQuery = "delete from Sessions where SessionId = @sessionId";
        private const string UpdateSessionTimeQuery = "update Sessions set LastUpdatedTime = @lastUpdatedTime where SessionId = @sessionId";
        private const string GetAllSessionsQuery = "select * from Sessions";
        private readonly ILogger log;

        public SessionDataAccess(ILogger log)
        {
            this.log = log;
        }

        public void SaveSession(Session session)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ReadWriteDatabase"].ConnectionString;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = SaveSessionQuery;
                        command.Parameters.AddWithValue("@sessionId", session.SessionId);
                        command.Parameters.AddWithValue("@lastUpdatedTime", session.LastUpdated);
                        command.Parameters.AddWithValue("@username", session.Username);
                        command.Parameters.AddWithValue("@rememberMe", session.RememberMe);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception e)
            {
                log.Warn(e, "Could not save session to database for user: {0} rememberMe: {1}", session.Username, session.RememberMe);
                throw new SessionDatabaseException();
            }
        }

        public void DeleteSession(Guid sessionId)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ReadWriteDatabase"].ConnectionString;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = DeleteSessionQuery;
                        command.Parameters.AddWithValue("@sessionId", sessionId);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception e)
            {
                log.Warn(e, "Could not delete session from database for user: {0}", sessionId);
                throw new SessionDatabaseException();
            }
        }

        public void UpdateSessionTime(Guid sessionId, DateTime dateTime)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ReadWriteDatabase"].ConnectionString;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = UpdateSessionTimeQuery;
                        command.Parameters.AddWithValue("@sessionId", sessionId);
                        command.Parameters.AddWithValue("@lastUpdatedTime", dateTime);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception e)
            {
                log.Warn(e, "Could not update session time for session: {0}", sessionId);
                throw new SessionDatabaseException();
            }
        }

        public IList<Session> GetAllSessions()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ReadonlyDatabase"].ConnectionString;

            try
            {
                IList<Session> sessions = new List<Session>();

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = GetAllSessionsQuery;

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                sessions.Add(new Session
                                    {
                                        SessionId = Guid.Parse(reader.GetString(reader.GetOrdinal("SessionId"))),
                                        LastUpdated = reader.GetDateTime(reader.GetOrdinal("LastUpdatedTime")),
                                        Username = reader.GetString(reader.GetOrdinal("Username")),
                                        RememberMe = reader.GetBoolean(reader.GetOrdinal("RememberMe"))
                                    });
                            }
                        }
                    }
                }

                return sessions;
            }
            catch (Exception e)
            {
                log.Warn(e, "Could not return list of sessions from the database");
                throw new SessionDatabaseException();
            }
        }
    }
}