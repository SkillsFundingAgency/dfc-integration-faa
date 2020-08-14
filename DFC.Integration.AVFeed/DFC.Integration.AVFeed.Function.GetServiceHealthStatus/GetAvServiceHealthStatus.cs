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
            var feedsServiceHealthCheck = new FeedsServiceHealthCheck() { FeedsServiceHealth = new Collection<ServiceStatus>(), ApplicationStatus = HttpStatusCode.OK, CheckDateTime = DateTime.Now };

            feedsServiceHealthCheck.FeedsServiceHealth.Add(await GetApprenticeshipFeedHealthStatusAsync());
            feedsServiceHealthCheck.FeedsServiceHealth.Add(await GetSitefinityHealthStatusAsync());

            //if we have any state thats is not green
            if (feedsServiceHealthCheck.FeedsServiceHealth.Any(s => s.Status != ServiceState.Green))
            {
                feedsServiceHealthCheck.ApplicationStatus = HttpStatusCode.BadGateway;
            }
            else
            {
                Logger.Trace("ServiceHealth status all - OK");
            }
            return feedsServiceHealthCheck;
        }

        public async Task<ServiceStatus> GetApprenticeshipFeedHealthStatusAsync()
        {
            var checkFrameWork = ""; 
            var checkStandard = "225";  //Plumbing and Domestic Heating Technician(Level 3)
            var serviceStatus = new ServiceStatus { ApplicationName = "Apprenticeship Feed", Status = ServiceState.Red, Notes = string.Empty };
            var checkSocMapping = new SocMapping() { SocCode = "5314", Frameworks = new string[] {}, Standards = new string[] {checkStandard} };
            serviceStatus.CheckParametersUsed = $"SocCode = {checkSocMapping.SocCode} - FrameWork = {checkFrameWork} - Standard = {checkStandard}";
            try
            {
                var apprenticeshipVacancySummaryResponse = await AvService.GetAVSumaryPageAsync(checkSocMapping, 1);
                serviceStatus.Status = ServiceState.Amber;

                if (apprenticeshipVacancySummaryResponse.TotalReturned > 0)
                {
                    var apprenticeshipVacancyDetailsResponse = await AvService.GetApprenticeshipVacancyDetailsAsync(apprenticeshipVacancySummaryResponse.Results.Take(1).FirstOrDefault().VacancyReference.ToString());
                    serviceStatus.Status = ServiceState.Green;
                }
            }
            catch (Exception ex)
            {
                LogFailedMessage(serviceStatus, ex);
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
                LogFailedMessage(serviceStatus, ex);
            }
            return serviceStatus;
        }

        private void LogFailedMessage(ServiceStatus serviceStatus, Exception ex)
        {
            serviceStatus.Notes = $"Service status check failed : {ex.Message}";
            Logger.Error($"Service status check failed", ex);
        }
        #endregion
    }
}
