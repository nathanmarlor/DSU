namespace dealstealunreal.com.Infrastructure.Communication
{
    using System;
    using System.Configuration;
    using System.Net;
    using System.Net.Mail;
    using Exceptions;
    using Interfaces;
    using Ninject.Extensions.Logging;

    public class EmailSender : IEmailSender
    {
        private ILogger log;

        public EmailSender(ILogger log)
        {
            this.log = log;
        }

        public void SendEmail(string emailAddress, string subject, string body)
        {
            log.Debug("Sending email to: {0} subject: {1} body: {2}", emailAddress, subject, body);

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
                log.Warn(e, "Could not send email to: {0} subject: {1} body: {2}", emailAddress, subject, body);
                throw new SendEmailException();
            }
        }
    }
}