using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanel.Shared.BO
{
    public class SalesrepBO : CommonAuditFields
    {
        public long UserId { get; set; }
        public long CompanyId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Phone { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsEmailVerified { get; set; } = true;
        public List<string> SalesrepCode { get; set; } = new List<string>() { "Default" };
        public string FullName => $"{FirstName} {LastName}";
    }
}
