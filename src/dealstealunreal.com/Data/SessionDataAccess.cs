namespace dealstealunreal.com.Data
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data.SqlClient;
    using Exceptions;
    using Interfaces;
    using Models.Sessions;

    public class SessionDataAccess : ISessionDataAccess
    {
        private const string SaveSessionQuery = "insert into Sessions (SessionId, LastUpdatedTime, Username, RememberMe) values (@sessionId, @lastUpdatedTime, @username, @rememberMe)";
        private const string DeleteSessionQuery = "delete from Sessions where SessionId = @sessionId";
        private const string UpdateSessionTimeQuery = "update Sessions set LastUpdatedTime = @lastUpdatedTime where SessionId = @sessionId";
        private const string GetAllSessionsQuery = "select * from Sessions";

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
            catch (SqlException e)
            {
                // TODO: Log exception
                throw new SessionDatabaseException(string.Format("Received Sql Exception when saving session {0} - {1}", session.SessionId, e.Message));
            }
            catch (Exception e)
            {
                throw new SessionDatabaseException(string.Format("Received general Exception when saving session {0} - {1}", session.SessionId, e.Message));
                // TODO: Log exception
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
            catch (SqlException e)
            {
                // TODO: Log exception
                throw new SessionDatabaseException(string.Format("Received Sql Exception when deleting session {0} - {1}", sessionId, e.Message));
            }
            catch (Exception e)
            {
                throw new SessionDatabaseException(string.Format("Received general Exception when deleting session {0} - {1}", sessionId, e.Message));
                // TODO: Log exception
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
            catch (SqlException e)
            {
                // TODO: Log exception
                throw new SessionDatabaseException(string.Format("Received Sql Exception when updating session {0} - {1}", sessionId, e.Message));
            }
            catch (Exception e)
            {
                throw new SessionDatabaseException(string.Format("Received general Exception when updating session {0} - {1}", sessionId, e.Message));
                // TODO: Log exception
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
            catch (SqlException e)
            {
                // TODO: Log exception
                throw new SessionDatabaseException(string.Format("Received Sql Exception when retrieving all sessions - {0}", e.Message));
            }
            catch (Exception e)
            {
                throw new SessionDatabaseException(string.Format("Received general Exception when retrieving all sessions - {0}", e.Message));
                // TODO: Log exception
            }
        }
    }
}