using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdminPanel.Shared.BO;
using Stripe;
using Stripe.Checkout;
namespace EZOrders.PaymentGateway.Stripe
{
    public interface IStripePaymentProcessor
    {
        Task<GenericResponse<string>> MakePayment(decimal amount);

        Task<GenericResponse<string>> GetStripeCustomer();
    }

    public class StripePaymentProcessor : IStripePaymentProcessor
    {
        public async Task<GenericResponse<string>> GetStripeCustomer()
        {
            var service = new CustomerService();
            Customer cust = service.Get("cus_NMcyXkSIbV0YIf");

            return new GenericResponse<string>
            {
                Data = "customer"
            };
        }

        public async Task<GenericResponse<string>> MakePayment(decimal amount)
        {
            try
            {
                // TODO: currency needs to be dynamic

                var options = new SessionCreateOptions
                {
                    LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmountDecimal = Convert.ToDecimal(amount)*100,
                            Currency ="aud",
                            Recurring = new SessionLineItemPriceDataRecurringOptions{
                                Interval = "month",
                                IntervalCount = 1
                            },
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name ="EZ-Orders Membership"
                            }
                        },
                        Quantity = 1
                    },
                },
                    Mode = "subscription",
                    SuccessUrl = "https://localhost:54820/stripe?status=success&package=1",
                    CancelUrl = "https://localhost:54820/stripe?status=failed",
                    SubscriptionData = new SessionSubscriptionDataOptions
                    {
                        TrialPeriodDays = 7,
                        Description = "EZ Orders monthly subscription to allow order placement and invoice generation",
                        TrialSettings = new SessionSubscriptionDataTrialSettingsOptions
                        {
                            EndBehavior = new SessionSubscriptionDataTrialSettingsEndBehaviorOptions
                            {
                                MissingPaymentMethod = "create_invoice"
                            }
                        }
                    }
                };

                var service = new SessionService();
                Session session = await service.CreateAsync(options);
                //Response.Headers.Add("Location", session.Url);
                return new GenericResponse<string>
                {
                    Data = session.Url
                };
            }
            catch (StripeException ex)
            {
                return new GenericResponse<string>
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }
    }
}
