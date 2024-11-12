using AdminPanel.Services.ServicesLayer;
using AdminPanel.Shared.BO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AdminPanel.Server.Controllers.CompanyAdmin
{
    [Route("[controller]/[action]")]
    [ApiController]
    [Authorize(Roles ="CompanyAdmin,SuperAdmin")]
    public class DiscountController : ControllerBase
    {
        private readonly IDiscountService _service;

        public DiscountController(IDiscountService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetAllRecords([FromQuery]long companyId)
        {
            return Ok(_service.GetAllRecords(companyId));
        }

        [HttpPost]
        public async Task<IActionResult> AddEditDiscount([FromBody]DiscountBO model)
        {
            return Ok(await _service.AddEditDiscount(model));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteDiscount([FromBody] int id)
        {
            return Ok(await _service.DeleteDiscount(id));
        }
    }
}
