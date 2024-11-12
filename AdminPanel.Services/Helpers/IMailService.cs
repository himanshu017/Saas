using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanel.Services.Helpers
{
    public interface IMailService
    {
        Task<bool> SendTestEmail();
        Task<bool> SendEmailAsync(string toEmail, string cc, string subject, string body);
        Task<string> SendEmailAsync(string toEmail, string cc, string subject, string templateName, Hashtable hashVars, string attachment);
    }
}
