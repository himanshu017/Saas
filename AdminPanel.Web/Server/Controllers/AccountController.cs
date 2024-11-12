using AdminPanel.Shared;
using AdminPanel.Shared.BO;
using AdminPanel.Web.Server.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Nager.PublicSuffix;
using System.Security.Claims;

namespace AdminPanel.Web.Server.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _service;
        private readonly WebSettings _webSettings;
        public AccountController(IAccountService service, IOptions<WebSettings> webSettings)
        {
            _service = service;
            _webSettings = webSettings.Value;
        }

        #region Get company info based on Domain Name

        [HttpGet]
        public async Task<IActionResult> GetDomainInfo([FromQuery] string url)
        {
            try
            {
                string subdomain = _webSettings.TestDomain;
                if (!url.Contains("localhost", StringComparison.OrdinalIgnoreCase))
                {
                    Uri uri = new Uri(url);
                    subdomain = uri.Host.Split('.')[0];
                }

                var res = await _service.GetDomainInfo(subdomain);
                res.ImagePath = _webSettings.ImagePath;
                return Ok(res);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region Validate user credentials
        [HttpPost]
        public async Task<IActionResult> ValidateUser([FromBody] LoginBO model)
        {
            var response = await _service.ValidateUser(model);

            if (response.IsSuccess)
            {
                var data = response.Data;

                var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, data.FullName),
                        new Claim("UserId", data.UserId.ToString()),
                        new Claim("CompanyId", data.Role == UserTypes.SuperAdmin.ToString() ? "0" : data.CompanyId.ToString()),
                        new Claim(ClaimTypes.Role, data.Role),
                        new Claim(ClaimTypes.Email, data.Email),
                        new Claim("CompanyName", data.CompanyName ?? ""),
                        new Claim("DomainName",data.DomainName ?? ""),
                        new Claim("Token",data.Token ?? "")
                    };

                var claimPrincipal = new ClaimsPrincipal(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme));

                var authenticationProps = new AuthenticationProperties()
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTime.UtcNow.AddHours(4),
                };

                await HttpContext.SignInAsync(claimPrincipal, authenticationProps);

                response.Data.Token = ""; // clear the token after saving to claims

                return Ok(response);
            }
            else
            {
                return Ok(new GenericResponse<LoginResponse>()
                {
                    IsSuccess = false,
                    Message = response.Message
                });
            }
        }
        #endregion

        #region Register a new app user
        [HttpPost]
        public async Task<IActionResult> RegisterUser([FromBody] AppUserRegisterBO model)
        {
            var response = await _service.Register(model);
            return Ok(response);
        }
        #endregion

        #region Create a forgot password request
        [HttpPost]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordModel model)
        {
            var response = await _service.ForgotPassword(model);
            return Ok(response);
        }
        #endregion

        #region Get logged user info in case the page is refreshed by the user after authenticating
        [HttpGet]
        public async Task<IActionResult> GetLoggedUserInfo()
        {
            try
            {
                if (User != null && User.Identity.IsAuthenticated)
                {
                    var userInfo = await _service.GetUserInfo(Convert.ToInt64(User.FindFirstValue("UserId")));
                    //userInfo.Token = User.FindFirstValue("Token"); // get the token value from claims
                    return Ok(userInfo);
                }
                else
                {
                    return Ok(new LoginResponse() { IsSuccess = false, Message = "User is not authenticated!" });
                }
            }
            catch (Exception ex)
            {
                return Ok(new LoginResponse() { IsSuccess = false, Message = ex.Message });
            }
        }
        #endregion

        #region Clear authentication data 
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return Ok();
        } 
        #endregion
    }
}
