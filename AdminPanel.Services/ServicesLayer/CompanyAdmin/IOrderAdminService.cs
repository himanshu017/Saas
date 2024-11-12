using AdminPanel.Shared.BO;
using AdminPanel.Shared.BO.CompanyAdmin;

namespace AdminPanel.Services.ServicesLayer.CompanyAdmin
{
    public interface IOrderAdminService
    {
        IEnumerable<OrderListBO> GetAllOrders(GetOrdersModel model);

        Task<GenericResponse<OrderDetailBO>> GetOrderDetails(long orderId);

        Task<GenericResponse<long>> UpdateOrderItems(ItemsBO ItemsBo);
    }
}
