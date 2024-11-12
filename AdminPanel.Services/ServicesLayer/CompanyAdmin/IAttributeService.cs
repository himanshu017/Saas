using AdminPanel.DataModel.Models;
using AdminPanel.Services.Repository;
using AdminPanel.Shared;
using AdminPanel.Shared.BO;
using AdminPanel.Shared.BO.CompanyAdmin;
using AutoMapper;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanel.Services.ServicesLayer.CompanyAdmin
{
    public interface IAttributeService
    {
        Task<IEnumerable<AttributeBO>> GetAllAttributes(long companyId, bool showAll = false);
        Task<BaseResponseBO> AddUpdateAttribute(AttributeBO model);
        Task<BaseResponseBO> DeleteAttribute(int id);
        IEnumerable<ProductAttrListingBO> GetProductAttributes(long productId);
        Task<ProductAttrMappingBO> GetAttributesValues(int mappingId);
        Task<BaseResponseBO> AddUpdateProductAttribute(ProductAttrMappingBO model);
        Task<BaseResponseBO> AddUpdateAttributeValue(AttributeValuesBO model);
        Task<BaseResponseBO> ToggleAttrState(ToggleAttrBO model);
    }

    public class AttributeService : IAttributeService
    {
        private readonly IGenericRepository<ProductAttribute> _attribute;
        private readonly IGenericRepository<ProductAttributeMapping> _attributeMapping;
        private readonly IGenericRepository<PredefinedProductAttributeValue> _preDefinedAttr;
        private readonly IGenericRepository<ProductAttributeValue> _prodAttrValues;
        private readonly IMapper _mapper;

        public AttributeService(IMapper mapper,
                                IGenericRepository<ProductAttribute> attribute,
                                IGenericRepository<PredefinedProductAttributeValue> preDefinedAttr,
                                IGenericRepository<ProductAttributeMapping> attributeMapping,
                                IGenericRepository<ProductAttributeValue> prodAttrValues
                                )
        {
            _attribute = attribute;
            _mapper = mapper;
            _preDefinedAttr = preDefinedAttr;
            _attributeMapping = attributeMapping;
            _prodAttrValues = prodAttrValues;
        }


        public async Task<IEnumerable<AttributeBO>> GetAllAttributes(long companyId, bool showAll = false)
        {
            try
            {
                var attributes = _attribute.GetWithConditions(x => x.CompanyId == companyId 
                                                                && (!showAll ? x.IsActive == true : true), "PredefinedProductAttributeValues", true)
                                           .ToList();

                return _mapper.Map<IEnumerable<AttributeBO>>(attributes);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<BaseResponseBO> AddUpdateAttribute(AttributeBO model)
        {
            try
            {
                var attribute = new ProductAttribute();
                if (model.Id > 0)
                {
                    attribute = await _attribute.GetSingleWithConditions(x => x.Id == model.Id, "PredefinedProductAttributeValues");

                    _mapper.Map(model, attribute);

                    await _attribute.UpdateAsync(attribute);
                }
                else
                {
                    _mapper.Map(model, attribute);
                    await _attribute.AddAsync(attribute);
                }

                return new BaseResponseBO();

            }
            catch (Exception ex)
            {
                return new BaseResponseBO()
                {
                    IsSuccess = false,
                    Message = $"{ex.Message} :: Inner Exception:: {ex.InnerException?.Message}"
                };
            }
        }

        public async Task<BaseResponseBO> DeleteAttribute(int id)
        {
            try
            {
                var attribute = await _attribute.GetSingleWithConditions(x => x.Id == id);

                attribute.IsActive = false;

                // do a soft delete instead of removing the db entry.
                await _attribute.UpdateAsync(attribute);

                return new BaseResponseBO();
            }
            catch (Exception ex)
            {
                return new BaseResponseBO()
                {
                    IsSuccess = false,
                    Message = $"{ex.Message} :: Inner Exception:: {ex.InnerException?.Message}"
                };
            }
        }

        public async Task<BaseResponseBO> AddUpdateProductAttribute(ProductAttrMappingBO model)
        {
            try
            {
                var prodAttrMapping = new ProductAttributeMapping();
                if (model.Id == 0)
                {
                    _mapper.Map(model, prodAttrMapping);

                    await _attributeMapping.AddAsync(prodAttrMapping);
                }
                else
                {
                    prodAttrMapping = await _attributeMapping.Get(model.Id);
                    _mapper.Map(model, prodAttrMapping);

                    await _attributeMapping.UpdateAsync(prodAttrMapping);
                }

                return new BaseResponseBO()
                {
                    Data = prodAttrMapping.Id
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseBO()
                {
                    IsSuccess = false,
                    Message = $"{ex.Message} :: Inner Exception:: {ex.InnerException?.Message}"
                };
            }
        }

        public async Task<BaseResponseBO> AddUpdateAttributeValue(AttributeValuesBO model)
        {
            try
            {
                var attrValue = new ProductAttributeValue();
                if (model.Id == 0)
                {
                    _mapper.Map(model, attrValue);

                    await _prodAttrValues.AddAsync(attrValue);
                }
                else
                {
                    attrValue = await _prodAttrValues.Get(model.Id);
                    _mapper.Map(model, attrValue);

                    await _prodAttrValues.UpdateAsync(attrValue);
                }

                return new BaseResponseBO();
            }
            catch (Exception ex)
            {
                return new BaseResponseBO()
                {
                    IsSuccess = false,
                    Message = $"{ex.Message} :: Inner Exception:: {ex.InnerException?.Message}"
                };
            }
        }

        public IEnumerable<ProductAttrListingBO> GetProductAttributes(long productId)
        {
            try
            {
                var prodAttrMappings = _attributeMapping.GetWithConditions(x => x.ProductId == productId
                                                                             //&& x.IsActive == true
                                                                             && x.Attribute.IsActive == true, "Attribute", asNoTracking: true)
                                                        .ToList();
                return _mapper.Map<IEnumerable<ProductAttrListingBO>>(prodAttrMappings);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<ProductAttrMappingBO> GetAttributesValues(int mappingId)
        {
            try
            {
                var mapping = await _attributeMapping.GetSingleWithConditions(x => x.Id == mappingId, "ProductAttributeValues");

                return _mapper.Map<ProductAttrMappingBO>(mapping);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<BaseResponseBO> ToggleAttrState(ToggleAttrBO model)
        {
            try
            {
                if (model.Type == ToggleAttrType.AttributeMapping)
                {
                    var record = await _attributeMapping.Get(model.Id);
                    record.IsActive = model.Value;

                    await _attributeMapping.UpdateAsync(record);
                }
                else if (model.Type == ToggleAttrType.AttributeValue)
                {
                    var record = await _prodAttrValues.Get(model.Id);
                    record.IsActive = model.Value;

                    await _prodAttrValues.UpdateAsync(record);
                }
                else
                {
                    var record = await _attribute.Get(model.Id);
                    record.IsActive = model.Value;

                    await _attribute.UpdateAsync(record);
                }

                return new BaseResponseBO();
            }
            catch (Exception ex)
            {
                return new BaseResponseBO()
                {
                    IsSuccess = false,
                    Message = $"{ex.Message} :: Inner Exception:: {ex.InnerException?.Message}"
                };
            }
        }
    }
}
