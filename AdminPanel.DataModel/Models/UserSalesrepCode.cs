﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace AdminPanel.DataModel.Models;

public partial class UserSalesrepCode
{
    public long UserId { get; set; }

    public string SalesrepCode { get; set; }

    public virtual User User { get; set; }
}