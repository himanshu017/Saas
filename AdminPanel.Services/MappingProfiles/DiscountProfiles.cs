using AdminPanel.DataModel.Models;
using AdminPanel.Shared;
using AdminPanel.Shared.BO;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanel.Services.MappingProfiles
{
    public class DiscountProfiles : Profile
    {
        public DiscountProfiles()
        {
            CreateMap<DiscountBO, Discount>();
            CreateMap<Discount, DiscountBO>()
                .ForMember(dest => dest.DiscountType, opt => opt.MapFrom(s => s.Type.TypeDesc))
                .ForMember(dest => dest.LimitationTypeDesc, opt => opt.MapFrom(s => s.LimitationType.TypeDesc))
                .ForMember(dest => dest.ProductCustomerSelections,
                            opt => opt.MapFrom(s =>
                                    (s.TypeId == (byte)DiscountTypes.Selected_products) ? s.Products.Select(p => p.ProductId).ToList() :
                                    (s.TypeId == (byte)DiscountTypes.Selected_Customers) ? s.Customers.Select(c => c.CustomerId).ToList() : null));
        }
    }
}
