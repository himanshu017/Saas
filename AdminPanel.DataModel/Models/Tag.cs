﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace AdminPanel.DataModel.Models;

public partial class Tag
{
    public long Id { get; set; }

    public string Title { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}