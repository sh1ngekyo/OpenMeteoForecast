using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpService
{
    public interface IHttpService : IDisposable
    {
        Task<HttpResponseMessage> GetAsync(string? requestUri, CancellationToken cancellationToken = default);
        Task<HttpResponseMessage> PostAsJsonAsync<TValue>(string? requestUri, TValue value, CancellationToken cancellationToken = default);
        Task<HttpResponseMessage> PutAsJsonAsync<TValue>(string? requestUri, TValue value, CancellationToken cancellationToken = default);
    }
}
