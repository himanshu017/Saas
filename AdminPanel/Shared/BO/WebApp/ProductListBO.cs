using AdminPanel.Shared.BO.CompanyAdmin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanel.Shared.BO.WebApp
{
    public class ProductListBO
    {
        public long ProductId { get; set; }
        public long CompanyId { get; set; }
        public string? Code { get; set; }
        public string? Gtin { get; set; }
        public string? Name { get; set; }
        public string? ShortDescription { get; set; }
        public string? FullDescription { get; set; }
        public string? ProductFeatures1 { get; set; }
        public string? ProductFeatures2 { get; set; }
        public string? MetaKeywords { get; set; }
        public string? MetaTitle { get; set; }
        public long? MainCategoryId { get; set; }
        public long? CategoryId { get; set; }
        public int? FilterId { get; set; }
        public long UnitId { get; set; }
        public string? UnitName { get; set; }
        public string? UnitCode { get; set; }
        public double? UnitsPerCarton { get; set; }
        public string? Barcode { get; set; }
        public string? BrandName { get; set; }
        public string? SupplierName { get; set; }
        public bool IsActive { get; set; }
        public bool IsAvailable { get; set; }
        public bool? IsRewardItem { get; set; }
        public bool IsCore { get; set; }
        public bool IsGst { get; set; }
        public bool IsBuyIn { get; set; }
        public bool IsNew { get; set; }
        public bool? IsOnSale { get; set; }
        public bool? IsBackSoon { get; set; }
        public bool IsFeatured { get; set; }
        public bool? IsSellBelowCost { get; set; }
        public bool? HasSpecialPrice { get; set; }
        public bool IsDonationItem { get; set; }
        public decimal? CostPrice { get; set; }
        public decimal? StandardCost { get; set; }
        public decimal Price { get; set; }
        public decimal? Price1 { get; set; }
        public decimal? Price2 { get; set; }
        public decimal? Price3 { get; set; }
        public decimal? Price4 { get; set; }
        public decimal? Price5 { get; set; }
        public decimal DiscountPrice { get; set; }
        public double? StockQuantity { get; set; }
        public double? MinOrderQty { get; set; }
        public double? MaxOrderQty { get; set; }
        public List<ProductImageBO> ProductImages { get; set; }
        public string? CategoryName { get; set; }
        public string? MainCategoryName { get; set; }
        public decimal? SpecialPrice { get; set; }
        public long? CommentId { get; set; }
        public string? CommentDescription { get; set; }

        // Loop through the result to get the info for the properties below
        public bool IsInFavList { get; set; } = false;
        public bool IsInCart { get; set; } = false;
        public long TempCartId { get; set; } = 0;
        public long TempCartItemId { get; set; } = 0;
        public long LastOrderUom { get; set; } = 0;
        public long FavListId { get; set; } = 0;
        public double CartQuantity { get; set; } = 0;
        public long CartUnitId { get; set; } = 0;
        public decimal CartPrice { get; set; }
        public IEnumerable<ProductAttrMappingBO>? ProductAttrMappings { get; set; }

        // for use in add to cart section
        public decimal? AttributePriceAdjustment { get; set; }
        public string? AttributeMappingJson { get; set; }
        public string? AttributesDisplay { get; set; }
        public string? SpecificationAttributes { get; set; }
        public List<string>? ProductTags { get; set; }
    }

    public class AttrEventcallbackBO
    {
        public Dictionary<int, int> SelectedAttributes { get; set; }
        public List<int> RequiredAttributes { get; set; }
        public decimal? PriceAdjustment { get; set; }
    }

    public class SuggestiveProductListBO
    {
        public long ProductId { get; set; }
        public long CompanyId { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public bool? IsRewardItem { get; set; }
        public bool IsCore { get; set; }
        public bool IsGst { get; set; }
        public bool IsBuyIn { get; set; }
        public bool IsNew { get; set; }
        public bool? IsOnSale { get; set; }
        public bool? IsBackSoon { get; set; }
        public bool IsFeatured { get; set; }
        public List<ProductImageBO>? ProductImages { get; set; }
        public string? CategoryName { get; set; }
        public string? MainCategoryName { get; set; }
        public decimal? SpecialPrice { get; set; }
        public string? UnitName { get; set; }
        public string? UnitCode { get; set; }
    }
}
