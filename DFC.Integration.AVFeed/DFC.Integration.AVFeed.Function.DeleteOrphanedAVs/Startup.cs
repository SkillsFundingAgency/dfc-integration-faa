using Autofac;
using DFC.Integration.AVFeed.AuditService;
using DFC.Integration.AVFeed.Core;
using DFC.Integration.AVFeed.Data.Interfaces;
using DFC.Integration.AVFeed.Data.Models;
using DFC.Integration.AVFeed.Function.Common;
using Microsoft.Azure.WebJobs;
using System.Threading.Tasks;

namespace DFC.Integration.AVFeed.Function.DeleteOrphanedAVs
{
    public static class Startup
    {
        public static ILifetimeScope ConfigureContainer(RunMode mode, IAsyncCollector<AuditRecord<object, object>> asyncCollector, AuditRecord<object, object> masterRecord)
        {
            var builder = ConfigureDI.ConfigureContainerWithCommonModules(mode);
            builder.RegisterModule<AutofacModule>();
            builder.RegisterModule<Repository.Sitefinity.AutofacModule>();
            if (mode == RunMode.Azure)
            {
                builder.Register(ctx => new CosmosAuditService(asyncCollector, masterRecord)).As<IAuditService>();
            }
            return builder.Build().BeginLifetimeScope();
        }

        public static async Task RunAsync(RunMode mode)
        {
           await RunAsync(mode, null, null);
        }

        public static async Task RunAsync(RunMode mode, IAsyncCollector<AuditRecord<object, object>> asyncCollector, AuditRecord<object, object> masterRecord)
       {
            var container = ConfigureContainer(mode, asyncCollector, masterRecord);
            var deleteOrphanedAVs = container.Resolve<IDeleteOrphanedAVs>();
            await deleteOrphanedAVs.ExecuteAsync();
       }
    }
}
