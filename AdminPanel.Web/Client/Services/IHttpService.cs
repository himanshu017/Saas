using AdminPanel.Shared.BO;
using Newtonsoft.Json;
using System.Net.Http.Json;

namespace AdminPanel.Web.Client.Services
{
    public interface IHttpService
    {
        Task<T> GetAllRecords<T>(string url, CancellationTokenSource cts);
        Task<T> PostRequest<T>(string url, object postData, CancellationTokenSource cts);

        // Dispose the cancellation tokens if the page is changed
        void DisposeToken(CancellationTokenSource token);
    }

    public class HttpService : IHttpService
    {
        private readonly HttpClient _httpClient;
        public HttpService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        #region Dispose cancellation tokens
        void IHttpService.DisposeToken(CancellationTokenSource token)
        {
            token.Cancel();
            token.Dispose();
        }
        #endregion

        public async Task<T> GetAllRecords<T>(string url, CancellationTokenSource cts)
        {
            try
            {
                if (await HttpClientJsonExtensions.GetFromJsonAsync(_httpClient, url, typeof(T), cts.Token) is T result)
                {
                    return result;
                }
                else
                {
                    throw new Exception($"Error fetching records.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching records. {ex.Message}");
            }
        }

        public async Task<T> PostRequest<T>(string url, object postData, CancellationTokenSource cts)
        {
            try
            {
                var req = await _httpClient.PostAsJsonAsync(url, postData, cts.Token);

                if (req.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<T>(await req.Content.ReadAsStringAsync());
                }
                else
                {
                    if (typeof(T) == typeof(GenericResponse<BaseResponseBO>))
                    {
                        var res = new GenericResponse<BaseResponseBO>()
                        {
                            IsSuccess = false,
                            Message = $"Unable to perform operation! Error:: {await req.Content.ReadAsStringAsync()}"
                        };
                        return (T)(object)res;
                    }
                    else
                    {
                        var res = new BaseResponseBO()
                        {
                            IsSuccess = false,
                            Message = $"Unable to perform operation! Error:: {await req.Content.ReadAsStringAsync()}"
                        };
                        return (T)(object)res;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
