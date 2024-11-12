using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace EZOrders.ImportServices
{

    public enum OrderFlowImportFor
    {
        None = 0,
        Product = 1,
        Customer = 2
    }

    public enum OrderFlowImportType
    {
        None,
        Excel,
        CSV,
    }
   

}
