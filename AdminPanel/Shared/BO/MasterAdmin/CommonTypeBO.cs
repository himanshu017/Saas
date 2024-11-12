using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanel.Shared.BO.MasterAdmin
{
    public class CommonTypeBO
    {
        public byte Id { get; set; }
        public byte TypeId { get; set; }
        public string TypeDesc { get; set; }
        public bool IsStatus { get; set; }
    }
}

