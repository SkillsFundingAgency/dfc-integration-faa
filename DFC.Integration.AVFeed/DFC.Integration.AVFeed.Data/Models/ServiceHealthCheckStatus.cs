namespace DFC.Integration.AVFeed.Data.Models
{
    using System.Net;
    using Newtonsoft.Json;

    public sealed class ServiceHealthCheckStatus
    {
        public ServiceHealthCheckStatus()
        {
            IsApplicationExternal = false;
            IsApplicationRunning = false;
            ApplicationStatus = HttpStatusCode.BadGateway;
        }
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
