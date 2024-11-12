using AdminPanel.DataModel.Models;
using AdminPanel.Shared.BO.WebApp;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanel.Services.MappingProfiles
{
    public class ProductProfiles : Profile
    {
        public ProductProfiles()
        {
            CreateMap<Product, ProductListBO>()
                 .ForMember(dest => dest.UnitName, opt => opt.MapFrom(s => s.ProductUnits.FirstOrDefault().Unit.Name))
                 .ForMember(dest => dest.UnitCode, opt => opt.MapFrom(s => s.ProductUnits.FirstOrDefault().Unit.Code))
                 .ForMember(dest => dest.UnitId, opt => opt.MapFrom(s => s.ProductUnits.FirstOrDefault().UnitId))
                 .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(s => s.Category.SubCategoryName))
                 .ForMember(dest => dest.MainCategoryId, opt => opt.MapFrom(s => s.Category.MainCategory.Id))
                 .ForMember(dest => dest.MainCategoryName, opt => opt.MapFrom(s => s.Category.MainCategory.CategoryName))
                 .ForMember(dest => dest.StockQuantity, opt => opt.MapFrom(s => s.ProductInventories.FirstOrDefault().StockQuantity))
                 .ForMember(dest => dest.SpecialPrice, opt => opt.MapFrom(s => s.SpecialPricings.FirstOrDefault().Price))
                 .ForMember(dest => dest.ProductImages, opt => opt.MapFrom(s => s.ProductImages))
                 .ForMember(dest => dest.ProductAttrMappings, opt => opt.MapFrom(s => s.ProductAttributeMappings.Where(x => x.IsActive == true)))
                 .ForMember(dest => dest.ProductTags, opt => opt.MapFrom(s => s.Tags.Select(s => s.Title)))

                 // properties below used in Cart page response
                 .ForMember(dest => dest.CartQuantity, opt => opt.MapFrom(s => s.TempCartItems.FirstOrDefault().Quantity))
                 .ForMember(dest => dest.TempCartId, opt => opt.MapFrom(s => s.TempCartItems.FirstOrDefault().TempCartId))
                 .ForMember(dest => dest.CartUnitId, opt => opt.MapFrom(s => s.TempCartItems.FirstOrDefault().UnitId))
                 .ForMember(dest => dest.CartPrice, opt => opt.MapFrom(s => s.TempCartItems.FirstOrDefault().Price))
                 .ForMember(dest => dest.TempCartItemId, opt => opt.MapFrom(s => s.TempCartItems.FirstOrDefault().Id))
                 .ForMember(dest => dest.CommentId, opt => opt.MapFrom(s => s.TempCartItems.FirstOrDefault().CommentId))
                 .ForMember(dest => dest.IsInCart, opt => opt.MapFrom(s => (s.TempCartItems.FirstOrDefault().TempCartId > 0) ? true : false))
                 .ForMember(dest => dest.CommentDescription, opt => opt.MapFrom(s => s.TempCartItems.FirstOrDefault().CommentId.HasValue ? s.TempCartItems.FirstOrDefault().Comment.CommentDescription : ""))
                 .ForMember(dest => dest.AttributeMappingJson, opt => opt.MapFrom(s => s.TempCartItems.FirstOrDefault().AttributeMappingJson))
                 .ForMember(dest => dest.AttributePriceAdjustment, opt => opt.MapFrom(s => s.TempCartItems.FirstOrDefault().AttributePriceAdjustment))
                 .ForMember(dest => dest.AttributesDisplay, opt => opt.MapFrom(s => string.Join(", ", s.TempCartItems.FirstOrDefault().AttributeValues.Select(x => $"<b>{x.ProductMapping.Attribute.Name}</b> : {x.Value}"))))
                 ;

            // favorite list mappings
            CreateMap<FavoriteList, FavoriteListBO>()
                .ForMember(dest => dest.TypeDesc, opt => opt.MapFrom(s => s.Type.Description));

            CreateMap<FavoriteListBO, FavoriteList>();
            CreateMap<FavoriteListItem, FavoriteListItemsBO>().ReverseMap();

            //CreateMap<Product, SuggestiveProductListBO>()
            //    .ForMember(dest => dest.UnitName, opt => opt.MapFrom(s => s.ProductUnits.FirstOrDefault().Unit.Name))
            //     .ForMember(dest => dest.UnitCode, opt => opt.MapFrom(s => s.ProductUnits.FirstOrDefault().Unit.Code))
            //    .ForMember(dest => dest.ProductImages, opt => opt.MapFrom(s => s.ProductImages))
            //    ;
        }
    }
}
