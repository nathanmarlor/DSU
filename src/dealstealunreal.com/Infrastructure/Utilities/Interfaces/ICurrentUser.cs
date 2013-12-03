namespace dealstealunreal.com.Infrastructure.Utilities
{
    using Models.User;

    /// <summary>
    /// Interface for current users
    /// </summary>
    public interface ICurrentUser
    {
        /// <summary>
        /// Gets current user
        /// </summary>
        /// <returns>User</returns>
        User GetCurrentUser();
    }
}