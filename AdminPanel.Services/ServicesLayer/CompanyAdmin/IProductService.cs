using AdminPanel.Shared.BO;
using AdminPanel.Shared.BO.CompanyAdmin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanel.Services.ServicesLayer.CompanyAdmin
{
    public interface IProductService
    {
        IEnumerable<ProductBO> GetAllProducts(GetProductModel model);
        Task<ProductBO> GetProductById(long id,string domain);
        Task<BaseResponseBO> AddUpdateProduct(ProductBO model);
        Task<BaseResponseBO> DeleteProduct(ProductBO model);
        Task<GetSpecialsResponse> GetCompanySpecials(long companyId);
        Task<BaseResponseBO> AddUpdateCompanySpecials(CompanySpecialsBO model);
        Task<bool> UpdateBitField(UpdateBitBO model);
    }
}
