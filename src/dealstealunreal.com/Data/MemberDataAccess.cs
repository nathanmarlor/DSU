namespace dealstealunreal.com.Data
{
    using System;
    using System.Configuration;
    using System.Data.SqlClient;
    using Exceptions;
    using Interfaces;
    using Models;
    using Models.User;
    using Ninject.Extensions.Logging;

    /// <summary>
    /// Member data access
    /// </summary>
    public class MemberDataAccess : IMemberDataAccess
    {
        private const string GetUserQuery = "select * from Users where Username = @userId or Email = @userId";
        private const string SaveUserQuery = "insert into Users (Username, Password, Email, ProfilePicture, Points) values(@userName, @password, @email, @profilePicture, @points)";
        private const string ChangePasswordQuery = "update Users set Password = @password where Username = @userName";
        private const string UpdateUserQuery = "update Users set Email = @email, ProfilePicture = @profilePicture where Username = @userName";
        private const string AddPointsQuery = "update Users set Points = Points + @pointValue where Users.Username = @userId";
        private readonly ILogger log;

        /// <summary>
        /// Initialises a new instance of the <see cref="MemberDataAccess"/> class. 
        /// </summary>
        /// <param name="log">Logging module</param>
        public MemberDataAccess(ILogger log)
        {
            this.log = log;
        }

        /// <summary>
        /// Get user
        /// </summary>
        /// <param name="userId">User id</param>
        /// <returns>User</returns>
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
                        command.Parameters.AddWithValue("@userId", userId);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new User
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
            catch (Exception e)
            {
                log.Warn(e, "Could not get user with id {0}", userId);
                throw new MemberDatabaseException();
            }

            throw new MemberDatabaseException(string.Format("The user {0} could not be found", userId));
        }

        /// <summary>
        /// Add point to user
        /// </summary>
        /// <param name="userId">User id</param>
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
                        command.Parameters.AddWithValue("@userId", userId);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception e)
            {
                log.Warn(e, "Could not add point to user with id {0}", userId);
                throw new MemberDatabaseException();
            }
        }

        /// <summary>
        /// Create new user
        /// </summary>
        /// <param name="details">User details</param>
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
            catch (Exception e)
            {
                log.Warn(e, "Could not create user with username: {0} password: {1} email: {2}", details.UserName, details.Password, details.Email);
                throw new MemberDatabaseException();
            }
        }

        /// <summary>
        /// Change password
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="password">New password</param>
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
            catch (Exception e)
            {
                log.Warn(e, "Could not change password for user: {0} password: {1}");
                throw new MemberDatabaseException();
            }
        }


        /// <summary>
        /// Update user
        /// </summary>
        /// <param name="user">User model</param>
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
            catch (Exception e)
            {
                log.Warn(e, "Could not update user: {0} profilePicture: {1} email: {2}", user.UserName, user.Email, user.ProfilePicture);
                throw new MemberDatabaseException();
            }
        }
    }
}