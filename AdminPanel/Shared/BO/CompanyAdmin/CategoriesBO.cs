using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanel.Shared.BO.CompanyAdmin
{
    public class CategoriesBO: CommonAuditFields
    {
        public long Id { get; set; }
        public string? CategoryName { get; set; }
        public bool IsActive { get; set; }

        public long CompanyId { get; set; }

        public bool IsFeatured { get; set; }

        public bool IsSpecial { get; set; }
        public int SortOrder { get; set; }
    }
    public class SubCategoriesBO: CommonAuditFields
    {
        public long Id { get; set; }
        public long MainCategoryId { get; set; }
        public string? SubCategoryName { get; set; }
        public bool IsActive { get; set; }

        public int SortOrder { get; set; }
    }

    public class FiltersBO : CommonAuditFields
    {
        public int FilterId { get; set; }
        public long CompanyId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public bool IsActive { get; set; }
    }

    public class UnitOfMeasureBO : CommonAuditFields
    {
        public long UnitId { get; set; }
        public long CompanyId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public bool IsActive { get; set; }
    }
}
