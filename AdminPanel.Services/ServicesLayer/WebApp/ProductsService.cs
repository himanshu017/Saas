using AdminPanel.DataModel.Models;
using AdminPanel.Services.Repository;
using AdminPanel.Shared.BO;
using AdminPanel.Shared.BO.CompanyAdmin;
using AdminPanel.Shared.BO.WebApp;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdminPanel.Services.Helpers;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;

namespace AdminPanel.Services.ServicesLayer
{
    /// <summary>
    /// This inherits all the generic methods + additional custom methods as required for complex queries.
    /// </summary>
    public class ProductsService : IProductsService
    {
        private readonly IProductRepository _repo;
        private readonly IGenericRepository<FavoriteList> _favList;
        private readonly IGenericRepository<FavoriteListItem> _favListItem;
        private readonly IMapper _mapper;

        public ProductsService(IProductRepository repo,
                               IGenericRepository<FavoriteList> favList,
                               IGenericRepository<FavoriteListItem> favListItem,
                               IMapper mapper)
        {
            _repo = repo;
            _favList = favList;
            _favListItem = favListItem;
            _mapper = mapper;
        }

        public async Task<GenericResponse<IEnumerable<FiltersBO>>> GetAllFilters(long companyId)
        {
            try
            {
                var filters = await _repo.GetAllFilters(companyId);

                return new GenericResponse<IEnumerable<FiltersBO>>(_mapper.Map<IEnumerable<FiltersBO>>(filters));
            }
            catch (Exception ex)
            {
                return new GenericResponse<IEnumerable<FiltersBO>>()
                {
                    IsSuccess = false,
                    Message = $"Error: {ex.Message} :: Inner: {ex.InnerException?.Message}"
                };
            }
        }

        public async Task<GenericResponse<IEnumerable<CategoryListBO>>> GetAllMainAndSubCategories(GetAllCategoriesBO model)
        {
            try
            {
                var res = await _repo.GetAllMainAndSubCategories(model);

                return new GenericResponse<IEnumerable<CategoryListBO>>(_mapper.Map<IEnumerable<CategoryListBO>>(res));
            }
            catch (Exception ex)
            {
                return new GenericResponse<IEnumerable<CategoryListBO>>()
                {
                    IsSuccess = false,
                    Message = $"Error: {ex.Message} :: Inner: {ex.InnerException?.Message}"
                };
            }
        }

        public async Task<GenericResponse<IEnumerable<UnitOfMeasureBO>>> GetAllUnits(long companyId)
        {
            try
            {
                var units = await _repo.GetAllUnits(companyId);

                return new GenericResponse<IEnumerable<UnitOfMeasureBO>>(_mapper.Map<IEnumerable<UnitOfMeasureBO>>(units));
            }
            catch (Exception ex)
            {
                return new GenericResponse<IEnumerable<UnitOfMeasureBO>>()
                {
                    IsSuccess = false,
                    Message = $"Error: {ex.Message} :: Inner: {ex.InnerException?.Message}"
                };
            }
        }

        public async Task<GenericPagedResponse<IEnumerable<ProductListBO>>> GetProductsBySearchFilters(GetProductListBO model)
        {
            try
            {
                var res = await _repo.GetProductsBySearchFilters(model);
                var totalRecords = res.Count();

                var data = res.OrderBy(model.SortColumn)
                              .Skip(model.PageNumber * model.PageSize)
                              .Take(model.PageSize)
                              .ToList();

                var mappedData = _mapper.Map<List<ProductListBO>>(data);

                var response = await _repo.GetAdditionalProductInfo(mappedData, model.UserId, model.CustomerId, model.CompanyId);

                return new GenericPagedResponse<IEnumerable<ProductListBO>>(response, totalRecords, model.PageSize);
            }
            catch (Exception ex)
            {
                return new GenericPagedResponse<IEnumerable<ProductListBO>>()
                {
                    IsSuccess = false,
                    Message = $"Error: {ex.Message} :: Inner: {ex.InnerException?.Message}"
                };
            }
        }

        public async Task<GenericResponse<IEnumerable<FavoriteListBO>>> GetFavoriteLists(GetFavLists model)
        {
            try
            {
                var favLists = await _repo.GetFavoriteLists(model);

                return new GenericResponse<IEnumerable<FavoriteListBO>>(_mapper.Map<IEnumerable<FavoriteListBO>>(favLists));
            }
            catch (Exception ex)
            {
                return new GenericResponse<IEnumerable<FavoriteListBO>>()
                {
                    IsSuccess = false,
                    Message = $"Error: {ex.Message} :: Inner: {ex.InnerException?.Message}"
                };
            }
        }

