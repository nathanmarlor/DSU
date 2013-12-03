namespace dealstealunreal.com.Infrastructure.Security.Interfaces
{
    /// <summary>
    /// Recover password interface
    /// </summary>
    public interface IRecoverPassword
    {
        /// <summary>
        /// Reset a password
        /// </summary>
        /// <param name="userId">User ID</param>
        void ResetPassword(string userId);
    }
}