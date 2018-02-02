using Autofac;
using DFC.Integration.AVFeed.AuditService;
using DFC.Integration.AVFeed.Core;
using DFC.Integration.AVFeed.Data.Interfaces;
using DFC.Integration.AVFeed.Data.Models;
using DFC.Integration.AVFeed.Function.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DFC.Integration.AVFeed.Function.GetMappings
{
    public static class Startup
    {
        public static ILifetimeScope ConfigureContainer(RunMode mode)
        {
            var builder = ConfigureDI.ConfigureContainerWithCommonModules(mode);
            builder.RegisterType<InMemoryAuditService>().As<IAuditService>().SingleInstance();
            builder.RegisterModule<Repository.Sitefinity.AutofacModule>();
            builder.RegisterModule<GetMappings.AutofacModule>();
            //builder.Register(ctx => ctx.Resolve<MapperConfiguration>().CreateMapper()).As<IMapper>();

            return builder.Build().BeginLifetimeScope();
        }

        public static async Task<FunctionResult<IEnumerable<SocMapping>>> RunAsync(RunMode mode)
        {
            var container = ConfigureContainer(mode);
            var getSocmMapping = container.Resolve<IGetSocMappingFunc>();
            var auditService = container.Resolve<IAuditService>();

            await getSocmMapping.Execute();

            return new FunctionResult<IEnumerable<SocMapping>>
            {
                Output = getSocmMapping.GetOutput(),
                AuditMessages = auditService.GetAuditRecords(),
            };
        }
    }
}
