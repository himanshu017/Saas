using AdminPanel.Shared.BO;
using AdminPanel.Shared.BO.CompanyAdmin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanel.Services.ServicesLayer.CompanyAdmin
{
    public interface ISettings
    {
        Task<CompanyBO> GetCompanyDetails(long companyId);
        Task<BaseResponseBO> SaveCompanyLogo(UpdateCompanyLogoModel model);
        Task<BaseResponseBO> UpdateCompanyDetails(CompanyBO model);
    }
}
