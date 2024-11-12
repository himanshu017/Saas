using AdminPanel.DataModel.Models;
using AdminPanel.Shared.BO;
using AdminPanel.Shared.BO.WebApp;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanel.Services.MappingProfiles
{
    public class CommonProfiles : Profile
    {
        public CommonProfiles()
        {
            CreateMap<AddressType, CommonDropdownBO>()
                 .ForMember(dest => dest.Id, opt => opt.MapFrom(s => s.Id))
                 .ForMember(dest => dest.Description, opt => opt.MapFrom(s => s.TypeDesc));

            CreateMap<Country, CommonDropdownBO>()
                 .ForMember(dest => dest.Id, opt => opt.MapFrom(s => s.Id))
                 .ForMember(dest => dest.Description, opt => opt.MapFrom(s => s.Name));

            CreateMap<State, CommonDropdownBO>()
                 .ForMember(dest => dest.Id, opt => opt.MapFrom(s => s.Id))
                 .ForMember(dest => dest.Description, opt => opt.MapFrom(s => s.StateName));

            CreateMap<City, CommonDropdownBO>()
                 .ForMember(dest => dest.Id, opt => opt.MapFrom(s => s.Id))
                 .ForMember(dest => dest.Description, opt => opt.MapFrom(s => s.City1));

            CreateMap<DiscountType, CommonDropdownBO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(s => s.Id))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(s => s.TypeDesc));

            CreateMap<DiscountLimitationType, CommonDropdownBO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(s => s.Id))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(s => s.TypeDesc));

            CreateMap<CompanyInformationalText, CompanyGlobalTextBO>().ReverseMap();
            CreateMap<CompanyPostedLink, PostedLinksBO>().ReverseMap();
            CreateMap<CompanySlider, CompanySlidersBO>().ReverseMap();

            CreateMap<DeliveryRun, DeliveryRunBO>().ReverseMap();

            CreateMap<GroupsBO, Group>();

            CreateMap<Group, GroupsBO>()
                  .ForMember(dest => dest.ReferemceIds, opt => opt.MapFrom(s => s.GroupDetails.Select(s => s.ReferenceId).ToList()));

            CreateMap<TargetMarketingBO, TargetMarketing>();
            CreateMap<TargetMarketing, TargetMarketingBO>()
                .ForMember(dest => dest.ProductIds, opt => opt.MapFrom(s => s.Products.Select(s => s.ProductId).ToList()))
                .ForMember(dest => dest.GroupName, opt => opt.MapFrom(s => s.Group.Name))
                .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(s => s.Customer.CustomerName));

            CreateMap<Message, MessageBO>().ReverseMap();

            CreateMap<Customer, CommonDropdownBO>()
                .ForMember(dest => dest.IdLong, opt => opt.MapFrom(s => s.CustomerId))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(s => s.CustomerName));

            CreateMap<Product, CommonDropdownBO>()
                .ForMember(dest => dest.IdLong, opt => opt.MapFrom(s => s.ProductId))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(s => s.Name));

            CreateMap<CompanyDataImportMapping, CommonDropdownBO>()
                 .ForMember(dest => dest.Id, opt => opt.MapFrom(s => s.Id))
                 .ForMember(dest => dest.Description, opt => opt.MapFrom(s => s.MappingName));

            CreateMap<DeliveryCutoffType, CommonDropdownBO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(s => s.Id))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(s => s.TypeDesc));

            // mapping for home page slider section
            CreateMap<CompanySlider, SliderBO>().ReverseMap();

            CreateMap<GlobalTimeZone, CommonDropdownBO>()
               .ForMember(dest => dest.Id, opt => opt.MapFrom(s => s.Id))
               .ForMember(dest => dest.Description, opt => opt.MapFrom(s => s.Name));

            // Delivery cutoff dates mapping
            CreateMap<DeliveryDatesBO, DeliveryDateCutoff>().ReverseMap();

            // Comments mapping for cart page data
            CreateMap<CommentsBO, Comment>().ReverseMap();
        }
    }
}
