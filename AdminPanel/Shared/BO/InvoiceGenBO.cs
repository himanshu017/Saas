using AdminPanel.Shared.BO.WebApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanel.Shared
{
    public class InvoiceGenBO
    {
        public string? LogoPath { get; set; }
        public string? CompanyName { get; set; }
        public string? CompanyEmail { get; set; }
        public string? CompanyPhone { get; set; }
        public string? PrimaryColor { get; set; }
        public string? SecondaryColor { get; set; }
        public OrderDetailsBO OrderModel { get; set; }
        public Dictionary<string, string>? InvoiceInfo { get; set; }
        public Dictionary<string, string>? AllCharges { get; set; }
    }
}
