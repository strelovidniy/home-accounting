using System.Text;
using MonoBankApi.Exceptions;
using MonoBankApi.Implements.Requests;
using MonoBankApi.Models.Responses;
using Newtonsoft.Json;

namespace HomeAccounting.Domain.Services.Realization
{
    public class MonoApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _shema = "application/json";
        private static readonly string _authHeader   = "X-Token";
        private static readonly string _baseEndpoint = "https://api.monobank.ua";

        public MonoApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<T> HttpGetAsync<T>(MonoRequest request, string apiToken)
        {
            _httpClient.BaseAddress = new Uri(_baseEndpoint);
            _httpClient.DefaultRequestHeaders.Add(_authHeader, apiToken);
            
            using (var response = await _httpClient.GetAsync(request.Uri).ConfigureAwait(false))
            {
                return await UnpackingResponseAsync<T>(response);
            }                
        }

        public async Task<T> HttpPostAsync<T>(MonoRequest request)
        {
            using (var response = await _httpClient.PostAsync(request.Uri,
                       new StringContent(request.Body, Encoding.UTF8, _shema)).ConfigureAwait(false))
            {
                return await UnpackingResponseAsync<T>(response);
            }
        }

        private async Task<T> UnpackingResponseAsync<T>(HttpResponseMessage response)
        {
            string json = await response.Content.ReadAsStringAsync();

            CheckStatus(response.IsSuccessStatusCode, json);

            return JsonConvert.DeserializeObject<T>(json);
        }
        private void CheckStatus(bool is200_OK, string json)
        {
            if (!is200_OK)
            {
                var err = JsonConvert.DeserializeObject<MonoError>(json);
                
                throw new MonoException(err.Description);
            }
        }
    }
}