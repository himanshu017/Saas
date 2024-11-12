using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanel.Shared.BO
{
    public class DiscountBO:CommonAuditFields
    {
        public int Id { get; set; }
        public long CompanyId { get; set; }
        public byte TypeId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? CouponCode { get; set; }
        public bool UsePercentage { get; set; }
        public double? DiscountPercentage { get; set; }
        public decimal? DiscountAmount { get; set; }
        public decimal? MaxDiscountAmount { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal? MinCartValue { get; set; }
        public bool RequiresCouponCode { get; set; }
        public byte? LimitationTypeId { get; set; }
        public byte? LimitationTimes { get; set; }
        public IEnumerable<long>? ProductCustomerSelections { get; set; }

        public string? DiscountType { get; set; }
        public string? LimitationTypeDesc { get; set; }
        public bool IsActive { get; set; }
    }
}
