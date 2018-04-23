namespace DFC.Integration.AVFeed.Function.GetServiceHealthStatus
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Autofac;
    using Common;
    using Core;
    using Data.Interfaces;
    using Data.Models;
    using DFC.Integration.AVFeed.Repository.Sitefinity;

    public static class Startup
    {
        public static async Task<FeedsServiceHealthCheck> RunAsync(RunMode mode)
        {
            var container = ConfigureContainer(mode);
            var getAvFunc = container.Resolve<IGetServiceHealthStatus>();
            return await getAvFunc.GetServiceHealthStateAsync().ConfigureAwait(false);
        }

        public static ILifetimeScope ConfigureContainer(RunMode mode)
        {
            var builder = ConfigureDI.ConfigureContainerWithCommonModules(mode);
            builder.RegisterModule<AutofacModule>();
            builder.RegisterModule<Repository.Sitefinity.AutofacModule>();
            builder.RegisterModule<Service.AutofacModule>();
            return builder.Build().BeginLifetimeScope();
        }
    }
}