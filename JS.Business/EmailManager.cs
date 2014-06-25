using System.Configuration;
using System.Net;
using System.Net.Mail;

namespace JS.Business
{
    /// <summary>
    /// Manager class for sending emails
    /// </summary>
    public static class EmailManager
    {
        /// <summary>
        /// Default SMTP Server
        /// </summary>
        public static string DefaultSMTPServer { get { return ConfigurationManager.AppSettings["SMTP_SERVER"]; } }

        /// <summary>
        /// Default SMTP Port
        /// </summary>
        public static int DefaultSMTPPort { get { return 25; } }

        /// <summary>
        /// Default Email Sender
        /// </summary>
        public static string DefaultSender { get { return "donotreply@hearthtml.com"; } }

        /// <summary>
        /// Default Username for smtp server
        /// </summary>
        public static string DefaultUsername { get { return ConfigurationManager.AppSettings["SMTP_SERVER_USER"]; } }

        /// <summary>
        /// Default password for smtp server
        /// </summary>
        public static string DefaultPassword { get { return ConfigurationManager.AppSettings["SMTP_SERVER_PASS"]; } }

        /// <summary>
        /// Sends an email with using a default configuration.
        /// </summary>
        /// <param name="subject">The subject line of the email</param>
        /// <param name="body">The body content of the email</param>
        /// <param name="toAddress">A collection of recipient address for the email</param>
        /// <returns>True if success, false otherwise</returns>
        public static void SendDefaultEmail(string subject,
                                            string body,
                                            params string[] toAddress)
        {
            SendEmail(DefaultSMTPServer,
                      DefaultSMTPPort,
                      DefaultUsername,
                      DefaultPassword,
                      DefaultSender,
                      subject,
                      body,
                      toAddress);
        }

        /// <summary>
        /// Sends an email.
        /// </summary>
        /// <param name="smtpServer">The smtp server to used to send the email</param>
        /// <param name="port">The port used to connect to the smtp server</param>
        /// <param name="smtpPassword">The password used to authenticate to the server</param>
        /// <param name="fromAddress">The sender of the email</param>
        /// <param name="subject">The subject line of the email</param>
        /// <param name="body">The body content of the email</param>
        /// <param name="toAddress">A collection of recipient address for the email</param>
        /// <param name="smtpUsername">The username used to authenticate to the server</param>
        /// <returns>True if success, false otherwise</returns>
        public static void SendEmail(string smtpServer,
                                     int port,
                                     string smtpUsername,
                                     string smtpPassword,
                                     string fromAddress,
                                     string subject,
                                     string body,
                                     params string[] toAddress)
        {
            MailMessage msg
                = new MailMessage
                {
                    From = new MailAddress(fromAddress),
                    Body = body,
                    Subject = subject,
                };

            if (toAddress != null)
            {
                foreach (string recipient in toAddress)
                {
                    msg.To.Add(new MailAddress(recipient));
                }
            }

            SmtpClient client = new SmtpClient(smtpServer, port)
            {
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(smtpUsername, smtpPassword),
                DeliveryMethod = SmtpDeliveryMethod.Network
            };

            client.Send(msg);
        }

    }
}