using AdminPanel.Services;
using AdminPanel.Shared.BO.MasterAdmin;
using Microsoft.AspNetCore.Mvc;

namespace AdminPanel.Server.Controllers.MasterAdmin
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyService _company;

        public CompanyController(ICompanyService company)
        {
            _company = company;
        }

        [HttpGet]
        public IActionResult GetAllCompanies()
        {
            return Ok(_company.GetAllCompanies());
        }

        [HttpGet]
        public async Task<IActionResult> GetCompanyConfiguration(long companyId)
        {

            var res = await _company.GetCompanyConfiguration(companyId);
            return Ok(res);
        }

        [HttpPost]
        public async Task<IActionResult> AddUpdateConfigSettings([FromBody] CompanyConfigSettingsBO model)
        {
            var res = await _company.AddUpdateConfigSettings(model);
            return Ok(res);
        }
    }
}
