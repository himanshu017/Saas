using AdminPanel.DataModel.Models;
using AdminPanel.Services.Repository;
using AdminPanel.Shared;
using AdminPanel.Shared.BO;
using AdminPanel.Shared.BO.WebApp;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Index.HPRtree;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanel.Services.ServicesLayer
{
    public class OrderService : IOrderService
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly IOrderRepository _repo;
        private readonly IGenericRepository<TempCart> _tempCart;
        private readonly IGenericRepository<TempCartItem> _tempCartItems;
        private readonly IGenericRepository<DeliveryDateCutoff> _cutOff;
        private readonly IGenericRepository<Comment> _comments;
        private readonly IGenericRepository<ProductAttributeValue> _attrValue;
        private readonly IMapper _mapper;

        public OrderService(IOrderRepository repo,
                            IGenericRepository<TempCart> tempCart,
                            IGenericRepository<TempCartItem> tempCartItems,
                            IMapper mapper,
                            IGenericRepository<DeliveryDateCutoff> cutOff,
                            IGenericRepository<Comment> comments,
                            IGenericRepository<ProductAttributeValue> attrValue
                            )
        {
            _repo = repo;
            _tempCart = tempCart;
            _tempCartItems = tempCartItems;
            _mapper = mapper;
            _cutOff = cutOff;
            _comments = comments;
            _attrValue = attrValue;
        }

        #region Add items to temp cart. Create a temp cart if it doesn't exist
        public async Task<GenericResponse<CartCountBO>> AddItemsToTempCart(TempCartBO model)
        {
            try
            {
                string message = "";

                var tempCart = await _tempCart
                                    .GetSingleWithConditions(x => x.CompanyId == model.CompanyId &&
                                    x.CustomerId == model.CustomerId && x.UserId == model.UserId, "TempCartItems,TempCartItems.AttributeValues");

                if (tempCart == null || tempCart.Id == 0)
                {
                    tempCart = new TempCart();

                    _mapper.Map(model, tempCart);
                    tempCart.CreatedOn = DateTime.UtcNow;
                    await _tempCart.AddAsync(tempCart);
                }

                foreach (var item in model.CartItems)
                {
                    var tempCartItem = tempCart.TempCartItems
                                               .FirstOrDefault(x => x.ProductId == item.ProductId &&
                                                                    x.TempCartId == tempCart.Id);
                    if (tempCartItem != null)
                    {
                        tempCartItem.Quantity = item.Quantity;
                        tempCartItem.CommentId = item.CommentId == 0 ? null : item.CommentId;
                        tempCartItem.ModifiedBy = model.UserId;
                        tempCartItem.ModifiedOn = DateTime.UtcNow;
                        tempCartItem.AttributePriceAdjustment = item.AttributePriceAdjustment;
                        tempCartItem.AttributeMappingJson = item.AttributeMappingJson;

                        AddUpdateItemAttributes(item.AttributeMappingJson, tempCartItem);

                        message = "Item(s) updated to cart!";
                    }
                    else
                    {
                        tempCartItem = new TempCartItem();
                        _mapper.Map(item, tempCartItem);
                        tempCartItem.TempCartId = tempCart.Id;
                        tempCartItem.CreatedOn = DateTime.UtcNow;

                        AddUpdateItemAttributes(item.AttributeMappingJson, tempCartItem);

                        tempCart.TempCartItems.Add(tempCartItem);
                        message = "Item(s) added to cart!";
                    }

                }

                await _tempCart.UpdateAsync(tempCart);

                // retur the cart count data here upon insert or update
                var cartCountModel = new GetCartInfoBO()
                {
                    CompanyId = model.CompanyId,
                    CustomerId = model.CustomerId,
                    UserId = model.UserId
                };

                var cartCount = await GetCartCount(cartCountModel);

                return new GenericResponse<CartCountBO>()
                {
                    Data = cartCount.Data,
                    IsSuccess = true,
                    Message = message
                };
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToLogString("AddItemsToTempCart Method"));
                return new GenericResponse<CartCountBO>()
                {
                    IsSuccess = false,
                    Message = $"Error : {ex.Message} :: Inner : {ex.InnerException?.Message}"
                };
            }
        }

        private void AddUpdateItemAttributes(string? attributeMappingJson, TempCartItem tempCartItem)
        {
            if (!string.IsNullOrEmpty(attributeMappingJson))
            {
                // clear the existing values
                if (tempCartItem.Id > 0)
                    tempCartItem.AttributeValues.Clear();

                var attrs = JsonConvert.DeserializeObject<Dictionary<int, int>>(attributeMappingJson).Select(x => x.Value).ToList();

                var itemAttributes = _attrValue.GetWithConditions(x => attrs.Contains(x.Id)).ToList();
                tempCartItem.AttributeValues = itemAttributes;
            }
        }
        #endregion

        #region Get total cart items and Cart Value with/without tax
        public async Task<GenericResponse<CartCountBO>> GetCartCount(GetCartInfoBO model)
        {
            try
            {
                var tempCart = await _tempCart
                                .GetSingleWithConditions(x => model.TempCartId > 0
                                ? x.Id == model.TempCartId
                                : (x.CompanyId == model.CompanyId && x.UserId == model.UserId && x.CustomerId == model.CustomerId), "TempCartItems");

                if (tempCart == null || tempCart.Id == 0)
                {
                    return new GenericResponse<CartCountBO>()
                    {
                        IsSuccess = false,
                        Message = $"No temp cart found for the current customer or user"
                    };
                }
                else
                {
                    var cartCount = new CartCountBO()
                    {
                        TempCartId = tempCart.Id,
                        CartCount = tempCart.TempCartItems.Count(),
                        CartTotal = tempCart.TempCartItems.Sum(x => ((x.Price + (x.AttributePriceAdjustment ?? 0)) * (decimal)x.Quantity)),
                        CartTotalWithTax = tempCart.TempCartItems.Sum(x => (x.IsGst == true ? ((((x.Price + (x.AttributePriceAdjustment ?? 0)) * (decimal).1) + (x.Price + (x.AttributePriceAdjustment ?? 0))) * (decimal)x.Quantity) : (x.Price + (x.AttributePriceAdjustment ?? 0)) * (decimal)x.Quantity))
                    };

                    return new GenericResponse<CartCountBO>()
                    {
                        Data = cartCount,
                        IsSuccess = true,
                        Message = $"Success"
                    };
                }

            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToLogString("GetCartCount Method"));

                return new GenericResponse<CartCountBO>()
                {
                    IsSuccess = false,
                    Message = $"Error : {ex.Message} :: Inner : {ex.InnerException?.Message}"
                };
            }
        }
        #endregion

        #region Delete single/multiple Cart Items 
        public async Task<GenericResponse<CartCountBO>> DeleteCartItems(DeleteCartItemsBO model)
        {
            try
            {
                var cartItems = new List<TempCartItem>();
                if (model.DeleteAll)
                {
                    cartItems = _tempCartItems.GetWithConditions(x => x.TempCartId == model.TempCartId).ToList();
                }
                else
                {
                    cartItems = _tempCartItems.GetWithConditions(x => model.Ids.Contains(x.Id)).ToList();
                }

                if (cartItems != null || cartItems?.Count() > 0)
                {
                    await _tempCartItems.DeleteRange(cartItems);
                }

                // retur the cart count data here upon insert or update
                var cartCountModel = new GetCartInfoBO()
                {
                    TempCartId = model.TempCartId
                };

                var cartCount = await GetCartCount(cartCountModel);

                return new GenericResponse<CartCountBO>()
                {
                    Data = cartCount.Data,
                    IsSuccess = true,
                    Message = "Item(s) deleted successfully!"
                };
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToLogString("DeleteCartItems Method"));
                return new GenericResponse<CartCountBO>()
                {
                    IsSuccess = false,
                    Message = $"Error: {ex.Message} :: Inner: {ex.InnerException?.Message}"
                };
            }
        }
        #endregion

        #region Get Cut off deivery day rules
        public async Task<GenericResponse<DeliveryDatesBO>> GetDeliveryDates(GetDeliveryDates model)
        {
            try
            {
                var response = new GenericResponse<DeliveryDatesBO>() { IsSuccess = true, Message = "Success" };
                var deliveryDatesBO = new DeliveryDatesBO();

                var cutOff = await _cutOff.GetSingleWithConditions(x => x.CompanyId == model.CompanyId, "Company,Company.TimeZone");

                if (cutOff != null && cutOff.Id > 0)
                {
                    _mapper.Map(cutOff, deliveryDatesBO);

                    DateTime requiredDate = DateTime.Now;
                    List<DateTime> availableDates = new List<DateTime>();
                    List<DayOfWeek> permittedDays = new();

                    foreach (var day in cutOff.PermittedDeliveryDays.Split(','))
                    {
                        permittedDays.Add((DayOfWeek)((int)Enum.Parse(typeof(WeekDay), day)));
                    }

                    var timezone = cutOff.Company.TimeZone.Zid;
                    var timezonInfo = TimeZoneInfo.FindSystemTimeZoneById(timezone);

                    var currentTime = DateTime.UtcNow.ToCustomerTime(timezonInfo);

                    requiredDate = currentTime;

                    // TimeSpan timeSpan = Convert.ToDateTime(cutOff.CutoffTime).TimeOfDay;

                    DateTime t1 = requiredDate;
                    DateTime t2 = DateTime.UtcNow.ToCustomerTime(timezonInfo).Date.Add(cutOff.CutoffTime.Value); // based on next day cutoff

                    if (cutOff.IsSameDayDelivery ?? false)
                    {
                        if (t1.TimeOfDay.Ticks < t2.TimeOfDay.Ticks)
                        {
                            requiredDate = currentTime.GetNextPermittedDate(permittedDays, 0);
                        }
                        else
                        {
                            requiredDate = currentTime.GetNextPermittedDate(permittedDays, 1);
                        }
                    }
                    else
                    {
                        requiredDate = currentTime.GetNextPermittedDate(permittedDays, 2);
                    }

                    DateTime startDate = requiredDate;

                    deliveryDatesBO.AvailableDates = availableDates;
                    deliveryDatesBO.PermittedDays = permittedDays;
                    deliveryDatesBO.StartDate = startDate;

                    response.Data = deliveryDatesBO;
                }
                else
                {
                    response.Data = null;
                    response.IsSuccess = false;
                    response.Message = "No cutoff data available.";
                }

                return response;
            }
            catch (Exception ex)
            {
                return new GenericResponse<DeliveryDatesBO>()
                {
                    IsSuccess = false,
                    Message = $"Error: {ex.Message} :: Inner: {ex.InnerException?.Message}"
                };
            }
        }
        #endregion

        #region Get Comments by Type
        public async Task<GenericResponse<IEnumerable<CommentsBO>>> GetCommentsByType(GetCommentsBO model)
        {
            try
            {
                var comments = await _comments
                                    .GetWithConditions(x => x.CustomerId == model.CustomerId
                                                    && x.TypeId == model.TypeId
                                                    && (model.GetUserSpecific ? x.UserId == model.UserId : true))
                                    .ToListAsync();

                var mapped = _mapper.Map<IEnumerable<CommentsBO>>(comments);

                return new GenericResponse<IEnumerable<CommentsBO>>()
                {
                    Data = mapped
                };
            }
            catch (Exception ex)
            {
                return new GenericResponse<IEnumerable<CommentsBO>>()
                {
                    IsSuccess = false,
                    Message = $"Error: {ex.Message} :: Inner: {ex.InnerException?.Message}"
                };
            }
        }
        #endregion

        #region Add Comments by Type
        public async Task<GenericResponse<CommentsBO>> AddCommentByType(CommentsBO model)
        {
            try
            {
                var comment = new Comment();
                _mapper.Map(model, comment);

                var res = await _comments.AddAsync(comment);

                return new GenericResponse<CommentsBO>()
                {
                    Data = _mapper.Map<CommentsBO>(res)
                };
            }
            catch (Exception ex)
            {
                return new GenericResponse<CommentsBO>()
                {
                    IsSuccess = false,
                    Message = $"Error: {ex.Message} :: Inner: {ex.InnerException?.Message}"
                };
            }
        }
        #endregion

        #region Place order to server

        public async Task<GenericResponse<long>> PlaceOrder(OrderBO model)
        {
            try
            {
                var order = new Order();

                //map orderModel into Order object. Mapping done in OrderProfiles.cs
                _mapper.Map(model, order);

                // map the attributed to the Order items table
                foreach (var item in order.OrderItems)
                {
                    if(!string.IsNullOrEmpty(item.AttributeMappingJson))
                    {
                        var attrs = JsonConvert.DeserializeObject<Dictionary<int, int>>(item.AttributeMappingJson).Select(x => x.Value).ToList();

                        var itemAttributes = _attrValue.GetWithConditions(x => attrs.Contains(x.Id)).ToList();
                        item.AttributeValues = itemAttributes;
                    }
                }

                // add all the additional charges to make up order total
                order.OrderTotal = (decimal)((order.TotalWithTax ?? 0) + (order.FreightCharge ?? 0) + (order.NonDeliveryDayCharge ?? 0) - (order.DiscountAmount ?? 0));

                var res = await _repo.PlaceOrder(order, model.TempCartId);

                // add code to send out email to the customer / user who placed the order
                // add code to generate order pdf or any other order files as necessary or requested by the client.

                return new GenericResponse<long>()
                {
                    Data = res,
                    Message = $"Order successfully placed. Order ref.# {res}"
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
        #endregion

        #region Get order details By Id

        public async Task<GenericResponse<OrderDetailsBO>> GetOrderDetails(long orderId)
        {
            try
            {
                var order = await _repo.GetOrderDetails(orderId);

                var mappedData = _mapper.Map<OrderDetailsBO>(order);

                return new GenericResponse<OrderDetailsBO>()
                {
                    IsSuccess = true,
                    Data = mappedData
                };
            }
            catch (Exception ex)
            {
                return new GenericResponse<OrderDetailsBO>()
                {
                    IsSuccess = false,
                    Message = $"Error: {ex.Message} :: Inner: {ex.InnerException?.Message}"
                };
            }
        }
        #endregion

        #region Get customer Order History by user or customerId
        public async Task<GenericResponse<IEnumerable<OrderDetailsBO>>> GetOrderHistory(GetOrderHistoryBO model)
        {
            try
            {
                var orders = await _repo.GetOrderHistory(model);
                var mappedData = _mapper.Map<IEnumerable<OrderDetailsBO>>(orders);

                return new GenericResponse<IEnumerable<OrderDetailsBO>>()
                {
                    Data = mappedData
                };
            }
            catch (Exception ex)
            {
                return new GenericResponse<IEnumerable<OrderDetailsBO>>()
                {
                    IsSuccess = false,
                    Message = $"Error: {ex.Message} :: Inner: {ex.InnerException?.Message}"
                };
            }
        }
        #endregion
    }
}
