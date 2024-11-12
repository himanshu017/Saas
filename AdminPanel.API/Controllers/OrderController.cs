using AdminPanel.Services.ServicesLayer;
using AdminPanel.Shared.BO.WebApp;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AdminPanel.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly ILogger<OrderController> _logger;
        private readonly IOrderService _service;

        public OrderController(IOrderService service, ILogger<OrderController> logger)
        {
            _logger = logger;
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> AddItemsToTempCart(TempCartBO model)
        {
            return Ok(await _service.AddItemsToTempCart(model));
        }

        [HttpPost]
        public async Task<IActionResult> GetCartCount(GetCartInfoBO model)
        {
            return Ok(await _service.GetCartCount(model));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCartItems(DeleteCartItemsBO model)
        {
            return Ok(await _service.DeleteCartItems(model));
        }

        [HttpPost]
        public async Task<IActionResult> GetDeliveryDates(GetDeliveryDates model)
        {
            return Ok(await _service.GetDeliveryDates(model));
        }

        [HttpGet]
        public async Task<IActionResult> GetCommentsByType([FromQuery] GetCommentsBO model)
        {
            return Ok(await _service.GetCommentsByType(model));
        }

        [HttpPost]
        public async Task<IActionResult> AddCommentByType([FromBody] CommentsBO model)
        {
            return Ok(await _service.AddCommentByType(model));
        }

        [HttpPost]
        public async Task<IActionResult> PlaceOrder([FromBody] OrderBO model)
        {
            return Ok(await _service.PlaceOrder(model));
        }

        [HttpGet]
        public async Task<IActionResult> GetOrderDetails([FromQuery] long orderId)
        {
            return Ok(await _service.GetOrderDetails(orderId));
        }

        [HttpPost]
        public async Task<IActionResult> GetOrderHistory([FromBody] GetOrderHistoryBO model)
        {
            return Ok(await _service.GetOrderHistory(model));
        }
    }
}
