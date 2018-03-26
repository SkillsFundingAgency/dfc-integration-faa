namespace DFC.Integration.AVFeed.Data.Interfaces
{
    using System.Threading.Tasks;
    using Models;
    using Newtonsoft.Json.Serialization;

    public interface IGetServiceHealthStatus
    {
        Task<FeedsServiceHealthCheck> GetServiceHealthStateAsync();
        Task<ServiceStatus> GetApprenticeshipFeedHealthStatusAsync();
        Task<ServiceStatus> GetSitefinityHealthStatusAsync();
      
    }
}
