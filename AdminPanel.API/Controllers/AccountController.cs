using AdminPanel.API.AuthTokenService;
using AdminPanel.Services.Repository.Account;
using AdminPanel.Shared.BO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AdminPanel.API.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;
        private readonly ITokenAuth _auth;
        private readonly IAccountRepository _account;
        public AccountController(ILogger<AccountController> logger, ITokenAuth auth, IAccountRepository account)
        {
            _logger = logger;
            _auth = auth;
            _account = account;
        }

        /// <summary>
        /// Authenticate app user
        /// </summary>
        /// <param name="model">username and password</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> ValidateUser([FromBody] LoginBO model)
        {
            var result = new GenericResponse<LoginResponse>();

            var response = await _account.ValidateUser(model);

            if (response.IsSuccess)
            {
                result.Data = await _auth.GenerateToken(response);
                result.IsSuccess = true;
            }
            else
            {
                result.IsSuccess = false;
            }
            result.Message = response.Message;

            return Ok(result);
        }

        /// <summary>
        /// Send reset password link to the user if the email is valid.
        /// </summary>
        /// <param name="model">Forgot password model</param>
        /// <returns>Email sent to the user or error message in case email is not valid</returns>
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordModel model)
        {
            var res = await _account.ForgotPassword(model);
            return Ok(res);
        }

        /// <summary>
        /// Reset password once the reset token in verified.
        /// </summary>
        /// <param name="model">new password along with reset token</param>
        /// <returns>Success or error message based on input.</returns>
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordBO model)
        {
            var res = await _account.ResetPassword(model);
            return Ok(res);
        }

        /// <summary>
        /// Application user registration method
        /// </summary>
        /// <param name="model">Properties required for User registration</param>
        /// <returns>BaseResponseBO</returns>
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> UserRegister([FromBody] AppUserRegisterBO model)
        {
            var res = await _account.UserRegister(model);
            return Ok(res);
        }

        /// <summary>
        ///  Resend Verification token if the previous one is invalid or expired
        /// </summary>
        /// <param name="model">User email</param>
        /// <returns>Send an email with new verification token</returns>
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> ResendToken([FromBody] ResendTokenBO model)
        {
            var res = await _account.ResendVerificationToken(model.Email, model.Type);
            return Ok(res);
        }

        /// <summary>
        ///  Get the information of the logged in used in case the page is refreshed
        /// </summary>
        /// <param name="userId">UserId</param>
        /// <returns>LoginResponse</returns>
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> GetUserInfo([FromBody] long userId)
        {
            return Ok(await _account.GetUserInfo(userId));
        }

        /// <summary>
        ///  Get company information from domain name
        /// </summary>
        /// <param name="domain">Domain name from the unique URL of the application link</param>
        /// <returns>DomainInfoBO object</returns>
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> GetDomainInfo([FromBody] string domain)
        {
            return Ok(await _account.GetDomainInfo(domain));
        }
    }
}
