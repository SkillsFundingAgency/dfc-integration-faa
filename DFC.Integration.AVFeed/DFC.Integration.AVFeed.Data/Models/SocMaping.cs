using System;
using System.Collections.Generic;

namespace DFC.Integration.AVFeed.Data.Models
{
    public class SocMapping : BaseIntegrationModel
    {
        public Guid SocMappingId { get; set; }
        public string SocCode { get; set; }
        public string AccessToken { get; set; }
        public IEnumerable<string> Standards { get; set; }
        public IEnumerable<string> Frameworks { get; set; }
    }
}
