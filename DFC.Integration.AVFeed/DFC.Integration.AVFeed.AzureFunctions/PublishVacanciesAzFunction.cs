using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using DFC.Integration.AVFeed.Data.Models;
using System.Threading.Tasks;

namespace DFC.Integration.AVFeed.AzureFunctions
{
    public static class PublishVacanciesAzFunction
    {
        [FunctionName(nameof(PublishVacanciesAzFunction))]
        public static async Task Run(
            [QueueTrigger("projectedavdetails")]
            ProjectedVacancyDetails myQueueItem,
            TraceWriter log,
            [DocumentDB("AVFeedAudit", "AuditRecords", ConnectionStringSetting = "AVAuditCosmosDB")]
            IAsyncCollector<AuditRecord<ProjectedVacancyDetails, PublishedVacancySummary>> auditRecord)
        {
            DateTime startTime = DateTime.UtcNow;

            Function.Common.ConfigureLog.ConfigureNLogWithAppInsightsTarget();

            var result = await Function.PublishSfVacancy.Startup.RunAsync(myQueueItem, Core.RunMode.Azure);

            await auditRecord.AddAsync(new AuditRecord<ProjectedVacancyDetails, PublishedVacancySummary>
            {
                CorrelationId = myQueueItem.CorrelationId,
                StartedAt = startTime,
                EndedAt = DateTime.UtcNow,
                Function = nameof(PublishVacanciesAzFunction),
                Input = myQueueItem,
                Output = result.Output,
                Audit = result.AuditMessages
            });

            log.Info($"C# Queue trigger function processed: {myQueueItem}");
        }
    }
}
