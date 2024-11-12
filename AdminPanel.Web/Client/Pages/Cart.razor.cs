using AdminPanel.Shared.BO.WebApp;
using AdminPanel.Shared.BO;
using AdminPanel.Web.Client.Components.Global;
using Radzen.Blazor;
using Radzen;
using AdminPanel.Web.Client.Components;
using AdminPanel.Shared;
using AdminPanel.Shared.BO.MasterAdmin;

namespace AdminPanel.Web.Client.Pages
{
    public partial class Cart : IDisposable
    {
        private CancellationTokenSource cts = new CancellationTokenSource();
        IEnumerable<ProductListBO>? cartItems;
        GetProductListBO model = new();
        TempCartBO cartModel = new();
        DeleteCartItemsBO deleteModel = new();

        RadzenDataGrid<ProductListBO>? grid;
        IList<ProductListBO> selectedItems;
        DeliveryDatesBO deliveryDates = new();
        GetDeliveryDates getDeliveryDatesModel = new();

        GenericModal? genModal;
        DateTime? OrderDate;
        IEnumerable<CustomerAddressBO>? customerAddresses;
        CustomerAddressBO? selectedAddress;
        OrderSettingsBO orderSettings;
        OrderBO orderModel = new();

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            model.IsCart = true;
            getDeliveryDatesModel.CompanyId = _state.CompanyId;
            await Task.Delay(10);
            SetInitialValues();
            await GetTempCartItems();
            await GetDeliveryDates();
            await GetCustomerAddresses();
        }

        void SetInitialValues()
        {
            cartModel.CompanyId = _state.CompanyId;
            cartModel.UserId = _state.UserId;
            cartModel.CustomerId = _state.CustomerId;
            cartModel.DeviceToken = "web view";
            cartModel.DeviceType = "Web";

            deleteModel.TempCartId = _state.CartCountBO.TempCartId;
            orderSettings = _state.OrderSettings;

            orderModel.UserId = _state.UserId;
            orderModel.CompanyId = _state.CompanyId;
            orderModel.CustomerId = _state.CustomerId;

            StateHasChanged();
        }

        async Task GetTempCartItems()
        {
            loading.Start();
            model.UserId = _state.UserId;
            model.CustomerId = _state.CustomerId;
            model.CompanyId = _state.CompanyId;

            try
            {
                var response = await _http.PostRequest<GenericResponse<IEnumerable<ProductListBO>>>($"api/Product/GetTempCartItems", model, cts);

                if (response.IsSuccess)
                {
                    cartItems = response.Data;
                    if (cartItems.Count() > 0)
                        orderModel.TempCartId = cartItems.FirstOrDefault().TempCartId;
                    ApplyFreightCharges();
                }
                else
                {
                    cartItems = null;
                }
            }
            finally
            {
                loading.Stop();
                await InvokeAsync(StateHasChanged);
            }
        }

        async Task GetDeliveryDates()
        {
            var response = await _http.PostRequest<GenericResponse<DeliveryDatesBO>>($"api/Order/GetDeliveryDates", getDeliveryDatesModel, cts);

            if (response.IsSuccess)
            {
                deliveryDates = response.Data;
            }

            await InvokeAsync(StateHasChanged);
        }

        async Task OnChange(dynamic value, ProductListBO prod)
        {
            if (value > 0)
            {
                prod.CartQuantity = value;
                prod.CartPrice = (decimal)value * prod.Price;
                await UpdateCart(value, prod, (prod.CommentId ?? 0));
            }
        }

        async Task UpdateCart(double qty, ProductListBO prod, long? commentId = 0)
        {
            try
            {
                var items = new List<TempCartItemsBO>();

                items.Add(new TempCartItemsBO()
                {
                    ProductId = prod.ProductId,
                    Price = prod.Price,
                    UnitId = prod.UnitId,
                    Quantity = (commentId > 0 || commentId == -1) ? prod.CartQuantity : qty, // if there is a value in commentId or its -1 (delete comment) then don't change the qty.
                    IsGst = prod.IsGst,
                    AttributePriceAdjustment = prod.AttributePriceAdjustment,
                    AttributeMappingJson = prod.AttributeMappingJson,
                    // add commentid if there is a value to comment id else keep the same. -1 means comment needs to be removed
                    CommentId = commentId == 0 ? prod.CommentId : (commentId == -1 ? null : commentId),
                });

                cartModel.CartItems = items;

                loading.Start();
                var response = await _http.PostRequest<GenericResponse<CartCountBO>>($"api/Order/AddItemsToTempCart", cartModel, cts);

                if (response.IsSuccess)
                {
                    _state.CartCountBO = response.Data;
                    _notify.SuccessMsg(response.Message);
                    prod.CommentId = commentId;
                    ApplyFreightCharges();
                }
                else
                {
                    _notify.ErrorMsg(response.Message);
                }

                await InvokeAsync(StateHasChanged);
            }
            catch (Exception ex)
            {
                _notify.ErrorMsg(ex.Message);
            }
            finally
            {
                loading.Stop();
            }

        }

