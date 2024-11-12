using AdminPanel.Shared.BO.MasterAdmin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanel.Shared.BO
{
    public class DefaultInfoBO
    {
        public ManagedFeaturesBO? ManagedFeatures { get; set; }
        public List<int>? WarehouseIds { get; set; }
        public string? Timezone { get; set; }
    }
}
