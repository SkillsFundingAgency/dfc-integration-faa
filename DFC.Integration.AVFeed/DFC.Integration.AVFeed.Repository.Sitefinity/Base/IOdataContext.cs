using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace DFC.Integration.AVFeed.Repository.Sitefinity
{
    public interface IOdataContext<T> where T: class, new()
    {
        Task<HttpClient> GetHttpClientAsync();

        Task<PagedOdataResult<T>> GetResult(Uri requestUri, bool shouldAudit);
        Task<string> PostAsync(Uri requestUri, T data);
        Task<string> PutAsync(Uri requestUri, string relatedEntityLink);
        Task DeleteAsync(string requestUri);
    }
}