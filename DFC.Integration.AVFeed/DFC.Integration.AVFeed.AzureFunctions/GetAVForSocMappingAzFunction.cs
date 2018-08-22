using DFC.Integration.AVFeed.Core;
using DFC.Integration.AVFeed.Data.Models;
using DFC.Integration.AVFeed.Function.Common;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using System;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
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
                await GetAVForSocMappingFunc(myQueueItem, projectedVacancySummary, auditRecord, invalidSocMappings, log);
            }
            finally
            {
                log.Info($"C# Queue trigger function processed: {myQueueItem}");
            }
        }

        private static async Task GetAVForSocMappingFunc(SocMapping myQueueItem, IAsyncCollector<ProjectedVacancySummary> projectedVacancySummary, IAsyncCollector<AuditRecord<object, object>> auditRecord, IAsyncCollector<SocMapping> invalidSocMappings, TraceWriter log, int attempt = 1)
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

                var projectedResult = Function.ProjectVacanciesForSoc.Startup.Run(RunMode.Azure, mappedResult);
                projectedResult.CorrelationId = myQueueItem.CorrelationId;
                await projectedVacancySummary.AddAsync(projectedResult).ConfigureAwait(false);
                await AuditProjections(myQueueItem, auditRecord, startTime, mappedResult, projectedResult);
            }
            catch (AvApiResponseException responseException)
            {
                if (responseException.StatusCode == HttpStatusCode.BadRequest)
                {
                    await invalidSocMappings.AddAsync(myQueueItem);
                    log.Warning($"Exception raised while fetching AV API -Url:{responseException.Message} with ErrorCode :{responseException.StatusCode}");
                }
                else if (responseException.StatusCode == ((HttpStatusCode)429) && attempt < 3)
                {
                    var retryInSeconds = 60;
                    int.TryParse(Regex.Match(responseException.Message, "\\d+")?.Value, out retryInSeconds);
                    Thread.Sleep(retryInSeconds * 1000);
                    await GetAVForSocMappingFunc(myQueueItem, projectedVacancySummary, auditRecord, invalidSocMappings, log, attempt++);
                }
                else
                {
                    throw;
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