using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanel.Shared.BO
{
    public class TargetMarketingBO: CommonAuditFields
    {
        public long Id { get; set; }
        public long CompanyId { get; set; }
        public long? GroupId { get; set; }
        public long? CustomerId { get; set; }
        public byte? MarketingTypeId { get; set; }
        public string? FileName { get; set; }
        public string? Title { get; set; }
        public bool IsActive { get; set; }
        public IEnumerable<long>? ProductIds { get; set; }
        public string? GroupName { get; set; }
        public string? CustomerName { get; set; }

        public UploadFileBO UploadedFile { get; set; }
    }
}
