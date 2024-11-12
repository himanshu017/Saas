using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanel.Shared.BO.WebApp
{
    public class GetOrderHistoryBO
    {
        public long CompanyId { get; set; }
        public long CustomerId { get; set; }
        public long UserId { get; set; }
        public bool AllUsers { get; set; } = false;
    }
}
