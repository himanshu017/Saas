using Microsoft.AspNetCore.Mvc;
using AdminPanel.Services.ServicesLayer;
using AdminPanel.Shared.BO.MasterAdmin;
using AdminPanel.Shared.BO.CompanyAdmin;
using AdminPanel.Services.ServicesLayer.CompanyAdmin;
using AdminPanel.Shared.BO;
using AdminPanel.Client.Services;
using System.Collections.Generic;

namespace AdminPanel.Server.Controllers.CompanyAdmin
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class ProductController : Controller
    {
        private readonly IProductService _prodService;
        private readonly ICategoriesService _catService;

        public ProductController(IProductService prodService, ICategoriesService catService)
        {
            _prodService = prodService;
            _catService = catService;
        }

        [HttpPost]
        public async Task<IActionResult> AddEditProduct([FromBody] ProductBO model)
        {
            // var DomainName = HttpContext.User.FindFirst("DomainName");
            var response = new BaseResponseBO() { IsSuccess = false };
            response = await _prodService.AddUpdateProduct(model);

            return Ok(response);
        }

        [HttpGet]
        public IActionResult GetAllProducts([FromQuery] GetProductModel model)
        {
            var products = _prodService.GetAllProducts(model);

            return Ok(products);
        }

        [HttpGet]
        public async Task<IActionResult> GetProductByid(long pid, string d)
        {
            var product = await _prodService.GetProductById(pid, d);
            if (product.CategoryId > 0)
            {
                var subcateoryData = await _catService.GetSubCategoryById(product.CategoryId ?? 0);
                product.MainCategoryId = subcateoryData.MainCategoryId;
            }

            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteProduct([FromBody] ProductBO model)
        {
            var response = new BaseResponseBO() { IsSuccess = false };
            response = await _prodService.DeleteProduct(model);

            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetCompanySpecials([FromQuery] long companyId)
        {
            var products = await _prodService.GetCompanySpecials(companyId);

            return Ok(products);
        }

        [HttpPost]
        public async Task<IActionResult> AddUpdateCompanySpecials([FromBody] CompanySpecialsBO model)
        {
            return Ok(await _prodService.AddUpdateCompanySpecials(model));
        }

        [HttpPost]
        public async Task<IActionResult> UpdateBitField([FromBody] UpdateBitBO model)
        {
            return Ok(await _prodService.UpdateBitField(model));
        }
    }
}