        public async Task<GenericPagedResponse<IEnumerable<ProductListBO>>> GetFavoriteListItems(GetProductListBO model)
        {
            try
            {
                var res = await _repo.GetFavoriteListItems(model);
                var totalRecords = res.Count();

                var data = res.OrderBy(p => p.Name)
                              .Skip(model.PageNumber * model.PageSize)
                              .Take(model.PageSize)
                              .ToList();

                var mappedData = _mapper.Map<List<ProductListBO>>(data);

                var response = await _repo.GetAdditionalProductInfo(mappedData, model.UserId, model.CustomerId, model.CompanyId);

                return new GenericPagedResponse<IEnumerable<ProductListBO>>(response, totalRecords, model.PageSize);
            }
            catch (Exception ex)
            {
                return new GenericPagedResponse<IEnumerable<ProductListBO>>()
                {
                    IsSuccess = false,
                    Message = $"Error: {ex.Message} :: Inner: {ex.InnerException?.Message}"
                };
            }
        }

        public async Task<GenericPagedResponse<IEnumerable<ProductListBO>>> GetCompanySpecials(GetProductListBO model)
        {
            try
            {
                var res = await _repo.GetCompanySpecials(model);

                var totalRecords = res.Count();

                var data = res.OrderBy(model.SortColumn)
                              .Skip(model.PageNumber * model.PageSize)
                              .Take(model.PageSize)
                              .ToList();

                var mappedData = _mapper.Map<List<ProductListBO>>(data);

                var response = await _repo.GetAdditionalProductInfo(mappedData, model.UserId, model.CustomerId, model.CompanyId);

                return new GenericPagedResponse<IEnumerable<ProductListBO>>(response, totalRecords, model.PageSize);
            }
            catch (Exception ex)
            {
                return new GenericPagedResponse<IEnumerable<ProductListBO>>()
                {
                    IsSuccess = false,
                    Message = $"Error: {ex.Message} :: Inner: {ex.InnerException?.Message}"
                };
            }
        }

        public async Task<GenericResponse<bool>> AddUpdateFavList(FavoriteListBO model)
        {
            try
            {
                var favList = new FavoriteList();

                if (model.Id == 0 || model.IsCopy)
                {
                    model.Id = 0;

                    _mapper.Map(model, favList);
                    favList.IsActive = true;

                    await _favList.AddAsync(favList);

                    if (model.IsCopy)
                    {
                        var list = await _favList.GetSingleWithConditions(x => x.Id == model.MainListId, "FavoriteListItems");
                        var listItems = new List<FavoriteListItem>();

                        foreach (var item in list.FavoriteListItems)
                        {
                            listItems.Add(new FavoriteListItem()
                            {
                                ListId = favList.Id,
                                ProductId = item.ProductId,
                                Price = item.Price,
                                IsActive = true,
                                UnitId = item.UnitId,
                                CreatedBy = model.CreatedBy,
                                CreatedOn = model.CreatedOn
                            });

                        }

                        favList.FavoriteListItems = listItems;
                        await _favList.UpdateAsync(favList);
                    }
                }
                else
                {
                    favList = await _favList.GetSingleWithConditions(x => x.Id == model.Id);
                    _mapper.Map(model, favList);

                    await _favList.UpdateAsync(favList);
                }

                return new GenericPagedResponse<bool>()
                {
                    Data = true,
                    IsSuccess = true,
                    Message = $"Success"
                };
            }
            catch (Exception ex)
            {
                return new GenericPagedResponse<bool>()
                {
                    IsSuccess = false,
                    Message = $"Error: {ex.Message} :: Inner: {ex.InnerException?.Message}"
                };
            }
        }

        public async Task<GenericResponse<bool>> DeleteFavList(long id)
        {
            try
            {
                var favList = await _favList.GetSingleWithConditions(x => x.Id == id, "FavoriteListItems");
                bool hasRecord = false;

                if (favList != null)
                {
                    hasRecord = true;

                    // delete all the items for the List
                    await _favListItem.DeleteRange(favList.FavoriteListItems);

                    // delete the list itself
                    await _favList.Delete(id);
                }

                return new GenericPagedResponse<bool>()
                {
                    Data = true,
                    IsSuccess = hasRecord,
                    Message = hasRecord ? "Success" : "No record found"
                };

            }
            catch (Exception ex)
            {
                return new GenericPagedResponse<bool>()
                {
                    IsSuccess = false,
                    Message = $"Error: {ex.Message} :: Inner: {ex.InnerException?.Message}"
                };
            }
        }

