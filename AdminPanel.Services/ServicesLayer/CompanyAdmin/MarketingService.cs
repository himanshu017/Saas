using AdminPanel.DataModel.Models;
using AdminPanel.Services.Repository;
using AdminPanel.Shared.BO;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanel.Services.ServicesLayer
{
    public class MarketingService : IMarketingService
    {
        private readonly IGenericRepository<TargetMarketing> _marketing;
        private readonly IGenericRepository<Product> _prods;

        private readonly IMapper _mapper;
        private readonly string rootpath = @"wwwroot/Marketing/";
        public MarketingService(IMapper mapper,
                                IGenericRepository<TargetMarketing> marketing,
                                IGenericRepository<Product> prods)
        {
            _mapper = mapper;
            _marketing = marketing;
            _prods = prods;
        }

        public IEnumerable<TargetMarketingBO> GetAllRecords(long companyId)
        {
            var records = _marketing.GetWithConditions(x => x.CompanyId == companyId, "Products,MarketingType,Group,Customer");
            return _mapper.Map<IEnumerable<TargetMarketingBO>>(records);
        }

        public async Task<BaseResponseBO> AddUpdateRecord(TargetMarketingBO model)
        {
            var response = new BaseResponseBO() { IsSuccess = true };

            try
            {
                var record = new TargetMarketing();
                if (model.Id == 0)
                {
                    _mapper.Map(model, record);
                    SaveImages(model);
                    record.FileName = model.FileName;

                    if (model.ProductIds != null && model.ProductIds.Count() > 0)
                    {
                        var products = _prods.GetAll().Where(x => x.CompanyId == model.CompanyId && model.ProductIds.Contains(x.ProductId)).ToList();

                        foreach (var prod in products)
                        {
                            record.Products.Add(prod);
                        }
                    }

                    await _marketing.AddAsync(record);

                    response.Message = "Record saved successfully";
                }
                else
                {
                    string directoryPath = Directory.GetCurrentDirectory().Replace("\\Server", "\\Client\\");
                    record = await _marketing.GetSingleWithConditions(x => x.Id == model.Id, "Products");

                    _mapper.Map(model, record);

                    if (!model.UploadedFile.KeepExisting)
                    {
                        string rootFolder = $"{rootpath}{model.DomainName}/";
                        if (File.Exists(Path.Combine(directoryPath, rootFolder, record.FileName)))
                        {
                            File.Delete(Path.Combine(directoryPath, rootFolder, record.FileName));
                        }
                        SaveImages(model);
                        record.FileName = model.FileName;
                    }

                    if (model.ProductIds != null && model.ProductIds.Count() > 0)
                    {
                        record.Products.Clear();

                        var products = _prods.GetAll()
                                             .Where(x => x.CompanyId == model.CompanyId && model.ProductIds.Contains(x.ProductId))
                                             .ToList();

                        foreach (var prod in products)
                        {
                            record.Products.Add(prod);
                        }
                    }
                    else
                    {
                        record.Products.Clear();
                    }

                    await _marketing.UpdateAsync(record);

                    response.Message = "Record updated successfully";
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

        public async Task<BaseResponseBO> DeleteRecord(long id)
        {
            try
            {
                string directoryPath = Directory.GetCurrentDirectory().Replace("\\Server", "\\Client\\");
                var record = await _marketing.GetSingleWithConditions(x => x.Id == id, "Products,Company");
                record.Products.Clear();

                string rootFolder = $"{rootpath}{record.Company.DomainName}/";
                if (File.Exists(Path.Combine(directoryPath, rootFolder, record.FileName)))
                {
                    File.Delete(Path.Combine(directoryPath, rootFolder, record.FileName));
                }

                await _marketing.Delete(id);

                return new BaseResponseBO()
                {
                    IsSuccess = true,
                    Message = "Record deleted successfully"
                };
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

        private void SaveImages(TargetMarketingBO model)
        {
            try
            {
                string storePath = $"{rootpath}{model.DomainName}/";

                // replaced server with client to access the file in frontent easily
                string directoryPath = Directory.GetCurrentDirectory().Replace("\\Server", "\\Client\\");

                string fullpath = Path.Combine(directoryPath, storePath);
                if (!Directory.Exists(fullpath))
                {
                    Directory.CreateDirectory(fullpath);
                }
                if (model.UploadedFile != null)
                {
                    var filename = $"{Guid.NewGuid().ToString("N").ToUpper()}{Path.GetExtension(model.UploadedFile.FileName)}";
                    model.FileName = filename;

                    var path = Path.Combine(directoryPath, storePath, filename);

                    using (var ms = new MemoryStream(model.UploadedFile.FileContent))
                    {
                        using (var fs = new FileStream(path, FileMode.Create))
                        {
                            ms.WriteTo(fs);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
