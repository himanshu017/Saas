using AdminPanel.Shared.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanel.Services.ServicesLayer
{
    public interface ICutoffService
    {
        IEnumerable<DeliveryCutoffBO> GetCutoffDetails(long companyId);
        Task<BaseResponseBO> UpdateCutoffDetails(DeliveryCutoffBO model);
        Task<DateTime> CalculateNextDeliveryDate(long companyId);
    }
}
