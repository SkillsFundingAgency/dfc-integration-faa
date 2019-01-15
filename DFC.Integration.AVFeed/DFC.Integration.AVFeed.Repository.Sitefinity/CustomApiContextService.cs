using DFC.Integration.AVFeed.Data.Interfaces;
using DFC.Integration.AVFeed.Repository.Sitefinity.Base;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace DFC.Integration.AVFeed.Repository.Sitefinity
{
    public class CustomApiContextService : ICustomApiContextService
    {
        private const string CONTENT_TYPE = "application/json";

        private readonly IApplicationLogger applicationLogger;
        private readonly ITokenClient tokenClient;
        private readonly ICustomApiConfig customApiConfig;
        private readonly IHttpClientService httpClientService;

        public CustomApiContextService(
            IApplicationLogger applicationLogger,
            ITokenClient tokenClient,
            ICustomApiConfig customApiConfig,
            IHttpClientService httpClientService)
        {
            this.applicationLogger = applicationLogger;
            this.tokenClient = tokenClient;
            this.customApiConfig = customApiConfig;
            this.httpClientService = httpClientService;
        }

        public async Task<HttpClient> GetHttpClientAsync()
        {
            var accessToken = await tokenClient.GetAccessTokenAsync();

            var httpClient = httpClientService.GetHttpClient();
            httpClient.SetBearerToken(accessToken);
            httpClient.DefaultRequestHeaders.Add("X-SF-Service-Request", "true");
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(CONTENT_TYPE));
            return httpClient;
        }

        public async Task<HttpStatusCode> DeleteAVsRecycleBinRecordsAsync(int numberToDelete)
        {
            using (var httpClient = await GetHttpClientAsync())
            {
                var ClearRequestUrl = customApiConfig.GetClearRequestUrl();
                var deleteUri = $"{ClearRequestUrl}?count={numberToDelete}";
                applicationLogger.Trace($"Start - ClearAVsRecycleBin {ClearRequestUrl.OriginalString} called with {numberToDelete} items");

                var result = await httpClient.DeleteAsync(deleteUri);
                applicationLogger.Trace($"End - ClearAVsRecycleBin {ClearRequestUrl.OriginalString} called with {numberToDelete} items: Result was {result.StatusCode}");

                if (result.StatusCode != HttpStatusCode.OK || result.StatusCode != HttpStatusCode.PartialContent)
                {
                    var content = await result.Content?.ReadAsStringAsync();
                    var errorMessage = $"Got unexpected response code {result.StatusCode} - {result.ReasonPhrase}";
                    applicationLogger.Error($"{errorMessage} - {content} from ClearAVsRecycleBin API request", new HttpRequestException(errorMessage));
                }

                return result.StatusCode;
            }
        }
    }
}