using AdminPanel.Shared.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanel.Services.ServicesLayer
{
    public interface IDiscountService
    {
        IEnumerable<DiscountBO> GetAllRecords(long companyId);
        Task<BaseResponseBO> AddEditDiscount(DiscountBO model);
        Task<BaseResponseBO> DeleteDiscount(int id);
    }
}
