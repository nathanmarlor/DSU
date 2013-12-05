namespace dealstealunreal.com.Infrastructure.Utilities
{

    /// <summary>
    /// Interface for current users
    /// </summary>
    public interface ICurrentUser
    {
        /// <summary>
        /// Gets current user
        /// </summary>
        /// <returns>User</returns>
        string GetCurrentUser();
    }
}