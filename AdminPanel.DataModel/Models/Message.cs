﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace AdminPanel.DataModel.Models;

public partial class Message
{
    public long Id { get; set; }

    public long CompanyId { get; set; }

    public long? GroupId { get; set; }

    public bool? IsSms { get; set; }

    public string MessageText { get; set; }

    public DateTime? CreatedOn { get; set; }

    public long? CreatedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public long? ModifiedBy { get; set; }

    public virtual Company Company { get; set; }

    public virtual Group Group { get; set; }

    public virtual ICollection<MessageRecipient> MessageRecipients { get; set; } = new List<MessageRecipient>();
}