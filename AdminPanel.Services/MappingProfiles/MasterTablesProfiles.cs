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
    public class MasterTablesProfiles: Profile
    {
        public MasterTablesProfiles()
        {
            CreateMap<CommonTypeBO, AddressType>().ReverseMap();
            CreateMap<CommonTypeBO, CommentType>().ReverseMap();
            CreateMap<CommonTypeBO, DiscountType>().ReverseMap();
            CreateMap<CommonTypeBO, DiscountLimitationType>().ReverseMap();
            CreateMap<CommonTypeBO, StatusType>().ReverseMap();


        }
    }
}
