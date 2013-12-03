namespace dealstealunreal.com.Infrastructure.Communication
{
    using System;
    using System.Configuration;
    using System.Net;
    using System.Net.Mail;
    using Exceptions;
    using Interfaces;
    using Ninject.Extensions.Logging;

    /// <summary>
    /// Email sender
    /// </summary>
    public class EmailSender : IEmailSender
    {
        private readonly ILogger log;

        /// <summary>
        /// Initialises a new instance of the <see cref="EmailSender"/> class. 
        /// </summary>
        /// <param name="log">Logging module</param>
        public EmailSender(ILogger log)
        {
            this.log = log;
        }

        /// <summary>
        /// Send email
        /// </summary>
        /// <param name="emailAddress">Address</param>
        /// <param name="subject">Subject</param>
        /// <param name="body">Body</param>
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