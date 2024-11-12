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
    public interface IHomeService
    {
        Task<GenericResponse<IEnumerable<SliderBO>>> GetCompanySliders(long companyId);
        Task<GenericResponse<IEnumerable<ProductListBO>>> GetFeaturedProducts(GetProductModel model);
        Task<GenericResponse<IEnumerable<ProductListBO>>> GetRecentlyOrderedProducts(GetProductModel model);
        Task<GenericResponse<IEnumerable<ProductListBO>>> GetFeaturedCategoryProducts(GetProductModel model);
        Task<GenericResponse<IEnumerable<CategoryListBO>>> GetAllCategories(long companyId);
    }
}
