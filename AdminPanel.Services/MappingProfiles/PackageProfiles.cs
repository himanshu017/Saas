using AdminPanel.DataModel.Models;
using AdminPanel.Shared.BO.MasterAdmin;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanel.Services.MappingProfiles
{
    public class PackageProfiles: Profile
    {
        public PackageProfiles()
        {
            CreateMap<Package, PackageBO>()
                .ForMember(dest => dest.IntervalName, opt => opt.MapFrom(s => s.Interval.Description));
            
            CreateMap<PackageBO, Package>(); 

            CreateMap<PackageFeaturesBO, PackageFeature>().ReverseMap();
            CreateMap<PackageIntervalBO, PackageInterval>().ReverseMap();
            CreateMap<FeatureDescriptionBO, FeatureDescription>().ReverseMap();
        }
    }
}
