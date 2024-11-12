using AdminPanel.DataModel.Models;
using AdminPanel.Shared.BO.CompanyAdmin;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanel.Services.MappingProfiles
{
    public class AttributeProfiles : Profile
    {
        public AttributeProfiles()
        {
            CreateMap<AttributeBO, ProductAttribute>();
            CreateMap<ProductAttribute, AttributeBO>()
                 .ForMember(dest => dest.HasPredefinedValues, opt => opt.MapFrom(s => s.PredefinedProductAttributeValues.Count() > 0));

            CreateMap<ProductAttributeMapping, ProductAttrMappingBO>()
                .ForMember(x => x.AttrValues, opt => opt.MapFrom(s => s.ProductAttributeValues));

            CreateMap<ProductAttrMappingBO, ProductAttributeMapping>()
                .ForMember(x => x.Attribute, opt => opt.Ignore()); ;

            CreateMap<AttributeValuesBO, ProductAttributeValue>().ReverseMap();

            CreateMap<PredefinedProductAttributeValue, PredefinedAttrValuesBO>().ReverseMap();

            CreateMap<ProductAttributeMapping, ProductAttrListingBO>()
                .ForMember(dest => dest.AttributeName, src => src.MapFrom(s => s.Attribute.Name))
            ; 

        }
    }
}
