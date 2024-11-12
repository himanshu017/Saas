using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanel.Shared.BO
{
    public class ResetPasswordBO
    {
        [Required(ErrorMessage = "Password is required")]
        [MinLength(8, ErrorMessage = "The password must contain 8 or more characters")]
        [RegularExpression("^(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[a-zA-Z]).{8,}$",
            ErrorMessage = "The passsword must contain atleast 1 Uppercase, Lowercase, number and special character")]
        public string? Password { get; set; }

        [Required(ErrorMessage = "Confirm Password is required")]
        [Compare(nameof(Password), ErrorMessage = "Password and Confirm password do not match")]
        public string? ConfirmPassword { get; set; }
        public string? ResetToken { get; set; }
    }
}
