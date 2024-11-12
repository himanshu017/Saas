using AdminPanel.Shared;
using AdminPanel.Shared.BO.WebApp;
using AdminPanel.Shared.BO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Security.Claims;
using EZOrders.InvoiceGen;

namespace AdminPanel.Web.Server.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly HttpClient _http;
        private readonly WebSettings _webSettings;

        public OrderController(HttpClient http, IOptions<WebSettings> webSettings)
        {
            _http = http;
            _webSettings = webSettings.Value;
        }

        #region Add Items to Temp Cart
        [HttpPost]
        public async Task<IActionResult> AddItemsToTempCart([FromBody] TempCartBO model)
        {
            try
            {
                var result = new GenericResponse<CartCountBO>();

                _http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", User?.FindFirstValue("Token"));
                var response = await _http.PostAsJsonAsync(_webSettings.ApiUrl + $"api/Order/AddItemsToTempCart", model);

                if (response.IsSuccessStatusCode)
                {
                    result = JsonConvert.DeserializeObject<GenericResponse<CartCountBO>>(await response.Content.ReadAsStringAsync());
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(new GenericResponse<CartCountBO>()
                {
                    IsSuccess = false,
                    Message = $"Error: {ex.Message} :: Inner: {ex.InnerException?.Message}",
                    IsAuthorized = !ex.Message.Contains("unauthorized", StringComparison.OrdinalIgnoreCase)
                });
            }
        }
        #endregion

        #region Get cart count and other details
        [HttpPost]
        public async Task<IActionResult> GetCartCount([FromBody] GetCartInfoBO model)
        {
            try
            {
                var result = new GenericResponse<CartCountBO>();

                _http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", User?.FindFirstValue("Token"));
                var response = await _http.PostAsJsonAsync(_webSettings.ApiUrl + $"api/Order/GetCartCount", model);

                if (response.IsSuccessStatusCode)
                {
                    result = JsonConvert.DeserializeObject<GenericPagedResponse<CartCountBO>>(await response.Content.ReadAsStringAsync());
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(new GenericResponse<CartCountBO>()
                {
                    IsSuccess = false,
                    Message = $"Error: {ex.Message} :: Inner: {ex.InnerException?.Message}",
                    IsAuthorized = !ex.Message.Contains("unauthorized", StringComparison.OrdinalIgnoreCase)
                });
            }
        }
        #endregion

        #region Delete Items from Cart
        [HttpPost]
        public async Task<IActionResult> DeleteCartItems([FromBody] DeleteCartItemsBO model)
        {
            try
            {
                var result = new GenericResponse<CartCountBO>();

                _http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", User?.FindFirstValue("Token"));
                var response = await _http.PostAsJsonAsync(_webSettings.ApiUrl + $"api/Order/DeleteCartItems", model);

                if (response.IsSuccessStatusCode)
                {
                    result = JsonConvert.DeserializeObject<GenericResponse<CartCountBO>>(await response.Content.ReadAsStringAsync());
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(new GenericResponse<CartCountBO>()
                {
                    IsSuccess = false,
                    Message = $"Error: {ex.Message} :: Inner: {ex.InnerException?.Message}",
                    IsAuthorized = !ex.Message.Contains("unauthorized", StringComparison.OrdinalIgnoreCase)
                });
            }
        }
        #endregion

        #region Get delivery dates based on cutoff
        [HttpPost]
        public async Task<IActionResult> GetDeliveryDates([FromBody] GetDeliveryDates model)
        {
            try
            {
                var result = new GenericResponse<DeliveryDatesBO>();

                _http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", User?.FindFirstValue("Token"));
                var response = await _http.PostAsJsonAsync(_webSettings.ApiUrl + $"api/Order/GetDeliveryDates", model);

                if (response.IsSuccessStatusCode)
                {
                    result = JsonConvert.DeserializeObject<GenericResponse<DeliveryDatesBO>>(await response.Content.ReadAsStringAsync());
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(new GenericResponse<DeliveryDatesBO>()
                {
                    IsSuccess = false,
                    Message = $"Error: {ex.Message} :: Inner: {ex.InnerException?.Message}",
                    IsAuthorized = !ex.Message.Contains("unauthorized", StringComparison.OrdinalIgnoreCase)
                });
            }
        }
        #endregion

        #region Get Comments by type
        [HttpGet]
        public async Task<IActionResult> GetCommentsByType([FromQuery] GetCommentsBO model)
        {
            try
            {
                _http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", User?.FindFirstValue("Token"));
                var response = await _http.GetFromJsonAsync<GenericResponse<IEnumerable<CommentsBO>>>(_webSettings.ApiUrl + $"api/Order/GetCommentsByType{Extensions.ConvertToQueryString(model)}");

                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(new GenericResponse<IEnumerable<CommentsBO>>()
                {
                    IsSuccess = false,
                    Message = $"Error: {ex.Message} :: Inner: {ex.InnerException?.Message}",
                    IsAuthorized = !ex.Message.Contains("unauthorized", StringComparison.OrdinalIgnoreCase)
                });
            }
        }
        #endregion

        #region Add Comment by Type
        [HttpPost]
        public async Task<IActionResult> AddCommentByType([FromBody] CommentsBO model)
        {
            try
            {
                var result = new GenericResponse<CommentsBO>();

                _http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", User?.FindFirstValue("Token"));
                var response = await _http.PostAsJsonAsync(_webSettings.ApiUrl + $"api/Order/AddCommentByType", model);

                if (response.IsSuccessStatusCode)
                {
                    result = JsonConvert.DeserializeObject<GenericResponse<CommentsBO>>(await response.Content.ReadAsStringAsync());
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(new GenericResponse<CommentsBO>()
                {
                    IsSuccess = false,
                    Message = $"Error: {ex.Message} :: Inner: {ex.InnerException?.Message}",
                    IsAuthorized = !ex.Message.Contains("unauthorized", StringComparison.OrdinalIgnoreCase)
                });
            }
        }
        #endregion

        #region Place order 
        [HttpPost]
        public async Task<IActionResult> PlaceOrder([FromBody] OrderBO model)
        {
            try
            {
                var result = new GenericResponse<long>();

                _http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", User?.FindFirstValue("Token"));
                var response = await _http.PostAsJsonAsync(_webSettings.ApiUrl + $"api/Order/PlaceOrder", model);

                if (response.IsSuccessStatusCode)
                {
                    result = JsonConvert.DeserializeObject<GenericResponse<long>>(await response.Content.ReadAsStringAsync());
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(new GenericResponse<long>()
                {
                    IsSuccess = false,
                    Message = $"Error: {ex.Message} :: Inner: {ex.InnerException?.Message}",
                    IsAuthorized = !ex.Message.Contains("unauthorized", StringComparison.OrdinalIgnoreCase)
                });
            }
        }
        #endregion

        #region Get Order Details By OrderId
        [HttpGet]
        public async Task<IActionResult> GetOrderDetails([FromQuery] long orderId)
        {
            try
            {
                _http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", User?.FindFirstValue("Token"));
                var response = await _http.GetFromJsonAsync<GenericResponse<OrderDetailsBO>>(_webSettings.ApiUrl + $"api/Order/GetOrderDetails?orderId={orderId}");

                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(new GenericResponse<OrderDetailsBO>()
                {
                    IsSuccess = false,
                    Message = $"Error: {ex.Message} :: Inner: {ex.InnerException?.Message}",
                    IsAuthorized = !ex.Message.Contains("unauthorized", StringComparison.OrdinalIgnoreCase)
                });
            }
        }
        #endregion

        #region Get Invoice PDF
        [HttpGet]
        public async Task<FileStreamResult> GetInvoicePdf([FromQuery] long orderId)
        {
            try
            {
                _http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", User?.FindFirstValue("Token"));
                var response = await _http.GetFromJsonAsync<GenericResponse<OrderDetailsBO>>(_webSettings.ApiUrl + $"api/Order/GetOrderDetails?orderId={orderId}");
                var invoiceModel = new InvoiceGenBO();
                if (response.IsSuccess)
                {
                    var orderModel = response.Data;
                    invoiceModel.OrderModel = orderModel;

                    AddDictionaryValues(invoiceModel, orderModel);
                }

                var ms = new MemoryStream(InvoiceGenerator.GeneratePDFDefault(invoiceModel));
                return new FileStreamResult(ms, "application/pdf");
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void AddDictionaryValues(InvoiceGenBO invoiceModel, OrderDetailsBO? orderModel)
        {
            var invoiceInfo = new Dictionary<string, string>();
            invoiceInfo.Add("Order Date:", $"{orderModel.CreatedOn:dd/MM/yyyy}");
            invoiceInfo.Add("Delivery Date:", $"{orderModel.DeliveryDate:dd/MM/yyyy}");
            invoiceInfo.Add("PO #:", $"{orderModel.PONumber}");
            invoiceInfo.Add("Order Total:", orderModel.OrderTotal.ToPrice(orderModel.CompanyBO.CurrencyInfo));

            var allCharges = new Dictionary<string, string>();
            allCharges.Add("Sub Total:", orderModel.TotalWithoutTax.ToPrice(orderModel.CompanyBO.CurrencyInfo));

            if (orderModel.DiscountAmount > 0)
                allCharges.Add("Discount:", $"-{orderModel.DiscountAmount.ToPrice(orderModel.CompanyBO.CurrencyInfo)}");

            if (orderModel.HasFreightCharges)
                allCharges.Add("Freight:", orderModel.FreightCharge.ToPrice(orderModel.CompanyBO.CurrencyInfo));

            if (orderModel.HasNonDeliveryDayCharges)
                allCharges.Add("Non-Delivery Day:", orderModel.NonDeliveryDayCharge.ToPrice(orderModel.CompanyBO.CurrencyInfo));

            allCharges.Add("Taxes:", orderModel.TotalTax.ToPrice(orderModel.CompanyBO.CurrencyInfo));
            allCharges.Add("Total:", orderModel.OrderTotal.ToPrice(orderModel.CompanyBO.CurrencyInfo));

            invoiceModel.InvoiceInfo = invoiceInfo;
            invoiceModel.AllCharges = allCharges;

            invoiceModel.CompanyName = orderModel.CompanyBO.CompanyName;
            invoiceModel.CompanyEmail = orderModel.CompanyBO.Email;
            invoiceModel.CompanyPhone = orderModel.CompanyBO.Phone;
            invoiceModel.PrimaryColor = orderModel.CompanyBO.PrimaryColor;
            invoiceModel.SecondaryColor = orderModel.CompanyBO.SecondaryColor;
            invoiceModel.LogoPath = $"{_webSettings.ImagePath}CompanyLogo/{orderModel.CompanyBO.DomainName}/{orderModel.CompanyBO.Logo}";
        }
        #endregion

        #region Get Order history
        [HttpPost]
        public async Task<IActionResult> GetOrderHistory([FromBody] GetOrderHistoryBO model)
        {
            try
            {
                var result = new GenericResponse<IEnumerable<OrderDetailsBO>>();
                _http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", User?.FindFirstValue("Token"));
                var response = await _http.PostAsJsonAsync(_webSettings.ApiUrl + $"api/Order/GetOrderHistory", model);

                if (response.IsSuccessStatusCode)
                {
                    result = JsonConvert.DeserializeObject<GenericResponse<IEnumerable<OrderDetailsBO>>>(await response.Content.ReadAsStringAsync());
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(new GenericResponse<IEnumerable<OrderDetailsBO>>()
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
