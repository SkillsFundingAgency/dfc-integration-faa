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
        public async static Task Run([TimerTrigger("0 */5 * * * *")]TimerInfo myTimer, 
            TraceWriter log,
            [DocumentDB("AVFeedAudit", "AuditRecords", ConnectionStringSetting = "AVAuditCosmosDB")]
            IAsyncCollector<AuditRecord<object, object>> auditRecord)
        {
            Guid correlationId = Guid.NewGuid();
            DateTime startTime = DateTime.UtcNow;

            log.Info($"DeleteOrphanedAVsFunction Timer trigger function executed at: {DateTime.Now} with CorrelationId:{correlationId}");

            Function.Common.ConfigureLog.ConfigureNLogWithAppInsightsTarget();

            //var result = await Startup.RunAsync(RunMode.Azure);

            await Startup.RunAsync(RunMode.Azure);

            await auditRecord.AddAsync(new AuditRecord<object, object>
            {
                CorrelationId = correlationId,
                StartedAt = startTime,
                EndedAt = DateTime.Now,
                Function = nameof(DeleteOrphanedAVsFunction),
                Input = $"Triggered  {myTimer.ScheduleStatus.Last.ToString()}",
                Output = "Removed xxx records"
            });

            log.Info($"DeleteOrphanedAVsFunction Timer trigger function completed at: {DateTime.Now} with CorrelationId:{correlationId}");
        }
    }
}
