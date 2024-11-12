using AdminPanel.Services.ServicesLayer.CompanyAdmin;
using AdminPanel.Shared.BO;
using AdminPanel.Shared.BO.CompanyAdmin;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AdminPanel.Server.Controllers.CompanyAdmin
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class AttributeController : ControllerBase
    {
        private readonly IAttributeService _attributeService;

        public AttributeController(IAttributeService attributeService)
        {
            _attributeService = attributeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAttributes(long companyId, bool showAll = false)
        {
            return Ok(await _attributeService.GetAllAttributes(companyId, showAll));
        }

        [HttpPost]
        public async Task<BaseResponseBO> AddUpdateAttribute(AttributeBO model)
        {
            return await _attributeService.AddUpdateAttribute(model);
        }

        [HttpDelete]
        public async Task<BaseResponseBO> DeleteAttribute(int id)
        {
            return await _attributeService.DeleteAttribute(id);
        }

        [HttpGet]
        public IEnumerable<ProductAttrListingBO> GetProductAttributes(long productId)
        {
            return _attributeService.GetProductAttributes(productId);
        }

        [HttpPost]
        public async Task<BaseResponseBO> AddUpdateProductAttribute(ProductAttrMappingBO model)
        {
            return await _attributeService.AddUpdateProductAttribute(model);
        }

        [HttpPost]
        public async Task<BaseResponseBO> AddUpdateAttributeValue(AttributeValuesBO model)
        {
            return await _attributeService.AddUpdateAttributeValue(model);
        }

        [HttpGet]
        public async Task<ProductAttrMappingBO> GetAttributesValues(int attrId)
        {
            return await _attributeService.GetAttributesValues(attrId);
        }

        [HttpPost]
        public async Task<BaseResponseBO> ToggleAttrState(ToggleAttrBO model)
        {
            return await _attributeService.ToggleAttrState(model);
        }
    }
}
