using AdminPanel.Shared.BO;
using AdminPanel.DataModel.Models;
using AdminPanel.Shared.BO.CompanyAdmin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdminPanel.Services.Repository;
using AutoMapper;

namespace AdminPanel.Services.ServicesLayer
{
    public class CategoriesService : ICategoriesService
    {
        private readonly IGenericRepository<MainCategory> _maincategory;
        private readonly IGenericRepository<SubCategory> _subcategory;
        private readonly IGenericRepository<Filter> _filter;
        private readonly IGenericRepository<UnitOfMeasurement> _unitofmeasurement;
        private readonly IMapper _mapper;

        public CategoriesService(IGenericRepository<MainCategory> maincategory,
                                 IGenericRepository<SubCategory> subcategory, 
                                 IGenericRepository<Filter> filter, 
                                 IGenericRepository<UnitOfMeasurement> unitofmeasurement,
                                 IMapper mapper)
        {
            _maincategory = maincategory;
            _subcategory = subcategory;
            _filter = filter;
            _unitofmeasurement = unitofmeasurement;
            _mapper = mapper;
        }

        public async Task<BaseResponseBO> AddUpdateCategories(CategoriesBO model)
        {
            try
            {
                var response = new BaseResponseBO() { IsSuccess = true };
                var category = _mapper.Map<MainCategory>(model);

                if (model.Id == 0)
                {
                    category.CreatedOn = DateTime.UtcNow;
                    await _maincategory.AddAsync(category);
                    response.Message = "success";
                }
                else
                {
                    category.ModifiedOn = DateTime.UtcNow;
                    await _maincategory.UpdateAsync(category);
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

        public async Task<BaseResponseBO> AddUpdateFilter(FiltersBO model)
        {
            try
            {
                var response = new BaseResponseBO() { IsSuccess = true };
                var filter = _mapper.Map<Filter>(model);

                if (model.FilterId == 0)
                {
                    filter.CreatedOn = DateTime.UtcNow;
                    await _filter.AddAsync(filter);
                    response.Message = "success";
                }
                else
                {
                    filter.ModifiedOn = DateTime.UtcNow;
                    await _filter.UpdateAsync(filter);
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

        public async Task<BaseResponseBO> AddUpdateSubCategories(SubCategoriesBO model)
        {
            try
            {
                var response = new BaseResponseBO() { IsSuccess = true };
                var category = _mapper.Map<SubCategory>(model);

                if (model.Id == 0)
                {
                    category.CreatedOn = DateTime.UtcNow;
                    await _subcategory.AddAsync(category);
                    response.Message = "success";
                }
                else
                {
                    category.ModifiedOn = DateTime.UtcNow;
                    await _subcategory.UpdateAsync(category);
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

        public async Task<BaseResponseBO> DeleteCategory(CategoriesBO model)
        {
            var response = new BaseResponseBO() { IsSuccess = true };

            var subcategories = _subcategory.GetWithConditions(x => x.MainCategoryId == model.Id).ToList();

            if (subcategories != null && subcategories.Count > 0)
            {
                await _subcategory.DeleteRange(subcategories);
            }

            var type = await _maincategory.Delete(model.Id);
            if (type == null)
            {
                response.IsSuccess = false;
            }

            return response;
        }

        public async Task<BaseResponseBO> DeleteFilter(FiltersBO model)
        {
            var response = new BaseResponseBO() { IsSuccess = true };
            var type = await _filter.Delete(model.FilterId);
            if (type == null)
            {
                response.IsSuccess = false;
            }

            return response;
        }

        public async Task<BaseResponseBO> DeleteSubCategory(SubCategoriesBO model)
        {
            var response = new BaseResponseBO() { IsSuccess = true };
            var type = await _subcategory.Delete(model.Id);
            if (type == null)
            {
                response.IsSuccess = false;
            }

            return response;
        }

        public async Task<IEnumerable<CategoriesBO>> GetAllCategories(long companyId)
        {
            var categories = _maincategory.GetAll().Where(x => x.CompanyId == companyId)
                                          .OrderBy(o => o.SortOrder)
                                          .ToList();
            return _mapper.Map<IEnumerable<CategoriesBO>>(categories);
        }

        public async Task<IEnumerable<SubCategoriesBO>> GetAllSubCategories(long id)
        {
            var subcategories = _subcategory.GetWithConditions(x => x.MainCategoryId == id)
                                            .OrderBy(o => o.SortOrder)
                                            .ToList();
            return _mapper.Map<IEnumerable<SubCategoriesBO>>(subcategories);
        }

        public async Task<IEnumerable<FiltersBO>> GetAllfilters(long companyId)
        {
            var categories = _filter.GetAll().Where(x => x.CompanyId == companyId)
                                          .ToList();
            return _mapper.Map<IEnumerable<FiltersBO>>(categories);
        }


        public async Task<IEnumerable<UnitOfMeasureBO>> GetAllUnitofMeasures(long companyId)
        {
            var categories = _unitofmeasurement.GetAll().Where(x => x.CompanyId == companyId)
                                          .ToList();
            return _mapper.Map<IEnumerable<UnitOfMeasureBO>>(categories);

        }
        public async Task<IEnumerable<CategoriesBO>> GetAllSpecialCategories(long companyId)
        {
            var categories = _maincategory.GetAll().Where(x => x.CompanyId == companyId && x.IsSpecial == true)
                                          .OrderBy(o => o.SortOrder)
                                          .ToList();
            return _mapper.Map<IEnumerable<CategoriesBO>>(categories);

        }

        public async Task<SubCategoriesBO> GetSubCategoryById(long id)
        {
            var subcategories = _subcategory.GetWithConditions(x => x.Id == id)
                                            .FirstOrDefault();
            return _mapper.Map<SubCategoriesBO>(subcategories);
        }

        public async Task<BaseResponseBO> DeleteUOM(UnitOfMeasureBO model)
        {
            var response = new BaseResponseBO() { IsSuccess = true };
            var type = await _unitofmeasurement.Delete(model.UnitId);
            if (type == null)
            {
                response.IsSuccess = false;
            }

            return response;
        }

        public async Task<BaseResponseBO> AddEditUOM(UnitOfMeasureBO model)
        {
            try
            {
                var response = new BaseResponseBO() { IsSuccess = true };
                var uom = _mapper.Map<UnitOfMeasurement>(model);

                if (model.UnitId == 0)
                {
                    uom.CreatedOn = DateTime.UtcNow;
                    await _unitofmeasurement.AddAsync(uom);
                    response.Message = "success";
                }
                else
                {
                    uom.ModifiedOn = DateTime.UtcNow;
                    await _unitofmeasurement.UpdateAsync(uom);
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
