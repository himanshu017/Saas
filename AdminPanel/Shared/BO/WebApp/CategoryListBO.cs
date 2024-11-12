using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanel.Shared.BO.WebApp
{
    public class CategoryListBO
    {
        public long Id { get; set; }
        public string? CategoryName { get; set; }
        public bool IsActive { get; set; }

        public long CompanyId { get; set; }

        public bool IsFeatured { get; set; }

        public bool IsSpecial { get; set; }
        public int SortOrder { get; set; }
        public int ItemCount { get; set; }
        public IEnumerable<SubCategoryListBO>? AllSubCategories { get; set; }
    }

    public class SubCategoryListBO
    {
        public long Id { get; set; }
        public string? SubCategoryName { get; set; }
        public bool IsActive { get; set; }
        public long CompanyId { get; set; }
        public int SortOrder { get; set; }
    }
}
