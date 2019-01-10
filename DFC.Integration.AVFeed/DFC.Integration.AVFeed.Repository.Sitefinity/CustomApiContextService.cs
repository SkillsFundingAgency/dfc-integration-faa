using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DFC.Integration.AVFeed.Data.Interfaces;

namespace DFC.Integration.AVFeed.Repository.Sitefinity
{
    public class CustomApiContextService : HttpClient, ICustomApiContextService
    {
        private const string CookieHeaderName = "Set-Cookie";
        private const string AuthorizationHeaderName = "Authorization";
        private const string BearerHeaderName = "Bearer";
        private static readonly Uri ClearRequestUrl = new Uri(ConfigurationManager.AppSettings.Get("Sitefinity.ClearAVBinURL"));
        private static string BaseAddressUrl => ConfigurationManager.AppSettings.Get("Sitefinity.BaseAddress");
        private static string AuthCookieEndpoint => ConfigurationManager.AppSettings.Get("Sitefinity.AuthCookieEndpoint");

        private readonly CookieContainer cookieContainer;
        private readonly IApplicationLogger applicationLogger;
        private readonly ITokenClient tokenClient;

        public CustomApiContextService(IApplicationLogger applicationLogger, ITokenClient tokenClient)
        {
            this.applicationLogger = applicationLogger;
            this.tokenClient = tokenClient;
            BaseAddress = new Uri(BaseAddressUrl);
            cookieContainer = new CookieContainer();
        }

        private async Task ConfigureCoookies()
        {
            var accessToken = await tokenClient.GetAccessTokenAsync();

            DefaultRequestHeaders.Add(AuthorizationHeaderName, $"{BearerHeaderName} {accessToken}");

            applicationLogger.Info($"Start - Auth Cookie endpoint {AuthCookieEndpoint} called.");

            var response = await GetAsync(new Uri(AuthCookieEndpoint));
            response.Headers.TryGetValues(CookieHeaderName, out var cookies);
            var cookieList = cookies as IList<string> ?? cookies.ToList();
            if (!cookieList.Any())
            {
                applicationLogger.Info(
                    $"Auth Cookie client {AuthCookieEndpoint} called failed with error {response.StatusCode}.");
                throw new ApplicationException("Couldn't get auth cookie token. Error: " + response.StatusCode);
            }
            
            cookieContainer.SetCookies(BaseAddress, string.Join(", ", cookieList));
        }

        public async Task ClearAVsRecycleBin(int itemCount)
        {
            await ConfigureCoookies();

            applicationLogger.Info($"Start - ClearAVsRecycleBin {ClearRequestUrl} called with {itemCount} items");

            var result = await PostAsync(ClearRequestUrl, new StringContent("{\"itemCount\":\""+ itemCount+"\"}", Encoding.UTF8, "application/json"));

            applicationLogger.Info($"End - ClearAVsRecycleBin {ClearRequestUrl} called with {itemCount} items: Result was {result.StatusCode}");
        }
    }
}
