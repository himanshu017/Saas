using AdminPanel.Shared.BO;
using AdminPanel.Shared.BO.MasterAdmin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanel.Shared.BO
{
    public class LoginResponse : BaseResponseBO
    {
        public long CompanyId { get; set; }
        public long UserId { get; set; }
        public long CustomerId { get; set; }
        public byte UserTypeID { get; set; }
        public string? Role { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? CompanyName { get; set; }
        public string? Logo { get; set; }
        public string? DomainName { get; set; }
        public string? Token { get; set; }
        public string? ManagedFeatures { get; set; }
        public string? OrderSettings { get; set; }
        public string FullName
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }

        public string? CurrencyInfo { get; set; } = "en-US";
        public string? CompanyConfig { get; set; }
    }
}
