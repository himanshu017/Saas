using AdminPanel.Services.ServicesLayer.CompanyAdmin;
using AdminPanel.Services.ServicesLayer;
using AdminPanel.Shared.BO.CompanyAdmin;
using Microsoft.AspNetCore.Mvc;

namespace AdminPanel.Server.Controllers.CompanyAdmin
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class OrdersController : Controller
    {
        private readonly IOrderAdminService _ordService;
        private readonly ICategoriesService _catService;

        public OrdersController(IOrderAdminService ordService, ICategoriesService catService)
        {
            _ordService = ordService;
            _catService = catService;
        }

        [HttpGet]
        public IActionResult GetAllOrders([FromQuery] GetOrdersModel model)
        {
            var products = _ordService.GetAllOrders(model);

            return Ok(products);
        }

        [HttpGet]
        public async Task<IActionResult> GetOrderDetails([FromQuery] long orderId)
        {
            return Ok(await _ordService.GetOrderDetails(orderId));
        }

        [HttpPost]
        public async Task<IActionResult> UpdateOrderItems(ItemsBO model)
        {
            return Ok(await _ordService.UpdateOrderItems(model));
        }
    }
}
