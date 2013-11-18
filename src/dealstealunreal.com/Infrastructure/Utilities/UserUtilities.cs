namespace dealstealunreal.com.Infrastructure.Utilities
{
    using System.Web.Security;
    using Data.Interfaces;
    using Exceptions;
    using Interfaces;
    using Models.User;
    using Sessions.Interfaces;

    public class UserUtilities : IUserUtilities
    {
        private readonly ISessionController sessionController;
        private readonly IMemberDataAccess memberDataAccess;

        public UserUtilities(ISessionController sessionController, IMemberDataAccess memberDataAccess)
        {
            this.sessionController = sessionController;
            this.memberDataAccess = memberDataAccess;
        }

        public User GetCurrentUser()
        {
            try
            {
                string Username = sessionController.GetCurrentUser().Username;

                return memberDataAccess.GetUser(Username);
            }
            catch (InvalidSessionException e)
            {
                // TODO: Log this!
            }
            catch (MemberDatabaseException e)
            {
                // TODO: Log this!
            }

            FormsAuthentication.SignOut();

            return null;
        }
    }
}