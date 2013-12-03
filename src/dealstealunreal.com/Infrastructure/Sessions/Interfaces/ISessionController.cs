namespace dealstealunreal.com.Infrastructure.Sessions.Interfaces
{
    using Models.Sessions;

    /// <summary>
    /// Interface for session controller
    /// </summary>
    public interface ISessionController
    {
        /// <summary>
        /// Logon and start a session
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="password">Password</param>
        /// <param name="rememberMe">Remember me</param>
        /// <returns>Success</returns>
        bool Logon(string username, string password, bool rememberMe);

        /// <summary>
        /// Ends a session
        /// </summary>
        void Logoff();

        /// <summary>
        /// Gets a current users session
        /// </summary>
        /// <returns>Users session</returns>
        Session GetCurrentUsersSession();

        /// <summary>
        /// Prunes expired sessions
        /// </summary>
        void PruneSessions();
    }
}