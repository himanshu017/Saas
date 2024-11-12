using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanel.Shared.BO.MasterAdmin
{
    public class StatusListBO
    {
        public byte Id { get; set; }
        public string StatusDesc { get; set; }
        public string StatusType { get; set; }
        public byte TypeId { get; set; }
    }
}
