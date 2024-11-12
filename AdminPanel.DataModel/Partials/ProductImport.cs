using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanel.DataModel.Models
{
    public partial class ProductImport
    {
        public object this[string propertyName]
        {
            get
            {
                PropertyInfo prop = this.GetType().GetProperty(propertyName);
                return prop.GetValue(this);
            }
            set
            {
                PropertyInfo prop = this.GetType().GetProperty(propertyName);
                prop.SetValue(this, value);
            }
        }
    }

    public partial class CustomerImport
    {
        public object this[string propertyName]
        {
            get
            {
                PropertyInfo prop = this.GetType().GetProperty(propertyName);
                return prop.GetValue(this);
            }
            set
            {
                PropertyInfo prop = this.GetType().GetProperty(propertyName);
                prop.SetValue(this, value);
            }
        }
    }

    public partial class SpecialPriceImport
    {
        public object this[string propertyName]
        {
            get
            {
                PropertyInfo prop = this.GetType().GetProperty(propertyName);
                return prop.GetValue(this);
            }
            set
            {
                PropertyInfo prop = this.GetType().GetProperty(propertyName);
                prop.SetValue(this, value);
            }
        }
    }
}
