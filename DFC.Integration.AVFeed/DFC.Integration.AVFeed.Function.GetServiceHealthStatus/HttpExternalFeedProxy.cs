namespace DFC.Integration.AVFeed.Function.GetServiceHealthStatus
{
    using System;
    using System.Net;
    using System.Threading.Tasks;
    using Interfaces;

    public class HttpExternalFeedProxy : IHttpExternalFeedProxy
    {
        #region Implementation of IHttpClientServiceProxy

        public async Task<HttpWebResponse> GetResponseFromUri(string uri)
        {
            var avWcfFeedRequest = WebRequest.CreateHttp(new Uri(uri));
            {
                return (HttpWebResponse) (await avWcfFeedRequest.GetResponseAsync().ConfigureAwait(false));
            }
        }

        #endregion
    }
}