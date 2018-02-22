namespace DFC.Integration.AVFeed.Function.GetServiceHealthStatus
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Autofac;
    using Core;
    using Common;
    using Data.Interfaces;
    using Data.Models;

    public static class Startup
    {
        public static async Task<ICollection<ServiceHealthCheckStatus>> RunAsync(RunMode mode)
        {
            var container = ConfigureContainer(mode);
            var getAvFunc = container.Resolve<IGetServiceHealthStatus>();
            ICollection<ServiceHealthCheckStatus> healthCheckStatuses = new List<ServiceHealthCheckStatus>();
            //{
            var avfeed = await getAvFunc.GetAvFeedHealthStatusInfoAsync();
            var sitefinityfeed = await getAvFunc.GetSitefinityHealthStatusInfoAsync();
            //};
            healthCheckStatuses.Add(avfeed);
            healthCheckStatuses.Add(sitefinityfeed);
            return await Task.FromResult<ICollection<ServiceHealthCheckStatus>>(healthCheckStatuses).ConfigureAwait(false);
        }
        public static ILifetimeScope ConfigureContainer(RunMode mode)
        {
            var builder = ConfigureDI.ConfigureContainerWithCommonModules(mode);
            builder.RegisterModule<GetServiceHealthStatus.AutofacModule>();
            return builder.Build().BeginLifetimeScope();
        }
    }
}
