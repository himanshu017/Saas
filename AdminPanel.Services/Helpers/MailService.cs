using AdminPanel.Shared;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using System.Collections;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Options;
using System.Net;
using MailKit.Security;

namespace AdminPanel.Services.Helpers
{
    public class MailService : IMailService
    {
        private readonly EmailConfig _emailConfig;
        public MailService(IOptions<EmailConfig> emailConfig)
        {
            _emailConfig = emailConfig.Value;
        }

        public async Task<bool> SendEmailAsync(string toEmail, string cc, string subject, string body)
        {
            try
            {
                MimeMessage message = new MimeMessage();

                message.From.Add(new MailboxAddress(_emailConfig.FromEmail, _emailConfig.FromEmail));
                message.To.Add(new MailboxAddress(toEmail, toEmail));

                if (!string.IsNullOrEmpty(cc))
                {
                    message.Cc.Add(new MailboxAddress(cc, cc));
                }

                message.Subject = subject;

                BodyBuilder bodyBuilder = new BodyBuilder();
                bodyBuilder.HtmlBody = body;

                message.Body = bodyBuilder.ToMessageBody();
                SmtpClient client = new SmtpClient();

                client.Connect(_emailConfig.SmtpServer, _emailConfig.SmtpPort, SecureSocketOptions.Auto);
                client.Authenticate(_emailConfig.SmtpUser, _emailConfig.SmtpPassword);


                await client.SendAsync(message);
                client.Disconnect(true);
                client.Dispose();

                return true;
            }
            catch (Exception ex)
            {
                // add email logs to record the errors
                return false;
            }
        }

        public async Task<string> SendEmailAsync(string toEmail, string cc, string subject, string templateName, Hashtable hashVars, string attachment)
        {
            try
            {
                if (string.IsNullOrEmpty(toEmail))
                {
                    throw new ArgumentNullException("To Email not found!");
                }

                MimeMessage message = new MimeMessage();

                message.From.Add(new MailboxAddress(_emailConfig.FromEmail, _emailConfig.FromEmail));
                message.To.Add(new MailboxAddress(toEmail, toEmail));

                if (!string.IsNullOrEmpty(cc))
                {
                    message.Cc.Add(new MailboxAddress(cc, cc));
                }

                message.Subject = subject;

                BodyBuilder bodyBuilder = new BodyBuilder();

                string content = ReadEmailTemplateFile(@_emailConfig.EmailTemplatePath + templateName + ".html");
                if (hashVars.Count > 0)
                {
                    content = ReplaceFileVariablesInTemplateFile(hashVars, content);
                }
                if (string.IsNullOrEmpty(content))
                {
                    throw new ArgumentNullException("Email body is empty");
                }

                bodyBuilder.HtmlBody = content;

                message.Body = bodyBuilder.ToMessageBody();
                SmtpClient client = new SmtpClient();

                client.Connect(_emailConfig.SmtpServer, _emailConfig.SmtpPort, SecureSocketOptions.Auto);
                client.Authenticate(_emailConfig.SmtpUser, _emailConfig.SmtpPassword);

                await client.SendAsync(message);
                client.Disconnect(true);
                client.Dispose();

                return "Sent";
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message} :: Inner: {ex.InnerException?.Message}";
            }
        }

        private string ReadEmailTemplateFile(string filePath)
        {
            string str = string.Empty;

            if (System.IO.File.Exists(filePath))
            {
                try
                {
                    StreamReader reader = System.IO.File.OpenText(filePath);
                    str = reader.ReadToEnd();
                    reader.Close();
                    return str;
                }
                catch (Exception)
                {
                    throw new IOException("Error!! File Not located in the specified location.<br />Path: " + filePath + "<br />");
                }
            }
            throw new IOException("Error!! File Not located in the specified location.<br />Path: " + filePath + "<br />");
        }

        private string ReplaceFileVariablesInTemplateFile(Hashtable hashVars, string content)
        {
            IDictionaryEnumerator enumerator = hashVars.GetEnumerator();
            while (enumerator.MoveNext())
            {
                content = content.Replace(enumerator.Key.ToString(), enumerator.Value?.ToString());
            }
            return content;
        }

        public async Task<bool> SendTestEmail()
        {
            try
            {
                MimeMessage message = new MimeMessage();

                message.From.Add(new MailboxAddress(_emailConfig.FromEmail, _emailConfig.FromEmail));
                message.To.Add(new MailboxAddress(_emailConfig.TestEmail, _emailConfig.TestEmail));

                message.Subject = "Test Subject";

                BodyBuilder bodyBuilder = new BodyBuilder();
                bodyBuilder.HtmlBody = "This is a test email.";

                message.Body = bodyBuilder.ToMessageBody();
                SmtpClient client = new SmtpClient();

                client.Connect(_emailConfig.SmtpServer, _emailConfig.SmtpPort, SecureSocketOptions.Auto);
                client.Authenticate(_emailConfig.SmtpUser, _emailConfig.SmtpPassword);


                await client.SendAsync(message);
                client.Disconnect(true);
                client.Dispose();

                return true;
            }
            catch (Exception ex)
            {
                // add email logs to record the errors
                return false;
            }
        }

    }
}
