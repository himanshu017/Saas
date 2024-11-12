using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanel.Shared.BO
{
    public class RegisterBO
    {
        [Display(Name = "First Name")]
        [Required(ErrorMessage ="First Name is required")]
        public string? FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage ="Last Name is required")]
        public string? LastName { get; set; }

        [Display(Name = "Company Name")]
        [Required(ErrorMessage ="Company Name is required")]
        public string? CompanyName { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "Email is required")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "Please enter a valid email address")]
        public string? Email { get; set; }

        
        [Display(Name = "Password")]
        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }
        
        public bool IsTermsAndConditions { get; set; }

        [Display(Name = "Domain Name")]
        [Required(ErrorMessage = "Domain Name is required")]
        public string? DomainName { get; set; }
    }

    public class AppUserRegisterBO
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public long CompanyId { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public bool IsTermsAndConditions { get; set; }
        public string? Phone { get; set; }
        public string? DeviceToken { get; set; }
        public string? DeviceType { get; set; }
        public byte UserTypeID { get; set; }
    }
}
