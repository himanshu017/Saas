using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanel.Shared.BO
{
    public class DeliveryCutoffBO : CommonAuditFields
    {
        public long Id { get; set; }
        public long CompanyId { get; set; }
        public bool IsCutoffEnabled { get; set; }
        public TimeSpan? CutoffTime { get; set; }
        public DateTime? CutoffDate { get; set; } = DateTime.Now;
        public bool IsSameDayDelivery { get; set; }
        public bool IsLimitDatesTo1Month { get; set; }
        public bool AdvancedCutoffRules { get; set; }
        public byte CutoffTypeId { get; set; }
        public int DailyOrderLimit { get; set; }
        public string? PermittedDeliveryDays { get; set; }
    }
}
