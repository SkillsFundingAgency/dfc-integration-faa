﻿using DFC.Integration.AVFeed.Data.Interfaces;
using System;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace DFC.Integration.AVFeed.Service
{
    public class ClientProxy : IApprenticeshipVacancyApi
    {
        private string _endpoint = ConfigurationManager.AppSettings.Get("FAA.URL");
        private string _subscriptionKey = ConfigurationManager.AppSettings.Get("FAA.SubscriptionKey");

        private IApplicationLogger logger;

        public ClientProxy(IApplicationLogger logger)
        {
            this.logger = logger;
        }
        public async Task<string> GetAsync(string requestQueryString, RequestType requestType )
        {
            using (var clientProxy = new HttpClient())
            {
                clientProxy.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _subscriptionKey);
                clientProxy.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));//ACCEPT header

                var requestRoute = requestType == RequestType.search ? $"{requestType.ToString()}?" : "";

                var fullRequest = $"{_endpoint}/{requestRoute}{requestQueryString}";

                logger.Trace($"Getting API data for request :'{fullRequest}'");

                var response = await clientProxy.GetAsync(fullRequest);

                string responseContent = string.Empty;
               
                responseContent =  await response.Content?.ReadAsStringAsync();
              
                if (!response.IsSuccessStatusCode)
                {
                    logger.Error($"Error status {response.StatusCode},  Getting API data for request :'{fullRequest}' \nResponse : {responseContent}", null );

                    //this will throw an exception as is not a success code
                    response.EnsureSuccessStatusCode();
                }
                return responseContent;
            }
        }
    }
}