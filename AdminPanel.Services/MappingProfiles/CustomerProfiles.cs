using AdminPanel.DataModel.Models;
using AdminPanel.Shared.BO;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanel.Services.MappingProfiles
{
    public class CustomerProfiles : Profile
    {
        public CustomerProfiles()
        {
            CreateMap<CustomerBO, Customer>();
            CreateMap<Customer, CustomerBO>()//.ReverseMap();
                 .ForMember(dest => dest.CustomerCategories, opt => opt.MapFrom(s => s.Categories))
                 .ForMember(dest => dest.ChildCustomerRecords, opt => opt.MapFrom(s => s.ChildCustomers))
                 .ForMember(dest => dest.CustomerRuns, opt => opt.MapFrom(s => s.DeliveryRuns.Select(s => s.Id).ToList()));

            CreateMap<User, CustomerUserBO>().ReverseMap();
            // .ForMember(dest => dest.UserFeatures, opt => opt.MapFrom(s => s.CustomerUserFeaturesMaster));

            CreateMap<CustomerAddressBO, Address>();

            CreateMap<Address, CustomerAddressBO>()
                .ForMember(dest => dest.AddressTypeName, opt => opt.MapFrom(s => s.AddressType.TypeDesc));

            CreateMap<CustomerUserFeaturesMaster, UserFeaturesBO>().ReverseMap();

            CreateMap<SalesrepBO, User>();
            CreateMap<User, SalesrepBO>()
                .ForMember(dest => dest.SalesrepCode, opt => opt.MapFrom(s => s.UserSalesrepCodes.Select(s => s.SalesrepCode).ToList()));
        }
    }
}
