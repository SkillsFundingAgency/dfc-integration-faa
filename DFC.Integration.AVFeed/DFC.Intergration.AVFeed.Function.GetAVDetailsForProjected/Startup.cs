using Autofac;
using DFC.Integration.AVFeed.AuditService;
using DFC.Integration.AVFeed.Core;
using DFC.Integration.AVFeed.Data.Interfaces;
using DFC.Integration.AVFeed.Data.Models;
using DFC.Integration.AVFeed.Function.Common;
using Microsoft.Azure.WebJobs;
using System.Threading.Tasks;

namespace DFC.Integration.AVFeed.Function.GetAVDetailsForProjectedAV
{
    /// <summary>
    /// Get details for projected vacancies
    /// </summary>
    public static class Startup
    {
        public static async Task<ProjectedVacancyDetails> RunAsync(ProjectedVacancySummary myQueueItem, RunMode mode)
        {
            return await RunAsync(myQueueItem, mode, null, null);
        }

        /// <summary>
        /// Runs the specified my queue item.
        /// </summary>
        /// <param name="myQueueItem">My queue item.</param>
        /// <param name="mode">The mode.</param>
        public static async Task<ProjectedVacancyDetails> RunAsync(ProjectedVacancySummary myQueueItem, RunMode mode, IAsyncCollector<AuditRecord<object, object>> asyncCollector, AuditRecord<object, object> masterRecord)
        {
            var container = ConfigureContainer(mode, asyncCollector, masterRecord);
            var getDetailsFunc = container.Resolve<IGetAvDetailsByIdsFunc>();
            await getDetailsFunc.ExecuteAsync(myQueueItem);
            return getDetailsFunc.GetOutput();
        }

        /// <summary>
        /// Configures the container.
        /// </summary>
        /// <param name="mode">The mode.</param>
        /// <returns></returns>
        public static ILifetimeScope ConfigureContainer(RunMode mode, IAsyncCollector<AuditRecord<object, object>> asyncCollector, AuditRecord<object, object> masterRecord)
        {
            var builder = ConfigureDI.ConfigureContainerWithCommonModules(mode);
            if (mode == RunMode.Azure)
            {
                builder.Register(ctx => new CosmosAuditService(asyncCollector, masterRecord));
            }
            builder.RegisterModule<Core.AutofacModule>();
            builder.RegisterModule<GetAVDetailsForProjectedAV.AutofacModule>();
            builder.RegisterModule<Service.AutofacModule>();
            return builder.Build().BeginLifetimeScope();
        }
    }
}
