using AdminPanel.Shared.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanel.Services.ServicesLayer
{
    public interface ICommonService
    {
        IEnumerable<CommonDropdownBO> GetAddressTypes();
        IEnumerable<CommonDropdownBO> GetAllCountries();
        IEnumerable<CommonDropdownBO> GetAllStatesByCountry(int countryId);
        IEnumerable<CommonDropdownBO> GetAllCitiesByState(int stateId);
        IEnumerable<CommonDropdownBO> GetAllCustomers(PaginatedResultBO model);
        IEnumerable<CommonDropdownBO> GetAllProducts(PaginatedResultBO model);

        IEnumerable<CommonDropdownBO> GetDiscountType();
        IEnumerable<CommonDropdownBO> GetDiscountLimitationType();
        IEnumerable<CommonDropdownBO> GetCutoffTypes();
        IEnumerable<CommonDropdownBO> GetTimeZones();

        #region Global texts section
        Task<CompanyGlobalTextBO> GetCompanyGlobaltexts(long companyId);
        Task<BaseResponseBO> AddUpdateCompanyGlobaltexts(CompanyGlobalTextBO model);
        #endregion

        #region Posted Links section
        IEnumerable<PostedLinksBO> GetCompanyPostedLinks(long companyId);
        Task<BaseResponseBO> AddUpdateCompanyPostedLinks(PostedLinksBO model);
        Task<BaseResponseBO> DeleteCompanyPostedLinks(long id);
        #endregion

        #region Company Slider Section
        IEnumerable<CompanySlidersBO> GetCompanySliders(long companyId, string domain);
        Task<BaseResponseBO> AddUpdateCompanySliders(CompanySlidersBO model);
        Task<BaseResponseBO> DeleteCompanySlider(long id);
        #endregion

        #region Delivery Run Section
        IEnumerable<DeliveryRunBO> GetDeliveryRuns(long companyId);
        Task<BaseResponseBO> AddUpdateDeliveryRun(DeliveryRunBO model);
        Task<BaseResponseBO> DeleteDeliveryRun(long id);
        #endregion

    }
}
