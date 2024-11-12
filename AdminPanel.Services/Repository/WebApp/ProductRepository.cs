using AdminPanel.DataModel.Context;
using AdminPanel.DataModel.Models;
using AdminPanel.Services.Repository.WebApp;
using AdminPanel.Shared.BO.WebApp;
using Microsoft.EntityFrameworkCore;
using AdminPanel.Services.Helpers;
using MimeKit.Cryptography;

namespace AdminPanel.Services.Repository
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(OrderflowContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Filter>> GetAllFilters(long companyId)
        {
            try
            {
                var filters = await _context.Filters.Where(x => x.IsActive == true && x.CompanyId == companyId).ToListAsync();
                return filters;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<MainCategory>> GetAllMainAndSubCategories(GetAllCategoriesBO model)
        {
            try
            {
                var categories = await _context.MainCategories
                                               .Where(x => x.IsActive == true && x.CompanyId == model.CompanyId && x.IsSpecial != (model.IsSpecial ? false : null))
                                               .If(model.ShowSubcategories, x => x.Include(e => e.SubCategories))
                                               .ToListAsync();

                return categories;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<UnitOfMeasurement>> GetAllUnits(long companyId)
        {
            try
            {
                var Units = await _context.UnitOfMeasurements.Where(x => x.CompanyId == companyId && x.IsActive == true).ToListAsync();

                return Units;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IQueryable<Product>> GetProductsBySearchFilters(GetProductListBO model)
        {
            try
            {
                var defaultInfo = new Helper(_context).GetDefaultUserData(model.CompanyId, model.UserId, model.CustomerId);
                var manageFeatures = defaultInfo.ManagedFeatures;
                var customerWarehouses = defaultInfo.WarehouseIds;


                var products = _context.Products
                                       .AsNoTracking() // using this as this is going to be a read only query
                                       .Include(p => p.ProductImages)
                                       .Include(u => u.ProductUnits.Where(pu => pu.IsDefault == true))
                                       .ThenInclude(u => u.Unit)
                                       .Include(s => s.Category)
                                       .ThenInclude(m => m.MainCategory)
                                       .Include(pa => pa.ProductAttributeMappings.OrderBy(o => o.DisplayOrder))
                                       .ThenInclude(pv => pv.ProductAttributeValues.OrderBy(o => o.DisplayOrder))
                                       .Include(pa => pa.ProductAttributeMappings)
                                       .ThenInclude(a => a.Attribute)
                                       .Include(sp => sp.SpecialPricings
                                                        .Where(x => x.CustomerId == model.CustomerId && x.CompanyId == model.CompanyId
                                                                 && x.IsActive == true
                                                                 && (x.StartTime == null || x.StartTime.Value.Date <= DateTime.Now.Date)
                                                                 && (x.EndTime == null || x.EndTime.Value.Date >= DateTime.Now.Date)
                                                                 && (x.UnitId == null || x.UnitId == x.Product.ProductUnits.FirstOrDefault().Unit.UnitId)
                                                              ))
                                       .Include(s => s.ProductInventories.Where(x => (customerWarehouses == null || customerWarehouses.Contains(x.WarehouseId))))
                                       .Where(p => p.CompanyId == model.CompanyId
                                                         && p.IsActive == true
                                                         && p.IsAvailable == true
                                                         && p.Category.IsActive == true
                                                         && p.Category.MainCategory.IsActive == true
                                                         && (string.IsNullOrEmpty(model.SearchString)
                                                           || p.Name.Contains(model.SearchString)
                                                           || p.Code.Contains(model.SearchString)
                                                           || p.Category.SubCategoryName.Contains(model.SearchString)
                                                           || p.Category.MainCategory.CategoryName.Contains(model.SearchString)
                                                           || p.BrandName.Contains(model.SearchString)
                                                           || p.Barcode.Contains(model.SearchString)
                                                           || p.SupplierName.Contains(model.SearchString))
                                                         && (model.FilterIds.Count() == 0 || model.FilterIds.Contains(p.FilterId))
                                                         && (model.CategoryIds.Count() == 0 || model.CategoryIds.Contains(p.Category.MainCategoryId))
                                                         && (model.SubCategoryIds.Count() == 0 || model.SubCategoryIds.Contains(p.CategoryId))
                                                         && (model.UomIds.Count() == 0 ||
                                                             model.UomIds.Contains(p.ProductUnits.FirstOrDefault().Unit.UnitId))
                                                            );
                await Task.Delay(1);

                return products;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<FavoriteList>> GetFavoriteLists(GetFavLists model)
        {
            try
            {
                var favoriteLists = await _context.FavoriteLists
                                            .Include(e => e.Type)
                                            .Where(x => x.CustomerId == model.CustomerId
                                                     && x.IsActive == true)
                                            .ToListAsync();

                return favoriteLists;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IQueryable<Product>> GetFavoriteListItems(GetProductListBO model)
        {
            try
            {
                var products = await GetProductsBySearchFilters(model);

                var result = (from p in products
                              join f in _context.FavoriteListItems on p.ProductId equals f.ProductId
                              where f.ListId == model.FavListId && f.IsActive == true
                              select p
                               );

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IQueryable<Product>> GetCompanySpecials(GetProductListBO model)
        {
            try
            {
                var products = await GetProductsBySearchFilters(model);

                var result = (from p in products
                              join s in _context.CompanySpecials on p.ProductId equals s.ProductId
                              where s.CompanyId == model.CompanyId
                              select p
                              );

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<ProductListBO>> GetAdditionalProductInfo(List<ProductListBO> data, long userId, long customerId, long companyId, bool isCart = false)
        {
            var defaultInfo = new Helper(_context).GetDefaultUserData(companyId, userId, customerId);
            var manageFeatures = defaultInfo.ManagedFeatures;

            foreach (var item in data)
            {
                if (manageFeatures.IsRememberLastUOM)
                {
                    var res = await _context.Orders
                                      .Include(o => o.OrderItems)
                                      .Where(x => x.UserId == userId)
                                      .OrderByDescending(x => x.OrderId)
                                      .GroupBy(g => g.OrderItems)
                                      .Select(x => x.FirstOrDefault().OrderItems.FirstOrDefault().UnitId)
                                      .FirstOrDefaultAsync();
                }

                if (!isCart)
                {
                    var cartData = (from t in _context.TempCarts
                                    join ti in _context.TempCartItems on t.Id equals ti.TempCartId
                                    where t.CompanyId == companyId && t.CustomerId == customerId
                                            && t.UserId == userId && ti.ProductId == item.ProductId
                                    select new
                                    {
                                        TempCartId = ti.TempCartId,
                                        CartQuantity = ti.Quantity
                                    }).FirstOrDefault();

                    if (cartData != null)
                    {
                        item.TempCartId = cartData.TempCartId;
                        item.CartQuantity = cartData.CartQuantity;
                        item.IsInCart = true;
                    }
                }
                else
                {
                    item.IsInCart = true;
                }
            }

            return data;
        }

        public async Task<IQueryable<Product>> GetTempCartItems(GetProductListBO model)
        {
            try
            {
                var products = await GetProductsBySearchFilters(model);

                var tempCart = await _context.TempCarts
                                            .Include(c => c.Comment)
                                            .Where(x => x.CompanyId == model.CompanyId &&
                                   x.CustomerId == model.CustomerId && x.UserId == model.UserId).FirstOrDefaultAsync();

                if (tempCart != null)
                {
                    var cartItems = (from prod in products
                                     join ci in _context.TempCartItems on prod.ProductId equals ci.ProductId
                                     where ci.TempCartId == tempCart.Id
                                     select prod
                                     );

                    return cartItems.Include(x => x.TempCartItems)
                                    .ThenInclude(cm => cm.Comment)
                                    .Include(av => av.TempCartItems)
                                    .ThenInclude(cm => cm.AttributeValues)
                                    .ThenInclude(av => av.ProductMapping)
                                    .ThenInclude(pm => pm.Attribute)
                                    ;

                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IQueryable<Product>> GetProductDetail(GetProdDetailBO model)
        {
            try
            {
                var defaultInfo = new Helper(_context).GetDefaultUserData(model.CompanyId, model.UserId, model.CustomerId);
                var manageFeatures = defaultInfo.ManagedFeatures;
                var customerWarehouses = defaultInfo.WarehouseIds;


                var product = _context.Products
                                       .AsNoTracking() // using this as this is going to be a read only query
                                       .Include(p => p.ProductImages)
                                       .Include(u => u.ProductUnits.Where(pu => pu.IsDefault == true))
                                       .ThenInclude(u => u.Unit)
                                       .Include(s => s.Category)
                                       .ThenInclude(m => m.MainCategory)
                                       .Include(pa => pa.ProductAttributeMappings.OrderBy(o => o.DisplayOrder))
                                       .ThenInclude(pv => pv.ProductAttributeValues.OrderBy(o => o.DisplayOrder))
                                       .Include(pa => pa.ProductAttributeMappings)
                                       .ThenInclude(a => a.Attribute)
                                       .Include(t => t.TempCartItems)
                                       .Include(t => t.Tags)
                                       .If((model.CustomerId != null && model.CustomerId > 0), x => x.Include(sp => sp.SpecialPricings
                                                        .Where(x => x.CustomerId == model.CustomerId && x.CompanyId == model.CompanyId
                                                                 && x.IsActive == true
                                                                 && (x.StartTime == null || x.StartTime.Value.Date <= DateTime.Now.Date)
                                                                 && (x.EndTime == null || x.EndTime.Value.Date >= DateTime.Now.Date)
                                                                 && (x.UnitId == null || x.UnitId == x.Product.ProductUnits.FirstOrDefault().Unit.UnitId)
                                                              )))
                                       .Include(s => s.ProductInventories.Where(x => (customerWarehouses == null || customerWarehouses.Contains(x.WarehouseId))))
                                       .Where(p => p.CompanyId == model.CompanyId
                                                         && p.IsActive == true
                                                         && p.IsAvailable == true
                                                         && p.Category.IsActive == true
                                                         && p.Category.MainCategory.IsActive == true
                                                         && p.ProductId == model.ProductId
                                                         );

                await Task.Delay(1);

                return product;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IQueryable<Product>> GetSuggestiveProducts(GetProductListBO model)
        {
            try
            {
                var products = await GetProductsBySearchFilters(model);

                var productIds = _context.Products
                                         .Where(x => x.ProductId == model.ProductId)
                                         .SelectMany(p => p.SuggestiveProducts.Select(sp => sp.ProductId))
                                         .ToList();

                //var tags = await _context.Products.FirstOrDefaultAsync(x => x.ProductId == model.ProductId);

                //List<string> tagsArry = new List<string>();

                //if(tags != null)
                //{
                //    if (!string.IsNullOrEmpty(tags.MetaKeywords))
                //    {
                //        tagsArry = tags.MetaKeywords.Split(',').ToList();
                //    }

                //    var similarByTags = products.Where(x => tagsArry.Contains(x.MetaKeywords)).Take(2);
                //}

                return products.Where(x => productIds.Contains(x.ProductId));
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
