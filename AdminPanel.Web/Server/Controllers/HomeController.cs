using AdminPanel.Shared;
using AdminPanel.Shared.BO;
using AdminPanel.Shared.BO.CompanyAdmin;
using AdminPanel.Shared.BO.WebApp;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace AdminPanel.Web.Server.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly HttpClient _http;
        private readonly WebSettings _webSettings;
        public HomeController(HttpClient http, IOptions<WebSettings> webSettings)
        {
            _http = http;
            _webSettings = webSettings.Value;
        }

        #region Get Company Sliders for Home Page
        [HttpGet]
        public async Task<IActionResult> GetCompanySliders([FromQuery] long companyId)
        {
            try
            {
                var response = await _http.GetFromJsonAsync<GenericResponse<IEnumerable<SliderBO>>>(_webSettings.ApiUrl + $"Home/GetCompanySliders?companyId={companyId}");

                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(new GenericResponse<IEnumerable<SliderBO>>()
                {
                    IsSuccess = false,
                    Message = $"Error: {ex.Message} :: Inner: {ex.InnerException?.Message}",
                    IsAuthorized = !ex.Message.Contains("unauthorized", StringComparison.OrdinalIgnoreCase)
                });
            }
        }
        #endregion

        #region Get Featured Products for Home Page
        [HttpPost]
        public async Task<IActionResult> GetFeaturedProducts([FromBody] GetProductModel model)
        {
            try
            {
                var response = await _http.GetFromJsonAsync<GenericResponse<IEnumerable<ProductListBO>>>(_webSettings.ApiUrl + $"Home/GetFeaturedProducts?CompanyId={model.CompanyId}&PageSize={model.PageSize}&PageNumber={model.PageNumber}");

                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(new GenericResponse<IEnumerable<ProductListBO>>()
                {
                    IsSuccess = false,
                    Message = $"Error: {ex.Message} :: Inner: {ex.InnerException?.Message}",
                    IsAuthorized = !ex.Message.Contains("unauthorized", StringComparison.OrdinalIgnoreCase)
                });
            }
        }
        #endregion

        #region Get Recently Ordered Products for Home Page
        [HttpPost]
        public async Task<IActionResult> GetRecentlyOrderedProducts([FromBody] GetProductModel model)
        {
            try
            {
                var response = await _http.GetFromJsonAsync<GenericResponse<IEnumerable<ProductListBO>>>(_webSettings.ApiUrl + $"Home/GetRecentlyOrderedProducts?CompanyId={model.CompanyId}&PageSize={model.PageSize}&PageNumber={model.PageNumber}");

                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(new GenericResponse<IEnumerable<ProductListBO>>()
                {
                    IsSuccess = false,
                    Message = $"Error: {ex.Message} :: Inner: {ex.InnerException?.Message}",
                    IsAuthorized = !ex.Message.Contains("unauthorized", StringComparison.OrdinalIgnoreCase)
                });
            }
        }
        #endregion

        #region Get Featured Categories and their Products for Home Page
        [HttpPost]
        public async Task<IActionResult> GetFeaturedCategoryProducts([FromBody] GetProductModel model)
        {
            try
            {
                var response = await _http.GetFromJsonAsync<GenericResponse<IEnumerable<ProductListBO>>>(_webSettings.ApiUrl + $"Home/GetFeaturedCategoryProducts?CompanyId={model.CompanyId}&PageSize={model.PageSize}&PageNumber={model.PageNumber}");

                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(new GenericResponse<IEnumerable<ProductListBO>>()
                {
                    IsSuccess = false,
                    Message = $"Error: {ex.Message} :: Inner: {ex.InnerException?.Message}",
                    IsAuthorized = !ex.Message.Contains("unauthorized", StringComparison.OrdinalIgnoreCase)
                });
            }
        }
        #endregion

        #region Get All Categories and their product count for Home Page
        [HttpGet]
        public async Task<IActionResult> GetAllCategories([FromQuery] long companyId)
        {
            try
            {
                var response = await _http.GetFromJsonAsync<GenericResponse<IEnumerable<CategoryListBO>>>(_webSettings.ApiUrl + $"Home/GetAllCategories?companyId={companyId}");

                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(new GenericResponse<IEnumerable<CategoryListBO>>()
                {
                    IsSuccess = false,
                    Message = $"Error: {ex.Message} :: Inner: {ex.InnerException?.Message}",
                    IsAuthorized = !ex.Message.Contains("unauthorized", StringComparison.OrdinalIgnoreCase)
                });
            }
        } 
        #endregion
    }
}
