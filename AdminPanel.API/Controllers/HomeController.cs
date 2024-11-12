using AdminPanel.Services.ServicesLayer;
using AdminPanel.Shared.BO.CompanyAdmin;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AdminPanel.API.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHomeService _service;

        public HomeController(ILogger<HomeController> logger, IHomeService service)
        {
            _logger = logger;
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetCompanySliders([FromQuery]long companyId)
        {
            return Ok(await _service.GetCompanySliders(companyId));
        }

        [HttpGet]
        public async Task<IActionResult> GetFeaturedProducts([FromQuery] GetProductModel model)
        {
            return Ok(await _service.GetFeaturedProducts(model));
        }

        [HttpGet]
        public async Task<IActionResult> GetFeaturedCategoryProducts([FromQuery] GetProductModel model)
        {
            return Ok(await _service.GetFeaturedCategoryProducts(model));
        }

        [HttpGet]
        public async Task<IActionResult> GetRecentlyOrderedProducts([FromQuery] GetProductModel model)
        {
            return Ok(await _service.GetRecentlyOrderedProducts(model));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategories([FromQuery] long companyId)
        {
            return Ok(await _service.GetAllCategories(companyId));
        }
    }
}
