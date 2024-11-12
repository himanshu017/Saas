using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdminPanel.DataModel.Models;
using AdminPanel.Shared.BO.CompanyAdmin;
using AdminPanel.Shared.BO.WebApp;

namespace AdminPanel.Services.MappingProfiles
{
    public class CategoriesProfile : Profile
    {
        public CategoriesProfile()
        {
            CreateMap<MainCategory, CategoriesBO>().ReverseMap();
            CreateMap<SubCategory, SubCategoriesBO>().ReverseMap();
            CreateMap<Filter, FiltersBO>().ReverseMap();
            CreateMap<Product, ProductBO>()
                .ForMember(dest => dest.ProductImagesList, opt => opt.MapFrom(s => s.ProductImages))
                .ForMember(dest => dest.UnitId, opt => opt.MapFrom(s => s.ProductUnits.FirstOrDefault().UnitId))
                .ForMember(dest => dest.Units, opt => opt.MapFrom(s => s.ProductUnits))
                .ForMember(dest => dest.MainCategoryId, opt => opt.MapFrom(s => s.Category.MainCategoryId))
                .ForMember(dest => dest.SuggestiveProductIds, opt => opt.MapFrom(s => s.SuggestiveProducts.Select(s => s.ProductId)))
                .ForMember(dest => dest.ProductTags, opt => opt.MapFrom(s => s.Tags.Select(s => s.Title)))
                ;

            CreateMap<ProductBO, Product>()
                .ForMember(dest => dest.ProductUnits, opt => opt.MapFrom(s => s.Units));

            CreateMap<ProductBO, ProductImage>();
            CreateMap<ProductImage, ProductImageBO>().ReverseMap();
            CreateMap<ProductUnitsBO, ProductUnit>();
            CreateMap<ProductUnit,ProductUnitsBO>()
                .ForMember(dest => dest.UnitName, opt => opt.MapFrom(s => s.Unit.Name)); ;


            CreateMap<UnitOfMeasurement, UnitOfMeasureBO>().ReverseMap();

            // mapping for all product listings
            CreateMap<ProductListBO, Product>();
            CreateMap<Product, ProductListBO>()
                .ForMember(dest => dest.MainCategoryId, opt => opt.MapFrom(s => s.Category.MainCategoryId))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(s => s.Category.SubCategoryName))
                .ForMember(dest => dest.MainCategoryName, opt => opt.MapFrom(s => s.Category.MainCategory.CategoryName));

            CreateMap<ProductListBO, ProductImage>().ReverseMap();

            CreateMap<ProductImage, FileViewModel>()
               .ForMember(dest => dest.Name, opt => opt.MapFrom(s => s.ImageName));

            CreateMap<CategoryListBO, MainCategory>();
            CreateMap<MainCategory, CategoryListBO>()
                .ForMember(dest => dest.ItemCount, opt => opt.MapFrom(s => s.SubCategories.SelectMany(p => p.Products).Count()))
                .ForMember(dest => dest.AllSubCategories, opt => opt.MapFrom(s => s.SubCategories));
            
            CreateMap<SubCategoryListBO, SubCategory>().ReverseMap();
            //CreateMap<IEnumerable<SubCategoryListBO>,IEnumerable<SubCategory>>().ReverseMap();
        }
    }
}
