using AdminPanel.Services;
using AdminPanel.Shared.BO.MasterAdmin;
using Microsoft.AspNetCore.Mvc;

namespace AdminPanel.Server.Controllers.MasterAdmin
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class PackageController : ControllerBase
    {
        private readonly IPackageService _package;
        public PackageController(IPackageService package)
        {
            _package = package;
        }

        [HttpGet]
        public IActionResult GetAllPackages()
        {
            return Ok(_package.GetAllpackages());
        }

        [HttpGet]
        public IActionResult GetAllPackageIntervals()
        {
            return Ok(_package.GetPackageIntervals());
        }

        [HttpPost]
        public async Task<IActionResult> AddEditPackage([FromBody]PackageBO model)
        {
            return Ok(await _package.AddUpdatePackage(model));
        }

        [HttpPost]
        public async Task<IActionResult> DeletePackage([FromBody] int id)
        {
            return Ok(await _package.DeletePackage(id));
        }

        [HttpGet]
        public async Task<IActionResult> GetPackageFeatures(int packageId)
        {

            var res = await _package.GetPackageFeatures(packageId);
            return Ok(res);
        }

        [HttpPost]
        public async Task<IActionResult> AddUpdatePackageFeatures([FromBody]PackageFeaturesBO model)
        {
            var res = await _package.AddUpdatePackageFeatures(model);
            return Ok(res);
        }

        [HttpGet]
        public async Task<IActionResult> GetFeatureDescriptions()
        {
            var res = await _package.GetFeatureDescriptions();
            return Ok(res);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateFeatureDescription([FromBody] string featureDescription)
        {
            var res = await _package.UpdateFeatureDescription(featureDescription);
            return Ok(res);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCompanyConfigDescription([FromBody] string configDescription)
        {
            var res = await _package.UpdateCompanyConfigDescription(configDescription);
            return Ok(res);
        }
    }
}
