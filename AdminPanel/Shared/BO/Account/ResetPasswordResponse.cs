using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanel.Shared.BO
{
    public class ResetPasswordResponse: BaseResponseBO
    {
        public bool IsRecent { get; set; }
    }
}
