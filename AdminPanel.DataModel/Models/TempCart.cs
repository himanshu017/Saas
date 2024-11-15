﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace AdminPanel.DataModel.Models;

public partial class TempCart
{
    public long Id { get; set; }

    public long CompanyId { get; set; }

    public long CustomerId { get; set; }

    public long UserId { get; set; }

    public long? AddressId { get; set; }

    public long? CommentId { get; set; }

    public long? DeliveryRunId { get; set; }

    public int? DiscountId { get; set; }

    public decimal? DiscountAmount { get; set; }

    public string DeviceType { get; set; }

    public string DeviceToken { get; set; }

    public string AppVersion { get; set; }

    public DateTime? CreatedOn { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public virtual Address Address { get; set; }

    public virtual Comment Comment { get; set; }

    public virtual Company Company { get; set; }

    public virtual Customer Customer { get; set; }

    public virtual DeliveryRun DeliveryRun { get; set; }

    public virtual ICollection<TempCartItem> TempCartItems { get; set; } = new List<TempCartItem>();

    public virtual User User { get; set; }
}