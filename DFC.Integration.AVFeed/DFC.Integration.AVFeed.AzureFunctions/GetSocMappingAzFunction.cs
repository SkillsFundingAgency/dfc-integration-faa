using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using DFC.Integration.AVFeed.Data.Models;
using System.Collections.Generic;
using DFC.Integration.AVFeed.Core;
using System.Threading.Tasks;

namespace DFC.Integration.AVFeed.AzureFunctions
{
    public static class GetSocMappingAzFunction
    {
        [FunctionName(nameof(GetSocMappingAzFunction))]
        public async static Task Run
        (
            [TimerTrigger("0 0 4 * * *")]TimerInfo myTimer, 
            TraceWriter log,
            [Queue("socmapping")] IAsyncCollector<SocMapping> output,
            [DocumentDB("AVFeedAudit", "AuditRecords", ConnectionStringSetting = "AVAuditCosmosDB")]
            IAsyncCollector<AuditRecord<string, IEnumerable<SocMapping>>> auditRecord
        )
        {
            Guid correlationId = Guid.NewGuid();
            DateTime startTime = DateTime.UtcNow;

            Function.Common.ConfigureLog.ConfigureNLogWithAppInsightsTarget();

            var result = await Function.GetMappings.Startup.RunAsync(RunMode.Azure);
            foreach (var item in result.Output)
            {
                item.CorrelationId = correlationId;
                item.AccessToken = string.Empty;
                await output.AddAsync(item);
               
            }

            await auditRecord.AddAsync(new AuditRecord<string, IEnumerable<SocMapping>>
            {
                CorrelationId = correlationId,
                StartedAt = startTime,
                EndedAt = DateTime.UtcNow,
                Function = nameof(GetSocMappingAzFunction),
                Input = myTimer.ScheduleStatus.Last.ToString(),
                Output = result.Output,
                Audit = result.AuditMessages
            });

            log.Info($"C# Timer trigger function executed at: {DateTime.Now} with CorrelationId:{correlationId}");
        }
    }
}
