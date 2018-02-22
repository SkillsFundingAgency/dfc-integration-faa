namespace DFC.Integration.AVFeed.Function.GetServiceHealthStatus.Interfaces
{
    using System.Net;
    using System.Threading.Tasks;

    public interface IHttpExternalFeedProxy
    {
        Task<HttpWebResponse> GetResponseFromUri(string uri);
    }
}