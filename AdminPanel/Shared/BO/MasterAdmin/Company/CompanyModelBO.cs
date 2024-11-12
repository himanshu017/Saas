using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanel.Shared.BO.MasterAdmin
{
    public class CompanyModelBO : CommonAuditFields
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
        public int? TimeZoneId { get; set; }

        public string ContactPerson
        {
            get { return FirstName + " " + LastName; }
        }

        public string DropDownText
        {
            get
            {
                return string.Concat(CompanyName, " - ", ContactPerson, " - ", Email, " - ", DomainName);
            }
        }
    }
}
