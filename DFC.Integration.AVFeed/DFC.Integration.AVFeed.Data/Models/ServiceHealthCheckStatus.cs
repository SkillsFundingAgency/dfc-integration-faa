namespace DFC.Integration.AVFeed.Data.Models
{
    using System;
    using System.Collections.ObjectModel;
    using System.Net;
    using Newtonsoft.Json;

    public class FeedsServiceHealthCheck
    {
        [JsonProperty("CheckDateTime")]
        public DateTime CheckDateTime { get; set; }

        [JsonProperty("ApplicationStatus")]
        public HttpStatusCode ApplicationStatus { get; set; }

        public Collection<ServiceStatus> FeedsServiceHealth { get; set; }
    }

    public enum ServiceState
    {
        Red,
        Green,
        Amber
    }

    public class ServiceStatus
    {
        [JsonProperty("ApplicationName")]
        public string ApplicationName { get; set; }

        [JsonProperty("Status")]
        public ServiceState Status { get; set; }

        [JsonProperty("CheckParametersUsed")]
        public string CheckParametersUsed { get; set; }

        [JsonProperty("Notes")]
        public string Notes { get; set; }
    }
}
