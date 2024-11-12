﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace AdminPanel.DataModel.Models;

public partial class Product
{
    public long ProductId { get; set; }

    public long CompanyId { get; set; }

    public string Code { get; set; }

    public string Sku { get; set; }

    public string Name { get; set; }

    public string ShortDescription { get; set; }

    public string FullDescription { get; set; }

    public string ProductFeatures1 { get; set; }

    public string ProductFeatures2 { get; set; }

    public string MetaKeywords { get; set; }

    public string MetaTitle { get; set; }

    public long? CategoryId { get; set; }

    public int? FilterId { get; set; }

    public double? UnitsPerCarton { get; set; }

    public string Barcode { get; set; }

    public string BrandName { get; set; }

    public string SupplierName { get; set; }

    public bool IsActive { get; set; }

    public bool? IsAvailable { get; set; }

    public bool? IsRewardItem { get; set; }

    public bool? IsCore { get; set; }

    public bool? IsGst { get; set; }

    public bool? IsBuyIn { get; set; }

    public bool? IsNew { get; set; }

    public bool? IsOnSale { get; set; }

    public bool? IsBackSoon { get; set; }

    public bool? IsFeatured { get; set; }

    public bool? IsSellBelowCost { get; set; }

    public bool? HasSpecialPrice { get; set; }

    public bool? IsDonationItem { get; set; }

    public decimal? CostPrice { get; set; }

    public decimal? StandardCost { get; set; }

    public decimal Price { get; set; }

    public decimal? Price1 { get; set; }

    public decimal? Price2 { get; set; }

    public decimal? Price3 { get; set; }

    public decimal? Price4 { get; set; }

    public decimal? Price5 { get; set; }

    public decimal? DiscountPrice { get; set; }

    public double? StockQuantity { get; set; }

    public double? MinOrderQty { get; set; }

    public double? MaxOrderQty { get; set; }

    public string SearchText { get; set; }

    public string SpecificationAttributes { get; set; }

    public DateTime? CreatedOn { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public long? ModifiedBy { get; set; }

    public virtual SubCategory Category { get; set; }

    public virtual Company Company { get; set; }

    public virtual ICollection<CompanySpecial> CompanySpecials { get; set; } = new List<CompanySpecial>();

    public virtual ICollection<FavoriteListItem> FavoriteListItems { get; set; } = new List<FavoriteListItem>();

    public virtual Filter Filter { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual ICollection<ProductAttributeMapping> ProductAttributeMappings { get; set; } = new List<ProductAttributeMapping>();

    public virtual ICollection<ProductImage> ProductImages { get; set; } = new List<ProductImage>();

    public virtual ICollection<ProductInventory> ProductInventories { get; set; } = new List<ProductInventory>();

    public virtual ICollection<ProductUnit> ProductUnits { get; set; } = new List<ProductUnit>();

    public virtual ICollection<SpecialPricing> SpecialPricings { get; set; } = new List<SpecialPricing>();

    public virtual ICollection<TempCartItem> TempCartItems { get; set; } = new List<TempCartItem>();

    public virtual ICollection<Discount> Discounts { get; set; } = new List<Discount>();

    public virtual ICollection<TargetMarketing> Marketings { get; set; } = new List<TargetMarketing>();

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();

    public virtual ICollection<Product> SuggestiveProducts { get; set; } = new List<Product>();

    public virtual ICollection<Tag> Tags { get; set; } = new List<Tag>();

    public virtual ICollection<Warehouse> Warehouses { get; set; } = new List<Warehouse>();
}