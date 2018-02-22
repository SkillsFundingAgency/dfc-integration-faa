using System;
using DFC.Integration.AVFeed.Function.GetServiceHealthStatus;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;

namespace DFC.Integration.AVFeed.AzureFunctions
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Core;
    using Data.Models;

    public static class GetAvHealthStatusInfo
    {
        [FunctionName("GetHealthStatusInfo")]
        public static async Task Run([TimerTrigger("0 */5 * * * *")]TimerInfo myTimer, TraceWriter log, [DocumentDB("AVFeedAudit", "ServiceHealthStatus", ConnectionStringSetting = "AVAuditCosmosDB")]
           IAsyncCollector<ICollection<ServiceHealthCheckStatus>> healthServiceStatus)
        {
            log.Info($"GetHealthStatusCheck function executed at: with :- {DateTime.Now}-:-{myTimer}");
            try
            {
                var healthStatus = await Startup.RunAsync(RunMode.Azure);
                await healthServiceStatus.AddAsync(healthStatus);
                if (healthStatus.Any(fail => !fail.IsApplicationRunning))
                {
                    throw new Exception("External Feed not responding");
                }
            }
            catch (Exception e)
            {
                log.Info($"GetHealthStatusCheck function throws exception at: :- {DateTime.Now}-:-{myTimer}-:-with Exception-:- {e.Message}");
                throw;
            }
          
        }
    }
}