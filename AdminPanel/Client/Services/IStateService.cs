using AdminPanel.Shared.BO.MasterAdmin;

namespace AdminPanel.Client.Services
{
    public interface IStateService
    {
        public long UserId { get; set; }
        public long CompanyId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string DomainName { get; set; }
        public bool InCompanySection { get; set; }
        public bool AllowMasterTableEditing { get; set; }
        public ManagedFeaturesBO? ManagedFeatures { get; set; }

        public event Action<long> OnCompanySelect;
        void SetCompanyForSuperAdmin(long companyId);
        public string? CurrencyInfo { get; set; }
        public CompanyConfigBO? CompanyConfig { get; set; }
    }
}
