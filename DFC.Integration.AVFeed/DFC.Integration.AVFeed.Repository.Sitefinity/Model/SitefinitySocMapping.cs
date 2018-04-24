using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace DFC.Integration.AVFeed.Repository.Sitefinity
{
    public class SitefinitySocMapping
    {
        public Guid Id { get; set; }
        [JsonProperty("SOCCode")]
        public string SocCode { get; set; }
        [JsonProperty("NavigateToApprenticeshipStandard")]
        public IEnumerable<NavigateToApprenticeshipStandard> Standards { get; set; }
        [JsonProperty("NavigateToApprenticeshipFramework")]
        public IEnumerable<NavigateToApprenticeshipFramework> Frameworks { get; set; }
    }
}
