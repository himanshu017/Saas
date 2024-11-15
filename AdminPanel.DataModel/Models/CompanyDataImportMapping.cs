﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace AdminPanel.DataModel.Models;

public partial class CompanyDataImportMapping
{
    public int Id { get; set; }

    public long CompanyId { get; set; }

    public string ImportType { get; set; }

    public string MappingName { get; set; }

    public string MappingDetails { get; set; }

    public DateTime? CreatedOn { get; set; }

    public long? CreatedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public long? ModifiedBy { get; set; }

    public virtual Company Company { get; set; }

    public virtual ICollection<ImportLog> ImportLogs { get; set; } = new List<ImportLog>();
}