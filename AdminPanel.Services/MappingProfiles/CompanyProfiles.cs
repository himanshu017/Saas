using AdminPanel.DataModel.Models;
using AdminPanel.Shared.BO.CompanyAdmin;
using AdminPanel.Shared.BO.MasterAdmin;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanel.Services.MappingProfiles
{
    public class CompanyProfiles: Profile
    {
        public CompanyProfiles()
        {
            CreateMap<Company, CompanyModelBO>().ReverseMap();
            CreateMap<CompanyConfigurationSetting, CompanyConfigSettingsBO>().ReverseMap();
            
            CreateMap<Company, CompanyBO>().ReverseMap();
        }
    }
}
