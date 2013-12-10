namespace dealstealunreal.com.Infrastructure.Utilities
{
    using Data.Interfaces;
    using Exceptions;
    using Ninject.Extensions.Logging;
    using Sessions.Interfaces;

    /// <summary>
    /// Current user
    /// </summary>
    public class CurrentUser : ICurrentUser
    {
        private readonly ILogger log;
        private readonly ISessionController sessionController;

        /// <summary>
        /// Initialises a new instance of the <see cref="CurrentUser"/> class. 
        /// </summary>
        /// <param name="log">Logging module</param>
        /// <param name="sessionController">Session controller</param>
        public CurrentUser(ILogger log, ISessionController sessionController)
        {
            this.log = log;
            this.sessionController = sessionController;
        }

        /// <summary>
        /// Gets current user
        /// </summary>
        /// <returns>User</returns>
        public string GetCurrentUser()
        {
            try
            {
                return sessionController.GetCurrentUsersSession().Username;
            }
            catch (InvalidSessionException)
            {
                log.Trace("Session could not be retrieved, continuing unauthenticated");
            }

            return string.Empty;
        }
    }
}