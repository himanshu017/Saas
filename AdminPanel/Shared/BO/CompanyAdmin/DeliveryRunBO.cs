using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanel.Shared.BO
{
    public class DeliveryRunBO : CommonAuditFields
    {
        public long Id { get; set; }
        public long CompanyId { get; set; }
        public string? RunNo { get; set; }
        public string? DaysOfWeek { get; set; }
    }
}
