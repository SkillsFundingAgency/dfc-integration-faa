namespace DFC.Integration.AVFeed.Function.Common
{
    using System.Configuration;
    using Microsoft.ApplicationInsights.NLogTarget;
    using NLog;
    using NLog.Config;

    /// <summary>
    /// Configure loging for the application
    /// </summary>
    public static class ConfigureLog
    {
        /// <summary>
        /// Configure Nlog and set Applications Insights as the target.
        /// </summary>
        public static void ConfigureNLogWithAppInsightsTarget()
        {
            var appInsightsKey = ConfigurationManager.AppSettings.Get("APPINSIGHTS_INSTRUMENTATIONKEY");
            if (!string.IsNullOrEmpty(appInsightsKey))
            {
                var config = new LoggingConfiguration();

                ApplicationInsightsTarget target = new ApplicationInsightsTarget();
                target.InstrumentationKey = appInsightsKey;

                LoggingRule rule = new LoggingRule("*", LogLevel.Trace, target);
                config.LoggingRules.Add(rule);

                LogManager.Configuration = config;
            }
        }
    }
}
