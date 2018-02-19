using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFC.Integration.AVFeed.Data.Models
{
    using Newtonsoft.Json;

    public enum  Status
    {
        Healthy=0x01,
        Weak=0x02,
        Dead =0x03,
    }

    public sealed class ServiceHealthCheckStatus
    {
        [JsonProperty("ApplicationName")]
        public string ApplicationName { get; set; }
        [JsonProperty("IsApplicationExternal")]
        public bool IsApplicationExternal { get; set; }
        [JsonProperty("IsApplicationRunning")]
        public bool IsApplicationRunning { get; set; }
        [JsonProperty("ApplicationStatus")]
        public Status ApplicationStatus { get; set; }

    }
}
