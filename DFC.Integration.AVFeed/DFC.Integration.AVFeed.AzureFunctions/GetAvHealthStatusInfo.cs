using System;
using DFC.Integration.AVFeed.Function.GetServiceHealthStatus;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using System.Linq;
using System.Threading.Tasks;
using DFC.Integration.AVFeed.Core;
using DFC.Integration.AVFeed.Data.Models;
using System.Net.Http;
using System.Net;
using Newtonsoft.Json;
using System.Text;

namespace DFC.Integration.AVFeed.AzureFunctions
{

    public static class GetAvHealthStatusInfo
    {
//        [FunctionName("GetHealthStatusInfo")]
//        public static async Task Run([TimerTrigger("0 */5 * * * *")]TimerInfo myTimer, TraceWriter log, [DocumentDB("AVFeedAudit", "ServiceHealthStatus", ConnectionStringSetting = "AVAuditCosmosDB")]
//           IAsyncCollector<FeedsServiceHealthCheck> healthServiceStatus)
 //       {
//            log.Info($"GetHealthStatusInfo function executed at: with :- {DateTime.Now}-:-{myTimer}");
//            try
//            {
//                var healthStatus = await Startup.RunAsync(RunMode.Azure);
//                await healthServiceStatus.AddAsync(healthStatus);
//            }
//            catch (Exception e)
 //           {
 //               log.Info($"GetHealthStatusInfo function throws exception at: :- {DateTime.Now}-:-{myTimer}-:-with Exception-:- {e.Message}");
 //               throw;
 //           }
 //       }
        


        [FunctionName("GetHealthStatusWebHook")]
        public static async Task<HttpResponseMessage> Run(HttpRequestMessage req, TraceWriter log, [DocumentDB("AVFeedAudit", "ServiceHealthStatus", ConnectionStringSetting = "AVAuditCosmosDB")]
           IAsyncCollector<FeedsServiceHealthCheck> healthServiceStatus)
        {
            log.Info($"GetHealthStatusInfo function executed at: with :- {DateTime.Now}");

            try
            {
                var healthStatus = await Startup.RunAsync(RunMode.Azure);
                await healthServiceStatus.AddAsync(healthStatus);


                return new HttpResponseMessage(healthStatus.ApplicationStatus)
                { Content = new StringContent(JsonConvert.SerializeObject(healthStatus), Encoding.UTF8, "application/json") };

            }
            catch (Exception e)
            {
                log.Info($"GetHealthStatusInfo function throws exception at: :- {DateTime.Now}-:-with Exception-:- {e.Message}");
                throw;
            }

        }
     
    }
}