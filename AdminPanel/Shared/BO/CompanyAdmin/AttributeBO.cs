using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanel.Shared.BO.CompanyAdmin
{
    public class AttributeBO
    {
        public int Id { get; set; }
        public long CompanyId { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public bool HasPredefinedValues { get; set; }
        public DateTime? CreatedOn { get; set; }
        public long CreatedBy { get; set; }
        public IEnumerable<PredefinedAttrValuesBO>? PredefinedAttributes { get; set; }
        public bool? IsActive { get; set; } = true;
    }

    public class AttributeValuesBO
    {
        public int Id { get; set; }
        public int ProductMappingId { get; set; }
        public string? Value { get; set; }
        public string? ColorRgb { get; set; }
        public decimal? PriceAdjustment { get; set; }
        public bool? UsePercentage { get; set; } = false;
        public decimal? WeightAdjustment { get; set; }
        public decimal? CostPrice { get; set; }
        public double? MinQuantity { get; set; }
        public bool? IsPreSelected { get; set; } = false;
        public long? ImageId { get; set; }
        public byte? DisplayOrder { get; set; } = 0;
        public bool? IsActive { get; set; } = true;
        public bool? IsDisabled => !IsActive;
        public int? StockQuantity { get; set; }
    }

    public class ProductAttrMappingBO
    {
        public int Id { get; set; }
        public long ProductId { get; set; }
        public int AttributeId { get; set; }
        public string? AttributeName { get; set; }
        public string? Placeholder { get; set; }
        public bool? IsRequired { get; set; } = false;
        public byte DisplayControlTypeId { get; set; }
        public byte? DisplayOrder { get; set; } = 0;
        public IEnumerable<AttributeValuesBO>? AttrValues { get; set; }
        public bool? IsActive { get; set; } = true;
    }

    public class ProductAttrListingBO 
    {
        public int Id { get; set; }
        public long ProductId { get; set; }
        public int AttributeId { get; set; }
        public string AttributeName { get; set; }
        public string? Placeholder { get; set; }
        public bool? IsRequired { get; set; } = false;
        public byte DisplayControlTypeId { get; set; }
        public byte? DisplayOrder { get; set; } = 0;
        public bool? IsActive { get; set; } = true;
    }

    public class ProductAttrValueListing
    {
        public string? Value { get; set; }
        public string? ColorRgb { get; set; }
        public decimal? PriceAdjustment { get; set; }
        public bool? UsePercentage { get; set; } = false;
        public decimal? WeightAdjustment { get; set; }
        public decimal? CostPrice { get; set; }
        public double? MinQuantity { get; set; }
        public bool? IsPreSelected { get; set; } = false;
        public long? ImageId { get; set; }
        public byte? DisplayOrder { get; set; } = 0;
        public bool? IsActive { get; set; } = true;
    }

    public class PredefinedAttrValuesBO
    {
        public int Id { get; set; }
        public int AttributeId { get; set; }
        public string Name { get; set; }
        public decimal? PriceAdjustment { get; set; }
        public bool? UsePercentage { get; set; }
        public decimal? WeightAdjustment { get; set; }
        public bool? IsPreSelected { get; set; }
        public int? DisplayOrder { get; set; }
        public decimal? Cost { get; set; }
    }

    public class ToggleAttrBO
    {
        public int Id { get; set; }
        public ToggleAttrType Type { get; set; }
        public bool Value { get; set; }
    }

}
