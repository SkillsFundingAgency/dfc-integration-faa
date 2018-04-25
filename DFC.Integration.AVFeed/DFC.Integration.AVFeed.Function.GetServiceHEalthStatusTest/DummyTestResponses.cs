using DFC.Integration.AVFeed.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFC.Integration.AVFeed.Function.GetServiceHealthStatusTest
{
    public static class DummyTestResponses
    {
        public static ApprenticeshipVacancySummaryResponse GetDummyApprenticeshipVacancySummaryResponse()
        {
            var apprenticeshipVacancySummaryResponse = new ApprenticeshipVacancySummaryResponse() { TotalReturned = 1 };
            var results = new List<ApprenticeshipVacancySummary>();
            results.Add(new ApprenticeshipVacancySummary() { VacancyReference = 123 });
            apprenticeshipVacancySummaryResponse.Results = results.ToArray();
            return apprenticeshipVacancySummaryResponse;
        }
    }
}
