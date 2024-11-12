using AdminPanel.DataModel.Models;
using AdminPanel.Shared.BO.CompanyAdmin;
using AdminPanel.Shared.BO.WebApp;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanel.Services.MappingProfiles
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<TempCartBO, TempCart>();
            CreateMap<TempCartItemsBO, TempCartItem>();

            CreateMap<OrderBO, Order>()
                .ForMember(dest => dest.TotalTax, opt => opt.MapFrom(s => s.OrderItems.Sum(s => s.TotalTax)))
                .ForMember(dest => dest.TotalWithTax, opt => opt.MapFrom(s => s.OrderItems.Sum(s => s.TotalPrice)))
                .ForMember(dest => dest.TotalWithoutTax, opt => opt.MapFrom(s => s.OrderItems.Sum(s => s.TotalWithoutTax)))
                ;

            //CreateMap<Order, OrderBO>()
            //    .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(s => s.Customer.CustomerName))
            //    .ForMember(dest => dest.CommentDesc, opt => opt.MapFrom(s => s.Comment.CommentDescription))
            //    .ForMember(dest => dest.StatusDesc, opt => opt.MapFrom(s => s.Status.StatusDesc))
            //    .ForMember(dest => dest.StreetAddress, opt => opt.MapFrom(s => $"{s.Address.Address1}, {s.Address.Address2}"));

            CreateMap<Order, OrderDetailsBO>()
               .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(s => s.Customer.CustomerName))
               .ForMember(dest => dest.CommentDesc, opt => opt.MapFrom(s => s.Comment.CommentDescription))
               .ForMember(dest => dest.StatusDesc, opt => opt.MapFrom(s => s.Status.StatusDesc))
               .ForMember(dest => dest.StreetAddress, opt => opt.MapFrom(s => $"{s.Address.Address1}, {s.Address.Address2}"))
               .ForMember(dest => dest.DelieryAddress, opt => opt.MapFrom(s => new[] { $"{s.Address.Address1},{s.Address.Address2}",
                                                                                        $"{s.Address.CityName}, {s.Address.StateName}",
                                                                                        $"{s.Address.CountryName}, {s.Address.PostalCode}" }))
               .ForMember(dest => dest.CompanyBO, opt => opt.MapFrom(s => s.Company))
               ;

            CreateMap<OrderItemsBO, OrderItem>();
            CreateMap<OrderItem, OrderItemsBO>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(s => s.Product.Name))
                .ForMember(dest => dest.CommentDesc, opt => opt.MapFrom(s => s.Comment.CommentDescription))
                .ForMember(dest => dest.UnitName, opt => opt.MapFrom(s => s.Unit.Name))
                .ForMember(dest => dest.AttributesDisplay, opt => opt.MapFrom(s => string.Join(", ", s.AttributeValues.Select(x => $"<b>{x.ProductMapping.Attribute.Name}</b> : {x.Value}"))))
                ;

            CreateMap<ItemsBO, OrderItem>().ForMember(dest => dest.OrderItemId, opt => opt.MapFrom(s => s.Id));
            CreateMap<OrderItem, ItemsBO>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(s => s.Product.Name))
                .ForMember(dest => dest.CommentDesc, opt => opt.MapFrom(s => s.Comment.CommentDescription))
                .ForMember(dest => dest.UnitName, opt => opt.MapFrom(s => s.Unit.Name))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(s => s.OrderItemId))
                .ForMember(dest => dest.OrderId, opt => opt.MapFrom(s => s.Order.OrderId));


            CreateMap<Order, OrderListBO>()
               .ForMember(dest => dest.UserFirstName, opt => opt.MapFrom(s => s.User.FirstName))
               .ForMember(dest => dest.UserLastName, opt => opt.MapFrom(s => s.User.LastName))
               .ForMember(dest => dest.UserEmail, opt => opt.MapFrom(s => s.User.Email))
               .ForMember(dest => dest.StatusDesc, opt => opt.MapFrom(s => s.Status.StatusDesc))
               .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(s => s.Customer.CustomerName))
               .ForMember(dest => dest.CustomerEmail, opt => opt.MapFrom(s => s.Customer.Email));

            CreateMap<Order, OrderDetailBO>()
               .ForMember(dest => dest.StatusDesc, opt => opt.MapFrom(s => s.Status.StatusDesc))
               .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(s => s.Customer.CustomerName))
              ;

        }
    }
}
