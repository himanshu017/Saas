using AdminPanel.DataModel.Models;
using AdminPanel.Services.Repository;
using AdminPanel.Shared.BO;
using AdminPanel.Shared.BO.CompanyAdmin;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanel.Services.ServicesLayer
{
    public class CommonService : ICommonService
    {
        private readonly IGenericRepository<AddressType> _addressType;
        private readonly IGenericRepository<Country> _country;
        private readonly IGenericRepository<State> _state;
        private readonly IGenericRepository<City> _city;
        private readonly IGenericRepository<CompanyInformationalText> _globaltexts;
        private readonly IGenericRepository<CompanyPostedLink> _postedLinks;
        private readonly IGenericRepository<CompanySlider> _sliders;
        private readonly IGenericRepository<DeliveryRun> _runs;
        private readonly IGenericRepository<Customer> _customer;
        private readonly IGenericRepository<Product> _product;
        private readonly IGenericRepository<DiscountType> _discountType;
        private readonly IGenericRepository<DiscountLimitationType> _discountLimitation;
        private readonly IGenericRepository<DeliveryCutoffType> _cutoffType;
        private readonly IGenericRepository<GlobalTimeZone> _timeZones;

        private readonly IMapper _mapper;
        private readonly string rootpath = @"wwwroot/Sliders/";
        public CommonService(IMapper mapper,
                            IGenericRepository<AddressType> addressType,
                            IGenericRepository<Country> country,
                            IGenericRepository<State> state,
                            IGenericRepository<City> city,
                            IGenericRepository<CompanyInformationalText> globalTexts,
                            IGenericRepository<CompanyPostedLink> postedLinks,
                            IGenericRepository<CompanySlider> sliders,
                            IGenericRepository<DeliveryRun> runs,
                            IGenericRepository<Customer> customer,
                            IGenericRepository<Product> product,
                            IGenericRepository<DiscountType> discountType,
                            IGenericRepository<DiscountLimitationType> discountLimitation,
                            IGenericRepository<DeliveryCutoffType> cutoffType,
                            IGenericRepository<GlobalTimeZone> timeZones
                            )
        {
            _mapper = mapper;
            _addressType = addressType;
            _country = country;
            _city = city;
            _state = state;
            _globaltexts = globalTexts;
            _postedLinks = postedLinks;
            _sliders = sliders;
            _runs = runs;
            _customer = customer;
            _product = product;
            _discountType = discountType;
            _discountLimitation = discountLimitation;
            _cutoffType = cutoffType;
            _timeZones = timeZones;
        }

        public IEnumerable<CommonDropdownBO> GetAddressTypes()
        {
            var addressTypes = _addressType.GetAll();
            return _mapper.Map<IEnumerable<CommonDropdownBO>>(addressTypes);
        }

        public IEnumerable<CommonDropdownBO> GetAllCountries()
        {
            var countries = _country.GetAll();
            return _mapper.Map<IEnumerable<CommonDropdownBO>>(countries);
        }

        public IEnumerable<CommonDropdownBO> GetAllStatesByCountry(int countryId)
        {
            var states = _state.GetWithConditions(x => x.CountryId == countryId);
            return _mapper.Map<IEnumerable<CommonDropdownBO>>(states);
        }

        public IEnumerable<CommonDropdownBO> GetAllCitiesByState(int stateId)
        {
            var cities = _city.GetWithConditions(x => x.StateId == stateId);
            return _mapper.Map<IEnumerable<CommonDropdownBO>>(cities);
        }

        public IEnumerable<CommonDropdownBO> GetAllCustomers(PaginatedResultBO model)
        {
            if (!string.IsNullOrEmpty(model.SelectedValues))
            {
                var ids = Array.ConvertAll(model.SelectedValues.Split(','), long.Parse);
                var selected = _customer.GetWithConditions(x => x.CompanyId == model.CompanyId && x.IsActive == true && ids.Contains(x.CustomerId)
                                                             && (string.IsNullOrEmpty(model.SearchString) || x.CustomerName.Contains(model.SearchString))).ToList();
                return _mapper.Map<List<CommonDropdownBO>>(selected);
            }

            var customerCount = _customer.GetAll().Where(x => x.CompanyId == model.CompanyId && x.IsActive == true).Count();

            var customers = _customer.GetWithConditions(x => x.CompanyId == model.CompanyId && x.IsActive == true
                                                          && (x.CustomerName.ToLower().Contains(model.SearchString) || string.IsNullOrEmpty(model.SearchString)))
                                     .OrderBy(o => o.CustomerId)
                                     .Skip(model.PageNumber)
                                     .Take(model.PageSize == 0 ? customerCount : model.PageSize)
                                     .ToList();

            var result = _mapper.Map<List<CommonDropdownBO>>(customers);

            if (result.Count() > 0)
            {
                result[0].TotalRecords = customerCount;
            }

            return result;
        }

        public IEnumerable<CommonDropdownBO> GetAllProducts(PaginatedResultBO model)
        {
            if (!string.IsNullOrEmpty(model.SelectedValues))
            {
                var ids = Array.ConvertAll(model.SelectedValues.Split(','), long.Parse);
                var selected = _product.GetWithConditions(x => x.CompanyId == model.CompanyId && x.IsActive == true && ids.Contains(x.ProductId)
                                                            && (string.IsNullOrEmpty(model.SearchString) || x.Name.Contains(model.SearchString))).ToList();
                return _mapper.Map<List<CommonDropdownBO>>(selected);
            }

            var productCount = _product.GetAll().Where(x => x.CompanyId == model.CompanyId && x.IsActive == true).Count();

            var products = _product.GetWithConditions(x => x.CompanyId == model.CompanyId && x.IsActive == true
                                                       && (string.IsNullOrEmpty(model.SearchString) || x.Name.ToLower().Contains(model.SearchString)))
                                   .OrderBy(o => o.Name)
                                   .Skip(model.PageNumber)
                                   .Take(model.PageSize == 0 ? productCount : model.PageSize)
                                   .ToList();

            var result = _mapper.Map<List<CommonDropdownBO>>(products);

            if (result.Count() > 0)
            {
                result[0].TotalRecords = productCount;
            }

            return result;
        }

        public IEnumerable<CommonDropdownBO> GetCutoffTypes()
        {
            var cutoffTypes = _cutoffType.GetAll();
            return _mapper.Map<IEnumerable<CommonDropdownBO>>(cutoffTypes);
        }

        public IEnumerable<CommonDropdownBO> GetTimeZones()
        {
            var timeZones = _timeZones.GetAll();
            return _mapper.Map<IEnumerable<CommonDropdownBO>>(timeZones);
        }

        #region Global Text section
        public async Task<CompanyGlobalTextBO> GetCompanyGlobaltexts(long companyId)
        {
            var texts = await _globaltexts.GetSingleWithConditions(x => x.CompanyId == companyId);
            if (texts == null)
                texts = new CompanyInformationalText();

            return _mapper.Map<CompanyGlobalTextBO>(texts);
        }

        public async Task<BaseResponseBO> AddUpdateCompanyGlobaltexts(CompanyGlobalTextBO model)
        {
            try
            {
                var response = new BaseResponseBO() { IsSuccess = true };
                var record = new CompanyInformationalText();
                record = await _globaltexts.Get(model.CompanyId);
                if (record == null)
                {
                    record = new CompanyInformationalText();
                    _mapper.Map(model, record);
                    await _globaltexts.AddAsync(record);
                    response.Message = "Record added successfully.";
                }
                else
                {
                    _mapper.Map(model, record);
                    await _globaltexts.UpdateAsync(record);
                    response.Message = "Record updated successfully.";
                }

                return response;
            }
            catch (Exception ex)
            {
                return new BaseResponseBO()
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }
        #endregion

        #region Posted Links section
        public IEnumerable<PostedLinksBO> GetCompanyPostedLinks(long companyId)
        {
            var links = _postedLinks.GetWithConditions(x => x.CompanyId == companyId);

            return _mapper.Map<IEnumerable<PostedLinksBO>>(links);
        }

        public async Task<BaseResponseBO> AddUpdateCompanyPostedLinks(PostedLinksBO model)
        {
            var response = new BaseResponseBO() { IsSuccess = true };
            try
            {
                var record = new CompanyPostedLink();
                if (model.Id == 0)
                {
                    _mapper.Map(model, record);
                    await _postedLinks.AddAsync(record);
                    response.Message = "Link added successfully";
                }
                else
                {
                    record = await _postedLinks.GetSingleWithConditions(x => x.Id == model.Id);
                    _mapper.Map(model, record);

                    await _postedLinks.UpdateAsync(record);
                    response.Message = "Link Updated successfully";
                }

                return response;
            }
            catch (Exception ex)
            {
                return new BaseResponseBO()
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }

        public async Task<BaseResponseBO> DeleteCompanyPostedLinks(long id)
        {
            try
            {
                await _postedLinks.Delete(id);
                return new BaseResponseBO()
                {
                    IsSuccess = true,
                    Message = "Link deleted successfully"
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseBO()
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }
        #endregion

        #region Company Slider section
        public IEnumerable<CompanySlidersBO> GetCompanySliders(long companyId, string domain)
        {
            var links = _sliders.GetWithConditions(x => x.CompanyId == companyId);

            var finalresult = _mapper.Map<IEnumerable<CompanySlidersBO>>(links);
            string storePath = $"{rootpath}{domain}/";
            var format = "image/png";

            foreach (var item in finalresult)
            {
                List<FileViewModel> list = new List<FileViewModel>();
                string fullpath = Path.Combine(Directory.GetCurrentDirectory(), storePath);
                fullpath = fullpath + "/" + item.Image;

                if (File.Exists(fullpath))
                {
                    FileViewModel viewmodel = new FileViewModel();

                    var imageBytes = System.IO.File.ReadAllBytes(fullpath);
                    viewmodel.Content = imageBytes;
                    viewmodel.ContentType = format;
                    viewmodel.Size = imageBytes.Length;
                    viewmodel.ImageDataUrl = $"data:{format};base64,{Convert.ToBase64String(imageBytes)}";
                    viewmodel.Name = item.Image;

                    list.Add(viewmodel);
                }
                item.SliderImagesList = list.AsEnumerable();
            }

            return finalresult;
        }

        public async Task<BaseResponseBO> AddUpdateCompanySliders(CompanySlidersBO model)
        {
            var response = new BaseResponseBO() { IsSuccess = true };
            try
            {
                var record = new CompanySlider();
                if (model.Id == 0)
                {
                    _mapper.Map(model, record);
                    await _sliders.AddAsync(record);
                    SaveImages(model);
                    response.Message = "Slider added successfully";
                }
                else
                {
                    record = await _sliders.GetSingleWithConditions(x => x.Id == model.Id);
                    string rootFolder = $"{rootpath}{model.DomainName}/";
                    if (File.Exists(Path.Combine(rootFolder, record.Image)))
                    {
                        File.Delete(Path.Combine(rootFolder, record.Image));
                    }
                    _mapper.Map(model, record);
                    await _sliders.UpdateAsync(record);
                    SaveImages(model);
                    response.Message = "Slider updated successfully";
                }

                return response;
            }
            catch (Exception ex)
            {
                return new BaseResponseBO()
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }

        public async Task<BaseResponseBO> DeleteCompanySlider(long id)
        {
            try
            {
                var record = await _sliders.GetSingleWithConditions(x => x.Id == id, "Company");
                var domain = record.Company.DomainName;

                string rootFolder = $"{rootpath}{domain}/";
                if (File.Exists(Path.Combine(rootFolder, record.Image)))
                {
                    File.Delete(Path.Combine(rootFolder, record.Image));
                }

                await _sliders.Delete(id);

                return new BaseResponseBO()
                {
                    IsSuccess = true,
                    Message = "Link deleted successfully"
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseBO()
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }

        private void SaveImages(CompanySlidersBO model)
        {
            try
            {
                string storePath = $"{rootpath}{model.DomainName}/";

                string fullpath = Path.Combine(Directory.GetCurrentDirectory(), storePath);
                if (!Directory.Exists(fullpath))
                {
                    Directory.CreateDirectory(fullpath);
                }
                if (model.SliderImagesList.Count() > 0)
                {

                    foreach (var item in model.SliderImagesList)
                    {

                        //  var filename = $"{Guid.NewGuid().ToString("N").ToUpper()}{Path.GetExtension(item.Name)}";
                        // model.Image = filename;
                        var path = Path.Combine(
                                        Directory.GetCurrentDirectory(), storePath,
                                       item.Name);

                        using (var ms = new MemoryStream(item.Content))
                        {
                            using (var fs = new FileStream(path, FileMode.Create))
                            {
                                ms.WriteTo(fs);
                            }
                        }
                        break;
                    }
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }


        #endregion

        #region Delivery runs section
        public IEnumerable<DeliveryRunBO> GetDeliveryRuns(long companyId)
        {
            var runs = _runs.GetWithConditions(x => x.CompanyId == companyId);

            return _mapper.Map<IEnumerable<DeliveryRunBO>>(runs);
        }

        public async Task<BaseResponseBO> AddUpdateDeliveryRun(DeliveryRunBO model)
        {
            var response = new BaseResponseBO() { IsSuccess = true };
            try
            {
                var record = new DeliveryRun();
                if (model.Id == 0)
                {
                    _mapper.Map(model, record);
                    await _runs.AddAsync(record);
                    response.Message = "Delivery run added.";
                }
                else
                {
                    record = await _runs.Get(model.Id);
                    _mapper.Map(model, record);
                    await _runs.UpdateAsync(record);
                    response.Message = "Delivery run updated";
                }

                return response;
            }
            catch (Exception ex)
            {
                return new BaseResponseBO()
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }

        public async Task<BaseResponseBO> DeleteDeliveryRun(long id)
        {
            try
            {
                var run = await _runs.GetSingleWithConditions(x => x.Id == id, "Customers");

                if (run.Customers.Count() > 0)
                {
                    run.Customers.Clear();
                }

                await _runs.Delete(id);
                return new BaseResponseBO()
                {
                    IsSuccess = true,
                    Message = "Delivery run deleted"
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseBO()
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }

        #endregion

        #region Discount 

        public IEnumerable<CommonDropdownBO> GetDiscountType()
        {
            var discountTypes = _discountType.GetAll();
            return _mapper.Map<IEnumerable<CommonDropdownBO>>(discountTypes);
        }

        public IEnumerable<CommonDropdownBO> GetDiscountLimitationType()
        {
            var limitationTypes = _discountLimitation.GetAll().OrderByDescending(o => o.TypeDesc);
            return _mapper.Map<IEnumerable<CommonDropdownBO>>(limitationTypes);
        }
        #endregion
    }
}
