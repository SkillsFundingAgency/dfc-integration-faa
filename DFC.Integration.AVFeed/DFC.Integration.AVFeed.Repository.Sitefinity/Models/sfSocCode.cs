using Newtonsoft.Json;
using System;
using System.Runtime.Serialization;

namespace DFC.Integration.AVFeed.Repository.Sitefinity.Models
{
    [DataContract]
    [JsonObject(IsReference = true)]
    public class SfSocCode
    {
        [DataMember]
        public Guid Id { get; set; }
    }
}
