using AdminPanel.Shared.BO.CompanyAdmin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanel.Shared.BO
{
    public class CompanySlidersBO: CommonAuditFields
    {
        public long Id { get; set; }
        public long CompanyId { get; set; }
        public string? Title { get; set; }
        public string? Summary { get; set; }
        public string? Image { get; set; }
        public string? Url { get; set; }
        public bool IsActive { get; set; }

        public IEnumerable<FileViewModel> SliderImagesList { get; set; }
    }
}
