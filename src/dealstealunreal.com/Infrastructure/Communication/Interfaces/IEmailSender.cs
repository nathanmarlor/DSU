namespace dealstealunreal.com.Infrastructure.Communication.Interfaces
{
    public interface IEmailSender
    {
        void SendEmail(string emailAddress, string subject, string body);
    }
}