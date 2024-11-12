using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanel.Shared.BO.WebApp
{
    public class FavoriteListBO : CommonAuditFields
    {
        public long Id { get; set; }
        public byte TypeId { get; set; }
        public long CustomerId { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public bool? IsSalesRepList { get; set; }
        public int? SortOrder { get; set; }
        public string TypeDesc { get; set; } = string.Empty;
        public bool IsCopy { get; set; }
        public long MainListId { get; set; }
    }

    public class FavoriteListItemsBO : CommonAuditFields
    {
        public long Id { get; set; }
        public long ListId { get; set; }
        public long ProductId { get; set; }
        public bool? IsActive { get; set; }
        public double? Quantity { get; set; }
        public decimal? Price { get; set; }
        public int? SortOrder { get; set; }
        public long? UnitId { get; set; }
    }

    public class GetFavLists
    {
        public long CustomerId { get; set; }
        public long CompanyId { get; set; }
        public long UserId { get; set; }
    }

    public class ManageFavListItemBO
    {
        public long ProductId { get; set; }
        public long ListId { get; set; }
        public bool IsDelete { get; set; }
        public long UserId { get; set; }
        public long CustomerId { get; set; }
    }
}
