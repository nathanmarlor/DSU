namespace dealstealunreal.com.Data
{
    using System;
    using System.Configuration;
    using System.Data.SqlClient;
    using Interfaces;
    using Models;
    using Models.User;
    using Exceptions;

    public class MemberDataAccess : IMemberDataAccess
    {
        private const string GetUserQuery = "select * from Users where Username = @userName";
        private const string SaveUserQuery = "insert into Users (Username, Password, Email, ProfilePicture, Points) values(@userName, @password, @email, @profilePicture, @points)";
        private const string ChangePasswordQuery = "update Users set Password = @password where Username = @userName";
        private const string UpdateUserQuery = "update Users set Email = @email, ProfilePicture = @profilePicture where Username = @userName";
        private const string AddPointsQuery = "update Users set Points = Points + @pointValue where Users.Username =  (select Username from Deals where DealId = 1)";

        public User GetUser(string userId)
        {
            userId = userId ?? string.Empty;

            string connectionString = ConfigurationManager.ConnectionStrings["ReadonlyDatabase"].ConnectionString;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = GetUserQuery;
                        command.Parameters.AddWithValue("@userName", userId);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new User()
                                    {
                                        UserName = reader.GetString(reader.GetOrdinal("Username")).Trim(),
                                        Email = reader.GetString(reader.GetOrdinal("Email")).Trim(),
                                        Password = reader.GetString(reader.GetOrdinal("Password")).Trim(),
                                        Points = reader.GetInt32(reader.GetOrdinal("Points")),
                                        ProfilePicture = reader.GetString(reader.GetOrdinal("ProfilePicture"))
                                    };
                            }
                        }
                    }
                }
            }
            catch (SqlException e)
            {
                // TODO: Log this error 
                throw new MemberDatabaseException(string.Format("Received Sql Exception when retrieving user {0} - {1}", userId, e.Message));
            }
            catch (Exception e)
            {
                // TODO: log this error
                throw new MemberDatabaseException(string.Format("Received general Exception when retrieving user {0} - {1}", userId, e.Message));
            }

            throw new MemberDatabaseException(string.Format("The user {0} could not be found", userId));
        }

        public void AddPoint(string userId)
        {
            int pointValue = int.Parse(ConfigurationManager.AppSettings["PointPerVote"]);

            string connectionString = ConfigurationManager.ConnectionStrings["ReadWriteDatabase"].ConnectionString;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = AddPointsQuery;

                        command.Parameters.AddWithValue("@pointValue", pointValue);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException e)
            {
                // TODO: Log this error 
                throw new MemberDatabaseException(string.Format("Received Sql Exception when adding user point {0} - {1}", userId, e.Message));
            }
            catch (Exception e)
            {
                // TODO: log this error
                throw new MemberDatabaseException(string.Format("Received general Exception when retrieving user {0} - {1}", userId, e.Message));
            }
        }

        public void CreateUser(Register details)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ReadWriteDatabase"].ConnectionString;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = SaveUserQuery;

                        command.Parameters.AddWithValue("@password", details.Password);
                        command.Parameters.AddWithValue("@email", details.Email);
                        command.Parameters.AddWithValue("@userName", details.UserName);
                        command.Parameters.AddWithValue("@profilePicture", details.ProfilePicturePath);
                        command.Parameters.AddWithValue("@points", 0);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException e)
            {
                // TODO: Log this error 
                throw new MemberDatabaseException(string.Format("Received Sql Exception when creating user {0} - {1}", details.UserName, e.Message));
            }
            catch (Exception e)
            {
                // TODO: log this error
                throw new MemberDatabaseException(string.Format("Received general Exception when retrieving user {0} - {1}", details.UserName, e.Message));
            }
        }

        public void ChangePassword(string userId, string password)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ReadWriteDatabase"].ConnectionString;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = ChangePasswordQuery;

                        command.Parameters.AddWithValue("@userName", userId);
                        command.Parameters.AddWithValue("@password", password);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException e)
            {
                // TODO: Log this error 
                throw new MemberDatabaseException(string.Format("Received Sql Exception when changing password of user {0} - {1}", userId, e.Message));
            }
            catch (Exception e)
            {
                // TODO: log this error
                throw new MemberDatabaseException(string.Format("Received general Exception when changing password of user {0} - {1}", userId, e.Message));
            }
        }

        public void UpdateUser(User user)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ReadWriteDatabase"].ConnectionString;

            if (!string.IsNullOrWhiteSpace(user.Password))
            {
                ChangePassword(user.UserName, user.Password);
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = UpdateUserQuery;

                        command.Parameters.AddWithValue("@email", user.Email);
                        command.Parameters.AddWithValue("@profilePicture", user.ProfilePicture);
                        command.Parameters.AddWithValue("@userName", user.UserName);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException e)
            {
                // TODO: Log this error 
                throw new MemberDatabaseException(string.Format("Received Sql Exception when updating user {0} - {1}", user.UserName, e.Message));
            }
            catch (Exception e)
            {
                // TODO: log this error
                throw new MemberDatabaseException(string.Format("Received general Exception when updating user {0} - {1}", user.UserName, e.Message));
            }
        }
    }
}