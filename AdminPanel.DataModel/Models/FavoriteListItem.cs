﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace AdminPanel.DataModel.Models;

public partial class FavoriteListItem
{
    public long Id { get; set; }

    public long ListId { get; set; }

    public long ProductId { get; set; }

    public bool? IsActive { get; set; }

    public double? Quantity { get; set; }

    public decimal? Price { get; set; }

    public int? SortOrder { get; set; }

    public long? UnitId { get; set; }

    public DateTime? CreatedOn { get; set; }

    public long? CreatedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public long? ModifiedBy { get; set; }

    public virtual FavoriteList List { get; set; }

    public virtual Product Product { get; set; }
}