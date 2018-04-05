using Autofac;

namespace DFC.Integration.AVFeed.Core.Extensions
{
    public static class AutofacExtensions
    {
        public static RunMode RunningMode(this ContainerBuilder builder)
        {
            object runMode = RunMode.Azure;
            if (builder != null)
            {
                builder.Properties.TryGetValue(nameof(RunMode), out runMode);
            }
            return (RunMode)runMode;
        }
    }
}