        async Task DeleteItems(long itemId, bool deleteMultiple = false)
        {
            try
            {
                var ids = new List<long>();

                if (deleteMultiple)
                {
                    if (!(await _notify.ConfirmDialog("Are you sure?", $"Delete Selected Cart Item(s)")))
                    {
                        return;
                    }

                    ids = selectedItems.Select(x => x.TempCartItemId).ToList();
                }
                else
                {
                    ids.Add(itemId);
                }

                deleteModel.Ids = ids.ToArray();

                loading.Start();

                var response = await _http.PostRequest<GenericResponse<CartCountBO>>($"api/Order/DeleteCartItems", deleteModel, cts);
                if (response.IsSuccess)
                {
                    _state.CartCountBO = response.Data;
                    _notify.SuccessMsg(response.Message);
                    selectedItems = null;
                    await GetTempCartItems();
                }
                else
                {
                    _notify.ErrorMsg(response.Message);
                }
            }
            catch (Exception ex)
            {
                _notify.ErrorMsg(ex.Message);
            }
            finally
            {
                loading.Stop();
                await InvokeAsync(StateHasChanged);
            }
        }

        void DateRender(DateRenderEventArgs args)
        {
            if (args.Date.Date >= deliveryDates.StartDate.Date)
            {
                if ((deliveryDates.IsLimitDatesTo1Month ?? false) && args.Date.Date > deliveryDates.StartDate.AddDays(30).Date)
                {
                    args.Disabled = true;
                    return;
                }

                if (deliveryDates?.PermittedDays != null)
                {
                    if (deliveryDates.PermittedDays.Contains(args.Date.DayOfWeek))
                    {
                        args.Disabled = false;
                        args.Attributes.Add("class", "avlb");
                    }
                    else
                    {
                        if (_state.ManagedFeatures.IsNonDeliveryDayOrdering)
                        {
                            if (args.Date.DayOfWeek != DayOfWeek.Sunday || (_state.ManagedFeatures.IsSundayOrdering == true && args.Date.DayOfWeek == DayOfWeek.Sunday))
                            {
                                args.Attributes.Add("class", "nonDel");
                                args.Disabled = false;
                            }
                            else
                            {
                                args.Disabled = true;
                            }
                        }
                        else
                        {
                            args.Disabled = true;
                        }
                    }
                }
            }
            else
            {
                args.Disabled = true;
            }
        }

        void OnDateChange(DateTime? date, string name, string format)
        {
            orderModel.DeliveryDate = date.Value;
            if (!deliveryDates.PermittedDays.Contains(date.Value.Date.DayOfWeek))
            {
                orderModel.HasNonDeliveryDayCharges = true;
                orderModel.NonDeliveryDayCharge = orderSettings.NonDeliveryDayCharges ?? 0;
            }

            StateHasChanged();
        }

        async Task GetCustomerAddresses()
        {
            var response = await _http.GetAllRecords<GenericResponse<IEnumerable<CustomerAddressBO>>>($"api/General/GetCustomerAddresses?customerId={_state.CustomerId}", cts);
            customerAddresses = response.Data;
        }

        async Task AddUpdateAddress(CustomerAddressBO? model = null)
        {
            var response = await _dialog.OpenAsync<AddEditCustomerAddress>($"Manage Customer Addresses",
             new Dictionary<string, object>() { { "Model", model != null ? model : new CustomerAddressBO() } },
             new DialogOptions()
             {
                 Width = "60%"
             });

            if (response != null)
            {
                var customerAddress = (CustomerAddressBO)response;
                customerAddress.CustomerId = _state.CustomerId;
                var res = await _http.PostRequest<GenericResponse<BaseResponseBO>>("api/General/AddEditCustomerAddress", customerAddress, cts);
                if (res.IsSuccess)
                {
                    _notify.SuccessMsg("Address added successfully");
                    await GetCustomerAddresses();
                    StateHasChanged();
                }
                else
                {
                    _notify.ErrorMsg(res.Message);
                }
            }
        }

