using AdminPanel.Shared.BO;
using AdminPanel.Shared.BO.CompanyAdmin;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdminPanel.Services.Helpers;
using System.Drawing.Imaging;
using AdminPanel.Services.Repository;
using AdminPanel.DataModel.Models;
using AutoMapper;

namespace AdminPanel.Services.ServicesLayer.CompanyAdmin
{
    public class Settings : ISettings
    {
        private readonly IGenericRepository<Company> _company;
        private readonly IMapper _mapper;

        private readonly string rootpath = @"wwwroot/CompanyLogo/";
        public Settings(IGenericRepository<Company> company, IMapper mapper)
        {
            _company = company;
            _mapper = mapper;
        }

        public async Task<BaseResponseBO> SaveCompanyLogo(UpdateCompanyLogoModel model)
        {
            try
            {
                var company = await _company.GetSingleWithConditions(x => x.CompanyId == model.CompanyId);

                string storePath = $"{rootpath}{company.DomainName}/";
                //.Replace("\\Server", "\\Client\\")
                string fullpath = Path.Combine(Directory.GetCurrentDirectory(), storePath);
                if (!Directory.Exists(fullpath))
                {
                    Directory.CreateDirectory(fullpath);
                }

                if (model.File != null)
                {
                    if (!string.IsNullOrEmpty(company.Logo))
                    {
                        if (File.Exists(Path.Combine(fullpath, company.Logo)))
                        {
                            File.Delete(Path.Combine(fullpath, company.Logo));
                        }
                    }

                    var path = Path.Combine(fullpath, model.File.Name);

                    using (var ms = new MemoryStream(model.File.Content))
                    {
                        Image image = Image.FromStream(ms);

                        // set the default height and width in the appsettings file.
                        Size s = new Size(300, 130);

                        var resized = image.resizeImage(s);
                        using (var ms2 = new MemoryStream())
                        {
                            resized.Save(ms2, ImageFormat.Png);
                            ms2.ToArray();

                            using (var fs = new FileStream(path, FileMode.Create))
                            {
                                ms2.WriteTo(fs);
                            }
                        }

                        company.Logo = model.File.Name;
                        company.ModifiedBy = model.UserId;
                        company.ModifiedOn = DateTime.UtcNow;

                        await _company.UpdateAsync(company);
                    }
                }

                return new BaseResponseBO() { IsSuccess = true, Message = "Logo saved successfully." };
            }
            catch (Exception ex)
            {
                return new BaseResponseBO() { IsSuccess = false, Message = ex.Message };
            }
        }

        public async Task<CompanyBO> GetCompanyDetails(long companyId)
        {
            try
            {
                var company = await _company.GetSingleWithConditions(x => x.CompanyId == companyId);
                var result = _mapper.Map<CompanyBO>(company);

                result.IsSuccess = true;
                result.Message = "Success";

                return result;
            }
            catch (Exception ex)
            {
                return new CompanyBO() { IsSuccess = false, Message = ex.Message };
            }
        }

        public async Task<BaseResponseBO> UpdateCompanyDetails(CompanyBO model)
        {
            try
            {
                var company = await _company.GetSingleWithConditions(x => x.CompanyId == model.CompanyId);
                
                company.ModifiedOn = DateTime.UtcNow;
                company.ModifiedBy = model.UserId;

                _mapper.Map(model, company);

                await _company.UpdateAsync(company);

                return new BaseResponseBO()
                {
                    IsSuccess = true,
                    Message = "Company details updated successfully"
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseBO()
                {
                    IsSuccess = false,
                    Message = $"Error: {ex.Message} Inner: {ex.InnerException?.Message}"
                };
            }
        }
    }
}
