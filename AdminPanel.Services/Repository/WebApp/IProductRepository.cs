using AdminPanel.DataModel.Models;
using AdminPanel.Shared.BO;
using AdminPanel.Shared.BO.WebApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanel.Services.Repository
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<IEnumerable<MainCategory>> GetAllMainAndSubCategories(GetAllCategoriesBO model);
        Task<IEnumerable<Filter>> GetAllFilters(long companyId);
        Task<IEnumerable<UnitOfMeasurement>> GetAllUnits(long companyId);
        Task<IQueryable<Product>> GetProductsBySearchFilters(GetProductListBO model);
        Task<IEnumerable<FavoriteList>> GetFavoriteLists(GetFavLists model);
        Task<IQueryable<Product>> GetFavoriteListItems(GetProductListBO model);
        Task<IQueryable<Product>> GetCompanySpecials(GetProductListBO model);
        Task<List<ProductListBO>> GetAdditionalProductInfo(List<ProductListBO> data, long userId, long customerId, long companyId, bool isCart = false);
        Task<IQueryable<Product>> GetTempCartItems(GetProductListBO model);
        Task<IQueryable<Product>> GetProductDetail(GetProdDetailBO model);
        Task<IQueryable<Product>> GetSuggestiveProducts(GetProductListBO model);


    }

}
