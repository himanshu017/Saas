using AdminPanel.Shared.BO.MasterAdmin;

namespace AdminPanel.Client.Services
{
    public class StateService : IStateService
    {
        public long UserId { get; set; }
        public long CompanyId { get; set; } = 0;
        public string FullName { get; set; } = "";
        public string Email { get; set; } = "";
        public string Role { get; set; } = "";
        public string DomainName { get ; set; }
        public bool InCompanySection { get; set; }
        public bool AllowMasterTableEditing { get; set; }
        public ManagedFeaturesBO? ManagedFeatures { get; set; }

        public event Action<long> OnCompanySelect;

        public void SetCompanyForSuperAdmin(long companyId)
        {
            CompanyId = companyId;
            NotifyOnCompanySelect();
        }

        private void NotifyOnCompanySelect() => OnCompanySelect?.Invoke(CompanyId);
        public string? CurrencyInfo { get; set; } = "en-US";
        public CompanyConfigBO? CompanyConfig { get; set; }
    }
}
