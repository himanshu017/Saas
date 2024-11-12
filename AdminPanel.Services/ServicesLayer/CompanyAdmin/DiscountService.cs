using AdminPanel.DataModel.Models;
using AdminPanel.Services.Repository;
using AdminPanel.Shared;
using AdminPanel.Shared.BO;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanel.Services.ServicesLayer
{
    public class DiscountService : IDiscountService
    {
        private readonly IMapper _mapper;
        private readonly IGenericRepository<Discount> _discount;
        private readonly IGenericRepository<Product> _product;
        private readonly IGenericRepository<Customer> _customer;
        public DiscountService(IMapper mapper,
                               IGenericRepository<Discount> discount,
                               IGenericRepository<Product> product,
                               IGenericRepository<Customer> customer)
        {
            _mapper = mapper;
            _discount = discount;
            _product = product;
            _customer = customer;
        }

        public IEnumerable<DiscountBO> GetAllRecords(long companyId)
        {
            try
            {
                var records = _discount.GetAll("Type,LimitationType,Customers,Products")
                                       .Where(x => x.CompanyId == companyId);

                return _mapper.Map<IEnumerable<DiscountBO>>(records);
            }
            catch (Exception ex)
            {
                throw new Exception($"Unable to fetch records. Error : {ex.Message} :: Internam Error : {ex.InnerException?.Message}");
            }
        }

        public async Task<BaseResponseBO> AddEditDiscount(DiscountBO model)
        {
            var response = new BaseResponseBO() { IsSuccess = true };
            try
            {
                var record = new Discount();
                if (model.Id == 0) // Insert new value
                {
                    _mapper.Map(model, record);

                    AddProductCustomerAssociation(model, record);

                    await _discount.AddAsync(record);
                    response.Message = "Record added successfully.";
                }
                else
                {
                    record = await _discount.GetSingleWithConditions(x => x.Id == model.Id, "Customers,Products");
                    _mapper.Map(model, record);

                    record.Products.Clear();
                    record.Customers.Clear();

                    AddProductCustomerAssociation(model, record);

                    await _discount.UpdateAsync(record);
                    response.Message = "Record updates successfully";
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

        private void AddProductCustomerAssociation(DiscountBO model, Discount record)
        {
            if (model.TypeId == (byte)DiscountTypes.Selected_products && model.ProductCustomerSelections?.Count() > 0)
            {
                var products = _product.GetWithConditions(x => model.ProductCustomerSelections.Contains(x.ProductId)).ToList();
                record.Products = products;
            }
            else if (model.TypeId == (byte)DiscountTypes.Selected_Customers && model.ProductCustomerSelections?.Count() > 0)
            {
                var customers = _customer.GetWithConditions(x => model.ProductCustomerSelections.Contains(x.CustomerId)).ToList();
                record.Customers = customers;
            }
        }

        public async Task<BaseResponseBO> DeleteDiscount(int id)
        {
            try
            {
                var record = await _discount.GetSingleWithConditions(x => x.Id == id, "Customers,Products");
                record.Products.Clear();
                record.Customers.Clear();

                await _discount.Delete(record);

                return new BaseResponseBO()
                {
                    IsSuccess = true,
                    Message = "Record deleted."
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


    }
}
