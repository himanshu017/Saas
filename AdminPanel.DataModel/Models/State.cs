﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace AdminPanel.DataModel.Models;

public partial class State
{
    public int Id { get; set; }

    public string StateCode { get; set; }

    public string StateName { get; set; }

    public int? CountryId { get; set; }

    public virtual ICollection<Address> Addresses { get; set; } = new List<Address>();

    public virtual ICollection<City> Cities { get; set; } = new List<City>();

    public virtual Country Country { get; set; }
}