using DFC.Integration.AVFeed.Data;
using System;
using System.Threading.Tasks;

namespace DFC.Integration.AVFeed.Function.GetServiceHealthStatus
{
    using System.IO;
    using System.Net;
    using Data.Interfaces;
    using Data.Models;
    using Microsoft.Azure.WebJobs.Host;

    public class GetAvServiceHealthStatus:IGetServiceHealthStatus
    {
        private TraceWriter Log;
        #region Implementation of IGetServiceHealthStatus
         public GetAvServiceHealthStatus(TraceWriter log)
         {
             Log = log;
         }
        public async Task<ServiceHealthCheckStatus> GetAvFeedHealthStatusInfoAsync()
        {
            await GetAvFeedWcfStatusAsync(string.Empty);
            return null;
        }

        private async Task<ServiceHealthCheckStatus> GetAvFeedWcfStatusAsync(string url)
        {
            try
            {
                var avWcfFeedRequest = WebRequest.CreateHttp(new Uri(url));
                avWcfFeedRequest.KeepAlive = true;
                avWcfFeedRequest.Connection = "Open";
                // Assign the response object of HttpWebRequest to a HttpWebResponse variable.
                var avWcfFeedResponse =  (HttpWebResponse)(await avWcfFeedRequest.GetResponseAsync().ConfigureAwait(false));
                Log.Info(avWcfFeedResponse.StatusCode == HttpStatusCode.OK
                    ? "Service Running and Up"
                    : $"\nIssue with the service :\n{avWcfFeedResponse.StatusCode}{avWcfFeedResponse.Server}");

                Log.Info($"\nThe HTTP request Headers for the first request are: \n{avWcfFeedRequest.Headers}");

                var avWcfFeedStreamResponse = avWcfFeedResponse.GetResponseStream();
                
                if (avWcfFeedStreamResponse != null)
                    using (var streamRead = new StreamReader(avWcfFeedStreamResponse))
                    {
                        var readBuff = new char[256];
                        var count = streamRead.Read(readBuff, 0, 256);
                    }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }
            return null;
        }
        private string doPUT(string URI, string body, String token)
        {
            Uri uri = new Uri(String.Format(URI));

            // Create the request
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(uri);
            httpWebRequest.Headers.Add(HttpRequestHeader.Authorization, "Bearer " + token);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "PUT";

            try
            {
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write(body);
                    streamWriter.Flush();
                    streamWriter.Close();
                }
            }
            catch (Exception ex)
            {
                Log.Error("Error setting up stream writer: " + ex.Message);
            }

            // Get the response
            HttpWebResponse httpResponse = null;
            try
            {
                httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            }
            catch (Exception ex)
            {
                Log.Error("Error from : HttpWebResponse exception " + uri + ": " + ex.Message);
                return null;
            }

            string result = null;
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                result = streamReader.ReadToEnd();
            }
            return result;
        }
        #endregion

        #region Implementation of IGetServiceHealthStatus

        

        public async Task<dynamic> GetAzureServelessHealthStatusInfoAsync()
        {
            return null;
        }

        #endregion
    }
}
