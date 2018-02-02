using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFC.Integration.AVFeed.Data.Models
{
    public class PublishedVacancySummary : BaseIntegrationModel
    {
        public string SocCode { get; set; }
        public Guid SocMappingId { get; set; }

        public IEnumerable<PublishedAV> Vacancies { get; set; }
    }
}
