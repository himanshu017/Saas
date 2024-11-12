using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanel.Shared.BO.Account
{
    public class DomainInfoBO : BaseResponseBO
    {
        public string? CompanyName { get; set; }
        public string? DomainName { get; set; }
        public long CompanyId { get; set; }
        public string? Logo { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? ImagePath { get; set; }
        public string? PrimaryColor { get; set; }
        public string? SecondaryColor { get; set; }
        public string? CurrencyInfo { get; set; } = "en-US";
    }
}
