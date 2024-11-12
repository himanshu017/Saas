using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanel.Shared.BO
{
    public class PaginatedResultBO
    {
        public long CompanyId { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public string? SearchString { get; set; } = string.Empty;
        public bool OrderByDesc { get; set; }
        public string? SortColumn { get; set; }
        public string? SelectedValues { get; set; }
        public bool IsCart { get; set; } = false;
    }
}
