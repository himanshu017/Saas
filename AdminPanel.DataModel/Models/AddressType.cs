﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace AdminPanel.DataModel.Models;

public partial class AddressType
{
    public byte Id { get; set; }

    public string TypeDesc { get; set; }

    public virtual ICollection<Address> Addresses { get; set; } = new List<Address>();
}