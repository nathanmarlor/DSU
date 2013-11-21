namespace dealstealunreal.com.Infrastructure.Security
{
    using System;
    using System.Linq;
    using Communication.Interfaces;
    using Data.Interfaces;
    using Exceptions;
    using Interfaces;
    using Models.User;

    public class RecoverPassword : IRecoverPassword
    {
        private readonly IMemberDataAccess memberDataAccess;
        private readonly IEmailSender emailSender;
        private readonly IHash hash;

        public RecoverPassword(IMemberDataAccess memberDataAccess, IEmailSender emailSender, IHash hash)
        {
            this.memberDataAccess = memberDataAccess;
            this.emailSender = emailSender;
            this.hash = hash;
        }

        public void ResetPassword(string userId)
        {
            try
            {
                User user = memberDataAccess.GetUser(userId);

                string newPass = GenerateRandomString();

                emailSender.SendEmail(user.Email, "DSU New Password", "Your new password is: " + newPass);

                memberDataAccess.ChangePassword(user.UserName, hash.HashString(newPass));
            }
            catch (MemberDatabaseException e)
            {
                // TODO: log this error
                throw new RecoverPasswordException(string.Format("User {0} does not exist", userId));
            }
            catch (SendEmailException e)
            {
                // TODO: log this error
                throw new RecoverPasswordException(string.Format("Could not send reset password email for user {0}", userId));
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