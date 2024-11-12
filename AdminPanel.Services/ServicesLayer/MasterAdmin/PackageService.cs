using AdminPanel.DataModel.Models;
using AdminPanel.Services.Repository;
using AdminPanel.Shared.BO;
using AdminPanel.Shared.BO.MasterAdmin;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanel.Services
{
    public class PackageService : IPackageService
    {
        private readonly IGenericRepository<Package> _repo;
        private readonly IGenericRepository<PackageFeature> _featuresRepo;
        private readonly IGenericRepository<PackageInterval> _interval;
        private readonly IGenericRepository<FeatureDescription> _desc;
        private readonly IMapper _mapper;

        public PackageService(IGenericRepository<Package> repo,
                              IGenericRepository<PackageFeature> featuresRepo,
                              IGenericRepository<PackageInterval> interval,
                              IGenericRepository<FeatureDescription> desc,
                              IMapper mapper)
        {
            _repo = repo;
            _featuresRepo = featuresRepo;
            _interval = interval;
            _desc = desc;
            _mapper = mapper;
        }


        public IEnumerable<PackageBO> GetAllpackages()
        {
            var packages = _repo.GetAll("Interval").ToList();
            return _mapper.Map<IEnumerable<PackageBO>>(packages);
        }

        public IEnumerable<PackageIntervalBO> GetPackageIntervals()
        {
            var packageIntervals = _interval.GetAll().ToList();
            return _mapper.Map<IEnumerable<PackageIntervalBO>>(packageIntervals);
        }

        public async Task<BaseResponseBO> AddUpdatePackage(PackageBO model)
        {
            try
            {
                var response = new BaseResponseBO() { IsSuccess = true };
                var package = _mapper.Map<Package>(model);

                if (model.PackageId == 0)
                {
                    package.CreatedOn = DateTime.UtcNow;
                    await _repo.AddAsync(package);
                    response.Message = "success";
                }
                else
                {
                    package.ModifiedOn = DateTime.UtcNow;
                    await _repo.UpdateAsync(package);
                }

                return response;
            }
            catch (Exception ex)
            {
                return new BaseResponseBO()
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<PackageFeaturesBO> GetPackageFeatures(int packageId)
        {
            var features = await _featuresRepo.GetSingleWithConditions(x => x.PackageId == packageId);
            var res = _mapper.Map<PackageFeaturesBO>(features);
            if (features.PackageId > 0)
            {
                // is set to true if record already exists. If false, a new record is added to the database table.
                res.IsUpdate = true;
            }
            return res;
        }

        public async Task<BaseResponseBO> AddUpdatePackageFeatures(PackageFeaturesBO model)
        {
            try
            {
                var response = new BaseResponseBO() { IsSuccess = true, Message = "success" };
                var features = _mapper.Map<PackageFeature>(model);

                if (model.IsUpdate)
                {
                    await _featuresRepo.UpdateAsync(features);
                }
                else
                {
                    await _featuresRepo.AddAsync(features);
                }

                return response;
            }
            catch (Exception ex)
            {
                return new BaseResponseBO()
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<BaseResponseBO> DeletePackage(int id)
        {
            try
            {
                var response = new BaseResponseBO() { IsSuccess = true, Message = "success" };
                await _repo.Delete(id);

                return response;
            }
            catch (Exception ex)
            {
                return new BaseResponseBO()
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<FeatureDescriptionBO> GetFeatureDescriptions()
        {
            try
            {
                var desc = await _desc.GetSingleWithConditions(x => x.Description != null);

                return _mapper.Map<FeatureDescriptionBO>(desc);
            }
            catch (Exception ex)
            {
                return new FeatureDescriptionBO()
                {
                    IsSuccess = false,
                    Message = $"Error: {ex.Message} :: Inner: {ex.InnerException?.Message}"
                };
            }
        }

        public async Task<BaseResponseBO> UpdateFeatureDescription(string featureDescription)
        {
            try
            {
                var response = new BaseResponseBO() { IsSuccess = true, Message = "success" };
                var desc = await _desc.GetSingleWithConditions(x => x.Description != null);

                if (desc == null || desc.Id == 0)
                {
                    var featureDesc = new FeatureDescription()
                    {
                        Description = featureDescription
                    };

                    await _desc.AddAsync(featureDesc);
                }
                else
                {
                    desc.Description = featureDescription;
                    await _desc.UpdateAsync(desc);
                }
                return response;
            }
            catch (Exception ex)
            {
                return new BaseResponseBO()
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<BaseResponseBO> UpdateCompanyConfigDescription(string configDescription)
        {
            try
            {
                var response = new BaseResponseBO() { IsSuccess = true, Message = "success" };
                var desc = await _desc.GetSingleWithConditions(x => x.CompanyConfigDescription != null);

                if (desc == null || desc.Id == 0)
                {
                    var featureDesc = new FeatureDescription()
                    {
                        CompanyConfigDescription = configDescription
                    };

                    await _desc.AddAsync(featureDesc);
                }
                else
                {
                    desc.CompanyConfigDescription = configDescription;
                    await _desc.UpdateAsync(desc);
                }
                return response;
            }
            catch (Exception ex)
            {
                return new BaseResponseBO()
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }
    }
}
