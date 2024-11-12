using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanel.Shared.BO.WebApp
{
    public class TempCartBO : CommonAuditFields
    {
        public long Id { get; set; }
        public long CompanyId { get; set; }
        public long CustomerId { get; set; }
        public long UserId { get; set; }
        public long? AddressId { get; set; }
        public long? CommentId { get; set; }
        public long? DeliveryRunId { get; set; }
        public string? DeviceType { get; set; }
        public string? DeviceToken { get; set; }
        public string? AppVersion { get; set; }
        public List<TempCartItemsBO>? CartItems { get; set; }
    }

    public class TempCartItemsBO
    {
        public long Id { get; set; }
        public long TempCartId { get; set; }
        public long ProductId { get; set; }
        public long? CommentId { get; set; }
        public double Quantity { get; set; }
        public decimal Price { get; set; }
        public long UnitId { get; set; }
        public bool? IsGst { get; set; }
        public bool? IsDonationItem { get; set; }
        public double? TaxRate { get; set; }
        public decimal? TaxAmount { get; set; }
        public decimal? AttributePriceAdjustment { get; set; }
        public string? AttributeMappingJson { get; set; }
    }
}
