using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;

namespace DFC.Integration.AVFeed.AzureFunctions
{
    public static class GetHealthStatusInfo
    {
        [FunctionName("GetHealthStatusInfo")]
        public static void Run([TimerTrigger("0 */5 * * * *")]TimerInfo myTimer, TraceWriter log)
        {
            log.Info($"C# Timer trigger function executed at: {DateTime.Now}");
        }
    }
}