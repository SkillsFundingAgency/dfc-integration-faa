using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DFC.Integration.AVFeed.Data.Interfaces;
using DFC.Integration.AVFeed.Repository.Sitefinity.Base;

namespace DFC.Integration.AVFeed.Repository.Sitefinity
{
    public class CustomApiContextService : HttpClient, ICustomApiContextService
    {
        private const string CookieHeaderName = "Set-Cookie";
        private const string AuthorizationHeaderName = "Authorization";
        private const string BearerHeaderName = "Bearer";
       
        private readonly CookieContainer cookieContainer;
        private readonly IApplicationLogger applicationLogger;
        private readonly ITokenClient tokenClient;
        private readonly ICustomApiConfig customApiConfig;

        private bool CookiesConfigured = false;

        public CustomApiContextService(IApplicationLogger applicationLogger, ITokenClient tokenClient, ICustomApiConfig customApiConfig)
        {
            this.applicationLogger = applicationLogger;
            this.tokenClient = tokenClient;
            this.customApiConfig = customApiConfig;
            BaseAddress = customApiConfig.GetBaseAddressUrl();
            cookieContainer = new CookieContainer();
        }

        private async Task ConfigureCoookies()
        {
            var accessToken = await tokenClient.GetAccessTokenAsync();

            DefaultRequestHeaders.Add(AuthorizationHeaderName, $"{BearerHeaderName} {accessToken}");

            applicationLogger.Trace($"Start - Auth Cookie endpoint {customApiConfig.GetAuthCookieEndpoint().OriginalString} called.");

            var response = await GetAsync(customApiConfig.GetAuthCookieEndpoint());
            response.Headers.TryGetValues(CookieHeaderName, out var cookies);
            var cookieList = cookies as IList<string> ?? cookies.ToList();
            if (!cookieList.Any())
            {
                applicationLogger.Trace(
                    $"Auth Cookie client {customApiConfig.GetAuthCookieEndpoint().OriginalString} called failed with error {response.StatusCode}.");
                throw new ApplicationException("Couldn't get auth cookie token. Error: " + response.StatusCode);
            }
            
            cookieContainer.SetCookies(BaseAddress, string.Join(", ", cookieList));
            CookiesConfigured = true;
        }

        public async Task<HttpStatusCode> DeleteAVsRecycleBinRecordsAsync(int itemCount)
        {
            if (!CookiesConfigured)
            {
                await ConfigureCoookies();
            }

            applicationLogger.Info($"Start - ClearAVsRecycleBin {customApiConfig.GetClearRequestUrl().OriginalString} called with {itemCount} items");
            var result = await PostAsync(customApiConfig.GetClearRequestUrl(), new StringContent("{\"itemCount\":\""+ itemCount+"\"}", Encoding.UTF8, "application/json"));
            applicationLogger.Info($"End - ClearAVsRecycleBin {customApiConfig.GetClearRequestUrl().OriginalString} called with {itemCount} items: Result was {result.StatusCode}");
            return result.StatusCode;
        }
    }
}
