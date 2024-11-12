using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanel.Shared.BO
{
    public class GroupsBO : CommonAuditFields
    {
        public long Id { get; set; }
        public long CompanyId { get; set; }
        public byte GroupTypeId { get; set; }
        public byte GroupScopeId { get; set; }
        public string? Name { get; set; }
        public bool IsActive { get; set; }
        public IEnumerable<long>? ReferemceIds { get; set; }
    }
}
