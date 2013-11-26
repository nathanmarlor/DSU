namespace dealstealunreal.com.Infrastructure.Utilities
{
    using Data.Interfaces;
    using Exceptions;
    using Models.User;
    using Ninject.Extensions.Logging;
    using Sessions.Interfaces;

    public class CurrentUser : ICurrentUser
    {
        private readonly ILogger log;
        private readonly ISessionController sessionController;
        private readonly IMemberDataAccess memberDataAccess;

        public CurrentUser(ILogger log, ISessionController sessionController, IMemberDataAccess memberDataAccess)
        {
            this.log = log;
            this.sessionController = sessionController;
            this.memberDataAccess = memberDataAccess;
        }

        public User GetCurrentUser()
        {
            try
            {
                string username = sessionController.GetCurrentUsersSession().Username;

                return memberDataAccess.GetUser(username);
            }
            catch (InvalidSessionException)
            {
                log.Trace("Session was retrieved but is not one we know about");
            }
            catch (MemberDatabaseException)
            {
                log.Trace("Could not retrieve current user, continuing unauthenticated");
            }

            sessionController.Logoff();

            return null;
        }
    }
}