﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace AdminPanel.DataModel.Models;

public partial class SpecialPricing
{
    public long Id { get; set; }

    public long CompanyId { get; set; }

    public long? CustomerId { get; set; }

    public long ProductId { get; set; }

    public decimal Price { get; set; }

    public bool? IsLimitedPeriod { get; set; }

    public DateTime? StartTime { get; set; }

    public DateTime? EndTime { get; set; }

    public bool? IsQuantitySpecific { get; set; }

    public double? MinQuantity { get; set; }

    public double? MaxQuantity { get; set; }

    public bool? IsUnitSpecific { get; set; }

    public long? UnitId { get; set; }

    public bool? IsActive { get; set; }

    public virtual Company Company { get; set; }

    public virtual Customer Customer { get; set; }

    public virtual Product Product { get; set; }

    public virtual UnitOfMeasurement Unit { get; set; }
}