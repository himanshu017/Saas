using AdminPanel.Shared;
using AdminPanel.Shared.BO;
using AdminPanel.Shared.BO.Account;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace AdminPanel.Web.Server.Services
{
    public class AccountService : IAccountService
    {
        private readonly HttpClient _http;
        private readonly WebSettings _webSettings;
        public AccountService(HttpClient http, IOptions<WebSettings> webSettings)
        {
            _http = http;
            _webSettings = webSettings.Value;
        }

        public async Task<GenericResponse<LoginResponse>> ValidateUser(LoginBO model)
        {
            try
            {
                var result = new GenericResponse<LoginResponse>();
                var response = await _http.PostAsJsonAsync(_webSettings.ApiUrl + "Account/ValidateUser", model);

                if (response.IsSuccessStatusCode)
                {
                    result = JsonConvert.DeserializeObject<GenericResponse<LoginResponse>>(await response.Content.ReadAsStringAsync());
                }

                return result;
            }
            catch (Exception ex)
            {
                return new GenericResponse<LoginResponse>()
                {
                    IsSuccess = false,
                    Message = $"ERROR :: {ex.Message} :: INNER : {ex.InnerException?.Message}"
                };
            }
        }

        public async Task<BaseResponseBO> Register(AppUserRegisterBO model)
        {
            try
            {
                var result = new BaseResponseBO();
                var response = await _http.PostAsJsonAsync(_webSettings.ApiUrl + "Account/UserRegister", model);

                if (response.IsSuccessStatusCode)
                {
                    result = JsonConvert.DeserializeObject<BaseResponseBO>(await response.Content.ReadAsStringAsync());
                }

                return result;
            }
            catch (Exception ex)
            {
                return new BaseResponseBO()
                {
                    IsSuccess = false,
                    Message = $"ERROR :: {ex.Message} :: INNER : {ex.InnerException?.Message}"
                };
            }
        }

        public async Task<BaseResponseBO> ForgotPassword(ForgotPasswordModel model)
        {
            try
            {
                var result = new BaseResponseBO();
                var response = await _http.PostAsJsonAsync(_webSettings.ApiUrl + "Account/ForgotPassword", model);

                if (response.IsSuccessStatusCode)
                {
                    result = JsonConvert.DeserializeObject<BaseResponseBO>(await response.Content.ReadAsStringAsync());
                }

                return result;
            }
            catch (Exception ex)
            {
                return new BaseResponseBO()
                {
                    IsSuccess = false,
                    Message = $"ERROR :: {ex.Message} :: INNER : {ex.InnerException?.Message}"
                };
            }
        }

        public async Task<ResetPasswordResponse> ResetPassword(ResetPasswordBO model)
        {
            try
            {
                var result = new ResetPasswordResponse();
                var response = await _http.PostAsJsonAsync(_webSettings.ApiUrl + "Account/ResetPassword", model);

                if (response.IsSuccessStatusCode)
                {
                    result = JsonConvert.DeserializeObject<ResetPasswordResponse>(await response.Content.ReadAsStringAsync());
                }

                return result;
            }
            catch (Exception ex)
            {
                return new ResetPasswordResponse()
                {
                    IsSuccess = false,
                    Message = $"ERROR :: {ex.Message} :: INNER : {ex.InnerException?.Message}"
                };
            }
        }

        public async Task<BaseResponseBO> ResendVerificationToken(ResendTokenBO model)
        {
            try
            {
                model.Type = "App";
                var result = new BaseResponseBO();
                var response = await _http.PostAsJsonAsync(_webSettings.ApiUrl + "Account/ResendToken", model);

                if (response.IsSuccessStatusCode)
                {
                    result = JsonConvert.DeserializeObject<BaseResponseBO>(await response.Content.ReadAsStringAsync());
                }

                return result;
            }
            catch (Exception ex)
            {
                return new BaseResponseBO()
                {
                    IsSuccess = false,
                    Message = $"ERROR :: {ex.Message} :: INNER : {ex.InnerException?.Message}"
                };
            }
        }

        public async Task<LoginResponse> GetUserInfo(long userId)
        {
            try
            {
                var result = new LoginResponse();
                var response = await _http.PostAsJsonAsync(_webSettings.ApiUrl + "Account/GetUserInfo", userId);

                if (response.IsSuccessStatusCode)
                {
                    result = JsonConvert.DeserializeObject<LoginResponse>(await response.Content.ReadAsStringAsync());

                    if (result.IsSuccess)
                    {
                        result.IsSuccess = true;
                    }
                    else
                    {
                        result.IsSuccess = false;
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                return new LoginResponse()
                {
                    IsSuccess = false,
                    Message = $"ERROR :: {ex.Message} :: INNER : {ex.InnerException?.Message}"
                };
            }
        }

        public async Task<DomainInfoBO> GetDomainInfo(string domain)
        {
            try
            {
                var result = new DomainInfoBO();
                var response = await _http.PostAsJsonAsync(_webSettings.ApiUrl + "Account/GetDomainInfo", domain);
                if (response.IsSuccessStatusCode)
                {
                    result = JsonConvert.DeserializeObject<DomainInfoBO>(await response.Content.ReadAsStringAsync());
                }

                return result;
            }
            catch (Exception ex)
            {
                return new DomainInfoBO()
                {
                    IsSuccess = false,
                    Message = $"ERROR :: {ex.Message} :: INNER : {ex.InnerException?.Message}"
                };
            }
        }
    }
}
