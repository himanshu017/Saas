using AdminPanel.Shared.BO;
using AdminPanel.Shared.BO.CompanyAdmin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanel.Services.ServicesLayer
{
    public interface ICustomerService
    {
        IEnumerable<CustomerBO> GetAllCustomers(GetCustomerModel model);
        Task<BaseResponseBO> AddEditCustomer(CustomerBO model);
        Task<IEnumerable<CustomerUserBO>> GetCustomerUsers(long customerId);
        Task<UserFeaturesBO> GetUserFeaturesByUserId(long userId);
        Task<BaseResponseBO> SaveUserFeatures(UserFeaturesBO model);
        Task<BaseResponseBO> AddEditCustomerUser(CustomerUserBO model);
        Task<BaseResponseBO> UpdateUserPassword(CustomerUserResetPassword model);
        Task<BaseResponseBO> DeleteCustomerUser(long userId);
        Task<IEnumerable<CustomerAddressBO>> GetCustomerAddresses(long customerId);
        Task<BaseResponseBO> AddEditCustomerAddress(CustomerAddressBO model);
        Task<BaseResponseBO> DeleteCustomerAddress(DeleteCustomerAddressBO model);
        IEnumerable<SalesrepBO> GetAllSalesreps(long companyId);
        Task<BaseResponseBO> AddEditSalesrep(SalesrepBO model);
        Task<BaseResponseBO> DeleteSalesrep(long userId);
        IEnumerable<string> AllSalesmanCodes(long companyId);
        Task<bool> UpdateBitField(UpdateBitBO model);
    }
}
