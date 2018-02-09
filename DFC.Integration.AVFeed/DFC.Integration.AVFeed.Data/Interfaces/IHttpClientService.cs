using System.Net.Http;

namespace DFC.Integration.AVFeed.Data.Interfaces
{
    /// <summary>
    /// IhttpClient Service Interface
    /// </summary>
    public interface IHttpClientService
    {
        /// <summary>
        /// Gets the HTTP client.
        /// </summary>
        /// <returns></returns>
        HttpClient GetHttpClient();
    }
}
