using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanel.Shared.BO.MasterAdmin
{
    public class CompanyConfigSettingsBO : CommonAuditFields
    {
        public long CompanyId { get; set; }
        public string ConfigSettings { get; set; }
        public bool IsUpdate { get; set; }
    }
}
