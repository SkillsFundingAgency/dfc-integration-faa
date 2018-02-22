using System;
using System.Threading.Tasks;
using System.Net;
using DFC.Integration.AVFeed.Core;
using DFC.Integration.AVFeed.Data.Interfaces;
using DFC.Integration.AVFeed.Data.Models;
using Microsoft.Azure.WebJobs.Host;
namespace DFC.Integration.AVFeed.Function.GetServiceHealthStatus
{
    using Interfaces;

    public class GetAvServiceHealthStatus:IGetServiceHealthStatus
    {
        private IHttpExternalFeedProxy HttpExternalFeedProxy { get; set; }

        // Default constructor required for DI Autofac
        public GetAvServiceHealthStatus(IHttpExternalFeedProxy httpClientServiceProxy)
        {
            this.HttpExternalFeedProxy = httpClientServiceProxy;
        }
        #region Implementation of IGetServiceHealthStatus
       
        public async Task<ServiceHealthCheckStatus> GetAvFeedHealthStatusInfoAsync()
        {
            return await GetAvFeedExternalFeedStatusAsync(Constants.ApprenticeshipEndpoint);
        }

        public async Task<ServiceHealthCheckStatus> GetSitefinityHealthStatusInfoAsync()
        {
           return await GetAvFeedExternalFeedStatusAsync(Constants.SiteFinityEndpoint);
        }
      
        public  async Task<ServiceHealthCheckStatus> GetAvFeedExternalFeedStatusAsync(string url)
        {
            var healthCheckStatus = new ServiceHealthCheckStatus {ApplicationName = url};
            try
            {
                using (var avWcfFeedResponse = await HttpExternalFeedProxy.GetResponseFromUri(url))
                {
                    healthCheckStatus.ApplicationStatus = avWcfFeedResponse.StatusCode;
                    healthCheckStatus.IsApplicationExternal = true;
                    healthCheckStatus.IsApplicationRunning = true;
                    healthCheckStatus.ApplicationStatusDescription = avWcfFeedResponse.StatusDescription;
                }
                  return healthCheckStatus;
            }
            catch (Exception ex)
            {
                healthCheckStatus.ApplicationStatusDescription = ex.Message;
                return healthCheckStatus;
            }
        }
        #endregion
    }
}
