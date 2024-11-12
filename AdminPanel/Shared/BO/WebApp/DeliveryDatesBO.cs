using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanel.Shared.BO.WebApp
{
    public class GetDeliveryDates
    {
        public long CompanyId { get; set; }
        public int AddressId { get; set; }
    }

    public class DeliveryDatesBO
    {
        public long CompanyId { get; set; }
        public bool? IsCutoffEnabled { get; set; }
        public TimeSpan? CutoffTime { get; set; }
        public DateTime? CutoffDate { get; set; }
        public bool? IsSameDayDelivery { get; set; }
        public bool? IsLimitDatesTo1Month { get; set; }
        public bool? AdvancedCutoffRules { get; set; }
        public byte? CutoffTypeId { get; set; }
        public int? DailyOrderLimit { get; set; }
        public string PermittedDeliveryDays { get; set; } = String.Empty;

        public DateTime StartDate { get; set; }
        public List<DateTime>? AvailableDates { get; set; }
        public List<DayOfWeek>? PermittedDays { get; set; }
    }
}
