﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace AdminPanel.DataModel.Models;

public partial class ProductUnit
{
    public long ProductId { get; set; }

    public long UnitId { get; set; }

    public int? UnitQuantity { get; set; }

    public bool? OrderMultiples { get; set; }

    public bool? IsDefault { get; set; }

    public decimal? Weight { get; set; }

    public virtual Product Product { get; set; }

    public virtual UnitOfMeasurement Unit { get; set; }
}