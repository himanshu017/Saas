using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanel.Shared.BO
{
    public class CompanyGlobalTextBO : CommonAuditFields
    {
        public long CompanyId { get; set; }
        public string? TermsConditions { get; set; }
        public string? NoticeBoard { get; set; }
        public string? LiquorControl { get; set; }
        public string? NonFood { get; set; }
        public string? InvoiceFooter { get; set; }
        public bool IsUpdate { get; set; }
    }
}
