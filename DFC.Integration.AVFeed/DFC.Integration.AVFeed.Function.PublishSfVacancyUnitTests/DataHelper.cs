using System;
using System.Collections.Generic;
using DFC.Integration.AVFeed.Data.Models;

namespace DFC.Integration.AVFeed.Function.PublishSfVacancyUnitTests
{
    /// <summary>
    /// Data Helper for Unit Tests
    /// </summary>
    public static class DataHelper
    {
        /// <summary>
        /// Gets the dummy projected vacancy summary.
        /// </summary>
        /// <returns></returns>
        public static ProjectedVacancyDetails GetDummyProjectedVacancyDetails()
        {
            return new ProjectedVacancyDetails
            {
                  SocCode = nameof(ProjectedVacancyDetails.SocCode),
                  SocMappingId = new Guid(),
                  CorrelationId = new Guid(),
                  Vacancies = new List<ApprenticeshipVacancyDetails>
                  {
                      new ApprenticeshipVacancyDetails
                      {
                          ExpectedStartDate = DateTime.Now.ToString(),
                          PostedDate = DateTime.Now.ToString(),
                          TrainingProviderName =  nameof(ApprenticeshipVacancySummary.TrainingProviderName),
                          VacancyReference = 1,
                          Title =  nameof(ApprenticeshipVacancySummary.Title),
                          VacancyUrl = new Uri(nameof(ApprenticeshipVacancySummary.VacancyUrl)),
                      },
                      new ApprenticeshipVacancyDetails
                      {
                          ExpectedStartDate = DateTime.Now.ToString(),
                          PostedDate = DateTime.Now.ToString(),
                          TrainingProviderName =  nameof(ApprenticeshipVacancySummary.TrainingProviderName),
                          VacancyReference = 2,
                          Title =  nameof(ApprenticeshipVacancySummary.Title),
                          VacancyUrl = new Uri(nameof(ApprenticeshipVacancySummary.VacancyUrl)),
                      },
                  }
            };
             
        }

       
    }
}