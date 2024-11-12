using AdminPanel.DataModel.Context;
using AdminPanel.Shared.BO;
using AdminPanel.Shared.BO.MasterAdmin;
using Newtonsoft.Json;
using System.Data.Entity;

namespace AdminPanel.Services.Repository.WebApp
{
    public class Helper
    {
        private readonly OrderflowContext _context;
        public Helper()
        {

        }

        public Helper(OrderflowContext context)
        {
            _context = context;
        }

        public DefaultInfoBO GetDefaultUserData(long companyId, long userId, long customerId)
        {
            var company = _context.Companies.Find(companyId);
            string? timeZone = _context.GlobalTimeZones.Find(company.TimeZoneId)?.Zid;

            if(userId == 0)
            {
                userId = _context.Users.Where(x => x.CompanyId == companyId && x.IsActive == true)
                                       .OrderByDescending(o => o.CreatedOn)
                                       .FirstOrDefault().UserId;
            }

            var response = new DefaultInfoBO()
            {
                ManagedFeatures = JsonConvert.DeserializeObject<ManagedFeaturesBO>(_context.CustomerUserFeaturesMasters.Where(x => x.UserId == userId).FirstOrDefault().ManagedFeatures),
                WarehouseIds = customerId > 0 ? _context.Warehouses.Where(x => x.Customers.Where(x => x.CustomerId == customerId).Count() > 0).Select(s => s.Id).ToList() : null,
                Timezone = timeZone // to be used in place for dates for order and other date time requirements
            };

            return response;
        }

    }
}
