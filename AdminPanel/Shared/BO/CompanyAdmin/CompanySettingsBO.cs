using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanel.Shared.BO.CompanyAdmin
{
    public class CompanySettingsBO
    {
    }

    public class UpdateCompanyLogoModel
    {
        public FileViewModel? File { get; set; }
        public long CompanyId { get; set; }
        public long UserId { get; set; }
    }
}
