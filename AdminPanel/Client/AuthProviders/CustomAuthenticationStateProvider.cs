using AdminPanel.Client.Services;
using AdminPanel.Shared;
using AdminPanel.Shared.BO;
using AdminPanel.Shared.BO.MasterAdmin;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Security.Claims;

namespace AdminPanel.Client.AuthProviders
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly IAuthService _api;
        private readonly IStateService _stateService;
        private LoginResponse _currentUser = new LoginResponse() { IsSuccess = false };
        public CustomAuthenticationStateProvider(IAuthService api, IStateService stateService)
        {
            _api = api;
            _stateService = stateService;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {

#if DEBUG
            _stateService.AllowMasterTableEditing = true;
#else
            _stateService.AllowMasterTableEditing = false;
#endif

            var userInfo = await _api.GetUserInfo();

            if (userInfo.IsSuccess)
            {
                _stateService.CompanyId = userInfo.CompanyId;
                _stateService.UserId = userInfo.UserId;
                _stateService.Role = userInfo.Role;
                _stateService.Email = userInfo.Email;
                _stateService.FullName = userInfo.FullName;
                _stateService.DomainName = userInfo.DomainName;
                _stateService.CurrencyInfo = userInfo.CurrencyInfo;

                if (userInfo.UserTypeID == (byte)UserTypes.CompanyAdmin)
                {
                    _stateService.ManagedFeatures = JsonConvert.DeserializeObject<ManagedFeaturesBO>(userInfo.ManagedFeatures);
                    _stateService.CompanyConfig = JsonConvert.DeserializeObject<CompanyConfigBO>(userInfo.CompanyConfig);
                }
                var claims = new[] {
                        new Claim(ClaimTypes.Name, userInfo.FullName),
                        new Claim("UserId", userInfo.UserId.ToString()),
                        new Claim("CompanyId", userInfo.CompanyId.ToString()),
                        new Claim(ClaimTypes.Role, userInfo.Role ?? "")
                    };
                var identity = new ClaimsIdentity(claims, "Server_authentication");

                return new AuthenticationState(new ClaimsPrincipal(identity));
            }
            else
            {
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }
        }

        public async Task<LoginResponse> Login(LoginBO model)
        {
            _currentUser = await _api.Login(model);
            if (_currentUser.IsSuccess)
            {
                NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
            }
            return _currentUser;
        }

        public async Task Logout()
        {
            await _api.Logout();
            _currentUser = new LoginResponse() { IsSuccess = false };
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public async Task<BaseResponseBO> Register(RegisterBO model)
        {
            return await _api.Register(model);
        }

        public async Task<BaseResponseBO> ValidateAccount(string token)
        {
            return await _api.ValidateAccount(token);
        }

        public async Task<BaseResponseBO> ResendVerificationToken(string email)
        {
            return await _api.ResendVerificationToken(email);
        }

        public async Task<BaseResponseBO> ForgotPassword(string email)
        {
            return await _api.ForgotPasssword(email);
        }

        public async Task<ResetPasswordResponse> ResetPassword(ResetPasswordBO model)
        {
            return await _api.ResetPasssword(model);
        }
    }
}
