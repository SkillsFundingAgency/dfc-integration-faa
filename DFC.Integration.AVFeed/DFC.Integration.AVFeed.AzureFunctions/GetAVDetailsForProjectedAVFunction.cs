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

namespace DFC.Integration.AVFeed.Function.GetAVDetailsForProjectedAV.AzFunc
{
    public static class GetAVDetailsForProjectedAVFunction
    {
        [FunctionName(nameof(GetAVDetailsForProjectedAVFunction))]
        public static async Task Run(
            [QueueTrigger("projectedavfeedforsocmapping")]
            ProjectedVacancySummary myQueueItem,
            TraceWriter log,
            [Queue("projectedavdetails")]
            IAsyncCollector<ProjectedVacancyDetails> projectedVacancyDetails,
            [DocumentDB("AVFeedAudit", "AuditRecords", ConnectionStringSetting = "AVAuditCosmosDB")]
            IAsyncCollector<AuditRecord<object, object>> auditRecord)
        {
            try
            {
                await GetDetailsForProjectedAvFunc(myQueueItem, projectedVacancyDetails, auditRecord);
            }
            finally
            {
                log.Info($"C# Queue trigger function processed: {myQueueItem}");
            }
        }

        private static async Task GetDetailsForProjectedAvFunc(ProjectedVacancySummary myQueueItem, IAsyncCollector<ProjectedVacancyDetails> projectedVacancyDetails, IAsyncCollector<AuditRecord<object, object>> auditRecord, int attempt = 1)
        {
            try
            {
                var startTime = DateTime.UtcNow;

                ConfigureLog.ConfigureNLogWithAppInsightsTarget();

                var getDetailsResult = await Startup.RunAsync(myQueueItem, RunMode.Azure, auditRecord, new AuditRecord<object, object>
                {
                    CorrelationId = myQueueItem.CorrelationId,
                    StartedAt = startTime,
                    EndedAt = DateTime.Now,
                    Function = nameof(GetAVDetailsForProjectedAVFunction),
                    Input = "",
                    Output = ""
                }).ConfigureAwait(false);

                getDetailsResult.CorrelationId = myQueueItem.CorrelationId;
                await AuditMapping(myQueueItem, auditRecord, startTime, getDetailsResult);
                await projectedVacancyDetails.AddAsync(getDetailsResult).ConfigureAwait(false);
            }
            catch (AvApiResponseException responseException)
            {
                if (responseException.StatusCode == ((HttpStatusCode)429) && attempt < 3)
                {
                    var retryInSeconds = 60;
                    int.TryParse(Regex.Match(responseException.Message, "\\d+")?.Value, out retryInSeconds);
                    Thread.Sleep(retryInSeconds * 1000);
                    await GetDetailsForProjectedAvFunc(myQueueItem, projectedVacancyDetails, auditRecord, attempt++);
                }
                else
                {
                    throw;
                }
            }
        }

        private static async Task AuditMapping(ProjectedVacancySummary myQueueItem, IAsyncCollector<AuditRecord<object, object>> auditRecord, DateTime startTime, ProjectedVacancyDetails getDetailsResult)
        {
            myQueueItem.AccessToken = null;
            await auditRecord.AddAsync(new AuditRecord<object, object>
            {
                CorrelationId = myQueueItem.CorrelationId,
                StartedAt = startTime,
                EndedAt = DateTime.Now,
                Function = nameof(GetAVDetailsForProjectedAVFunction),
                Input = myQueueItem,
                Output = new
                {
                    CorrelationId = myQueueItem.CorrelationId,
                    SocCode = getDetailsResult.SocCode,
                    SocMappingId = getDetailsResult.SocMappingId,
                    VacanciesCount = getDetailsResult.Vacancies.Count()
                },
            });

            int index = 1;
            foreach (var item in getDetailsResult.Vacancies)
            {
                await auditRecord.AddAsync(new AuditRecord<object, object>
                {
                    CorrelationId = myQueueItem.CorrelationId,
                    StartedAt = startTime,
                    EndedAt = DateTime.Now,
                    Function = nameof(GetAVDetailsForProjectedAVFunction),
                    Input = myQueueItem,
                    Output = new
                    {
                        CorrelationId = myQueueItem.CorrelationId,
                        SocCode = getDetailsResult.SocCode,
                        SocMappingId = getDetailsResult.SocMappingId,
                        Vacancy = item,
                        Index = index++
                    },
                });
            }
        }
    }
}