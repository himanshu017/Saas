using Microsoft.AspNetCore.Mvc;
using AdminPanel.Services.ServicesLayer;
using AdminPanel.Shared.BO.MasterAdmin;
using AdminPanel.Shared.BO.CompanyAdmin;
using System.Security.Claims;

namespace AdminPanel.Server.Controllers.CompanyAdmin
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class CategoriesController : Controller
    {
        private readonly ICategoriesService _catService;

        public CategoriesController(ICategoriesService catService)
        {
            _catService = catService;
        }

        #region Manage Categories

        [HttpGet]
        public async Task<IActionResult> GetAllCategories(long cid)
        {
            return Ok(await _catService.GetAllCategories(cid));
        }

        [HttpPost]
        public async Task<IActionResult> AddEditMainCategories([FromBody] CategoriesBO model)
        {
            return Ok(await _catService.AddUpdateCategories(model));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(CategoriesBO model)
        {
            return Ok(await _catService.DeleteCategory(model));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSubCategoryById(long id)
        {
            return Ok(await _catService.GetAllSubCategories(id));
        }

        [HttpPost]
        public async Task<IActionResult> AddEditMainSubCategories([FromBody] SubCategoriesBO model)
        {
            return Ok(await _catService.AddUpdateSubCategories(model));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteSubCategory(SubCategoriesBO model)
        {
            return Ok(await _catService.DeleteSubCategory(model));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSpecialCategories(long cid)
        {
            return Ok(await _catService.GetAllSpecialCategories(cid));
        }
        #endregion

        #region Manage Filters

        [HttpGet]
        public async Task<IActionResult> GetAllFilters(long cid)
        {
            return Ok(await _catService.GetAllfilters(cid));
        }

        [HttpPost]
        public async Task<IActionResult> AddEditFilters([FromBody] FiltersBO model)
        {
            return Ok(await _catService.AddUpdateFilter(model));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteFilter(FiltersBO model)
        {
            return Ok(await _catService.DeleteFilter(model));
        }


        #endregion
        [HttpGet]
        public async Task<IActionResult> GetAllUnitofMeasures(long cid)
        {
            return Ok(await _catService.GetAllUnitofMeasures(cid));
        }
        [HttpPost]
        public async Task<IActionResult> AddEditUOM([FromBody] UnitOfMeasureBO model)
        {
            return Ok(await _catService.AddEditUOM(model));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUOM(UnitOfMeasureBO model)
        {
            return Ok(await _catService.DeleteUOM(model));
        }
    }
}
