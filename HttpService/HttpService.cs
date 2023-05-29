using System.ComponentModel.Design;
using System.Net.Http.Json;

namespace HttpService
{
    public class HttpService : IHttpService
    {
        private readonly HttpClient _httpClient;

        public HttpService(string baseUrl)
        {
            _httpClient = new HttpClient() { BaseAddress = new Uri(baseUrl) };
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }

        public async Task<HttpResponseMessage> GetAsync(string? requestUri, CancellationToken cancellationToken = default)
        {
            return await _httpClient.GetAsync(requestUri, cancellationToken);
        }

        public async Task<HttpResponseMessage> PostAsJsonAsync<TValue>(string? requestUri, TValue value, CancellationToken cancellationToken = default)
        {
            return await _httpClient.PostAsJsonAsync<TValue>(requestUri, value, cancellationToken);
        }
        public async Task<HttpResponseMessage> PutAsJsonAsync<TValue>(string? requestUri, TValue value, CancellationToken cancellationToken = default)
        {
            return await _httpClient.PutAsJsonAsync<TValue>(requestUri, value, cancellationToken);
        }
    }
}