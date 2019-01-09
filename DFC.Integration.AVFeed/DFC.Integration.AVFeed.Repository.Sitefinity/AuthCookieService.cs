using System;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using DFC.Integration.AVFeed.Data.Interfaces;

namespace DFC.Integration.AVFeed.Repository.Sitefinity
{
    public class AuthCookieService : HttpClient, IAuthCookieService
    {
        private const string CookieHeaderName = "Set-Cookie";
        private static readonly Uri ClearRequestUrl = new Uri(ConfigurationManager.AppSettings.Get("Sitefinity.ClearAVBinURL"));

        private static string BaseAddressUrl => ConfigurationManager.AppSettings.Get("Sitefinity.BaseAddress");
        private readonly IApplicationLogger applicationLogger;
         
        private readonly ITokenClient tokenClient;
        private static string AuthCookieEndpoint => ConfigurationManager.AppSettings.Get("Sitefinity.AuthCookieEndpoint");

        public AuthCookieService(IApplicationLogger applicationLogger, ITokenClient tokenClient)
        {
            this.applicationLogger = applicationLogger;
            this.tokenClient = tokenClient;
            BaseAddress = new Uri(BaseAddressUrl);
        }

        public async Task<string> GetAuthCookieAsync()
        {
            var accessToken = await tokenClient.GetAccessTokenAsync();
            
            DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);

            applicationLogger.Info($"Start - Auth Cookie endpoint {AuthCookieEndpoint} called.");

            var response = await GetAsync(new Uri(AuthCookieEndpoint));

            response.Headers.TryGetValues(CookieHeaderName, out var cookies);

            var cookieList = cookies as string[] ?? cookies.ToArray();
            if (!cookieList.Any())
            {
                applicationLogger.Info($"Auth Cookie client {AuthCookieEndpoint} called failed with error {response.StatusCode}.");
                    throw new ApplicationException("Couldn't get auth cookie token. Error: " + response.StatusCode);
            }

            //var cookieContainer = new CookieContainer();
            //cookieContainer.SetCookies(BaseAddress, string.Join(", ", cookieList));
            //var result = await PostAsync(ClearRequestUrl, new StringContent("{\"itemCount\":\"12\"}", Encoding.UTF8, "application/json"));

            return string.Join(", ", cookieList);
        }
    }
}
