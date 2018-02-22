using System;
using System.Threading.Tasks;
using DFC.Integration.AVFeed.Core;
using DFC.Integration.AVFeed.Data.Interfaces;
using DFC.Integration.AVFeed.Data.Models;

namespace DFC.Integration.AVFeed.Function.GetServiceHealthStatus
{
    using Interfaces;

    public class GetAvServiceHealthStatus:IGetServiceHealthStatus
    {
        private IHttpExternalFeedProxy HttpExternalFeedProxy { get; set; }

        public GetAvServiceHealthStatus(IHttpExternalFeedProxy httpClientServiceProxy)
        {
            HttpExternalFeedProxy = httpClientServiceProxy;
        }
        #region Implementation of IGetServiceHealthStatus
       
        public async Task<ServiceHealthCheckStatus> GetApprenticeshipFeedHealthStatusAsync()
        {
            return await GetExternalFeedStatusAsync(Constants.ApprenticeshipEndpoint);
        }

        public async Task<ServiceHealthCheckStatus> GetSitefinityHealthStatusAsync()
        {
           return await GetExternalFeedStatusAsync(Constants.SiteFinityEndpoint);
        }
      
        public  async Task<ServiceHealthCheckStatus> GetExternalFeedStatusAsync(string url)
        {
            var healthCheckStatus = new ServiceHealthCheckStatus {ApplicationName = url};
            try
            {
                using (var avWcfFeedResponse = await HttpExternalFeedProxy.GetResponseFromUri(url))
                {
                    healthCheckStatus.ApplicationStatus = avWcfFeedResponse.StatusCode;
                    healthCheckStatus.IsApplicationExternal = true;
                    healthCheckStatus.IsApplicationRunning = true;
                    healthCheckStatus.ApplicationStatusDescription = $"Application endpoint at :{url} is in healthy state."; 
                }
                  return healthCheckStatus;
            }
            catch (Exception)
            {
                healthCheckStatus.FailedAt = DateTime.UtcNow;
                healthCheckStatus.ApplicationStatusDescription = $"Application endpoint at :{url} failed to respond.Check endpoint configuration.";
                return healthCheckStatus;
            }
        }
        #endregion
    }
}
