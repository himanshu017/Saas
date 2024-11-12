using AdminPanel.DataModel.Models;
using AdminPanel.Shared.BO;
using AdminPanel.Shared.BO.WebApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanel.Services.ServicesLayer
{
    public interface IOrderService
    {
        Task<GenericResponse<CartCountBO>> AddItemsToTempCart(TempCartBO model);
        Task<GenericResponse<CartCountBO>> GetCartCount(GetCartInfoBO model);
        Task<GenericResponse<CartCountBO>> DeleteCartItems(DeleteCartItemsBO model);
        Task<GenericResponse<DeliveryDatesBO>> GetDeliveryDates(GetDeliveryDates model);
        Task<GenericResponse<IEnumerable<CommentsBO>>> GetCommentsByType(GetCommentsBO model);
        Task<GenericResponse<CommentsBO>> AddCommentByType(CommentsBO model);

        // Order related methods
        Task<GenericResponse<long>> PlaceOrder(OrderBO model);
        Task<GenericResponse<OrderDetailsBO>> GetOrderDetails(long orderId);
        Task<GenericResponse<IEnumerable<OrderDetailsBO>>> GetOrderHistory(GetOrderHistoryBO model);

    }
}
