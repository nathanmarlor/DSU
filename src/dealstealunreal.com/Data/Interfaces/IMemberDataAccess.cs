namespace dealstealunreal.com.Data.Interfaces
{
    using Models;
    using Models.User;

    /// <summary>
    /// Interface for member data access
    /// </summary>
    public interface IMemberDataAccess
    {
        /// <summary>
        /// Get user
        /// </summary>
        /// <param name="userId">User id</param>
        /// <returns>User</returns>
        User GetUser(string userId);

        /// <summary>
        /// Gets a facebook user
        /// </summary>
        /// <param name="id">ID</param>
        User GetFacebookUser(long id);

        /// <summary>
        /// Add point to user
        /// </summary>
        /// <param name="userId">User id</param>
        void AddPoint(string userId);

        /// <summary>
        /// Create new user
        /// </summary>
        /// <param name="details">User details</param>
        void CreateUser(Register details);

        /// <summary>
        /// Change password
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="password">New password</param>
        void ChangePassword(string userId, string password);

        /// <summary>
        /// Update user
        /// </summary>
        /// <param name="user">User model</param>
        void UpdateUser(User user);
    }
}