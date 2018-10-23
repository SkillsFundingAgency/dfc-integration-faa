using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFC.Integration.AVFeed.Data.Models
{
    public class OrphanedVacancySummary
    {
        public Guid Id { get; set; }
        public string VacancyId { get; set; }
        public DateTime PublicationDate { get; set; }
        public string Title { get; set; }
    }
}