        public async Task<GenericResponse<bool>> AddDeleteFavlistItem(ManageFavListItemBO model)
        {
            try
            {
                bool result = false;

                if (model.IsDelete)
                {
                    var item = await _favListItem.GetSingleWithConditions(x => x.ListId == model.ListId && x.ProductId == model.ProductId);

                    if (item != null)
                    {
                        result = true;
                        await _favListItem.Delete(item);
                    }

                    return new GenericPagedResponse<bool>()
                    {
                        Data = true,
                        IsSuccess = result,
                        Message = result ? "Item deleted from list" : "No record found"
                    };
                }
                else
                {
                    var exists = _favListItem.GetWithConditions(x => x.ListId == model.ListId && x.ProductId == model.ProductId).Count() > 0;

                    if (exists)
                    {
                        return new GenericPagedResponse<bool>()
                        {
                            Data = false,
                            IsSuccess = false,
                            Message = "Item already exists in the selected list."
                        };
                    }

                    var favListItem = new FavoriteListItem()
                    {
                        ProductId = model.ProductId,
                        ListId = model.ListId,
                        IsActive = true,
                        CreatedBy = model.UserId,
                        CreatedOn = DateTime.UtcNow,
                    };

                    await _favListItem.AddAsync(favListItem);
                    result = true;

                    return new GenericPagedResponse<bool>()
                    {
                        Data = true,
                        IsSuccess = result,
                        Message = result ? "Item added to list" : "Error"
                    };
                }
            }
            catch (Exception ex)
            {
                return new GenericPagedResponse<bool>()
                {
                    IsSuccess = false,
                    Message = $"Error: {ex.Message} :: Inner: {ex.InnerException?.Message}"
                };
            }
        }

        public async Task<GenericResponse<IEnumerable<ProductListBO>>> GetTempCartItems(GetProductListBO model)
        {
            try
            {
                var res = await _repo.GetTempCartItems(model);
                var totalRecords = res.Count();

                var mappedData = _mapper.Map<List<ProductListBO>>(res.ToList());

                var response = await _repo.GetAdditionalProductInfo(mappedData, model.UserId, model.CustomerId, model.CompanyId, true);

                return new GenericResponse<IEnumerable<ProductListBO>>(response);
            }
            catch (Exception ex)
            {
                return new GenericPagedResponse<IEnumerable<ProductListBO>>()
                {
                    IsSuccess = false,
                    Message = $"Error: {ex.Message} :: Inner: {ex.InnerException?.Message}"
                };
            }
        }

        public async Task<GenericResponse<ProductListBO>> GetProductDetail(GetProdDetailBO model)
        {
            try
            {
                var res = await _repo.GetProductDetail(model);
                var mappedData = _mapper.Map<List<ProductListBO>>(res);
                ProductListBO data = new();

                if(model.CustomerId > 0)
                {
                    var response = await _repo.GetAdditionalProductInfo(mappedData, model.UserId, model.CustomerId, model.CompanyId);
                    data = response.FirstOrDefault();
                }
                else
                {
                    data = mappedData.FirstOrDefault();
                }
                

                return new GenericResponse<ProductListBO>()
                {
                    IsSuccess = true,
                    Data = data,
                    Message = "Success"
                };
            }
            catch (Exception ex)
            {
                return new GenericResponse<ProductListBO>()
                {
                    IsSuccess = false,
                    Message = $"Error: {ex.Message} :: Inner: {ex.InnerException?.Message}"
                };
            }
        }

        public async Task<GenericResponse<IEnumerable<ProductListBO>>> GetSuggestiveProducts(GetProductListBO model)
        {
            try
            {
                var res = await _repo.GetSuggestiveProducts(model);
                var mappedData = _mapper.Map<List<ProductListBO>>(res);
                List<ProductListBO> data = new();

                if (model.CustomerId > 0)
                {
                    data = await _repo.GetAdditionalProductInfo(mappedData, model.UserId, model.CustomerId, model.CompanyId);
                }
                else
                {
                    data = mappedData;
                }


                return new GenericResponse<IEnumerable<ProductListBO>>()
                {
                    IsSuccess = true,
                    Data = data,
                    Message = "Success"
                };
            }
            catch (Exception ex)
            {
                return new GenericResponse<IEnumerable<ProductListBO>>()
                {
                    IsSuccess = false,
                    Message = $"Error: {ex.Message} :: Inner: {ex.InnerException?.Message}"
                };
            }
        }
    }
}
