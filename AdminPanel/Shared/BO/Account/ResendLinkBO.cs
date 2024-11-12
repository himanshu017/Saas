using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanel.Shared.BO
{
    public class ResendLinkBO
    {
        [Required(ErrorMessage = "Email is required")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "Please enter a valid email address")]
        public string? Email { get; set; }
    }

    public class ResendTokenBO
    {
        public string? Email { get; set; }
        public string Type { get; set; } = string.Empty;
    }

}
