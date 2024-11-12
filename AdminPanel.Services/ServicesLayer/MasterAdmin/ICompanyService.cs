using AdminPanel.Shared.BO;
using AdminPanel.Shared.BO.MasterAdmin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanel.Services
{
    public interface ICompanyService
    {
        IEnumerable<CompanyModelBO> GetAllCompanies();
        Task<CompanyConfigSettingsBO> GetCompanyConfiguration(long companyId);
        Task<BaseResponseBO> AddUpdateConfigSettings(CompanyConfigSettingsBO model);
    }
}
