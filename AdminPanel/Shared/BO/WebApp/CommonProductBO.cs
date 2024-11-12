using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanel.Shared.BO.WebApp
{
    public class GetAllCategoriesBO
    {
        public long CompanyId { get; set; }
        public bool ShowSubcategories { get; set; } = true;
        public bool IsSpecial { get; set; } = false;
    }

    public class GetProductListBO : PaginatedResultBO
    {
        public long UserId { get; set; }
        public long CustomerId { get; set; }
        public bool IsSlaesRep { get; set; } = false;
        public List<long?> UomIds { get; set; } = new();
        public List<int?> FilterIds { get; set; }= new();
        public List<long?> CategoryIds { get; set; } = new();
        public List<long?> SubCategoryIds { get; set; }= new();
        public float? PriceFrom { get; set; } = 0;
        public float? PriceTo { get; set; } = 0;
        public bool IsFavList { get; set; } = false;
        public long FavListId { get; set; }
        public bool IsSuggestive { get; set; } = false;
        public long? ProductId { get; set; } = 0;
    }

    public class DistinctFilterVals
    {
        public long? Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class GetProdDetailBO
    {
        public long CompanyId { get; set; }
        public long ProductId { get; set; }
        public long CustomerId { get; set; }
        public long UserId { get; set; }
    }

}
