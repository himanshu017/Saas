using AdminPanel.Shared.BO.Account;
using AdminPanel.Shared.BO.MasterAdmin;
using AdminPanel.Shared.BO.WebApp;

namespace AdminPanel.Web.Client.Services
{
    public class StateService : IStateService
    {
        public long UserId { get; set; }
        public long CompanyId { get; set; } = 0;
        public long CustomerId { get; set; } = 0;
        public string? FullName { get; set; } = "";
        public string? Email { get; set; } = "";
        public string? Role { get; set; } = "";
        public string? DomainName { get; set; }
        public string? Logo { get; set; } = "";
        public ManagedFeaturesBO? ManagedFeatures { get; set; } = new();
        public string? ImagePath { get; set; }
        public bool IsAuthenticated { get; set; }
        public string? CompanyEmail { get; set; }
        public string? CompanyPhone { get; set; }
        public string? PrimaryColor { get; set; }
        public string? SecondaryColor { get; set; }
        public CartCountBO? CartCountBO { get; set; }
        public OrderSettingsBO? OrderSettings { get; set; }
        public string? CurrencyInfo { get; set; } 
    }
}
