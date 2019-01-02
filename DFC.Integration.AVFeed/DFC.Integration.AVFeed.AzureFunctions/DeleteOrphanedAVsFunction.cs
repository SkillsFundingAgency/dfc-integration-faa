using System;
using System.Threading.Tasks;
using DFC.Integration.AVFeed.Core;
using DFC.Integration.AVFeed.Data.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;

namespace DFC.Integration.AVFeed.Function.DeleteOrphanedAVs
{
    public static class DeleteOrphanedAVsFunction
    {
        [FunctionName("DeleteOrphanedAVsFunction")]
        public async static Task Run([TimerTrigger("0 01 2 * * *")]TimerInfo myTimer, 
            TraceWriter log,
            [DocumentDB("AVFeedAudit", "AuditRecords", ConnectionStringSetting = "AVAuditCosmosDB")]
            IAsyncCollector<AuditRecord<object, object>> auditRecord)
        {
            Guid correlationId = Guid.NewGuid();
            DateTime startDate = DateTime.Now;
          
            Function.Common.ConfigureLog.ConfigureNLogWithAppInsightsTarget();
            log.Info($"DeleteOrphanedAVsFunction Timer trigger function executed at: {startDate} with CorrelationId:{correlationId}");

            await Startup.RunAsync(RunMode.Azure, auditRecord, new AuditRecord<object, object>
            {
                CorrelationId = correlationId,
                StartedAt = startDate,
                EndedAt = DateTime.Now,
                Function = nameof(DeleteOrphanedAVsFunction),
                Input = "",
                Output = ""
            });

            log.Info($"DeleteOrphanedAVsFunction Timer trigger function completed at: {DateTime.Now} with CorrelationId:{correlationId}");
        }
    }
}
