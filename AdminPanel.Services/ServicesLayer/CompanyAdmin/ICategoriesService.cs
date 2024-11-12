using AdminPanel.Shared.BO;
using AdminPanel.Shared.BO.CompanyAdmin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanel.Services.ServicesLayer
{
   
    public interface ICategoriesService
    {
        Task< IEnumerable<CategoriesBO>> GetAllCategories(long companyId);
        Task <IEnumerable<SubCategoriesBO>> GetAllSubCategories(long id);
        Task<BaseResponseBO> AddUpdateCategories(CategoriesBO model);
        Task<BaseResponseBO> AddUpdateSubCategories(SubCategoriesBO model);
        Task<BaseResponseBO> DeleteCategory(CategoriesBO model);
        Task<BaseResponseBO> DeleteSubCategory(SubCategoriesBO model);

        Task<BaseResponseBO> AddUpdateFilter(FiltersBO model);

        Task<BaseResponseBO> DeleteFilter(FiltersBO model);

        Task<IEnumerable<FiltersBO>> GetAllfilters(long companyId);


        Task<IEnumerable<UnitOfMeasureBO>> GetAllUnitofMeasures(long companyId);

        Task<IEnumerable<CategoriesBO>> GetAllSpecialCategories(long companyId);

        Task<SubCategoriesBO> GetSubCategoryById(long id);

        Task<BaseResponseBO> AddEditUOM(UnitOfMeasureBO model);

        Task<BaseResponseBO> DeleteUOM(UnitOfMeasureBO model);

    }
}
