using AdminPanel.Shared.BO;
using Newtonsoft.Json;
using System.Net.Http.Json;

namespace AdminPanel.Client.Services
{
    public interface IHttpService
    {
        Task<T> GetAllRecords<T>(string url);
        Task<T> PostRequest<T>(string url, object postData);
    }

    public class HttpService : IHttpService
    {
        private readonly HttpClient _httpClient;

        public HttpService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<T> GetAllRecords<T>(string url)
        {
            try
            {
                T result = await HttpClientJsonExtensions.GetFromJsonAsync<T>(_httpClient, url);
                if (result != null )
                {
                    return result;
                }
                else
                {
                    return default(T);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching records. {ex.Message}");
            }
        }

        public async Task<T> PostRequest<T>(string url, object postData)
        {
            try
            {
                T result = default(T);

                var req = await _httpClient.PostAsJsonAsync(url, postData);

                if (req.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<T>(await req.Content.ReadAsStringAsync());
                }
                else
                {
                    var res = new BaseResponseBO()
                    {
                        IsSuccess = false,
                        Message = "Unable to perform operation!"
                    };
                    return (T)(object)res;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
