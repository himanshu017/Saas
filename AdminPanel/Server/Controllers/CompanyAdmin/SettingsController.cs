using AdminPanel.Services.ServicesLayer.CompanyAdmin;
using AdminPanel.Shared.BO.CompanyAdmin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AdminPanel.Server.Controllers.CompanyAdmin
{
    [Route("[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class SettingsController : ControllerBase
    {
        private readonly ISettings _settings;
        public SettingsController(ISettings settings)
        {
            _settings = settings;
        }

        [HttpGet]
        public async Task<IActionResult> GetCompanyDetails([FromQuery] long companyId)
        {
            var res = await _settings.GetCompanyDetails(companyId);
            return Ok(res);
        }

        [HttpPost]
        public async Task<IActionResult> AddUpdateCompanyLogo([FromBody] UpdateCompanyLogoModel model)
        {
            var res = await _settings.SaveCompanyLogo(model);
            return Ok(res);
        }


        [HttpPost]
        public async Task<IActionResult> UpdateCompanyDetails([FromBody] CompanyBO model)
        {
            var res = await _settings.UpdateCompanyDetails(model);
            return Ok(res);
        }
    }
}
