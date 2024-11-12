using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NLog;
using Stripe;

namespace AdminPanel.Server.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class WebhookController : ControllerBase
    {
        // This is your Stripe CLI webhook secret for testing your endpoint locally.
        const string endpointSecret = "whsec_18329ac56e28ff28f274007a10b302fb8a403aba50a4b7f6968feb2dac2b1681";
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        [HttpPost]
        public async Task<IActionResult> Stripe()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            try
            {
                var stripeEvent = EventUtility.ConstructEvent(json,
                    Request.Headers["Stripe-Signature"], endpointSecret);

                // Handle the event
                //if (stripeEvent.Type == Events.PaymentIntentSucceeded)
                if (stripeEvent.Type == Events.ChargeSucceeded)
                {
                    // add the code to save the transaction details against the company that chose the plan
                    _logger.Info("Inside 1");
                }
                else if (stripeEvent.Type == Events.CustomerSubscriptionCreated)
                {
                    _logger.Info("Inside 2");
                }
                else if (stripeEvent.Type == Events.CustomerSubscriptionDeleted)
                {
                    _logger.Info("Inside 3");
                }
                else if (stripeEvent.Type == Events.CustomerSubscriptionTrialWillEnd)
                {
                    _logger.Info("Inside 4");
                }
                else if (stripeEvent.Type == Events.InvoicePaid)
                {
                    _logger.Info("Inside 5");
                }
                else
                {
                    _logger.Info("Inside 6");
                    Console.WriteLine("Unhandled event type: {0}", stripeEvent.Type);
                    Console.WriteLine("Unknown Json: {0}", json);
                }

                _logger.Debug($"Event Type: {stripeEvent.Type} :: Json Data :{json}");

                return Ok();
            }
            catch (StripeException e)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> StripeSubscription()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            try
            {
                var stripeEvent = EventUtility.ConstructEvent(json,
                    Request.Headers["Stripe-Signature"], endpointSecret);

                // Handle the event
                if (stripeEvent.Type == Events.SubscriptionScheduleAborted)
                {
                }
                else if (stripeEvent.Type == Events.SubscriptionScheduleCanceled)
                {
                }
                else if (stripeEvent.Type == Events.SubscriptionScheduleCompleted)
                {
                }
                else if (stripeEvent.Type == Events.SubscriptionScheduleCreated)
                {
                }
                else if (stripeEvent.Type == Events.SubscriptionScheduleExpiring)
                {
                }
                else if (stripeEvent.Type == Events.SubscriptionScheduleReleased)
                {
                }
                else if (stripeEvent.Type == Events.SubscriptionScheduleUpdated)
                {
                }
                // ... handle other event types
                else
                {
                    Console.WriteLine("Unhandled event type: {0}", stripeEvent.Type);
                }

                return Ok();
            }
            catch (Exception)
            {
                // code to log the exception
                return BadRequest();
            }
        }
    }

}
