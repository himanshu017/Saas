using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanel.Shared.BO.WebApp
{
    public class CartBO
    {

    }

    public class GetCartInfoBO
    {
        public long CompanyId { get; set; }
        public long CustomerId { get; set; }
        public long UserId { get; set; }
        public bool IsSalesRep { get; set; } = false;
        public long TempCartId { get; set; }
    }

    public class CartCountBO
    {
        public long TempCartId { get; set; }
        public int CartCount { get; set; } = 0;
        public decimal CartTotal { get; set; }
        public decimal CartTotalWithTax { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal DeliveryCharges { get; set; }
        public string DiscountCode { get; set; } = string.Empty;
    }

    public class DeleteCartItemsBO 
    {
        public long TempCartId { get; set; }
        public bool DeleteAll { get; set; } = false;
        public long[]? Ids { get; set; }

    }

    public class GetCommentsBO
    {
        public long CustomerId { get; set; }
        public long UserId { get; set; }
        public byte TypeId { get; set; }
        public bool GetUserSpecific { get; set; } = false;
    }

}
