using AdminPanel.DataModel.Models;
using AdminPanel.Services.ServicesLayer;
using AdminPanel.Shared.BO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AdminPanel.API.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class CommonController : ControllerBase
    {
        private readonly ILogger<CommonController> _logger;
        private readonly ICommonService _service;
        private readonly ICustomerService _customer;
        public CommonController(ICommonService service, 
                                ILogger<CommonController> logger,
                                ICustomerService customer)
        {
            _service = service;
            _logger = logger;
            _customer = customer;
        }

        [HttpGet]
        public IActionResult GetAllCountries()
        {
            return Ok(_service.GetAllCountries());
        }

        [HttpGet]
        public IActionResult GetStatesByCountry([FromQuery] int countryId)
        {
            return Ok(_service.GetAllStatesByCountry(countryId));
        }

        [HttpGet]
        public IActionResult GetCitiesByState([FromQuery] int stateId)
        {
            return Ok(_service.GetAllCitiesByState(stateId));
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

    }
}
