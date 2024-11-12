using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanel.Shared.BO
{
    public abstract class CommonAuditFields
    {
        public DateTime? CreatedOn { get; set; }
        public long? CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public long? ModifiedBy { get; set; }
        public string? DomainName { get; set; }
        public int TotalRecords { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
