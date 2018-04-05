using Microsoft.ApplicationInsights.NLogTarget;
using NLog;
using NLog.Config;
using System.Configuration;

namespace DFC.Integration.AVFeed.Function.Common
{
    public static class ConfigureLog
    {
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
