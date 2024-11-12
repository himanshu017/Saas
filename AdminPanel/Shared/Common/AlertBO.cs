using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanel.Shared
{
    public class AlertBO
    {
        public string Message { get; set; } = "";
        public string Type { get; set; } = "danger";
        public bool Visible { get; set; } = false;
    }
}
