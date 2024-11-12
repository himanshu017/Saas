
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace EzWeb
{
    public class EmailSettings
    {
        public string EmailId { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public bool UseSSL { get; set; }
    }

    public class EmailData
    {
        public string EmailToId { get; set; }
        public string EmailToName { get; set; }
        public string EmailSubject { get; set; }
        public string EmailBody { get; set; }
    }
    public interface IEmailService
    {
        bool SendEmail(EmailData emailData);
    }
    public class EmailService : IEmailService
    {
        EmailSettings _emailSettings = null;
        public EmailService(IOptions<EmailSettings> options)
        {
            _emailSettings = options.Value;
        }
        public bool SendEmail(EmailData emailData)
        {
            try
            {
                MailMessage emailMessage = new MailMessage
                {
                    From = new MailAddress(_emailSettings.EmailId)
                };
                MailAddress emailTo = new MailAddress( emailData.EmailToId, emailData.EmailToName);
                emailMessage.To.Add(emailTo);
                emailMessage.Subject = emailData.EmailSubject;
                emailMessage.Body = emailData.EmailBody;
                SmtpClient client = new SmtpClient();

                var basicCredential = new NetworkCredential(_emailSettings.Name, _emailSettings.Password);
                client.Host = _emailSettings.Host;
                client.Port = _emailSettings.Port;
                client.Credentials = basicCredential;
                client.EnableSsl = _emailSettings.UseSSL;
                client.Send(emailMessage);
                client.Dispose();

                
                return true;
            }
            catch (Exception ex)
            {
                //Log Exception Details
                return false;
            }
        }
    }
}
