using AdminPanel.Services.ServicesLayer;
using AdminPanel.Shared.BO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AdminPanel.Server.Controllers.CompanyAdmin
{
    [Route("[controller]/[action]")]
    [ApiController]
    [Authorize(Roles = "CompanyAdmin, SuperAdmin")]
    public class CutoffController : ControllerBase
    {
        private readonly ICutoffService _service;
        public CutoffController(ICutoffService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetCutoffDetails([FromQuery] long companyId)
        {
            return Ok(_service.GetCutoffDetails(companyId));
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCutoffDetails([FromBody] DeliveryCutoffBO model)
        {
            return Ok(await _service.UpdateCutoffDetails(model));
        }
    }
}
