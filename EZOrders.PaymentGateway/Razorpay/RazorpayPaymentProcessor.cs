using AdminPanel.Shared.BO;
using Razorpay.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Newtonsoft.Json;
using EZOrders.PaymentGateway.BO;

namespace EZOrders.PaymentGateway.Razorpay
{
    public interface IRazorpayPaymentProcessor
    {
        Task<GenericResponse<string>> MakePayment(decimal amount);
    }

    public class RazorpayPaymentProcessor : IRazorpayPaymentProcessor
    {
        private readonly RazorpayClient client;
        public RazorpayPaymentProcessor()
        {
            client = new RazorpayClient("rzp_test_CNnfUoUSYX8Iza", "o2nN9C3FPetXJhn6BVFYlpJh");
        }

        public async Task<GenericResponse<string>> MakePayment(decimal amount)
        {
            //Dictionary<string, object> options = new Dictionary<string, object>();
            //options.Add("amount", "100");
            //options.Add("currency", "INR");
            //var order = client.Order.Create(options);

            // Set up the HttpClient
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("https://api.razorpay.com/v1/");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes("rzp_test_CNnfUoUSYX8Iza:o2nN9C3FPetXJhn6BVFYlpJh")));
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // Create the request data as a JSON string

            var postData = new
            {
                amount = 1000,
                currency = "INR",
                accept_partial = true,
                first_min_partial_amount = 100,
                
                reference_id = "Test-Blazor_1",
                description = "First test payment",
                customer = new
                {
                    name = "Gaurav Kumar",
                    contact = "+919000090000",
                    email = "gaurav.kumar@example.com"
                },
                notify = new
                {
                    sms = true,
                    email = true
                },
                reminder_enable = true,
                callback_url = "https://localhost:54820/stripe?status=success&package=1",
                callback_method = "get"
            };

            // Create and send the POST request
            HttpResponseMessage response = await httpClient.PostAsync("payment_links/", new StringContent(JsonConvert.SerializeObject(postData), Encoding.UTF8, "application/json"));
            string url = "";
            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Response: " + responseBody);
                var parsedResponse = JsonConvert.DeserializeObject<RazorPayResponse>(responseBody);
                url = parsedResponse.Short_Url.ToString();
            }
            else
            {
                Console.WriteLine("Request failed with status code: " + response.StatusCode);
            }

            return new GenericResponse<string>
            {
                Data = url,
                IsSuccess = true
            };
        }
    }
}
