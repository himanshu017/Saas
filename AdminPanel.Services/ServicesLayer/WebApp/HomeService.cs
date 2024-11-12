using AdminPanel.DataModel.Models;
using AdminPanel.Services.Repository;
using AdminPanel.Shared.BO;
using AdminPanel.Shared.BO.CompanyAdmin;
using AdminPanel.Shared.BO.WebApp;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanel.Services.ServicesLayer
{
    public class HomeService : IHomeService
    {
        private readonly IGenericRepository<CompanySlider> _repo;
        private readonly IGenericRepository<Product> _product;
        private readonly IGenericRepository<MainCategory> _category;
        private readonly IGenericRepository<TempCart> _tempCart;
        private readonly IMapper _mapper;

        public HomeService(IGenericRepository<CompanySlider> repo,
                           IGenericRepository<Product> product,
                           IGenericRepository<MainCategory> category,
                           IGenericRepository<TempCart> tempCart,
                           IMapper mapper)
        {
            _repo = repo;
            _product = product;
            _category = category;
            _mapper = mapper;
            _tempCart = tempCart;
        }

        public async Task<GenericResponse<IEnumerable<SliderBO>>> GetCompanySliders(long companyId)
        {
            try
            {
                var sliders = await _repo.GetWithConditions(x => x.IsActive == true && x.CompanyId == companyId).ToListAsync();

                var list = _mapper.Map<IEnumerable<SliderBO>>(sliders);

                return new GenericResponse<IEnumerable<SliderBO>>()
                {
                    Data = list,
                    IsSuccess = true,
                    Message = "Success"
                };

            }
            catch (Exception ex)
            {
                return new GenericResponse<IEnumerable<SliderBO>>()
                {
                    IsSuccess = false,
                    Message = $"Error: {ex.Message} :: Inner:: {ex.InnerException?.Message}"
                };
            }
        }

        public async Task<GenericResponse<IEnumerable<ProductListBO>>> GetFeaturedProducts(GetProductModel model)
        {
            try
            {
                var products = await GetProductsWithConditions(x => x.IsFeatured == true
                                                                  && x.IsActive == true
                                                                  && x.CompanyId == model.CompanyId, model);

                var list = _mapper.Map<IEnumerable<ProductListBO>>(products);


                return new GenericResponse<IEnumerable<ProductListBO>>()
                {
                    Data = list,
                    IsSuccess = true,
                    Message = "Success"
                };
            }
            catch (Exception ex)
            {
                return new GenericResponse<IEnumerable<ProductListBO>>()
                {
                    IsSuccess = false,
                    Message = $"Error: {ex.Message} :: Inner:: {ex.InnerException?.Message}"
                };
            }
        }

        public async Task<GenericResponse<IEnumerable<ProductListBO>>> GetRecentlyOrderedProducts(GetProductModel model)
        {
            try
            {
                // need to replace the code with actual recent items data once the ordering functionality is working.
                // till then fetching top ten items from the products table.
                var products = await GetProductsWithConditions(x => x.IsActive == true
                                                                 && x.CompanyId == model.CompanyId
                                                                 && x.OrderItems.Count() > 0, model, true);

                var list = _mapper.Map<IEnumerable<ProductListBO>>(products);

                return new GenericResponse<IEnumerable<ProductListBO>>()
                {
                    Data = list,
                    IsSuccess = true,
                    Message = "Success"
                };
            }
            catch (Exception ex)
            {
                return new GenericResponse<IEnumerable<ProductListBO>>()
                {
                    IsSuccess = false,
                    Message = $"Error: {ex.Message} :: Inner:: {ex.InnerException?.Message}"
                };
            }
        }

        public async Task<GenericResponse<IEnumerable<ProductListBO>>> GetFeaturedCategoryProducts(GetProductModel model)
        {
            try
            {
                var categories = await _category.GetWithConditions(x => x.IsFeatured == true
                                                                     && x.IsActive == true
                                                                     && x.CompanyId == model.CompanyId
                                                                     && x.SubCategories.Where(s => s.Products.Count() > 0).Count() > 0, "SubCategories", true)
                                                .OrderBy(o => o.CategoryName)
                                                .Skip(0).Take(3)
                                                .ToListAsync();

                List<Product> products = new();

                foreach (var cat in categories)
                {
                    products.AddRange(await GetProductsWithConditions(x => cat.SubCategories.Contains(x.Category), model));
                }


                var list = _mapper.Map<IEnumerable<ProductListBO>>(products);

                return new GenericResponse<IEnumerable<ProductListBO>>()
                {
                    Data = list,
                    IsSuccess = true,
                    Message = "Success"
                };
            }
            catch (Exception ex)
            {
                return new GenericResponse<IEnumerable<ProductListBO>>()
                {
                    IsSuccess = false,
                    Message = $"Error: {ex.Message} :: Inner:: {ex.InnerException?.Message}"
                };
            }
        }

        public async Task<GenericResponse<IEnumerable<CategoryListBO>>> GetAllCategories(long companyId)
        {
            try
            {
                var categories = await _category.GetWithConditions(x => x.IsActive == true
                                                                     && x.CompanyId == companyId, "SubCategories,SubCategories.Products", true).ToListAsync();

                var res = _mapper.Map<IEnumerable<CategoryListBO>>(categories);

                return new GenericResponse<IEnumerable<CategoryListBO>>()
                {
                    Data = res,
                    IsSuccess = true,
                    Message = "Success"
                };
            }
            catch (Exception ex)
            {
                return new GenericResponse<IEnumerable<CategoryListBO>>()
                {
                    IsSuccess = false,
                    Message = $"Error: {ex.Message} :: Inner:: {ex.InnerException?.Message}"
                };
            }
        }

        private async Task<IEnumerable<Product>> GetProductsWithConditions(Expression<Func<Product, bool>> expression, GetProductModel model, bool isRecent = false)
        {
            try
            {
                var products = _product.GetWithConditions(expression, "ProductImages,Category,Category.MainCategory,ProductUnits,ProductUnits.Unit,TempCartItems,OrderItems", true);

                if (isRecent)
                {
                    products.OrderByDescending(s => s.OrderItems.OrderByDescending(o => o.CreatedOn));
                }
                else
                {
                    products.OrderBy(o => o.Name);
                }

                return await products.Skip(model.PageSize * model.PageNumber)
                                     .Take(model.PageSize)
                                     .ToListAsync(); ;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
