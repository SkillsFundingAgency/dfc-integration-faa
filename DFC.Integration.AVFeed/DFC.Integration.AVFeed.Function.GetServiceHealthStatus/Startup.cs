namespace DFC.Integration.AVFeed.Function.GetServiceHealthStatus
{
    using System.Threading.Tasks;
    using Autofac;
    using Core;
    using Common;
    using Data.Interfaces;
    using Data.Models;

    public static class Startup
    {
        public static async Task<ServiceHealthCheckStatus> RunAsync(RunMode mode,  AuditRecord<object, object> masterRecord)
        {
            var container = ConfigureContainer(mode);
            var getAvFunc = container.Resolve<IGetServiceHealthStatus>();
            await getAvFunc.GetAvFeedHealthStatusInfoAsync();
            return await Task.FromResult<ServiceHealthCheckStatus>(null).ConfigureAwait(false);
        }
        public static ILifetimeScope ConfigureContainer(RunMode mode)
        {
            var builder = ConfigureDI.ConfigureContainerWithCommonModules(mode);
            builder.RegisterModule<GetServiceHealthStatus.AutofacModule>();
            return builder.Build().BeginLifetimeScope();
        }
    }
}
