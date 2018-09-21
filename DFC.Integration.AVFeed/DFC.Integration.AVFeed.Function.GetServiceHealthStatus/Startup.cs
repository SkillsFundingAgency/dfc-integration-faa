namespace DFC.Integration.AVFeed.Function.GetServiceHealthStatus
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Autofac;
    using Common;
    using Core;
    using Data.Interfaces;
    using Data.Models;
    using DFC.Integration.AVFeed.AuditService;
    using DFC.Integration.AVFeed.Repository.Sitefinity;
    using Microsoft.Azure.WebJobs;

    public static class Startup
    {
        public static async Task<FeedsServiceHealthCheck> RunAsync(RunMode mode)
        {
            return await RunAsync(mode, null, null);
        }
        public static async Task<FeedsServiceHealthCheck> RunAsync(RunMode mode, IAsyncCollector<AuditRecord<object, object>> asyncCollector, AuditRecord<object, object> masterRecord)
        {
            var container = ConfigureContainer(mode, asyncCollector, masterRecord);
            var getAvFunc = container.Resolve<IGetServiceHealthStatus>();
            return await getAvFunc.GetServiceHealthStateAsync().ConfigureAwait(false);
        }

        public static ILifetimeScope ConfigureContainer(RunMode mode, IAsyncCollector<AuditRecord<object, object>> asyncCollector, AuditRecord<object, object> masterRecord)
        {
            var builder = ConfigureDI.ConfigureContainerWithCommonModules(mode);
            builder.RegisterModule<AutofacModule>();
            builder.RegisterModule<Repository.Sitefinity.AutofacModule>();
            builder.RegisterModule<Service.AutofacModule>();
            if (mode == RunMode.Azure)
            {
                builder.Register(ctx => new CosmosAuditService(asyncCollector, masterRecord)).As<IAuditService>();
            }

            return builder.Build().BeginLifetimeScope();
        }
    }
}