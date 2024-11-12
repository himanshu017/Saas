using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanel.Shared.BO
{
    public class LoginBO
    {
        [Required(ErrorMessage = "Username / Email is required")]
        public string? UserName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }

        public string? DeviceType { get; set; }
        public string? DeviceToken { get; set; }
        public string? DomainName { get; set; }
    }

    public class ForgotPasswordModel
    {
        [Required(ErrorMessage = "Username / Email is required")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "Please enter a valid email address")]
        public string? Email { get; set; }

        public string? Domain { get; set; }

        public byte UserTypeId { get; set; }

        public long CompanyId { get; set; }

    }


    public class ValidateLinkModel
    {
        public bool IsExpired { get; set; }

        public string? ExpiryTicks { get; set; }

    }
    public class ChangePasswordModel
    {
        public string? QueryParam { get; set; }
        public string? OldPassword { get; set; }
        public string? NewPassword { get; set; }
    }
}
