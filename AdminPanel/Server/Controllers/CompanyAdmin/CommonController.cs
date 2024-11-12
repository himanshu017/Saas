using AdminPanel.Services.ServicesLayer;
using AdminPanel.Shared.BO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AdminPanel.Server.Controllers.CompanyAdmin
{
    [Route("[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class CommonController : ControllerBase
    {
        private readonly ICommonService _service;

        public CommonController(ICommonService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetAllAddressTypes()
        {
            return Ok(_service.GetAddressTypes());
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
        public IActionResult GetAllCustomers([FromQuery] PaginatedResultBO model)
        {
            return Ok(_service.GetAllCustomers(model));
        }

        [HttpGet]
        public IActionResult GetAllProducts([FromQuery] PaginatedResultBO model)
        {
            return Ok(_service.GetAllProducts(model));
        }

        [HttpGet]
        public IActionResult GetCutoffTypes()
        {
            return Ok(_service.GetCutoffTypes());
        }

        [HttpGet]
        public IActionResult GetTimeZones()
        {
            return Ok(_service.GetTimeZones());
        }

        #region Global texts section
        [HttpGet]
        public async Task<IActionResult> GetCompanyGlobaltexts([FromQuery] int companyId)
        {
            return Ok(await _service.GetCompanyGlobaltexts(companyId));
        }

        [HttpPost]
        public async Task<IActionResult> AddUpdateCompanyGlobaltexts([FromBody] CompanyGlobalTextBO model)
        {
            return Ok(await _service.AddUpdateCompanyGlobaltexts(model));
        } 
        #endregion

        #region Posted links section
        [HttpGet]
        public IActionResult GetCompanyPostedLinks([FromQuery] int companyId)
        {
            return Ok(_service.GetCompanyPostedLinks(companyId));
        }

        [HttpPost]
        public async Task<IActionResult> AddUpdateCompanyPostedLinks([FromBody] PostedLinksBO model)
        {
            return Ok(await _service.AddUpdateCompanyPostedLinks(model));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCompanyPostedLinks([FromBody] long id)
        {
            return Ok(await _service.DeleteCompanyPostedLinks(id));
        } 
        #endregion

        #region Slider section

        [HttpGet]
        public IActionResult GetCompanySliders([FromQuery] int companyId, string d)
        {
            return Ok(_service.GetCompanySliders(companyId, d));
        }

        [HttpPost]
        public async Task<IActionResult> AddUpdateCompanysliders([FromBody] CompanySlidersBO model)
        {
            return Ok(await _service.AddUpdateCompanySliders(model));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCompanySliders([FromBody] long id)
        {
            return Ok(await _service.DeleteCompanySlider(id));
        } 
        #endregion

        #region Delivery run section
        [HttpGet]
        public IActionResult GetDeliveryRuns([FromQuery] int companyId)
        {
            return Ok(_service.GetDeliveryRuns(companyId));
        }

        [HttpPost]
        public async Task<IActionResult> AddUpdateDeliveryRun([FromBody] DeliveryRunBO model)
        {
            return Ok(await _service.AddUpdateDeliveryRun(model));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteDeliveryRun([FromBody] long id)
        {
            return Ok(await _service.DeleteDeliveryRun(id));
        }
        #endregion

        #region Discount section
        [HttpGet]
        public IActionResult GetDiscountType()
        {
            return Ok(_service.GetDiscountType());
        }

        [HttpGet]
        public IActionResult GetDiscountLimitationType()
        {
            return Ok(_service.GetDiscountLimitationType());
        }
        #endregion
    }
}
