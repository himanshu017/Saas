using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace EzWeb.Pages
{
    public class Contact : PageModel
    {
        private readonly ILogger<Contact> _logger;

        [BindProperty]
        [Required(ErrorMessage = "Please Enter Username..")]
        [Display(Name = "UserName")]
        public string? Name { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Please Enter Email...")]
        [Display(Name = "Email")]
        public string? Email { get; set; }
        [BindProperty]
        public string? Phone { get; set; }
        [BindProperty]
        public string? Comments { get; set; }

        public bool IsSucccess { get; set; }

        IEmailService _emailService = null;
        public Contact(ILogger<Contact> logger, IEmailService emailService)
        {
            _logger = logger;
            _emailService = emailService;
        }

        public void OnGet()
        {
        }
        public void OnPost()
        {
            var name = Request.Form["Name"];
            var Email = Request.Form["Email"];
            var Phone = Request.Form["Phone"];
            var Comments = Request.Form["Comments"];
            bool issend = false;
            if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(Email) && !string.IsNullOrEmpty(Comments))
            {
                EmailData emailData = new EmailData();
                emailData.EmailToId = Email;
                emailData.EmailBody = Comments;
                emailData.EmailToName = name;
                emailData.EmailSubject = "Ez Contact Request";
                issend = _emailService.SendEmail(emailData);
            }


            IsSucccess = issend;

        }
    }
}