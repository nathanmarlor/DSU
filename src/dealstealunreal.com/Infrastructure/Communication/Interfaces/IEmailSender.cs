namespace dealstealunreal.com.Infrastructure.Communication.Interfaces
{
    /// <summary>
    /// Interface for sending emails
    /// </summary>
    public interface IEmailSender
    {
        /// <summary>
        /// Send email
        /// </summary>
        /// <param name="emailAddress">Address</param>
        /// <param name="subject">Subject</param>
        /// <param name="body">Body</param>
        void SendEmail(string emailAddress, string subject, string body);
    }
}