﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace AdminPanel.DataModel.Models;

public partial class GroupScope
{
    public byte Id { get; set; }

    public string ScopeDesc { get; set; }

    public virtual ICollection<Group> Groups { get; set; } = new List<Group>();
}