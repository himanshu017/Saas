using AdminPanel.Services.ServicesLayer;
using AdminPanel.Shared.BO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AdminPanel.Server.Controllers.CompanyAdmin
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class MarketingController : ControllerBase
    {
        private readonly IMarketingService _service;

        public MarketingController(IMarketingService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetAllRecords([FromQuery] long companyId)
        {
            return Ok(_service.GetAllRecords(companyId));
        }

        [HttpPost]
        public async Task<IActionResult> AddUpdateRecords([FromBody] TargetMarketingBO model)
        {
            return Ok(await _service.AddUpdateRecord(model));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteRecord([FromBody] long id)
        {
            return Ok(await _service.DeleteRecord(id));
        }

    }
}
