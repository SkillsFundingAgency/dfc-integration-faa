using System;
using DFC.Integration.AVFeed.Function.GetServiceHealthStatus;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;

namespace DFC.Integration.AVFeed.AzureFunctions
{
    using System.Threading.Tasks;
    using Core;
    using Data.Models;

    public static class GetAvHealthStatusInfo
    {
        [FunctionName("GetHealthStatusInfo")]
        public static async Task Run([TimerTrigger("0 */5 * * * *")]TimerInfo myTimer, TraceWriter log, [DocumentDB("AVFeedAudit", "ServiceHealthStatus", ConnectionStringSetting = "AVAuditCosmosDB")]
           IAsyncCollector<ServiceHealthCheckStatus> healthServiceStatus)
        {
            log.Info($"C# Timer trigger function executed at: with the Timer Configuration : {DateTime.Now}-:-{myTimer}");

            var healthStatus = await Startup.RunAsync(RunMode.Console, null);
            await healthServiceStatus.AddAsync(healthStatus);
        }
    }
}