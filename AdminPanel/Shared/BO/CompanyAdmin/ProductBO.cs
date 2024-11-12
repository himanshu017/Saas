using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanel.Shared.BO.CompanyAdmin
{
    public class ProductBO : CommonAuditFields
    {
        public long ProductId { get; set; }
        public long CompanyId { get; set; }
        [Required(ErrorMessage = "Product Code is Required")]
        public string? Code { get; set; }
        public string? Gtin { get; set; }
        [Required(ErrorMessage = "Product Name is Required")]
        public string? Name { get; set; }
        public string? ShortDescription { get; set; }
        public string? FullDescription { get; set; }
        public string? ProductFeatures1 { get; set; }
        public string? ProductFeatures2 { get; set; }
        public string? MetaKeywords { get; set; }
        public string? MetaTitle { get; set; }

        [Required(ErrorMessage = "Category is Required")]
        public long? MainCategoryId { get; set; }

        [Required(ErrorMessage = "Sub Category is Required")]
        public long? CategoryId { get; set; }
        public int? FilterId { get; set; }
        public long UnitId { get; set; }
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
        [Required(ErrorMessage = "Price is Required")]
        public decimal Price { get; set; }
        public decimal? Price1 { get; set; }
        public decimal? Price2 { get; set; }
        public decimal? Price3 { get; set; }
        public decimal? Price4 { get; set; }
        public decimal? Price5 { get; set; }
        public decimal? DiscountPrice { get; set; }
        public double? StockQuantity { get; set; }
        public double? MinOrderQty { get; set; }
        public IEnumerable<FileViewModel>? ProductImagesList { get; set; }
        public IEnumerable<ProductUnitsBO>? Units { get; set; }
        public string? SpecificationAttributes { get; set; }
        public IEnumerable<long>? SuggestiveProductIds { get; set; }
        public List<string>? ProductTags { get; set; }
        public List<string>? AllExistingTags { get; set; }
    }


    public class FileViewModel
    {
        public byte[]? Content { get; set; }
        public string? ImageDataUrl { get; set; }
        public string? Name { get; set; }
        public long? Size { get; set; }
        public string? ContentType { get; set; }

    }

    public class ProductImageBO
    {
        public long ProductId { get; set; }
        public string? ImageName { get; set; }
        public byte? DisplayOrder { get; set; }
    }

    public class GetProductModel
    {
        public long CompanyId { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public string? SearchString { get; set; }
        public bool OrderByDesc { get; set; }
        public string? SortColumn { get; set; }
    }

    public class ProductUnitsBO
    {
        public long ProductId { get; set; }
        public long UnitId { get; set; }
        public string? UnitName { get; set; }
        public int? UnitQuantity { get; set; } = 1;
        public bool? OrderMultiples { get; set; } = false;
        public bool? IsDefault { get; set; } = false;
        public decimal? Weight { get; set; }
    }

    public class UpdateBitBO
    {
        public long Id { get; set; }
        public string ColName { get; set; }
        public bool Value { get; set; }
        public long ModifiedBy { get; set; }
    }

    public class ProductSpecificationBO
    {
        public string? Property { get; set; }
        public string? Value { get; set; }
    }
}
