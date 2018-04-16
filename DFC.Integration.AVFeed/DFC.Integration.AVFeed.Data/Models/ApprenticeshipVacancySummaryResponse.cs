using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFC.Integration.AVFeed.Data.Models
{
    public class ApprenticeshipVacancySummaryResponse
    {
        public int TotalMatched { get; set; }
        public int TotalReturned { get; set; }
        public int CurrentPage { get; set; }
        public double TotalPages { get; set; }

        public string SortBy { get; set; }

        public IEnumerable<ApprenticeshipVacancySummary> Results { get; set; }

    }
}
