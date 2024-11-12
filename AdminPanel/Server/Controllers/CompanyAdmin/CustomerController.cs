using AdminPanel.Services.ServicesLayer;
using AdminPanel.Shared.BO;
using AdminPanel.Shared.BO.CompanyAdmin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AdminPanel.Server.Controllers.CompanyAdmin
{
    [Route("[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customer;

        public CustomerController(ICustomerService customerService)
        {
            _customer = customerService;
        }

        [HttpGet]
        public IActionResult GetAllCustomers([FromQuery] GetCustomerModel model)
        {
            var customers = _customer.GetAllCustomers(model);
            return Ok(customers);
        }

        [HttpPost]
        public async Task<IActionResult> AddEditCustomer([FromBody] CustomerBO model)
        {
            return Ok(await _customer.AddEditCustomer(model));
        }

        [HttpGet]
        public async Task<IActionResult> GetCustomerUsers([FromQuery] long customerId)
        {
            return Ok(await _customer.GetCustomerUsers(customerId));
        }

        [HttpGet]
        public async Task<IActionResult> GetUserFeaturesByUserId([FromQuery] long userId)
        {
            return Ok(await _customer.GetUserFeaturesByUserId(userId));
        }

        [HttpPost]
        public async Task<IActionResult> SaveUserFeatures([FromBody] UserFeaturesBO model)
        {
            return Ok(await _customer.SaveUserFeatures(model));
        }

        [HttpPost]
        public async Task<IActionResult> AddEditCustomerUser([FromBody] CustomerUserBO model)
        {
            return Ok(await _customer.AddEditCustomerUser(model));
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUserPassword([FromBody] CustomerUserResetPassword model)
        {
            return Ok(await _customer.UpdateUserPassword(model));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCustomerUser([FromBody] long userId)
        {
            return Ok(await _customer.DeleteCustomerUser(userId));
        }

        [HttpGet]
        public async Task<IActionResult> GetCustomerAddresses([FromQuery] long customerId)
        {
            return Ok(await _customer.GetCustomerAddresses(customerId));
        }

        [HttpPost]
        public async Task<IActionResult> AddEditCustomerAddress([FromBody] CustomerAddressBO model)
        {
            return Ok(await _customer.AddEditCustomerAddress(model));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCustomerAddress([FromBody] DeleteCustomerAddressBO model)
        {
            return Ok(await _customer.DeleteCustomerAddress(model));
        }

        [HttpGet]
        public IActionResult GetAllSalesreps([FromQuery] long companyId)
        {
            return Ok(_customer.GetAllSalesreps(companyId));
        }

        [HttpPost]
        public async Task<IActionResult> AddEditSalesrep([FromBody] SalesrepBO model)
        {
            return Ok(await _customer.AddEditSalesrep(model));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteSalesrep([FromBody] long userId)
        {
            return Ok(await _customer.DeleteSalesrep(userId));
        }

        [HttpGet]
        public IActionResult GetAllSalesrepCode([FromQuery] long companyId)
        {
            return Ok(_customer.AllSalesmanCodes(companyId));
        }

        [HttpPost]
        public async Task<IActionResult> UpdateBitField([FromBody] UpdateBitBO model)
        {
            return Ok(await _customer.UpdateBitField(model));
        }
    }
}
