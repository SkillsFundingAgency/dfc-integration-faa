using System;
using DFC.Integration.AVFeed.Function.GetServiceHealthStatus;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using System.Linq;
using System.Threading.Tasks;
using DFC.Integration.AVFeed.Core;
using DFC.Integration.AVFeed.Data.Models;
using System.Net.Http;
using System.Net;
using Newtonsoft.Json;
using System.Text;

namespace DFC.Integration.AVFeed.AzureFunctions
{

    public static class GetAvHealthStatusInfo
    {
        [FunctionName("GetHealthStatusInfo")]
        public static async Task Run([TimerTrigger("0 0 * * * *")]TimerInfo myTimer, TraceWriter log, [DocumentDB("AVFeedAudit", "ServiceHealthStatus", ConnectionStringSetting = "AVAuditCosmosDB")]
           IAsyncCollector<AuditRecord<object, object>> healthServiceStatus)
        {
            Guid correlationId = Guid.NewGuid();
            DateTime startDate = DateTime.Now;

            Function.Common.ConfigureLog.ConfigureNLogWithAppInsightsTarget();

            log.Info($"GetHealthStatusInfo [correlationId:{correlationId}] function executed at: with :- {DateTime.Now}-:-{myTimer}");
            try
            {
                var healthStatus = await Startup.RunAsync(RunMode.Azure, healthServiceStatus, new AuditRecord<object, object>
                {
                    CorrelationId = correlationId,
                    StartedAt = startDate,
                    EndedAt = DateTime.Now,
                    Function = nameof(GetAvHealthStatusInfo),
                    Input = "",
                    Output = ""
                });

                await healthServiceStatus.AddAsync(new AuditRecord<object, object>
                {
                    CorrelationId = correlationId,
                    StartedAt = startDate,
                    EndedAt = DateTime.Now,
                    Function = nameof(GetAvHealthStatusInfo),
                    Input = $"Triggered at {myTimer}",
                    Output = healthStatus
                });

                if (healthStatus.FeedsServiceHealth.Any(service => service.Status != ServiceState.Green ))
                {
                    throw new Exception($"External Feed(s) not responding. Please check the logs in the ComosDB. Check response details : {JsonConvert.SerializeObject(healthStatus)}");
                }
            }
           catch (Exception e)
           {
               log.Info($"GetHealthStatusInfo [correlationId:{correlationId}] function throws exception at: :- {DateTime.Now}-:-{myTimer}-:-with Exception-:- {e.Message}");
               throw;
           }
        }
      
    }
}