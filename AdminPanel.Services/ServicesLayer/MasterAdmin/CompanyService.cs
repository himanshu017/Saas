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
    public class CompanyService : ICompanyService
    {
        private readonly IGenericRepository<Company> _repo;
        private readonly IGenericRepository<CompanyConfigurationSetting> _configRepo;
        private readonly IMapper _mapper;

        public CompanyService(IGenericRepository<Company> repo,
                              IGenericRepository<CompanyConfigurationSetting> configRepo,
                              IMapper mapper)
        {
            _repo = repo;
            _configRepo = configRepo;
            _mapper = mapper;
        }

        public IEnumerable<CompanyModelBO> GetAllCompanies()
        {
            var companies = _repo.GetAll().ToList();
            return _mapper.Map<IEnumerable<CompanyModelBO>>(companies);
        }

        public async Task<CompanyConfigSettingsBO> GetCompanyConfiguration(long companyId)
        {
            var config = await _configRepo.GetSingleWithConditions(x => x.CompanyId == companyId);
            var res = _mapper.Map<CompanyConfigSettingsBO>(config);
            if (config.CompanyId > 0)
            {
                // is set to true if record already exists. If false, a new record is added to the database table.
                res.IsUpdate = true;
            }
            return res;
        }

        public async Task<BaseResponseBO> AddUpdateConfigSettings(CompanyConfigSettingsBO model)
        {
            try
            {
                var response = new BaseResponseBO() { IsSuccess = true, Message = "success" };
                var configSetting = _mapper.Map<CompanyConfigurationSetting>(model);
                if (model.IsUpdate)
                {
                    await _configRepo.UpdateAsync(configSetting);
                }
                else
                {
                    await _configRepo.AddAsync(configSetting);
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
