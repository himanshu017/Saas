using AdminPanel.Services.ServicesLayer;
using AdminPanel.Shared.BO;
using AdminPanel.Shared.BO.WebApp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AdminPanel.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    //[Authorize(Roles = "AppUser")]
    public class ProductController : ControllerBase
    {
        private readonly ILogger<ProductController> _logger;
        private readonly IProductsService _service;

        public ProductController(IProductsService service, ILogger<ProductController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllMainAndSubCategories([FromQuery] GetAllCategoriesBO model)
        {
            return Ok(await _service.GetAllMainAndSubCategories(model));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllFilters([FromQuery] long companyId)
        {
            return Ok(await _service.GetAllFilters(companyId));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUnits([FromQuery] long companyId)
        {
            return Ok(await _service.GetAllUnits(companyId));
        }

        [HttpPost]
        public async Task<IActionResult> GetProductsBySearchFilters([FromBody] GetProductListBO model)
        {
            return Ok(await _service.GetProductsBySearchFilters(model));
        }

        [HttpGet]
        public async Task<IActionResult> GetFavoriteLists([FromQuery] GetFavLists model)
        {
            return Ok(await _service.GetFavoriteLists(model));
        }

        [HttpPost]
        public async Task<IActionResult> GetFavoriteListItems([FromBody] GetProductListBO model)
        {
            return Ok(await _service.GetFavoriteListItems(model));
        }

        [HttpPost]
        public async Task<IActionResult> AddUpdateFavList([FromBody] FavoriteListBO model)
        {
            return Ok(await _service.AddUpdateFavList(model));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteFavList([FromBody] long id)
        {
            return Ok(await _service.DeleteFavList(id));
        }

        [HttpPost]
        public async Task<IActionResult> AddDeleteFavlistItem([FromBody] ManageFavListItemBO model)
        {
            return Ok(await _service.AddDeleteFavlistItem(model));
        }

        [HttpPost]
        public async Task<IActionResult> GetCompanySpecials([FromBody] GetProductListBO model)
        {
            return Ok(await _service.GetCompanySpecials(model));
        }

        [HttpPost]
        public async Task<IActionResult> GetTempCartItems([FromBody] GetProductListBO model)
        {
            return Ok(await _service.GetTempCartItems(model));
        }

        [HttpPost]
        public async Task<IActionResult> GetProductDetail([FromBody] GetProdDetailBO model)
        {
            return Ok(await _service.GetProductDetail(model));
        }

        [HttpPost]
        public async Task<IActionResult> GetSuggestiveProducts([FromBody] GetProductListBO model)
        {
            return Ok(await _service.GetSuggestiveProducts(model));
        }
    }
}
