using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using DFC.Integration.AVFeed.Data.Models;
using System.Linq;
using DFC.Integration.AVFeed.Function.Common;
using DFC.Integration.AVFeed.Core;
using System.Threading.Tasks;

namespace DFC.Integration.AVFeed.Function.GetAVForSoc.AzFunc
{
    public static class GetAVForSocMappingAzFunction
    {
        [FunctionName(nameof(GetAVForSocMappingAzFunction))]
        public static async Task Run(
            [QueueTrigger("socmapping")]
            SocMapping myQueueItem,
            [Queue("socmapping-invalid")]
            IAsyncCollector<SocMapping> invalidSocMappings,
            TraceWriter log,
            [Queue("projectedavfeedforsocmapping")]
            IAsyncCollector<ProjectedVacancySummary> projectedVacancySummary,
            [DocumentDB("AVFeedAudit", "AuditRecords", ConnectionStringSetting = "AVAuditCosmosDB")]
            IAsyncCollector<AuditRecord<object, object>> auditRecord)
        {
            try
            {
                var startTime = DateTime.UtcNow;

                ConfigureLog.ConfigureNLogWithAppInsightsTarget();

                var mappedResult = await Startup.RunAsync(myQueueItem, RunMode.Azure, auditRecord, new AuditRecord<object, object>
                {
                    CorrelationId = myQueueItem.CorrelationId,
                    StartedAt = startTime,
                    EndedAt = DateTime.Now,
                    Function = nameof(GetAVForSocMappingAzFunction),
                    Input = "",
                    Output = ""
                }).ConfigureAwait(false);
                await AuditMapping(myQueueItem, auditRecord, startTime, mappedResult);
                if (mappedResult.IsValidVacancy)
                {
                    var projectedResult = Function.ProjectVacanciesForSoc.Startup.Run(RunMode.Azure, mappedResult);
                    projectedResult.CorrelationId = myQueueItem.CorrelationId;
                    await projectedVacancySummary.AddAsync(projectedResult).ConfigureAwait(false);
                    await AuditProjections(myQueueItem, auditRecord, startTime, mappedResult, projectedResult);
                }
                else
                {
                    mappedResult.AccessToken = null;
                    await invalidSocMappings.AddAsync(myQueueItem);
                }
            }
            finally
            {
                log.Info($"C# Queue trigger function processed: {myQueueItem}");
            }
        }

        private static async Task AuditProjections(SocMapping myQueueItem, IAsyncCollector<AuditRecord<object, object>> auditRecord, DateTime startTime, MappedVacancySummary mappedResult, ProjectedVacancySummary projectedResult)
        {
            myQueueItem.AccessToken = null;
            await auditRecord.AddAsync(new AuditRecord<object, object>
            {
                CorrelationId = myQueueItem.CorrelationId,
                StartedAt = startTime,
                EndedAt = DateTime.Now,
                Function = "ProjectVacanciesAzFunction",
                Input = new
                {
                    CorrelationId = myQueueItem.CorrelationId,
                    SocCode = mappedResult.SocCode,
                    SocMappingId = mappedResult.SocMappingId,
                    VacanciesCount = mappedResult.Vacancies.Count()
                },
                Output = projectedResult,
            });
        }

        private static async Task AuditMapping(SocMapping myQueueItem, IAsyncCollector<AuditRecord<object, object>> auditRecord, DateTime startTime, MappedVacancySummary mappedResult)
        {
            myQueueItem.AccessToken = null;
            await auditRecord.AddAsync(new AuditRecord<object, object>
            {
                CorrelationId = myQueueItem.CorrelationId,
                StartedAt = startTime,
                EndedAt = DateTime.Now,
                Function = nameof(GetAVForSocMappingAzFunction),
                Input = myQueueItem,
                Output = new
                {
                    CorrelationId = myQueueItem.CorrelationId,
                    SocCode = mappedResult.SocCode,
                    SocMappingId = mappedResult.SocMappingId,
                    VacanciesCount = mappedResult.Vacancies.Count()
                },
            });

            int index = 1;
            foreach (var item in mappedResult.Vacancies)
            {
                await auditRecord.AddAsync(new AuditRecord<object, object>
                {
                    CorrelationId = myQueueItem.CorrelationId,
                    StartedAt = startTime,
                    EndedAt = DateTime.Now,
                    Function = nameof(GetAVForSocMappingAzFunction),
                    Input = myQueueItem,
                    Output = new
                    {
                        CorrelationId = myQueueItem.CorrelationId,
                        SocCode = mappedResult.SocCode,
                        SocMappingId = mappedResult.SocMappingId,
                        Vacancy = item,
                        Index = index++
                    },
                });
            }
        }
    }
}
