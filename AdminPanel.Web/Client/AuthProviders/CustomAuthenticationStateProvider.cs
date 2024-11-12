using AdminPanel.Shared;
using AdminPanel.Shared.BO;
using AdminPanel.Shared.BO.MasterAdmin;
using AdminPanel.Shared.BO.WebApp;
using AdminPanel.Web.Client.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Security.Claims;

namespace AdminPanel.Web.Client.AuthProviders
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly IAuthService _api;
        private readonly IStateService _stateService;
        private readonly IDomainService _domainService;
        private GenericResponse<LoginResponse> _currentUser = new GenericResponse<LoginResponse>() { IsSuccess = false };
        private readonly NavigationManager _nav;

        public CustomAuthenticationStateProvider(IAuthService api, IStateService stateService, NavigationManager nav, IDomainService domainService)
        {
            _api = api;
            _stateService = stateService;
            _nav = nav;
            _domainService = domainService;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            // get the domain info from the server based on the URL
            var domainInfo = await _api.GetDomainInfo(_nav.BaseUri);

            var userInfo = await _api.GetUserInfo();
            _stateService.ImagePath = domainInfo?.ImagePath ?? "";
            _stateService.CompanyEmail = domainInfo?.Email ?? "";
            _stateService.CompanyPhone = domainInfo?.Phone ?? "";
            _stateService.IsAuthenticated = false;
            _stateService.PrimaryColor = domainInfo.PrimaryColor;
            _stateService.SecondaryColor = domainInfo.SecondaryColor;
            _stateService.Logo = domainInfo.Logo;
            _stateService.CurrencyInfo = domainInfo.CurrencyInfo;

            if (userInfo.IsSuccess)
            {
                _stateService.CompanyId = userInfo.CompanyId;
                _stateService.CustomerId = userInfo.CustomerId;
                _stateService.UserId = userInfo.UserId;
                _stateService.Role = userInfo.Role;
                _stateService.Email = userInfo.Email;
                _stateService.FullName = userInfo.FullName;
                _stateService.DomainName = userInfo.DomainName;
                _stateService.IsAuthenticated = true;
                
                _stateService.ManagedFeatures = string.IsNullOrEmpty(userInfo.ManagedFeatures)
                                                      ? new ManagedFeaturesBO()
                                                      : JsonConvert.DeserializeObject<ManagedFeaturesBO>(userInfo.ManagedFeatures);

                _stateService.OrderSettings = string.IsNullOrEmpty(userInfo.OrderSettings) 
                                                    ? new OrderSettingsBO() 
                                                    : JsonConvert.DeserializeObject<OrderSettingsBO>(userInfo.OrderSettings);

                var claims = new[] {
                        new Claim(ClaimTypes.Name, userInfo.FullName),
                        new Claim("UserId", userInfo.UserId.ToString()),
                        new Claim("CompanyId", userInfo.CompanyId.ToString()),
                        new Claim(ClaimTypes.Role, userInfo.Role ?? ""),
                        new Claim("Token", userInfo.Token?? "")
                    };
                var identity = new ClaimsIdentity(claims, "Web_authentication");


                var getCartModel = new GetCartInfoBO()
                {
                    CustomerId = _stateService.CustomerId,
                    CompanyId = _stateService.CompanyId,
                    UserId = _stateService.UserId
                };

                _stateService.CartCountBO = await _api.GetCartCount(getCartModel);

                _domainService.SetStateData();
                return new AuthenticationState(new ClaimsPrincipal(identity));
            }
            else
            {
                _stateService.DomainName = domainInfo?.DomainName ?? "";
                _stateService.CompanyId = domainInfo.CompanyId;

                _domainService.SetStateData();
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }
        }

        public async Task<GenericResponse<LoginResponse>> Login(LoginBO model)
        {
            _currentUser = await _api.Login(model);
            if (_currentUser.IsSuccess)
            {
                NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
            }
            return _currentUser;
        }

        public async Task<BaseResponseBO> Register(AppUserRegisterBO model)
        {
            return await _api.Register(model);
        }

        public async Task<BaseResponseBO> ForgotPassword(ForgotPasswordModel model)
        {
            return await _api.ForgotPasssword(model);
        }

        public async Task<ResetPasswordResponse> ResetPassword(ResetPasswordBO model)
        {
            return await _api.ResetPasssword(model);
        }

        public async Task Logout()
        {
            await _api.Logout();
            _currentUser = null;
            _stateService.ManagedFeatures = new();
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
    }
}
