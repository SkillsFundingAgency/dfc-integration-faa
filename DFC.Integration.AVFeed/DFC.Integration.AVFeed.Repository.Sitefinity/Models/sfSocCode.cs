using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DFC.Integration.AVFeed.Repository.Sitefinity
{
    [DataContract]
    [JsonObject(IsReference = true)]
    public class SfSocCode
    {
        [DataMember]
        public Guid Id { get; set; }
        [DataMember]
        public List<string> apprenticeshipstandards { get; set; }
        [DataMember]
        public List<string> apprenticeshipframeworks { get; set; }

    }
}
