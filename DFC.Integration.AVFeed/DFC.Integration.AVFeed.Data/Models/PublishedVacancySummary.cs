using System;
using System.Collections.Generic;

namespace DFC.Integration.AVFeed.Data.Models
{
    public class PublishedVacancySummary : BaseIntegrationModel
    {
        public string SocCode { get; set; }
        public Guid SocMappingId { get; set; }

        public IEnumerable<PublishedAV> Vacancies { get; set; }
    }
}
