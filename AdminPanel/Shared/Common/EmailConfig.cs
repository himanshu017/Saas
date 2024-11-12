using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanel.Shared
{
    public class EmailConfig
    {
        public string? SmtpServer { get; set; }
        public int SmtpPort { get; set; }
        public string? SmtpApiUsername { get; set; }
        public string? SmtpUser { get; set; }
        public string? SmtpPassword { get; set; }
        public string? SmtpApiKey { get; set; }
        public string? FromEmail { get; set; }
        public bool IsDevelopment { get; set; }
        public string? TestEmail { get; set; }
        public string? AdminEmail { get; set; }
        public string? EmailTemplatePath { get; set; }
    }
}
