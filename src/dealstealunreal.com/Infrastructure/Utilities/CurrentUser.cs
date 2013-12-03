namespace dealstealunreal.com.Infrastructure.Utilities
{
    using Data.Interfaces;
    using Exceptions;
    using Models.User;
    using Ninject.Extensions.Logging;
    using Sessions.Interfaces;

    /// <summary>
    /// Current user
    /// </summary>
    public class CurrentUser : ICurrentUser
    {
        private readonly ILogger log;
        private readonly ISessionController sessionController;
        private readonly IMemberDataAccess memberDataAccess;

        /// <summary>
        /// Initialises a new instance of the <see cref="CurrentUser"/> class. 
        /// </summary>
        /// <param name="log">Logging module</param>
        /// <param name="sessionController">Session controller</param>
        /// <param name="memberDataAccess">Member data access</param>
        public CurrentUser(ILogger log, ISessionController sessionController, IMemberDataAccess memberDataAccess)
        {
            this.log = log;
            this.sessionController = sessionController;
            this.memberDataAccess = memberDataAccess;
        }

        /// <summary>
        /// Gets current user
        /// </summary>
        /// <returns>User</returns>
        public User GetCurrentUser()
        {
            try
            {
                string username = sessionController.GetCurrentUsersSession().Username;

                return memberDataAccess.GetUser(username);
            }
            catch (InvalidSessionException)
            {
                log.Trace("Session could not be retrieved, continuing unauthenticated");
            }
            catch (MemberDatabaseException)
            {
                log.Trace("Session was retrieved but the user details could not be loaded, continuing unauthenticated");
            }

            sessionController.Logoff();

            return null;
        }
    }
}