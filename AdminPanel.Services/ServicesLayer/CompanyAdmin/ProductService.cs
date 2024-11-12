using AdminPanel.DataModel.Models;
using AdminPanel.Services.Helpers;
using AdminPanel.Services.Repository;
using AdminPanel.Shared.BO;
using AdminPanel.Shared.BO.CompanyAdmin;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanel.Services.ServicesLayer.CompanyAdmin
{
    public class ProductService : IProductService
    {
        private readonly IGenericRepository<Product> _product;
        private readonly IGenericRepository<ProductImage> _productImages;
        private readonly IGenericRepository<CompanySpecial> _specials;
        private readonly IGenericRepository<Tag> _tags;
        private readonly IMapper _mapper;
        private readonly string rootpath = @"wwwroot/ProductsImages/";
        public ProductService(IGenericRepository<Product> product,
                              IGenericRepository<ProductImage> productImages,
                              IGenericRepository<CompanySpecial> specials,
                              IGenericRepository<Tag> tags,
                              IMapper mapper)
        {
            _product = product;
            _productImages = productImages;
            _specials = specials;
            _mapper = mapper;
            _tags = tags;
        }

        public async Task<BaseResponseBO> AddUpdateProduct(ProductBO model)
        {
            try
            {
                var response = new BaseResponseBO() { IsSuccess = true };
                var product = new Product();
                if (model.ProductId == 0)
                {
                    _mapper.Map(model, product);

                    SaveProductImages(model, product);
                    string imgResponse = SaveImages(model);
                    product.IsActive = true;

                    AddUpdateSuggestiveProducts(model, product);
                    await AddUpdateProductTags(model, product);

                    await _product.AddAsync(product);
                    response.Message = (imgResponse == "Success" ? "Product record created!" : "Product record created! Issue with uploading product images, please try adding the images again.");
                }
                else
                {
                    if (model.ProductImagesList.Count() > 0)
                    {
                        var imagesList = _productImages.GetAll().Where(x => x.ProductId == model.ProductId).ToList();
                        await _productImages.DeleteRange(imagesList);

                        string rootFolder = $"{rootpath}{model.DomainName}/";
                        foreach (var item in imagesList)
                        {
                            if (File.Exists(Path.Combine(rootFolder, item.ImageName)))
                            {
                                // If file found, delete it    
                                File.Delete(Path.Combine(rootFolder, item.ImageName));
                            }
                        }
                    }

                    product = await _product.GetSingleWithConditions(x => x.ProductId == model.ProductId, "ProductUnits,SuggestiveProducts,Tags");
                    product.ProductUnits.Clear();

                    _mapper.Map(model, product);

                    SaveProductImages(model, product);
                    string imgResponse = SaveImages(model);

                    AddUpdateSuggestiveProducts(model, product);
                    await AddUpdateProductTags(model, product);

                    await _product.UpdateAsync(product);

                    response.Message = (imgResponse == "Success" ? "Product record updated!" : "Product record updated! Issue with uploading product images, please try adding the images again.");
                }

                return response;
            }
            catch (Exception ex)
            {
                return new BaseResponseBO()
                {
                    IsSuccess = false,
                    Message = $"{ex.Message} :: Inner Exception:: {ex.InnerException?.Message} ",
                };
            }
        }

        private string SaveImages(ProductBO model)
        {
            try
            {
                string storePath = $"{rootpath}{model.DomainName}/";

                string fullpath = Path.Combine(Directory.GetCurrentDirectory(), storePath);
                if (!Directory.Exists(fullpath))
                {
                    Directory.CreateDirectory(fullpath);
                }
                if (model.ProductImagesList.Count() > 0)
                {
                    int index = 1;

                    foreach (var item in model.ProductImagesList)
                    {
                        var filename = Guid.NewGuid() + Path.GetExtension(item.Name);

                        if (item.ContentType == "image/png" || item.ContentType == "image/jpeg")
                        {
                            filename = $"{model.Code}_{index}{Path.GetExtension(item.Name)}";
                        }
                        var path = Path.Combine(
                                        Directory.GetCurrentDirectory(), storePath,
                                       filename);

                        using (var ms = new MemoryStream(item.Content))
                        {
                            using (var fs = new FileStream(path, FileMode.Create))
                            {
                                ms.WriteTo(fs);
                            }
                        }
                        index++;
                        // string sspath = "thumb_" + filename;
                        //var thumbpath = Path.Combine(Directory.GetCurrentDirectory(), storePath, sspath);

                        //SaveThumbnail(path, 232, 242, 85, thumbpath, "thumb");

                    }
                }

                return "Success";
            }
            catch (Exception ex)
            {
                return "Error saving product images";
            }
        }

        private void SaveProductImages(ProductBO model, Product product)
        {
            int Index = 1;
            for (int i = 0; i < model.ProductImagesList.Count(); i++)
            {
                ProductImage productImageBO = new ProductImage();
                productImageBO.ProductId = product.ProductId;
                productImageBO.ImageName = $"{product.Code}_{Index}{Path.GetExtension(model.ProductImagesList.ElementAt(i).Name)}";
                productImageBO.DisplayOrder = Convert.ToByte(Index);
                product.ProductImages.Add(productImageBO);
                Index++;
            }
        }

        private void AddUpdateSuggestiveProducts(ProductBO model, Product product)
        {
            if (model.SuggestiveProductIds != null)
            {
                product.SuggestiveProducts.Clear();

                var suggestiveProducts = _product.GetWithConditions(x => model.SuggestiveProductIds.Contains(x.ProductId)).ToList();

                suggestiveProducts.ForEach(x => product.SuggestiveProducts.Add(x));
            }
        }

        private async Task AddUpdateProductTags(ProductBO model, Product product)
        {
            if (model.ProductTags != null && model.ProductTags.Count() > 0)
            {
                product.Tags.Clear();

                var existingTags =  _tags.GetAll().ToList();

                var uniqueTags = model.ProductTags.Except(existingTags.Select(x => x.Title), StringComparer.OrdinalIgnoreCase);

                var addToTags = uniqueTags.Select(x => new Tag
                {
                    Title = x
                }).ToList();

                await _tags.BulkInsert(addToTags);

                var prodTags = _tags.GetWithConditions(x => model.ProductTags.Contains(x.Title)).ToList();

                prodTags.ForEach(x => product.Tags.Add(x));
            }
        }

        public IEnumerable<ProductBO> GetAllProducts(GetProductModel model)
        {
            var productCount = _product.GetAll().Where(x => x.CompanyId == model.CompanyId && x.IsActive == true).Count();
            var products = _product.GetAll("ProductImages,ProductUnits")
                                     .Where(x => x.CompanyId == model.CompanyId)
                                     .OrderByDescending(o => o.ModifiedOn)
                                     .Skip(model.PageSize * model.PageNumber)
                                     .Take(model.PageSize == 0 ? productCount : model.PageSize)
                                     .ToList();
            var res = _mapper.Map<IEnumerable<ProductBO>>(products);
            if (productCount > 0)
            {
                res.ToList()[0].TotalRecords = productCount;
            }
            return res;
        }

        public async Task<ProductBO> GetProductById(long id, string domain)
        {
            var product = await _product.GetSingleWithConditions(x => x.ProductId == id, "ProductImages,ProductUnits,ProductUnits.Unit,SuggestiveProducts,Tags");

            var res = _mapper.Map<ProductBO>(product);
            string storePath = $"{rootpath}{domain}/";
            var format = "image/png";
            foreach (var item in res.ProductImagesList)
            {
                string fullpath = Path.Combine(Directory.GetCurrentDirectory().Replace("server", "client"), storePath);
                fullpath = fullpath + "/" + item.Name;

                if (fullpath.IndexOf("https") == -1 && File.Exists(fullpath))
                {
                    var imageBytes = System.IO.File.ReadAllBytes(fullpath);
                    item.Content = imageBytes;
                    item.ContentType = format;
                    item.Size = imageBytes.Length;
                    item.ImageDataUrl = $"data:{format};base64,{Convert.ToBase64String(imageBytes)}";
                }
            }

            res.AllExistingTags = _tags.GetAll().Select(s => s.Title).ToList();

            return res;
        }

        public async Task<BaseResponseBO> DeleteProduct(ProductBO model)
        {
            try
            {
                var response = new BaseResponseBO() { IsSuccess = true };
                var product = new Product();

                var imagesList = _productImages.GetAll().Where(x => x.ProductId == model.ProductId).ToList();
                await _productImages.DeleteRange(imagesList);

                string rootFolder = $"{rootpath}{model.DomainName}/";
                foreach (var item in imagesList)
                {
                    if (File.Exists(Path.Combine(rootFolder, item.ImageName)))
                    {
                        // If file found, delete it    
                        File.Delete(Path.Combine(rootFolder, item.ImageName));
                    }
                }

                var type = await _product.Delete(model.ProductId);
                response.Message = "Product record deleted!";
                if (type == null)
                {
                    response.IsSuccess = false;
                    response.Message = "Product record not found!";
                }

                return response;
            }
            catch (Exception ex)
            {
                return new BaseResponseBO()
                {
                    IsSuccess = false,
                    Message = $"{ex.Message} :: Inner Exception:: {ex.InnerException?.Message} ",
                };
            }
        }

        public async Task<GetSpecialsResponse> GetCompanySpecials(long companyId)
        {
            try
            {
                var specials = _specials.GetWithConditions(x => x.CompanyId == companyId).ToList();

                return new GetSpecialsResponse()
                {
                    IsSuccess = true,
                    Message = "Success",
                    ProductIds = (specials != null && specials.Count > 0) ? specials.Select(s => s.ProductId) : null
                };
            }
            catch (Exception ex)
            {
                return new GetSpecialsResponse()
                {
                    IsSuccess = false,
                    Message = $"{ex.Message} :: Inner Exception:: {ex.InnerException?.Message} ",
                };
            }
        }

        public async Task<BaseResponseBO> AddUpdateCompanySpecials(CompanySpecialsBO model)
        {
            try
            {
                var specials = _specials.GetWithConditions(x => x.CompanyId == model.CompanyId).ToList();
                var companySpecials = new List<CompanySpecial>();

                if (specials == null || specials.Count == 0)
                {
                    foreach (var item in model.ProductIds)
                    {
                        companySpecials.Add(new CompanySpecial()
                        {
                            CompanyId = model.CompanyId,
                            ProductId = item,
                            CreatedBy = model.UserId,
                            CreatedOn = DateTime.UtcNow
                        });
                    }

                    await _specials.BulkInsert(companySpecials);
                }
                else
                {
                    var existing = specials.Select(s => s.ProductId);

                    var unChanged = existing.Intersect(model.ProductIds).ToList();
                    var removed = existing.Except(unChanged).ToList();
                    var added = model.ProductIds.Except(unChanged).Except(removed).ToList();

                    foreach (var item in added)
                    {
                        companySpecials.Add(new CompanySpecial()
                        {
                            CompanyId = model.CompanyId,
                            ProductId = item,
                            CreatedBy = model.UserId,
                            CreatedOn = DateTime.UtcNow
                        });
                    }

                    await _specials.BulkInsert(companySpecials);

                    if (removed != null && removed.Count > 0)
                    {
                        await _specials.DeleteRange(_specials.GetWithConditions(s => removed.Contains(s.ProductId)));
                    }
                }

                return new BaseResponseBO()
                {
                    IsSuccess = true,
                    Message = "Success"
                };
            }
            catch (Exception ex)
            {
                return new BaseResponseBO()
                {
                    IsSuccess = false,
                    Message = $"{ex.Message} :: Inner Exception:: {ex.InnerException?.Message} ",
                };
            }
        }

        public async Task<bool> UpdateBitField(UpdateBitBO model)
        {
            try
            {
                await _product.UpdateBitField(model.Id, model.ColName, model.Value, model.ModifiedBy);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
