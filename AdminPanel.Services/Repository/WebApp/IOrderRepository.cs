using AdminPanel.DataModel.Models;
using AdminPanel.Shared.BO;
using AdminPanel.Shared.BO.WebApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanel.Services.Repository
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        Task<long> PlaceOrder(Order model, long tempCartId);
        Task<Order> GetOrderDetails(long orderId);
        Task<IEnumerable<Order>> GetOrderHistory(GetOrderHistoryBO model);

        Task<long> UpdateOrderItems(OrderItem model);
    }
}
