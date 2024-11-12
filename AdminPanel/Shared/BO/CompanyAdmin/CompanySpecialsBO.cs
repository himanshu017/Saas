using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanel.Shared.BO.CompanyAdmin
{
    public class CompanySpecialsBO
    {
        public long UserId { get; set; }
        public long CompanyId { get; set; }
        public IEnumerable<long> ProductIds { get; set; }
    }

    public class GetSpecialsResponse : BaseResponseBO
    {
        public IEnumerable<long> ProductIds { get; set; }
    }
}
