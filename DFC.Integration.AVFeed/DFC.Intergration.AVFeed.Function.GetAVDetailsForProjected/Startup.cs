using Autofac;
using DFC.Integration.AVFeed.AuditService;
using DFC.Integration.AVFeed.Core;
using DFC.Integration.AVFeed.Data.Interfaces;
using DFC.Integration.AVFeed.Data.Models;
using DFC.Integration.AVFeed.Function.Common;
using System.Threading.Tasks;

namespace DFC.Intergration.AVFeed.Function.GetAVDetailsForProjected
{
    /// <summary>
    /// Get details for projected vacancies
    /// </summary>
    public static class Startup
    {
        /// <summary>
        /// Runs the specified my queue item.
        /// </summary>
        /// <param name="myQueueItem">My queue item.</param>
        /// <param name="mode">The mode.</param>
        public static async Task<FunctionResult<PublishedVacancySummary>> RunAsync(ProjectedVacancyDetails myQueueItem, RunMode mode)
        {
            var container = ConfigureContainer(mode);
            var publishFunc = container.Resolve<IPublishAVFunc>();
            var auditService = container.Resolve<IAuditService>();
            await publishFunc.ExecuteAsync(myQueueItem);
            return new FunctionResult<PublishedVacancySummary> {
                Output = publishFunc.GetOutput(),
                AuditMessages = auditService.GetAuditRecords(),
            };
        }

        /// <summary>
        /// Configures the container.
        /// </summary>
        /// <param name="mode">The mode.</param>
        /// <returns></returns>
        public static ILifetimeScope ConfigureContainer(RunMode mode)
        {
            var builder = ConfigureDI.ConfigureContainerWithCommonModules(mode);
            builder.RegisterType<InMemoryAuditService>().As<IAuditService>().SingleInstance();
            builder.RegisterModule<Integration.AVFeed.Core.AutofacModule>();
            return builder.Build().BeginLifetimeScope();
        }
    }
}
