namespace DFC.Integration.AVFeed.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using Newtonsoft.Json;

    public class FeedsServiceHealthCheck
    {
        public List<ServiceHealthCheckStatus> FeedsServiceHealth { get; set; }
    }
    public class ServiceHealthCheckStatus
    {
        public ServiceHealthCheckStatus()
        {
            IsApplicationExternal = true;
            IsApplicationRunning = false;
            ApplicationStatus = HttpStatusCode.Gone;
        }
        [JsonProperty("FailedAt")]
        public DateTime? FailedAt { get; set; }

        [JsonProperty("ApplicationName")]
        public string ApplicationName { get; set; }

        [JsonProperty("IsApplicationExternal")]
        public bool IsApplicationExternal { get; set; }

        [JsonProperty("IsApplicationRunning")]
        public bool IsApplicationRunning { get; set; }

        [JsonProperty("ApplicationStatus")]
        public HttpStatusCode ApplicationStatus { get; set; }

        [JsonProperty("ApplicationStatusDescription")]
        public string ApplicationStatusDescription { get; set; }
    }
}
