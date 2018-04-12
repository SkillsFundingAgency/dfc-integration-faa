using Autofac;
using DFC.Integration.AVFeed.AuditService;
using DFC.Integration.AVFeed.Core;
using DFC.Integration.AVFeed.Data.Interfaces;
using DFC.Integration.AVFeed.Data.Models;
using DFC.Integration.AVFeed.Function.Common;
using DFC.Integration.AVFeed.Service;
using Microsoft.Azure.WebJobs;
using System.Threading.Tasks;

namespace DFC.Integration.AVFeed.Function.GetAVForSoc
{
    public static class Startup
    {
        public static async Task<MappedVacancySummary> RunAsync(SocMapping myQueueItem, RunMode mode)
        {
            return await RunAsync(myQueueItem, mode, null, null);
        }
        public static async Task<MappedVacancySummary> RunAsync(SocMapping myQueueItem, RunMode mode, IAsyncCollector<AuditRecord<object, object>> asyncCollector, AuditRecord<object,object> masterRecord)
        {
            var container = ConfigureContainer(mode, asyncCollector, masterRecord);
            var getAvFunc = container.Resolve<IGetAvForSocFunc>();
            await getAvFunc.Execute(myQueueItem);

            return getAvFunc.GetOutput();
        }

        public static ILifetimeScope ConfigureContainer(RunMode mode, IAsyncCollector<AuditRecord<object, object>> asyncCollector, AuditRecord<object, object> masterRecord)
        {
            var builder = ConfigureDI.ConfigureContainerWithCommonModules(mode);
            if (mode == RunMode.Azure)
            {
                builder.Register(ctx => new CosmosAuditService(asyncCollector, masterRecord));
            }

            builder.RegisterModule<Service.AutofacModule>();
            builder.RegisterModule<GetAVForSoc.AutofacModule>();
          
            return builder.Build().BeginLifetimeScope();
        }
    }
}
