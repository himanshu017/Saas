using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanel.Shared.Common
{
    public interface IAutidStamps
    {
        DateTime? CreatedOn { get; set; }
        DateTime? ModifiedOn { get; set; }
        long? ModifiedBy { get; set; }
        long? CreatedBy { get; set; }
    }
}
