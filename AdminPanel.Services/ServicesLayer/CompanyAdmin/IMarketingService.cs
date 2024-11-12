using AdminPanel.Shared.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanel.Services.ServicesLayer
{
    public interface IMarketingService
    {
        IEnumerable<TargetMarketingBO> GetAllRecords(long companyId);
        Task<BaseResponseBO> AddUpdateRecord(TargetMarketingBO record);
        Task<BaseResponseBO> DeleteRecord(long id);
    }
}
