using AdminPanel.Shared;
using AdminPanel.Shared.BO.WebApp;
using AdminPanel.Shared.BO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Security.Claims;

namespace AdminPanel.Web.Server.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class GeneralController : ControllerBase
    {
        private readonly HttpClient _http;
        private readonly WebSettings _webSettings;

        public GeneralController(HttpClient http, IOptions<WebSettings> webSettings)
        {
            _http = http;
            _webSettings = webSettings.Value;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCountries()
        {
            try
            {
                var result = new GenericResponse<IEnumerable<CommonDropdownBO>>();

                var response = await _http.GetFromJsonAsync<IEnumerable<CommonDropdownBO>>(_webSettings.ApiUrl + $"Common/GetAllCountries");

                result.Data = response;
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(new GenericResponse<IEnumerable<CommonDropdownBO>>()
                {
                    IsSuccess = false,
                    Message = $"Error: {ex.Message} :: Inner: {ex.InnerException?.Message}",
                    IsAuthorized = !ex.Message.Contains("unauthorized", StringComparison.OrdinalIgnoreCase)
                });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetStatesByCountry([FromQuery] int countryId)
        {
            try
            {
                var result = new GenericResponse<IEnumerable<CommonDropdownBO>>();
                var response = await _http.GetFromJsonAsync<IEnumerable<CommonDropdownBO>>(_webSettings.ApiUrl + $"Common/GetStatesByCountry?countryId={countryId}");

                result.Data = response;
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(new GenericResponse<IEnumerable<CommonDropdownBO>>()
                {
                    IsSuccess = false,
                    Message = $"Error: {ex.Message} :: Inner: {ex.InnerException?.Message}",
                    IsAuthorized = !ex.Message.Contains("unauthorized", StringComparison.OrdinalIgnoreCase)
                });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetCitiesByState([FromQuery] int stateId)
        {
            try
            {
                var result = new GenericResponse<IEnumerable<CommonDropdownBO>>();
                var response = await _http.GetFromJsonAsync<IEnumerable<CommonDropdownBO>>(_webSettings.ApiUrl + $"Common/GetCitiesByState?stateId={stateId}");

                result.Data = response;
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(new GenericResponse<IEnumerable<CommonDropdownBO>>()
                {
                    IsSuccess = false,
                    Message = $"Error: {ex.Message} :: Inner: {ex.InnerException?.Message}",
                    IsAuthorized = !ex.Message.Contains("unauthorized", StringComparison.OrdinalIgnoreCase)
                });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetCustomerAddresses([FromQuery] long customerId)
        {
            try
            {
                var result = new GenericResponse<IEnumerable<CustomerAddressBO>>();
                var response = await _http.GetFromJsonAsync<IEnumerable<CustomerAddressBO>>(_webSettings.ApiUrl + $"Common/GetCustomerAddresses?customerId={customerId}");

                result.Data = response;
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(new GenericResponse<IEnumerable<CustomerAddressBO>>()
                {
                    IsSuccess = false,
                    Message = $"Error: {ex.Message} :: Inner: {ex.InnerException?.Message}",
                    IsAuthorized = !ex.Message.Contains("unauthorized", StringComparison.OrdinalIgnoreCase)
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddEditCustomerAddress([FromBody] CustomerAddressBO model)
        {
            try
            {
                var result = new GenericResponse<BaseResponseBO>();

                _http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", User?.FindFirstValue("Token"));
                var response = await _http.PostAsJsonAsync(_webSettings.ApiUrl + $"Common/AddEditCustomerAddress", model);

                if (response.IsSuccessStatusCode)
                {
                    result.Data = JsonConvert.DeserializeObject<BaseResponseBO>(await response.Content.ReadAsStringAsync());
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(new GenericResponse<BaseResponseBO>()
                {
                    IsSuccess = false,
                    Message = $"Error: {ex.Message} :: Inner: {ex.InnerException?.Message}",
                    IsAuthorized = !ex.Message.Contains("unauthorized", StringComparison.OrdinalIgnoreCase)
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCustomerAddress([FromBody] DeleteCustomerAddressBO model)
        {
            try
            {
                var result = new GenericResponse<BaseResponseBO>();

                _http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", User?.FindFirstValue("Token"));
                var response = await _http.PostAsJsonAsync(_webSettings.ApiUrl + $"Common/DeleteCustomerAddress", model);

                if (response.IsSuccessStatusCode)
                {
                    result.Data = JsonConvert.DeserializeObject<BaseResponseBO>(await response.Content.ReadAsStringAsync());
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(new GenericResponse<BaseResponseBO>()
                {
                    IsSuccess = false,
                    Message = $"Error: {ex.Message} :: Inner: {ex.InnerException?.Message}",
                    IsAuthorized = !ex.Message.Contains("unauthorized", StringComparison.OrdinalIgnoreCase)
                });
            }
        }
    }
}
