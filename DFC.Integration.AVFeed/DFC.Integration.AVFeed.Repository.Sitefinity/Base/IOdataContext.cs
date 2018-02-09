using DFC.Integration.AVFeed.Repository.Sitefinity.Model;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace DFC.Integration.AVFeed.Repository.Sitefinity
{
    public interface IOdataContext<T> where T: class, new()
    {
        Task<HttpClient> GetHttpClientAsync();

        Task<PagedOdataResult<T>> GetResult(Uri requestUri);
        Task<string> PostAsync(Uri requestUri, T data);
        Task<string> PutAsync(Uri requestUri, string relatedEntityLink);
    }
}