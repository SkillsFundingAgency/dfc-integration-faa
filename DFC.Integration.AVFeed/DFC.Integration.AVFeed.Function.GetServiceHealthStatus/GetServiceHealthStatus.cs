using DFC.Integration.AVFeed.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFC.Integration.AVFeed.Function.GetServiceHealthStatus
{
    using System.IO;
    using System.Net;
    using Microsoft.Azure.WebJobs.Host;

    public class GetServiceHealthStatus:IGetServiceHealthStatus
    {
        private TraceWriter Log;
        #region Implementation of IGetServiceHealthStatus
         public GetServiceHealthStatus(TraceWriter log)
         {
             Log = log;
         }
        public async Task<dynamic> GetHealthStatusInfo()
        {
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
    }
}
