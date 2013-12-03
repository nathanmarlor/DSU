namespace dealstealunreal.com.Infrastructure.Communication.Interfaces
{
    using Models.Facebook;

    /// <summary>
    /// Interface for facebook authentication
    /// </summary>
    public interface IFacebookAuthenticate
    {
        /// <summary>
        /// Authenticate with facebook
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <param name="token">Token</param>
        /// <returns>Response</returns>
        FacebookResponse Authenticate(string userId, string token);
    }
}