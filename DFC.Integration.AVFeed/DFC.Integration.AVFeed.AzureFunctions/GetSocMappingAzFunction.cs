using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using DFC.Integration.AVFeed.Data.Models;
using System.Collections.Generic;
using DFC.Integration.AVFeed.Core;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;
using System.Linq;

namespace DFC.Integration.AVFeed.AzureFunctions
{
    public static class GetSocMappingAzFunction
    {
        [FunctionName(nameof(GetSocMappingAzFunction))]
        public async static Task Run
        (
            [TimerTrigger("0 31 3 * * *")]TimerInfo myTimer,
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
            var resultWithData = result.Output.Where(i => i.Standards?.Count() > 0 || i.Frameworks?.Count() > 0);
            var counter = 1;
            var total = resultWithData.Count();
            foreach (var item in resultWithData)
            {
                item.CorrelationId = correlationId;
                //item.AccessToken = string.Empty;
                await output.AddAsync(item);
                await output.FlushAsync();
                await Task.Delay(Constants.AVFeedSecondsBetweenGetForSOC * 1000);
                log.Info($"C# Timer trigger function executing at: {DateTime.Now} with CorrelationId:{correlationId} - Added SOC mapping to the queue total so far added {counter++} of {total} to the queue and waiting for {Constants.AVFeedSecondsBetweenGetForSOC} seconds");
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
