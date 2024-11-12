
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace EzWeb.Pages
{
    public class IndexModel : PageModel
    {
        

        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }
        public void OnPost()
        {
            //var ss= string.Format("Name: {0} {1}", person.Name, person.Email);
        }
    }
}