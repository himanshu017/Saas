using AdminPanel.Shared.BO.Account;
using AdminPanel.Shared.BO.MasterAdmin;
using AdminPanel.Shared.BO.WebApp;

namespace AdminPanel.Web.Client.Services
{
    public interface IStateService
    {
        public long UserId { get; set; }
        public long CompanyId { get; set; }
        public long CustomerId { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Role { get; set; }
        public string? DomainName { get; set; }
        public ManagedFeaturesBO? ManagedFeatures { get; set; }

        //get slider details on login so it doesn't conflict with jquery
        public string? ImagePath { get; set; }
        public bool IsAuthenticated { get; set; }
        public string? CompanyEmail { get; set; }
        public string? CompanyPhone { get; set; }
        public string? Logo { get; set; }

        
        // app colors as set from the admin section
        public string? PrimaryColor { get; set; }
        public string? SecondaryColor { get; set; }
        public CartCountBO? CartCountBO { get; set; }
        public OrderSettingsBO? OrderSettings { get; set; }
        public string CurrencyInfo { get; set; }
    }
}
