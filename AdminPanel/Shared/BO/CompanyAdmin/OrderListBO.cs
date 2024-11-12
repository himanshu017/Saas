using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanel.Shared.BO.CompanyAdmin
{
    public class OrderListBO
    {
        public Int64 OrderNumber { get; set; }

        public Int64 OrderId { get; set; }

        public DateTime DeliveryDate { get; set; }

        public decimal OrderTotal { get; set; }

        public decimal TotalTax { get; set; }

        public bool HasFreightCharges { get; set; }

        public decimal FreightCharge { get; set; }

        public string? StatusDesc { get; set; }

        public int StatusId { get; set; }

        public string CustomerName { get; set; }

        public string UserFirstName { get; set; }
        public string UserLastName { get; set; }
        public string UserEmail { get; set; }
        public string CustomerEmail { get; set; }
        public Int64 CompanyId { get; set; }

        public int TotalRecords { get; set; }
    }

    public class GetOrdersModel
    {
        public long CompanyId { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public string? SearchString { get; set; }
        public bool OrderByDesc { get; set; }
        public string? SortColumn { get; set; }
    }

    public class OrderDetailBO:CommonAuditFields
    {
        public long TempCartId { get; set; }
        public long OrderId { get; set; }
        public long OrderNumber { get; set; }
        public long CompanyId { get; set; }
        public long CustomerId { get; set; }
        public string? CustomerName { get; set; }
        public long UserId { get; set; }
        public long AddressId { get; set; } = 0;
        public string? StreetAddress { get; set; }
        public long? CommentId { get; set; }
        public string? CommentDesc { get; set; }
        public int? DeliveryRunId { get; set; }
        public byte StatusId { get; set; } = (byte)OrderStatus.Pending;
        public string? StatusDesc { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string PONumber { get; set; } = string.Empty;
        public int DiscountId { get; set; }
        public bool HasFreightCharges { get; set; } = false;
        public bool HasNonDeliveryDayCharges { get; set; } = false;
        public string DeviceType { get; set; } = "Web";
        public string DeviceToken { get; set; } = "EZ Web 1.0";
        public string AppVersion { get; set; } = "1.0";
        public string Latitude { get; set; } = string.Empty;
        public string Longitude { get; set; } = string.Empty;
        /// <summary>
        /// specify if someone needs to add more order specific notes. Maybe the company admin?
        /// </summary>
        public string OrderNote { get; set; } = string.Empty;
        public decimal? FreightCharge { get; set; }
        public decimal? NonDeliveryDayCharge { get; set; }

        // value for columns below need to be calculated at the time of placing an order.
        public decimal? TotalWithTax { get; set; }
        public decimal? TotalWithoutTax { get; set; }
        public decimal? DiscountAmount { get; set; }
        public decimal? TotalTax { get; set; }
        public decimal OrderTotal { get; set; }
        public IEnumerable<ItemsBO> OrderItems { get; set; }

        public string[] DelieryAddress { get; set; }
    }

    public class ItemsBO:CommonAuditFields
    {
        public long Id { get; set; }
        public long OrderId { get; set; }
        public long CompanyId { get; set; }
        public long ProductId { get; set; }
        public string? ProductName { get; set; }
        public long? CommentId { get; set; }
        public string? CommentDesc { get; set; }
        public double Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal? Cost { get; set; }
        public long UnitId { get; set; }
        public string? UnitName { get; set; }
        public bool IsGst { get; set; } = false;
        public bool IsPantry { get; set; } = true;
        public bool IsDonationItem { get; set; } = false;

        // value for columns below need to be calculated at the time of placing an order.
        public decimal? TotalTax => IsGst ? ((decimal).1 * (Price * (decimal)Quantity)) : 0;
        public decimal? PriceInclTax => Price + (IsGst ? ((decimal).1 * Price) : 0);
        public decimal TotalPrice => (decimal)PriceInclTax * (decimal)Quantity;
        public decimal? TotalWithoutTax => TotalPrice - TotalTax;
    }

}
