using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanel.Shared.BO
{
    public class UserFeaturesBO: CommonAuditFields
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string? ManagedFeatures { get; set; }
    }
}
