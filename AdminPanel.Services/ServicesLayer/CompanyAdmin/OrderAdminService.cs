using AdminPanel.DataModel.Models;
using AdminPanel.Services.Repository;
using AdminPanel.Shared.BO;
using AdminPanel.Shared.BO.CompanyAdmin;
using AutoMapper;

namespace AdminPanel.Services.ServicesLayer.CompanyAdmin
{
    public class OrderAdminService : IOrderAdminService
    {
        private readonly IGenericRepository<Order> _order;
        private readonly IGenericRepository<OrderItem> _orderitem;
        private readonly IGenericRepository<Customer> _customers;
        private readonly IGenericRepository<User> _users;
        private readonly IGenericRepository<Status> _status;
        private readonly IOrderRepository _repo;
        private readonly IMapper _mapper;
        private readonly string rootpath = @"wwwroot/ProductsImages/";
        public OrderAdminService(IGenericRepository<Order> orders,
                              IGenericRepository<Customer> customers,
                              IGenericRepository<User> users,
                               IGenericRepository<Status> status,
                              IMapper mapper,
                              IOrderRepository repo, IGenericRepository<OrderItem> orderitem)
        {
            _order = orders;
            _customers = customers;
            _users = users;
            _status = status;
            _mapper = mapper;
            _repo = repo;
            _orderitem = orderitem;
        }

        public IEnumerable<OrderListBO> GetAllOrders(GetOrdersModel model)
        {
            var productCount = _order.GetAll().Where(x => x.CompanyId == model.CompanyId).Count();
            var products = _order.GetAll("Customer,User")
                                     .Where(x => x.CompanyId == model.CompanyId)
                                     .OrderByDescending(o => o.ModifiedOn)
                                     .Skip(model.PageSize * model.PageNumber)
                                     .Take(model.PageSize == 0 ? productCount : model.PageSize)
                                     .ToList();
            var res = _mapper.Map<IEnumerable<OrderListBO>>(products);
            if (productCount > 0)
            {
                res.ToList()[0].TotalRecords = productCount;
            }
            return res;
        }

        #region Get order details By Id

        public async Task<GenericResponse<OrderDetailBO>> GetOrderDetails(long orderId)
        {
            try
            {
                var order = await _repo.GetOrderDetails(orderId);

                var mappedData = _mapper.Map<OrderDetailBO>(order);

                return new GenericResponse<OrderDetailBO>()
                {
                    IsSuccess = true,
                    Data = mappedData
                };
            }
            catch (Exception ex)
            {
                return new GenericResponse<OrderDetailBO>()
                {
                    IsSuccess = false,
                    Message = $"Error: {ex.Message} :: Inner: {ex.InnerException?.Message}"
                };
            }
        }
        #endregion

        public async Task<GenericResponse<long>> UpdateOrderItems(ItemsBO ItemsBo)
        {
            try
            {
                var mappedData = _mapper.Map<OrderItem>(ItemsBo);
                //var order = await _repo.UpdateOrderItems(mappedData);
                var order = await _orderitem.UpdateAsync(mappedData);


                return new GenericResponse<long>()
                {
                    IsSuccess = true,
                    Data = order.OrderId
                };
            }
            catch (Exception ex)
            {
                return new GenericResponse<long>()
                {
                    IsSuccess = false,
                    Message = $"Error: {ex.Message} :: Inner: {ex.InnerException?.Message}"
                };
            }
        }
    }
}
