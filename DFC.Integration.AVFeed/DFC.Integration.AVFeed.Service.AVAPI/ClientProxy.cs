using DFC.Integration.AVFeed.Data.Interfaces;
using DFC.Integration.AVFeed.Service.AVSoapAPI.FAA;
using System;
using System.Configuration;
using System.Net.Http;
using System.ServiceModel;
using System.Threading.Tasks;

namespace DFC.Integration.AVFeed.Service.AVAPI
{
    public class ClientProxy : IApprenticeshipVacancyApi
    {
        private string _endpoint = ConfigurationManager.AppSettings.Get("FAA.URL");
        private IApplicationLogger logger;

        public ClientProxy(IApplicationLogger logger)
        {
            this.logger = logger;
        }
        public async Task<string> GetAsync(string requestQueryString)
        {
            using (var clientProxy = new HttpClient())
            {
                var fullRequest = $"{_endpoint}/Search?{requestQueryString}";
                var response = await clientProxy.GetAsync(fullRequest);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    logger.Trace($"Error status {response.StatusCode},  Getting API data for request :'{fullRequest}'");
                }
            }
        }

    }
}
