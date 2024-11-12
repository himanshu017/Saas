using AdminPanel.Shared.BO;
using AdminPanel.Shared.BO.Account;
using AdminPanel.Shared.BO.WebApp;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http.Json;

namespace AdminPanel.Web.Client.Services
{
    public interface IAuthService
    {
        Task<GenericResponse<LoginResponse>> Login(LoginBO model);
        Task Logout();
        Task<LoginResponse> GetUserInfo();
        Task<BaseResponseBO> Register(AppUserRegisterBO model);
        Task<BaseResponseBO> ValidateAccount(string token);
        Task<BaseResponseBO> ResendVerificationToken(string email);
        Task<BaseResponseBO> ForgotPasssword(ForgotPasswordModel model);
        Task<ResetPasswordResponse> ResetPasssword(ResetPasswordBO model);
        Task<DomainInfoBO> GetDomainInfo(string domain);
        Task<CartCountBO> GetCartCount(GetCartInfoBO model);
    }

    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;

        public AuthService(HttpClient client)
        {
            _httpClient = client;
        }

        public async Task<GenericResponse<LoginResponse>> Login(LoginBO model)
        {
            try
            {
                var response = new GenericResponse<LoginResponse>();

                var result = await _httpClient.PostAsJsonAsync("Account/ValidateUser", model);

                if (result.IsSuccessStatusCode)
                {
                    response = JsonConvert.DeserializeObject<GenericResponse<LoginResponse>>(result.Content.ReadAsStringAsync().Result);
                    if (response != null && response.IsSuccess)
                    {
                        response.IsSuccess = true;
                    }
                    else
                    {
                        response.IsSuccess = false;
                    }
                }

                return response;
            }
            catch (Exception ex)
            {
                return new GenericResponse<LoginResponse>()
                {
                    IsSuccess = false,
                    Message = ex.Message + Environment.NewLine + (ex.InnerException?.Message)
                };
            }
        }

        public async Task<LoginResponse> GetUserInfo()
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<LoginResponse>("Account/GetLoggedUserInfo");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                throw;
            }
        }

        public async Task Logout()
        {
            var result = await _httpClient.GetAsync("Account/Logout");
            result.EnsureSuccessStatusCode();
        }

        public async Task<BaseResponseBO> Register(AppUserRegisterBO model)
        {
            try
            {
                var response = new BaseResponseBO();

                var result = await _httpClient.PostAsJsonAsync("Account/RegisterUser", model);
                if (result.IsSuccessStatusCode)
                {
                    response = JsonConvert.DeserializeObject<BaseResponseBO>(result.Content.ReadAsStringAsync().Result);
                }

                return response;
            }
            catch (Exception ex)
            {
                return new BaseResponseBO()
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<BaseResponseBO> ValidateAccount(string token)
        {
            try
            {
                var response = new BaseResponseBO();

                var result = await _httpClient.PostAsJsonAsync("Auth/ValidateAccount", token);
                if (result.IsSuccessStatusCode)
                {
                    response = JsonConvert.DeserializeObject<BaseResponseBO>(result.Content.ReadAsStringAsync().Result);
                }

                return response;
            }
            catch (Exception ex)
            {
                return new BaseResponseBO()
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<BaseResponseBO> ResendVerificationToken(string email)
        {
            try
            {
                var response = new BaseResponseBO();

                var result = await _httpClient.PostAsJsonAsync("Auth/ResendVerificationToken", email);
                if (result.IsSuccessStatusCode)
                {
                    response = JsonConvert.DeserializeObject<BaseResponseBO>(result.Content.ReadAsStringAsync().Result);
                }

                return response;
            }
            catch (Exception ex)
            {
                return new BaseResponseBO()
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<BaseResponseBO> ForgotPasssword(ForgotPasswordModel model)
        {
            try
            {
                var response = new BaseResponseBO();

                var result = await _httpClient.PostAsJsonAsync("Account/ForgotPassword", model);
                if (result.IsSuccessStatusCode)
                {
                    response = JsonConvert.DeserializeObject<BaseResponseBO>(result.Content.ReadAsStringAsync().Result);
                }

                return response;
            }
            catch (Exception ex)
            {
                return new BaseResponseBO()
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ResetPasswordResponse> ResetPasssword(ResetPasswordBO model)
        {
            try
            {
                var response = new ResetPasswordResponse();

                var result = await _httpClient.PostAsJsonAsync("Auth/ResetPassword", model);
                if (result.IsSuccessStatusCode)
                {
                    response = JsonConvert.DeserializeObject<ResetPasswordResponse>(result.Content.ReadAsStringAsync().Result);
                }

                return response;
            }
            catch (Exception ex)
            {
                return new ResetPasswordResponse()
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<DomainInfoBO> GetDomainInfo(string url)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<DomainInfoBO>($"Account/GetDomainInfo?url={url}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                throw;
            }
        }

        public async Task<CartCountBO> GetCartCount(GetCartInfoBO model)
        {
            try
            {
                var cartCountBO = new CartCountBO();
                var result = await _httpClient.PostAsJsonAsync("api/order/GetCartCount", model);

                if (result.IsSuccessStatusCode)
                {
                    var response = JsonConvert.DeserializeObject<GenericPagedResponse<CartCountBO>>(result.Content.ReadAsStringAsync().Result);
                    if (response.IsSuccess)
                    {
                        cartCountBO = response.Data;
                    }
                    else
                    {
                        cartCountBO = new CartCountBO();
                    }
                }

                return cartCountBO;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
