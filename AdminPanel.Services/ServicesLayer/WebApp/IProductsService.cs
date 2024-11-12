using AdminPanel.Shared.BO;
using AdminPanel.Shared.BO.CompanyAdmin;
using AdminPanel.Shared.BO.WebApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanel.Services.ServicesLayer
{
    public interface IProductsService
    {
        Task<GenericResponse<IEnumerable<CategoryListBO>>> GetAllMainAndSubCategories(GetAllCategoriesBO model);
        Task<GenericResponse<IEnumerable<FiltersBO>>> GetAllFilters(long companyId);
        Task<GenericResponse<IEnumerable<UnitOfMeasureBO>>> GetAllUnits(long companyId);
        Task<GenericPagedResponse<IEnumerable<ProductListBO>>> GetProductsBySearchFilters(GetProductListBO model);
        Task<GenericResponse<IEnumerable<FavoriteListBO>>> GetFavoriteLists(GetFavLists model);
        Task<GenericResponse<bool>> AddUpdateFavList(FavoriteListBO model);
        Task<GenericResponse<bool>> DeleteFavList(long id);
        Task<GenericResponse<bool>> AddDeleteFavlistItem(ManageFavListItemBO model);
        Task<GenericPagedResponse<IEnumerable<ProductListBO>>> GetFavoriteListItems(GetProductListBO model);
        Task<GenericPagedResponse<IEnumerable<ProductListBO>>> GetCompanySpecials(GetProductListBO model);

        Task<GenericResponse<IEnumerable<ProductListBO>>> GetTempCartItems(GetProductListBO model);
        Task<GenericResponse<ProductListBO>> GetProductDetail(GetProdDetailBO model);
        Task<GenericResponse<IEnumerable<ProductListBO>>> GetSuggestiveProducts(GetProductListBO model);
    }
}
