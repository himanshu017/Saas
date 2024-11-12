using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanel.Shared.BO
{
    public class CustomerUserBO : CommonAuditFields
    {
        public long UserId { get; set; }
        public long CompanyId { get; set; }
        public long CustomerId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Phone { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsEmailVerified { get; set; } = true;

        public string FullName => $"{FirstName} {LastName}";
    }

    public class CustomerUserResetPassword
    {
        public long UserId { get; set; }

        public string? OldPassword { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [MinLength(8, ErrorMessage = "The password must contain 8 or more characters")]
        [RegularExpression("^(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[a-zA-Z]).{8,}$",
           ErrorMessage = "The passsword must contain atleast 1 Uppercase, Lowercase, number and special character")]
        public string? Password { get; set; }

        [Required(ErrorMessage = "Confirm Password is required")]
        [Compare(nameof(Password), ErrorMessage = "Password and Confirm password do not match")]
        public string? ConfirmPassword { get; set; }

        public bool? CheckExisting { get; set; } = false;
    }
}
