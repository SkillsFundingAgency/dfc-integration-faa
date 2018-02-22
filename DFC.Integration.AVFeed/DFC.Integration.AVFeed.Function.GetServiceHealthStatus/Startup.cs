namespace DFC.Integration.AVFeed.Function.GetServiceHealthStatus
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Autofac;
    using Common;
    using Core;
    using Data.Interfaces;
    using Data.Models;

    public static class Startup
    {
        public static async Task<FeedsServiceHealthCheck> RunAsync(RunMode mode)
        {
            var container = ConfigureContainer(mode);
            var getAvFunc = container.Resolve<IGetServiceHealthStatus>();
            var healthCheckStatuses = new FeedsServiceHealthCheck
            {
                FeedsServiceHealth = new List<ServiceHealthCheckStatus>
                {
                    await getAvFunc.GetApprenticeshipFeedHealthStatusAsync(),
                    await getAvFunc.GetSitefinityHealthStatusAsync()
                }
            };
            return await Task.FromResult(healthCheckStatuses).ConfigureAwait(false);
        }

        public static ILifetimeScope ConfigureContainer(RunMode mode)
        {
            var builder = ConfigureDI.ConfigureContainerWithCommonModules(mode);
            builder.RegisterModule<AutofacModule>();
            return builder.Build().BeginLifetimeScope();
        }
    }
}