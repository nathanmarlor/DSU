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
        /// <returns>Success</returns>
        bool ResetPassword(string userId);
    }
}