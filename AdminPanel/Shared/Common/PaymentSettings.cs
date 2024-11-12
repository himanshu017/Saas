using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanel.Shared.Common
{
    public class PaymentSettings
    {
        public string? PaypalClientId { get; set; }
        public string? PaypalSecret { get; set; }
        public string? StripeClientId { get; set; }
        public string? StripeSecret { get; set; }
        public bool IsLive { get; set; } = false;
    }
}
