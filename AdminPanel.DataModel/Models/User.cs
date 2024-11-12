﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace AdminPanel.DataModel.Models;

public partial class User
{
    public long UserId { get; set; }

    public byte UserTypeId { get; set; }

    public long? CompanyId { get; set; }

    public long? CustomerId { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string UserName { get; set; }

    public string Email { get; set; }

    public string Phone { get; set; }

    public bool? IsActive { get; set; }

    public bool? IsEmailVerified { get; set; }

    public bool? AllowEmailNotification { get; set; }

    public bool? AllowSmsNotification { get; set; }

    public bool? TwoFactorAuthentication { get; set; }

    public string VerificationCode { get; set; }

    public string ResetCode { get; set; }

    public DateTime? CreatedOn { get; set; }

    public long? CreatedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public long? ModifiedBy { get; set; }

    public virtual Company Company { get; set; }

    public virtual Customer Customer { get; set; }

    public virtual ICollection<CustomerUserFeaturesMaster> CustomerUserFeaturesMasters { get; set; } = new List<CustomerUserFeaturesMaster>();

    public virtual ICollection<MessageRecipient> MessageRecipients { get; set; } = new List<MessageRecipient>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<TempCart> TempCarts { get; set; } = new List<TempCart>();

    public virtual ICollection<UserDeviceLocationAndToken> UserDeviceLocationAndTokens { get; set; } = new List<UserDeviceLocationAndToken>();

    public virtual ICollection<UserPassword> UserPasswords { get; set; } = new List<UserPassword>();

    public virtual ICollection<UserSalesrepCode> UserSalesrepCodes { get; set; } = new List<UserSalesrepCode>();

    public virtual UserType UserType { get; set; }

    public virtual ICollection<Address> Addresses { get; set; } = new List<Address>();
}