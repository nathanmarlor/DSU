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

        /// <summary>
        /// Initialises a new instance of the <see cref="RecoverPassword"/> class. 
        /// </summary>
        /// <param name="log">Logging module</param>
        /// <param name="memberDataAccess">Member data access</param>
        /// <param name="emailSender">Email sender</param>
        /// <param name="hash">Password hasher</param>
        public RecoverPassword(ILogger log, IMemberDataAccess memberDataAccess, IEmailSender emailSender, IHash hash)
        {
            this.log = log;
            this.memberDataAccess = memberDataAccess;
            this.emailSender = emailSender;
            this.hash = hash;
        }

        /// <summary>
        /// Reset a password
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <returns>Success</returns>
        public bool ResetPassword(string userId)
        {
            try
            {
                User user = memberDataAccess.GetUser(userId);

                string newPass = GenerateRandomString();

                emailSender.SendEmail(user.Email, "DSU New Password", "Your new password is: " + newPass);

                memberDataAccess.ChangePassword(user.UserName, hash.HashString(newPass));

                return true;
            }
            catch (MemberDatabaseException)
            {
                log.Debug("Invalid user specified {0} - cannot reset password", userId);
            }
            catch (SendEmailException)
            {
                log.Debug("Error when sending reset email for user {0}", userId);
            }

            return false;
        }

        /// <summary>
        /// Generate a random password
        /// </summary>
        /// <returns>Random password</returns>
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