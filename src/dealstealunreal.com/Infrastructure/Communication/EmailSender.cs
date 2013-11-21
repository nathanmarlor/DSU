namespace dealstealunreal.com.Infrastructure.Communication
{
    using System;
    using System.Configuration;
    using System.Net;
    using System.Net.Mail;
    using Exceptions;
    using Interfaces;

    public class EmailSender : IEmailSender
    {
        public void SendEmail(string emailAddress, string subject, string body)
        {
            string host = ConfigurationManager.AppSettings["SmtpHost"];
            string email = ConfigurationManager.AppSettings["SmtpAccount"];
            string password = ConfigurationManager.AppSettings["SmtpPassword"];
            string from = ConfigurationManager.AppSettings["EmailFrom"];
            int port = int.Parse(ConfigurationManager.AppSettings["SmtpPort"]);
            bool isSsl = ConfigurationManager.AppSettings["SmtpSsl"].Equals("true");
            bool useCredential = ConfigurationManager.AppSettings["SmtpUseCredential"].Equals("true");

            MailMessage mail = new MailMessage(from, emailAddress, subject, body);

            SmtpClient smtpClient = new SmtpClient(host, port)
                {
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    EnableSsl = isSsl,
                    UseDefaultCredentials = !useCredential,
                    Credentials = useCredential ? new NetworkCredential(email, password) : null,
                    Timeout = 2
                };

            try
            {
                smtpClient.Send(mail);
            }
            catch (Exception e)
            {
                // TODO: Log
                throw new SendEmailException();
            }
        }
    }
}