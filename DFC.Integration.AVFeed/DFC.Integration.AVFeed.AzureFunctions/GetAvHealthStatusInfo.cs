using System;
using DFC.Integration.AVFeed.Function.GetServiceHealthStatus;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using System.Linq;
using System.Threading.Tasks;
using DFC.Integration.AVFeed.Core;
using DFC.Integration.AVFeed.Data.Models;

namespace DFC.Integration.AVFeed.AzureFunctions
{

    public static class GetAvHealthStatusInfo
    {
        [FunctionName("GetHealthStatusInfo")]
        public static async Task Run([TimerTrigger("0 */5 * * * *")]TimerInfo myTimer, TraceWriter log, [DocumentDB("AVFeedAudit", "ServiceHealthStatus", ConnectionStringSetting = "AVAuditCosmosDB")]
           IAsyncCollector<FeedsServiceHealthCheck> healthServiceStatus)
        {
            log.Info($"GetHealthStatusInfo function executed at: with :- {DateTime.Now}-:-{myTimer}");
            try
            {
                var healthStatus = await Startup.RunAsync(RunMode.Azure);
                await healthServiceStatus.AddAsync(healthStatus);
                if (healthStatus.FeedsServiceHealth.Any(fail => !fail.IsApplicationRunning))
                {
                    throw new Exception($"External Feed {healthStatus?.FeedsServiceHealth?.FirstOrDefault(fail => !fail.IsApplicationRunning)?.ApplicationName}. not responding.Please check the documents in the ComosDB");
                }
            }
            catch (Exception e)
            {
                log.Info($"GetHealthStatusInfo function throws exception at: :- {DateTime.Now}-:-{myTimer}-:-with Exception-:- {e.Message}");
                throw;
            }
        }
    }
}