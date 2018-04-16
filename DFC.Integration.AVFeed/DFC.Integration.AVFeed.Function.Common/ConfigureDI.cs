namespace DFC.Integration.AVFeed.Function.Common
{
    using Autofac;
    using DFC.Integration.AVFeed.AuditService;
    using DFC.Integration.AVFeed.Core;
    using DFC.Integration.AVFeed.Data.Interfaces;

    /// <summary>
    /// Configure DI for this module
    /// </summary>
    public static class ConfigureDI
    {
        /// <summary>
        /// Configure DI container builder for this module
        /// </summary>
        /// <param name="mode">Set depending on where we are running Azure or Console</param>
        /// <returns>The Container Builder</returns>
        public static ContainerBuilder ConfigureContainerWithCommonModules(RunMode mode)
        {
            ContainerBuilder builder = new ContainerBuilder();
            builder.Properties.Add(nameof(RunMode), mode);
            builder.RegisterType<InMemoryAuditService>().As<IAuditService>();
            builder.RegisterModule<Core.AutofacModule>();

            //builder.RegisterModule<Repository.Audit.AutofacModule>();
            return builder;
        }

        //public static void ConfigureLog(TraceWriter logWriter) { }
        //{
        //    var config = new LoggingConfiguration();

        //    // Add the AzureFuctionLogTarget Target
        //    var azureTarget = new AzureFunctionLogTarget(log);
        //    config.AddTarget("azure", azureTarget);

        //    // Define the Layout
        //    azureTarget.Layout = @"${level:uppercase=true}|${threadid:padCharacter=0:padding=3}|${message}";

        //    // Create a rule so that all logging will be sent to AzureFunctionLogTarget
        //    var rule1 = new LoggingRule("*", LogLevel.Trace, azureTarget);
        //    config.LoggingRules.Add(rule1);

        //    // Assign the newly created configuration to the active LogManager NLog instance
        //    LogManager.Configuration = config;
        //}
    }
}