        async Task DeleteAddress(CustomerAddressBO model)
        {
            if (await _notify.ConfirmDialog("Delete address", "Are you sure?"))
            {
                var deleteModel = new DeleteCustomerAddressBO()
                {
                    AddressId = model.Id,
                    CustomerId = _state.CustomerId,
                    UserId = _state.UserId
                };
                var res = await _http.PostRequest<GenericResponse<BaseResponseBO>>("api/General/DeleteCustomerAddress", deleteModel, cts);
                if (res.IsSuccess)
                {
                    _notify.SuccessMsg("Address deleted successfully");
                    await GetCustomerAddresses();
                    StateHasChanged();
                }
                else
                {
                    _notify.ErrorMsg(res.Message);
                }
            }
        }

        void ValidateOrderConditions()
        {
            if (_state.CartCountBO.CartTotalWithTax < orderSettings.MinCartValue && !orderSettings.AllowOrderBelowMin)
            {
                _notify.WarningMsg($"Your cart total of {_state.CartCountBO.CartTotalWithTax.ToPrice(_state.CurrencyInfo)} is less than the Minimum Order value of {orderSettings.MinCartValue.ToPrice()}. Please add more items to cart to proceed.", "Minimum Cart value");
                return;
            }

            if (orderModel.AddressId == 0)
            {
                _notify.WarningMsg("Please choose a delivery address to proceed.", "No Delivery address");
                return;
            }
            OrderDate = null;
            genModal.Open();
            StateHasChanged();
        }

        void ApplyFreightCharges()
        {
            if (orderSettings.MinCartValueForFreeDelivery.HasValue && orderSettings.MinCartValueForFreeDelivery > 0)
            {
                if (_state.CartCountBO.CartTotalWithTax < orderSettings.MinCartValueForFreeDelivery)
                {
                    orderModel.HasFreightCharges = (orderSettings.FreightCharges.HasValue && orderSettings.FreightCharges.Value > 0);
                    orderModel.FreightCharge = orderModel.HasFreightCharges ? orderSettings.FreightCharges.Value : 0;
                }
                else
                {
                    orderModel.FreightCharge = 0;
                }
            }
        }

        async Task ManageComments(CommentTypes type, long id, ProductListBO prod)
        {
            var comment = await _dialog.OpenAsync<ManageComments>($"{type.ToString()} Comments",
             new Dictionary<string, object>() { { "Type", type }, { "SelectedCommentId", id } },
             new DialogOptions()
             {
                 Width = "35% !important"
             });

            if (comment != null)
            {
                var cmnt = (CommentsBO)comment;
                await UpdateCart(0, prod, cmnt.Id);
                prod.CommentDescription = cmnt.CommentDescription;
                // add code to attach the comment to the product or delivery
                // need a place to show the delivery comment too
                // if id=-1 then remove the comment for the product
            }
        }

        async Task PlaceOrder()
        {
            if (await _notify.ConfirmDialog("Place Order", $"Place your order for {OrderDate.Value.ToString("ddd dd MMM")}?"))
            {
                genModal.Close();
                loading.Start();
                try
                {
                    orderModel.OrderItems = AddItemsToOrderModel(cartItems);
                    orderModel.CreatedBy = _state.UserId;
                    orderModel.CreatedOn = DateTime.Now;

                    var res = await _http.PostRequest<GenericResponse<long>>("api/Order/PlaceOrder", orderModel, cts);
                    if (res.IsSuccess)
                    {
                        _notify.SuccessMsg(res.Message);
                        _state.CartCountBO = new(); // clear the cart count
                        _nav.NavigateTo($"/OrderDetail?orderId={res.Data}"); // redirect to the order details page
                        StateHasChanged();
                    }
                    else
                    {
                        _notify.ErrorMsg(res.Message);
                    }
                    StateHasChanged();
                }
                finally
                {
                    loading.Stop();
                }
            }
        }

        private IEnumerable<OrderItemsBO> AddItemsToOrderModel(IEnumerable<ProductListBO> items)
        {
            var orderItems = new List<OrderItemsBO>();
            foreach (var item in items)
            {
                var orderItem = new OrderItemsBO()
                {
                    ProductId = item.ProductId,
                    CommentId = item.CommentId == 0 ? null : item.CommentId,
                    Quantity = item.CartQuantity,
                    Price = item.Price,
                    Cost = item.CostPrice,
                    UnitId = item.UnitId,
                    IsGst = item.IsGst,
                    IsPantry = _state.ManagedFeatures.IsPantryList, // should be based on the managed feature so that the item is added to pantry after order is placed.
                    IsDonationItem = item.IsDonationItem,
                    AttributePriceAdjustment = item.AttributePriceAdjustment,
                    AttributeMappingJson = item.AttributeMappingJson,
                    CreatedOn = DateTime.UtcNow,
                    ModifiedOn = DateTime.UtcNow
                };

                orderItems.Add(orderItem);
            }
            return orderItems;
        }

        public void Dispose()
        {
            _http.DisposeToken(cts);
        }
    }
}
