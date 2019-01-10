using DFC.Integration.AVFeed.Data.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFC.Integration.AVFeed.Service.AVAPIUnitTests
{
    public static class FAAAPIDummyResposnes
    {
        public static string GetDummyApprenticeshipVacancySummaryResponse(int currentPage, int totalMatches, int nunmberToReturn, int pageSize, int diffrentProvidersPage)
        {

            var r = new ApprenticeshipVacancySummaryResponse
            {
                CurrentPage = currentPage,
                TotalMatched = totalMatches,
                TotalPages = totalMatches / pageSize,
                TotalReturned = nunmberToReturn
            };

            var recordsToReturn = new List<ApprenticeshipVacancySummary>();

            for (var ii = 0; ii < nunmberToReturn; ii++)
            {
                recordsToReturn.Add(new ApprenticeshipVacancySummary
                {
                    VacancyReference = ii,
                    Title = $"Title {ii}",
                    TrainingProviderName = $"TrainingProviderName {((currentPage == diffrentProvidersPage) ? ii : currentPage)}"
                });
            }

            r.Results = recordsToReturn.ToArray();

            return JsonConvert.SerializeObject(r);
        }

        public static  string GetDummyApprenticeshipVacancyDetailsResponse()
        {

            var r = new ApprenticeshipVacancyDetails
            {
                VacancyReference = 123
            };

            return JsonConvert.SerializeObject(r);
        }

        public static string GetDummyApprenticeshipVacancySummaryResponseSameProvider(int currentPage, int totalPages)
        {
            var numberOnPage = 5;
            var r = new ApprenticeshipVacancySummaryResponse
            {
                CurrentPage = currentPage,
                TotalMatched = 10,
                TotalPages = totalPages,
                TotalReturned = numberOnPage
            };

            var recordsToReturn = new List<ApprenticeshipVacancySummary>();

            for (var ii = 0; ii < numberOnPage; ii++)
            {
                recordsToReturn.Add(new ApprenticeshipVacancySummary
                {
                    VacancyReference = ii,
                    Title = $"Title {ii}",
                    TrainingProviderName = $"SameProvider"
                });
            }

            r.Results = recordsToReturn.ToArray();

            return JsonConvert.SerializeObject(r);
        }

    }
}
