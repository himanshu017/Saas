using AdminPanel.Shared.BO;
using AdminPanel.Shared.BO.MasterAdmin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanel.Services
{
    public interface IPackageService
    {
        IEnumerable<PackageBO> GetAllpackages();
        IEnumerable<PackageIntervalBO> GetPackageIntervals();
        Task<BaseResponseBO> AddUpdatePackage(PackageBO model);
        Task<PackageFeaturesBO> GetPackageFeatures(int packageId);
        Task<BaseResponseBO> AddUpdatePackageFeatures(PackageFeaturesBO model);
        Task<BaseResponseBO> DeletePackage(int id);
        Task<FeatureDescriptionBO> GetFeatureDescriptions();
        Task<BaseResponseBO> UpdateFeatureDescription(string featureDescription);
        Task<BaseResponseBO> UpdateCompanyConfigDescription(string configDescription);
    }
}
