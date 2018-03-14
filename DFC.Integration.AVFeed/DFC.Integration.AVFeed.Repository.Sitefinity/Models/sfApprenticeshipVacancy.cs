using Newtonsoft.Json;
using System;
using System.Runtime.Serialization;

namespace DFC.Integration.AVFeed.Repository.Sitefinity.Models
{
    [DataContract]
   public class SfApprenticeshipVacancy
    {
        [DataMember]
        [JsonIgnore]
        public Guid Id { get; set; }

        [JsonProperty("Id")]
        private Guid IdSetter
        {
            set
            {
                Id = value;
            }
        }

        [JsonIgnore]
        public SfSocCode SOCCode { get; set; }
        [JsonProperty("SOCCode")]
        private SfSocCode SOCCodeSetter
        {
            set
            {
                SOCCode = value;
            }
        }

        [DataMember]
        [JsonIgnore]
        public DateTime LastModified { get; set; }
        [DataMember]
        public DateTime PublicationDate { get; set; }
        [DataMember]
        public string UrlName { get; set; }
        [DataMember]
        public string URL { get; set; }
        [DataMember]
        public string Location { get; set; }
        [DataMember]
        public string WageUnitType { get; set; }
        [DataMember]
        public string WageAmount { get; set; }
        [DataMember]
        public string Title { get; set; }
        [DataMember]
        public string VacancyId { get; set; }
    }
}
