using Autofac;
using DFC.Integration.AVFeed.AuditService;
using DFC.Integration.AVFeed.Core;
using DFC.Integration.AVFeed.Data.Interfaces;
using DFC.Integration.AVFeed.Data.Models;
using DFC.Integration.AVFeed.Function.Common;
using Microsoft.Azure.WebJobs;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace DFC.Integration.AVFeed.Function.ClearRecycleBin
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


        public static void Run(RunMode mode)
        {
            Run(mode, null, null);
        }

        public static void Run(RunMode mode, IAsyncCollector<AuditRecord<object, object>> asyncCollector, AuditRecord<object, object> masterRecord)
        {
            var container = ConfigureContainer(mode, asyncCollector, masterRecord);
            var clearRecycleBin = container.Resolve<IClearRecycleBin>();
            clearRecycleBin.ClearRecycleBinAVs();
        }
      
    }
}
