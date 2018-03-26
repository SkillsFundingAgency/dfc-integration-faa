using System;
using System.Threading.Tasks;
using DFC.Integration.AVFeed.Core;
using DFC.Integration.AVFeed.Data.Interfaces;
using DFC.Integration.AVFeed.Data.Models;

namespace DFC.Integration.AVFeed.Function.GetServiceHealthStatus
{
    using DFC.Integration.AVFeed.Repository.Sitefinity;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Net;

    public class GetAvServiceHealthStatus:IGetServiceHealthStatus
    {
        private ISocSitefinityOdataRepository SitefinityOdataRepository;
        private IAVService AvService;
        private IApplicationLogger Logger;

        public GetAvServiceHealthStatus(ISocSitefinityOdataRepository sitefinityOdataRepository, IAVService avService, IApplicationLogger logger)
        {
            SitefinityOdataRepository = sitefinityOdataRepository;
            AvService = avService;
            Logger = logger;
        }
        #region Implementation of IGetServiceHealthStatus


        public async Task<FeedsServiceHealthCheck> GetServiceHealthStateAsync()
        {
            var feedsServiceHealthCheck = new FeedsServiceHealthCheck() { FeedsServiceHealth = new Collection<ServiceStatus>() };

            feedsServiceHealthCheck.FeedsServiceHealth.Add(await GetApprenticeshipFeedHealthStatusAsync());
            feedsServiceHealthCheck.FeedsServiceHealth.Add(await GetSitefinityHealthStatusAsync());

            //if we have any state thats is not green
            if (feedsServiceHealthCheck.FeedsServiceHealth.Any(s => s.Status != ServiceState.Green))
            {
                feedsServiceHealthCheck.ApplicationStatus = HttpStatusCode.BadGateway;
            }

            return feedsServiceHealthCheck;
        }

        public async Task<ServiceStatus> GetApprenticeshipFeedHealthStatusAsync()
        {
            var checkFrameWork = "Plumbing and Heating";
            var serviceStatus = new ServiceStatus { ApplicationName = "Apprenticeship Feed", Status = ServiceState.Red, Notes = string.Empty };
            var checkSocMapping = new SocMapping() { SocCode = "5314", Frameworks = new string[] { checkFrameWork }, Standards = new string[] { } };
            serviceStatus.CheckParametersUsed = $"SocCode = {checkSocMapping.SocCode} - FrameWork = {checkFrameWork}";
            try
            {
                var result = await AvService.GetApprenticeshipVacancyDetails(checkSocMapping);
                serviceStatus.Status = ServiceState.Green;
            }
            catch (Exception ex)
            {
                LogFailedMessage(serviceStatus, ex.Message);
            }
            return serviceStatus;
        }

        public async Task<ServiceStatus> GetSitefinityHealthStatusAsync()
        {
            var serviceStatus = new ServiceStatus { ApplicationName = "Sitefinity OdataRepository", Status = ServiceState.Red, Notes = string.Empty };
            try
            {
                var result = await SitefinityOdataRepository.GetAllAsync();
                serviceStatus.Status = ServiceState.Green;
            }
            catch (Exception ex)
            {
                LogFailedMessage(serviceStatus, ex.Message);
            }
            return serviceStatus;
        }

        private void LogFailedMessage(ServiceStatus serviceStatus, string traceMessage)
        {
            var activityId = Guid.NewGuid().ToString();
            serviceStatus.Notes = $"Service status check failed, check logs with activity id {activityId}";
            Logger.Info($"Service status check failed activityId = {activityId} - {traceMessage}");
        }
        #endregion
    }
}
