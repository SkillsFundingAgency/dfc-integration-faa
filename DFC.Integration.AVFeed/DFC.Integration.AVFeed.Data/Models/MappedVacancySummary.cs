using System;
using System.Collections.Generic;

namespace DFC.Integration.AVFeed.Data.Models
{
    public class MappedVacancyDetails : BaseIntegrationModel
    {
        public string SocCode { get; set; }
        public Guid SocMappingId { get; set; }

        public IEnumerable<ApprenticeshipVacancyDetails> Vacancies { get; set; }
        public string AccessToken { get; set; }
    }
}
