using System;
using System.Collections.Generic;

namespace DFC.Integration.AVFeed.Data.Models
{
    public class MappedVacancySummary : BaseIntegrationModel
    {
        public string SocCode { get; set; }
        public Guid SocMappingId { get; set; }
        public IEnumerable<ApprenticeshipVacancySummary> Vacancies { get; set; }
        public string AccessToken { get; set; }
        public bool IsValidVacancy { get; set; }
    }
}
