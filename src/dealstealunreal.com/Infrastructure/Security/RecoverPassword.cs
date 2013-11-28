namespace dealstealunreal.com.Infrastructure.Security
{
    using System;
    using System.Linq;
    using Communication.Interfaces;
    using Data.Interfaces;
    using Exceptions;
    using Interfaces;
    using Models.User;
    using Ninject.Extensions.Logging;

    public class RecoverPassword : IRecoverPassword
    {
        private readonly IMemberDataAccess memberDataAccess;
        private readonly IEmailSender emailSender;
        private readonly IHash hash;
        private readonly ILogger log;

        public RecoverPassword(ILogger log, IMemberDataAccess memberDataAccess, IEmailSender emailSender, IHash hash)
        {
            this.log = log;
            this.memberDataAccess = memberDataAccess;
            this.emailSender = emailSender;
            this.hash = hash;
        }

        public void ResetPassword(string userId)
        {
            User user = new User();

            try
            {
                user = memberDataAccess.GetUser(userId);

                string newPass = GenerateRandomString();

                emailSender.SendEmail(user.Email, "DSU New Password", "Your new password is: " + newPass);

                memberDataAccess.ChangePassword(user.UserName, hash.HashString(newPass));
            }
            catch (MemberDatabaseException)
            {
                log.Debug("Invalid user specified {0} - cannot reset password", userId);
                throw new RecoverPasswordException();
            }
            catch (SendEmailException)
            {
                log.Debug("Error when sending reset email for user {0}", userId);
                throw new RecoverPasswordException();
            }
        }

        private static string GenerateRandomString()
        {
            const string Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string(
                Enumerable.Repeat(Chars, 8)
                          .Select(s => s[random.Next(s.Length)])
                          .ToArray());
        }
    }
}