using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanel.Shared
{
    public class AppSettings
    {
        public string? AppUrl { get; set; }
        public string? EmailVerificationUrl { get; set; }
        public string? PasswordResetUrl { get; set; }
        public int TokenExpirationTimeout { get; set; }
        public bool RecordLoginHistory { get; set; }
        public bool AllowMasterTableEditing { get; set; }
        public string? Secret { get; set; }
    }
}
