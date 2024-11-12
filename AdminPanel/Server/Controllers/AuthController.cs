using AdminPanel.Shared.BO;
using AdminPanel.Services.Repository.Account;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using AdminPanel.Shared;
using System.Net;
using EZOrders.PaymentGateway.Stripe;
using NLog;
using EZOrders.PaymentGateway.Razorpay;

namespace AdminPanel.Server.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly IAccountRepository _account;
        private readonly IStripePaymentProcessor _stripe;
        private readonly IRazorpayPaymentProcessor _rPay;
        public AuthController(IAccountRepository account, IStripePaymentProcessor stripe, IRazorpayPaymentProcessor rPay)
        {
            _account = account;
            _stripe = stripe;
            _rPay = rPay;
        }

        [HttpPost]
        public async Task<IActionResult> ValidateLogin([FromBody] LoginBO model)
        {
            var response = await _account.ValidateUser(model);

            if (response.IsSuccess)
            {
                var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, response.FullName),
                        new Claim("UserId", response.UserId.ToString()),
                        new Claim("CompanyId", response.Role == UserTypes.SuperAdmin.ToString() ? "0" : response.CompanyId.ToString()),
                        new Claim(ClaimTypes.Role, response.Role),
                        new Claim(ClaimTypes.Email, response.Email),
                        new Claim("CompanyName", response.CompanyName ?? ""),
                        new Claim("DomainName",response.DomainName??"")
                    };

                var claimPrincipal = new ClaimsPrincipal(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme));

                var authenticationProps = new AuthenticationProperties()
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTime.UtcNow.AddHours(2),
                };

                await HttpContext.SignInAsync(claimPrincipal, authenticationProps);
            }
            _logger.Info("First test response");
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetUserInfo()
        {
            try
            {
                if (User != null && User.Identity.IsAuthenticated)
                {
                    LoginResponse authResponse = await _account.GetUserInfo(Convert.ToInt64(User.FindFirstValue("UserId")));
                    authResponse.IsSuccess = true;
                    authResponse.Message = "Success";
                    return Ok(authResponse);
                }
                else
                {
                    return Ok(new LoginResponse()
                    {
                        IsSuccess = false,
                        Message = "User not authenticated"
                    });
                }
            }
            catch (Exception ex)
            {
                return Ok(new LoginResponse()
                {
                    IsSuccess = false,
                    Message = ex.Message
                });
            }
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return Ok("Success");
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterBO model)
        {
            var response = await _account.Register(model);

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ValidateAccount([FromBody] string token)
        {
            var response = await _account.ValidateAccount(token);

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ResendVerificationToken([FromBody] string email)
        {
            var response = await _account.ResendVerificationToken(email);

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword([FromBody] string email)
        {
            ForgotPasswordModel forgotPasswordModel = new ForgotPasswordModel();
            forgotPasswordModel.Email = email;
            forgotPasswordModel.UserTypeId = (byte)UserTypes.CompanyAdmin;
            var response = await _account.ForgotPassword(forgotPasswordModel);

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordBO model)
        {
            var response = await _account.ResetPassword(model);

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> MakePayment([FromBody] string? amount)
        {
            var response = await _rPay.MakePayment(Convert.ToDecimal(amount));
           // var response = await _stripe.MakePayment(Convert.ToDecimal(amount));
            return Ok(response);
        }

    }
}
