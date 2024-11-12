using AdminPanel.Shared.BO.CompanyAdmin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanel.Shared.BO
{
    public class CommonDropdownBO
    {
        public int Id { get; set; }
        public long IdLong { get; set; }
        public string? Description { get; set; }
        public int TotalRecords { get; set; }
    }

    public class FieldInfo
    {
        public string Name { get; set; }
        public string FieldType { get; set; }
        public bool IsRequired { get; set; }
    }

    public class ImportBaseClass
    {
        public List<string>? ImportedColumns { get; set; }
        public IEnumerable<FieldInfo>? TableColumns { get; set; }
        public string? ImportFor { get; set; }

        public Dictionary<string, string>? MappingFields { get; set; }

        public byte[] bytearray { get; set; }


        public string? MappingTitle { get; set; }

        public int? MappingId { get; set; }

        public long CompanyId { get; set; }

        public string? ImportedFileName { get; set; }

        public List<FileViewModel> FileList{ get; set; }

}

   


}