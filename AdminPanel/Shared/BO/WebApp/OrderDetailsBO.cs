using AdminPanel.Shared.BO.CompanyAdmin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanel.Shared.BO.WebApp
{
    public class OrderDetailsBO : OrderBO
    {
        public string[] BillingAddress { get; set; }
        public string[] DelieryAddress { get; set; }
        public CompanyBO CompanyBO { get; set; }
    }
}
