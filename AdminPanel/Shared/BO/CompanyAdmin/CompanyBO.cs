using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanel.Shared.BO.CompanyAdmin
{
    public class CompanyBO : BaseResponseBO
    {
        public long CompanyId { get; set; }
        public string? CompanyName { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Phone2 { get; set; }
        public string? Fax { get; set; }
        public bool? IsEmailMarketing { get; set; }
        public bool IsMobileOrdering { get; set; }
        public bool IsProofOfDelivery { get; set; }
        public bool IsWebOrdering { get; set; }
        public bool IsActie { get; set; } = false;
        public string? Logo { get; set; }
        public string? DomainName { get; set; }
        public int? TimeZoneId { get; set; }
        public string? PrimaryColor { get; set; }
        public string? SecondaryColor { get; set; }
        public DateTime? CreatedOn { get; set; }
        public long UserId { get; set; }
        public string? CurrencyInfo { get; set; } = "en-US";
    }
}
