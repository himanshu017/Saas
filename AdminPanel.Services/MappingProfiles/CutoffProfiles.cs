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
    public class CutoffProfiles : Profile
    {
        public CutoffProfiles()
        {
            CreateMap<DeliveryDateCutoff, DeliveryCutoffBO>().ReverseMap();
        }
    }
}
