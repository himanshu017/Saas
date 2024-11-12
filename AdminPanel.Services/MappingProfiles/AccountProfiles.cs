using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdminPanel.DataModel.Models;
using AdminPanel.Shared.BO;
using AdminPanel.Shared.BO.Account;

namespace AdminPanel.Services.MappingProfiles
{
    public class AccountProfiles : Profile
    {
        public AccountProfiles()
        {
            // Login respose profile
            CreateMap<User, LoginResponse>()
                .ForMember(dest => dest.Role, opt => opt.MapFrom(s => s.UserType.Role))
                .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(s => s.Company.CompanyName))
                .ForMember(dest => dest.DomainName, opt => opt.MapFrom(s => s.Company.DomainName))
                .ForMember(dest => dest.Logo, opt => opt.MapFrom(s => s.Company.Logo))
                .ForMember(dest => dest.ManagedFeatures, opt => opt.MapFrom(s => s.CustomerUserFeaturesMasters.FirstOrDefault().ManagedFeatures))
                .ForMember(dest => dest.OrderSettings, opt => opt.MapFrom(s => s.Company.CompanyConfigurationSetting.ConfigSettings))
                .ForMember(dest => dest.CurrencyInfo, opt => opt.MapFrom(s => s.Company.CurrencyInfo))
                .ForMember(dest => dest.CompanyConfig, opt => opt.MapFrom(s => s.Company.CompanyConfigurationSetting.ConfigSettings));

            // Register new user mappings
            CreateMap<Company, RegisterBO>().ReverseMap();
            CreateMap<User, RegisterBO>().ReverseMap();
            CreateMap<User, AppUserRegisterBO>().ReverseMap();
            
            CreateMap<Company, DomainInfoBO>().ReverseMap();
        }
    }
}
