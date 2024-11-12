using AdminPanel.Shared.BO;
using Newtonsoft.Json;
using System.Net.Http.Json;

namespace AdminPanel.Client.Services
{
    public interface IAuthService
    {
        Task<LoginResponse> Login(LoginBO model);
        Task Logout();
        Task<LoginResponse> GetUserInfo();
        Task<BaseResponseBO> Register(RegisterBO model);
        Task<BaseResponseBO> ValidateAccount(string token);
        Task<BaseResponseBO> ResendVerificationToken(string email);
        Task<BaseResponseBO> ForgotPasssword(string email);
        Task<ResetPasswordResponse> ResetPasssword(ResetPasswordBO model);
    }

    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;

        public AuthService(HttpClient client)
        {
            _httpClient = client;
        }

        public async Task<LoginResponse> Login(LoginBO model)
        {
            try
            {
                var response = new LoginResponse();

                var result = await _httpClient.PostAsJsonAsync("Auth/ValidateLogin", model);

                if (result.IsSuccessStatusCode)
                {
                    response = JsonConvert.DeserializeObject<LoginResponse>(result.Content.ReadAsStringAsync().Result);
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
                return new LoginResponse()
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
                return await _httpClient.GetFromJsonAsync<LoginResponse>("Auth/GetUserInfo");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                throw;
            }
        }

        public async Task Logout()
        {
            var result = await _httpClient.GetAsync("Auth/Logout");
            result.EnsureSuccessStatusCode();
        }

        public async Task<BaseResponseBO> Register(RegisterBO model)
        {
            try
            {
                var response = new BaseResponseBO();

                var result = await _httpClient.PostAsJsonAsync("Auth/Register", model);
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

        public async  Task<BaseResponseBO> ResendVerificationToken(string email)
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

        public async Task<BaseResponseBO> ForgotPasssword(string email)
        {
            try
            {
                var response = new BaseResponseBO();

                var result = await _httpClient.PostAsJsonAsync("Auth/ForgotPassword", email);
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

    }
}
