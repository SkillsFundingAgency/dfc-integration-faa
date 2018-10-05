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

            log.Info($"C# Timer trigger function for  {nameof(GetSocMappingAzFunction)} executing at: {startTime}");

            var result = await Function.GetMappings.Startup.RunAsync(RunMode.Azure);
            var resultWithData = result.Output.Where(i => i.Standards?.Count() > 0 || i.Frameworks?.Count() > 0);

            var counter = 0;
            var total = resultWithData.Count();

            log.Info($"Got {total} SOCs that have a linked standard or framework CorrelationId:{correlationId}");

            //Max request the AV API will do is 100 over 60 second, each of the SOC will generate at least 2 calls so no more the 50 over a 1 min 
            //period should be fed in.
            foreach (var item in resultWithData)
            {
                    item.CorrelationId = correlationId;
                    await output.AddAsync(item);
                    await output.FlushAsync();
                    log.Info($"Pushing on to que {DateTime.Now}  for SOC {item.SocCode} SOC Id = {item.SocMappingId} - Added to the queue  {++counter} of {total} with CorrelationId:{correlationId}");
                    await Task.Delay(3000);     //
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
