using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using DFC.Integration.AVFeed.Data.Models;
using System.Linq;
using DFC.Integration.AVFeed.Function.Common;
using DFC.Integration.AVFeed.Core;
using System.Threading.Tasks;

namespace DFC.Integration.AVFeed.Function.GetAVDetailsForProjectedAV.AzFunc
{
    using System.Diagnostics;
    using System.Threading;

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
            Stopwatch stopWatch=new Stopwatch();
            try
            {
                stopWatch.Start();
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
            finally
            {
                log.Info($"C# Queue trigger function processed: {myQueueItem}");
                stopWatch.Stop();
                if (stopWatch.Elapsed.Seconds < 30)
                {
                    Thread.Sleep(30-stopWatch.Elapsed.Seconds);
                }
               log.Info($"C# GetAVDetailsForProjectedAVFunction ElapsedTime[Second] - ElapsedTime : {stopWatch.Elapsed.Seconds} - {stopWatch.Elapsed}");
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
