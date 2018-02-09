using System.Net.Http;
using DFC.Integration.AVFeed.Data.Interfaces;

namespace DFC.Integration.AVFeed.Repository.Sitefinity
{
    /// <summary>
    /// Implementation of IhttpClientService Interface
    /// </summary>
    /// <seealso cref="DFC.Integration.AVFeed.Data.Interfaces.IHttpClientService" />
    public class HttpClientService : IHttpClientService
    {
        /// <summary>
        /// Gets the HTTP client.
        /// </summary>
        /// <returns></returns>
        public HttpClient GetHttpClient()
        {
            return new HttpClient();
        }
    }
}
