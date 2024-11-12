using AdminPanel.Shared;
using AdminPanel.Shared.BO;
using AdminPanel.Shared.BO.CompanyAdmin;
using AdminPanel.Shared.BO.WebApp;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Security.Claims;

namespace AdminPanel.Web.Server.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly HttpClient _http;
        private readonly WebSettings _webSettings;
        public ProductController(HttpClient http, IOptions<WebSettings> webSettings)
        {
            _http = http;
            _webSettings = webSettings.Value;
        }

        #region Get All Active Main & Sub categories
        [HttpGet]
        public async Task<IActionResult> GetAllMainAndSubCategories([FromQuery] GetAllCategoriesBO model)
        {
            try
            {
                _http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", User?.FindFirstValue("Token"));
                var response = await _http.GetFromJsonAsync<GenericResponse<IEnumerable<CategoryListBO>>>(_webSettings.ApiUrl + $"api/Product/GetAllMainAndSubCategories{Extensions.ConvertToQueryString(model)}");

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

        #region Get All Active Filters
        [HttpGet]
        public async Task<IActionResult> GetAllFilters([FromQuery] long companyId)
        {
            try
            {
                _http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", User?.FindFirstValue("Token"));
                var response = await _http.GetFromJsonAsync<GenericResponse<IEnumerable<FiltersBO>>>(_webSettings.ApiUrl + $"api/Product/GetAllFilters?companyId={companyId}");
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(new GenericResponse<IEnumerable<FiltersBO>>()
                {
                    IsSuccess = false,
                    Message = $"Error: {ex.Message} :: Inner: {ex.InnerException?.Message}",
                    IsAuthorized = !ex.Message.Contains("unauthorized", StringComparison.OrdinalIgnoreCase)
                });
            }
        }
        #endregion

        #region Get All Active Units
        [HttpGet]
        public async Task<IActionResult> GetAllUnits([FromQuery] long companyId)
        {
            try
            {
                _http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", User?.FindFirstValue("Token"));
                var response = await _http.GetFromJsonAsync<GenericResponse<IEnumerable<UnitOfMeasureBO>>>(_webSettings.ApiUrl + $"api/Product/GetAllUnits?companyId={companyId}");
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(new GenericResponse<IEnumerable<UnitOfMeasureBO>>()
                {
                    IsSuccess = false,
                    Message = $"Error: {ex.Message} :: Inner: {ex.InnerException?.Message}",
                    IsAuthorized = !ex.Message.Contains("unauthorized", StringComparison.OrdinalIgnoreCase)
                });
            }
        }
        #endregion

        #region Get Products based on Search filters
        [HttpPost]
        public async Task<IActionResult> GetProductsBySearchFilters([FromBody] GetProductListBO model)
        {
            try
            {
                var result = new GenericPagedResponse<IEnumerable<ProductListBO>>();

                _http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", User?.FindFirstValue("Token"));
                var response = await _http.PostAsJsonAsync(_webSettings.ApiUrl + $"api/Product/GetProductsBySearchFilters",model);

                if (response.IsSuccessStatusCode)
                {
                    result = JsonConvert.DeserializeObject<GenericPagedResponse<IEnumerable<ProductListBO>>>(await response.Content.ReadAsStringAsync());
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(new GenericPagedResponse<IEnumerable<ProductListBO>>()
                {
                    IsSuccess = false,
                    Message = $"Error: {ex.Message} :: Inner: {ex.InnerException?.Message}",
                    IsAuthorized = !ex.Message.Contains("unauthorized", StringComparison.OrdinalIgnoreCase)
                });
            }
        }
        #endregion

        #region Get Fav lists for customer
        [HttpGet]
        public async Task<IActionResult> GetFavoriteLists([FromQuery] GetFavLists model)
        {
            try
            {
                var result = new GenericResponse<IEnumerable<ProductListBO>>();

                _http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", User?.FindFirstValue("Token"));
                var response = await _http.GetFromJsonAsync<GenericResponse<IEnumerable<FavoriteListBO>>>(_webSettings.ApiUrl + $"api/Product/GetFavoriteLists{Extensions.ConvertToQueryString(model)}");

                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(new GenericResponse<IEnumerable<FavoriteListBO>>()
                {
                    IsSuccess = false,
                    Message = $"Error: {ex.Message} :: Inner: {ex.InnerException?.Message}",
                    IsAuthorized = !ex.Message.Contains("unauthorized", StringComparison.OrdinalIgnoreCase)
                });
            }
        }
        #endregion

        #region Add Update favorite list
        [HttpPost]
        public async Task<IActionResult> AddUpdateFavList([FromBody] FavoriteListBO model)
        {
            try
            {
                var result = new GenericResponse<bool>();

                _http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", User?.FindFirstValue("Token"));
                var response = await _http.PostAsJsonAsync(_webSettings.ApiUrl + $"api/Product/AddUpdateFavList", model);

                if (response.IsSuccessStatusCode)
                {
                    result = JsonConvert.DeserializeObject<GenericPagedResponse<bool>>(await response.Content.ReadAsStringAsync());
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(new GenericResponse<bool>()
                {
                    IsSuccess = false,
                    Message = $"Error: {ex.Message} :: Inner: {ex.InnerException?.Message}",
                    IsAuthorized = !ex.Message.Contains("unauthorized", StringComparison.OrdinalIgnoreCase)
                });
            }
        }
        #endregion

        #region Delete Favorite list
        [HttpPost]
        public async Task<IActionResult> DeleteFavList([FromBody] long id)
        {
            try
            {
                var result = new GenericResponse<bool>();

                _http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", User?.FindFirstValue("Token"));
                var response = await _http.PostAsJsonAsync(_webSettings.ApiUrl + $"api/Product/DeleteFavList", id);

                if (response.IsSuccessStatusCode)
                {
                    result = JsonConvert.DeserializeObject<GenericPagedResponse<bool>>(await response.Content.ReadAsStringAsync());
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(new GenericResponse<bool>()
                {
                    IsSuccess = false,
                    Message = $"Error: {ex.Message} :: Inner: {ex.InnerException?.Message}",
                    IsAuthorized = !ex.Message.Contains("unauthorized", StringComparison.OrdinalIgnoreCase)
                });
            }
        }
        #endregion

        #region Get Products for selected favorite list
        [HttpPost]
        public async Task<IActionResult> GetFavoriteListItems([FromBody] GetProductListBO model)
        {
            try
            {
                var result = new GenericPagedResponse<IEnumerable<ProductListBO>>();

                _http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", User?.FindFirstValue("Token"));
                var response = await _http.PostAsJsonAsync(_webSettings.ApiUrl + $"api/Product/GetFavoriteListItems", model);

                if (response.IsSuccessStatusCode)
                {
                    result = JsonConvert.DeserializeObject<GenericPagedResponse<IEnumerable<ProductListBO>>>(await response.Content.ReadAsStringAsync());
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(new GenericPagedResponse<IEnumerable<ProductListBO>>()
                {
                    IsSuccess = false,
                    Message = $"Error: {ex.Message} :: Inner: {ex.InnerException?.Message}",
                    IsAuthorized = !ex.Message.Contains("unauthorized", StringComparison.OrdinalIgnoreCase)
                });
            }
        }
        #endregion

        #region Add Delete items from Favorite list
        [HttpPost]
        public async Task<IActionResult> AddDeleteFavlistItem([FromBody] ManageFavListItemBO model)
        {
            try
            {
                var result = new GenericResponse<bool>();

                _http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", User?.FindFirstValue("Token"));
                var response = await _http.PostAsJsonAsync(_webSettings.ApiUrl + $"api/Product/AddDeleteFavlistItem", model);

                if (response.IsSuccessStatusCode)
                {
                    result = JsonConvert.DeserializeObject<GenericResponse<bool>>(await response.Content.ReadAsStringAsync());
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(new GenericResponse<bool>()
                {
                    IsSuccess = false,
                    Message = $"Error: {ex.Message} :: Inner: {ex.InnerException?.Message}",
                    IsAuthorized = !ex.Message.Contains("unauthorized", StringComparison.OrdinalIgnoreCase)
                });
            }
        }
        #endregion

        #region Get Company Specials data
        [HttpPost]
        public async Task<IActionResult> GetCompanySpecials([FromBody] GetProductListBO model)
        {
            try
            {
                var result = new GenericPagedResponse<IEnumerable<ProductListBO>>();

                _http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", User?.FindFirstValue("Token"));
                var response = await _http.PostAsJsonAsync(_webSettings.ApiUrl + $"api/Product/GetCompanySpecials", model);

                if (response.IsSuccessStatusCode)
                {
                    result = JsonConvert.DeserializeObject<GenericPagedResponse<IEnumerable<ProductListBO>>>(await response.Content.ReadAsStringAsync());
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(new GenericPagedResponse<IEnumerable<ProductListBO>>()
                {
                    IsSuccess = false,
                    Message = $"Error: {ex.Message} :: Inner: {ex.InnerException?.Message}",
                    IsAuthorized = !ex.Message.Contains("unauthorized", StringComparison.OrdinalIgnoreCase)
                });
            }
        }
        #endregion

        #region Get Temp cart items
        [HttpPost]
        public async Task<IActionResult> GetTempCartItems([FromBody] GetProductListBO model)
        {
            try
            {
                var result = new GenericResponse<IEnumerable<ProductListBO>>();

                _http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", User?.FindFirstValue("Token"));
                var response = await _http.PostAsJsonAsync(_webSettings.ApiUrl + $"api/Product/GetTempCartItems", model);

                if (response.IsSuccessStatusCode)
                {
                    result = JsonConvert.DeserializeObject<GenericResponse<IEnumerable<ProductListBO>>>(await response.Content.ReadAsStringAsync());
                }

                return Ok(result);
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

        #region Get Product Detail
        [HttpPost]
        public async Task<IActionResult> GetProductDetail([FromBody] GetProdDetailBO model)
        {
            try
            {
                var result = new GenericResponse<ProductListBO>();

                _http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", User?.FindFirstValue("Token"));
                var response = await _http.PostAsJsonAsync(_webSettings.ApiUrl + $"api/Product/GetProductDetail", model);

                if (response.IsSuccessStatusCode)
                {
                    result = JsonConvert.DeserializeObject<GenericResponse<ProductListBO>>(await response.Content.ReadAsStringAsync());
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(new GenericResponse<ProductListBO>()
                {
                    IsSuccess = false,
                    Message = $"Error: {ex.Message} :: Inner: {ex.InnerException?.Message}",
                    IsAuthorized = !ex.Message.Contains("unauthorized", StringComparison.OrdinalIgnoreCase)
                });
            }
        }
        #endregion

        #region Get Suggestive items for a product
        [HttpPost]
        public async Task<IActionResult> GetSuggestiveProducts([FromBody] GetProductListBO model)
        {
            try
            {
                var result = new GenericResponse<List<ProductListBO>>();

                _http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", User?.FindFirstValue("Token"));
                var response = await _http.PostAsJsonAsync(_webSettings.ApiUrl + $"api/Product/GetSuggestiveProducts", model);

                if (response.IsSuccessStatusCode)
                {
                    result = JsonConvert.DeserializeObject<GenericResponse<List<ProductListBO>>>(await response.Content.ReadAsStringAsync());
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(new GenericResponse<List<ProductListBO>>()
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
