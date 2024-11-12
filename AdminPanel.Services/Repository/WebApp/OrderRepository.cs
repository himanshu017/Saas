using AdminPanel.DataModel.Context;
using AdminPanel.DataModel.Models;
using AdminPanel.Shared.BO;
using AdminPanel.Shared.BO.WebApp;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanel.Services.Repository
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        private readonly IMapper _mapper;
        public OrderRepository(OrderflowContext context) : base(context)
        {
        }


        public async Task<long> PlaceOrder(Order model, long tempCartId)
        {
            try
            {
                // get company and set the next order invoice number for the company.
                var company = await _context.Companies.FindAsync(model.CompanyId);
                model.OrderNumber = company.NextInvoiceNumber;
                company.NextInvoiceNumber = company.NextInvoiceNumber + 1;

                // save the order and items to the database.
                var response = await _context.Orders.AddAsync(model);
                await _context.SaveChangesAsync();

                // remove the items from the temp cart and temp cart items after successfull order save
                _context.TempCartItems.RemoveRange(_context.TempCartItems.Where(x => x.TempCartId == tempCartId));
                _context.TempCarts.Remove(_context.TempCarts.Find(tempCartId));
                await _context.SaveChangesAsync();

                return model.OrderId;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Order> GetOrderDetails(long orderId)
        {
            try
            {
                var order = await _context.Orders
                                          .Include(x => x.Customer)
                                          .Include(x => x.Company)
                                          .Include(x => x.Address)
                                          .Include(x => x.Comment)
                                          .Include(x => x.OrderItems)
                                          .ThenInclude(x => x.Product)
                                          .Include(x => x.OrderItems)
                                          .ThenInclude(x => x.Unit)
                                          .Include(x => x.OrderItems)
                                          .ThenInclude(x => x.Comment)
                                          .Include(x => x.Status)
                                          // attrubute specific 
                                          .Include(x => x.OrderItems)
                                          .ThenInclude(x => x.AttributeValues)
                                          .ThenInclude(av => av.ProductMapping)
                                          .ThenInclude(pm => pm.Attribute)
                                          
                                          .Where(x => x.OrderId == orderId)
                                          .FirstOrDefaultAsync();

                return order;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<Order>> GetOrderHistory(GetOrderHistoryBO model)
        {
            try
            {
                var order = await _context.Orders
                                          .Include(x => x.Customer)
                                          .Include(x => x.User)
                                          .Include(x => x.Status)
                                          .Where(x => x.CompanyId == model.CompanyId
                                                   && (model.AllUsers ? x.CustomerId == model.CustomerId : x.UserId == model.UserId))
                                          .ToListAsync();

                return order;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<long> UpdateOrderItems(OrderItem model)
        {
            try
            {
                var orderitem = _context.OrderItems.Where(x => x.OrderItemId == model.OrderItemId).FirstOrDefault();
                if (orderitem != null)
                {
                    orderitem.Quantity = model.Quantity;
                    await _context.SaveChangesAsync();
                }
                return model.OrderId;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
