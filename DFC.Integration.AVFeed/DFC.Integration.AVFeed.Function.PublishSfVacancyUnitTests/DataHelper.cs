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
        public static ProjectedVacancySummary GetDummyProjectedVacancySummary()
        {
            return new ProjectedVacancySummary
            {
                  SocCode = nameof(ProjectedVacancySummary.SocCode),
                  SocMappingId = new Guid(),
                  CorrelationId = new Guid(),
                  Vacancies = new List<ApprenticeshipVacancySummary>
                  {
                      new ApprenticeshipVacancySummary
                      {
                          AddressDataPostCode = nameof(ApprenticeshipVacancySummary.AddressDataPostCode),
                          AddressDataTown =  nameof(ApprenticeshipVacancySummary.AddressDataTown),
                          PossibleStartDate = DateTime.Now,
                          CreatedDateTime = DateTime.Now,
                          FrameworkCode =  nameof(ApprenticeshipVacancySummary.FrameworkCode),
                          LearningProviderName =  nameof(ApprenticeshipVacancySummary.LearningProviderName),
                          VacancyReference = 1,
                          VacancyTitle =  nameof(ApprenticeshipVacancySummary.VacancyTitle),
                          VacancyUrl =  nameof(ApprenticeshipVacancySummary.VacancyUrl),
                          WageText =  nameof(ApprenticeshipVacancySummary.WageText)
                      },
                      new ApprenticeshipVacancySummary
                      {
                          AddressDataPostCode = nameof(ApprenticeshipVacancySummary.AddressDataPostCode),
                          AddressDataTown =  nameof(ApprenticeshipVacancySummary.AddressDataTown),
                          PossibleStartDate = DateTime.Now,
                          CreatedDateTime = DateTime.Now,
                          FrameworkCode =  nameof(ApprenticeshipVacancySummary.FrameworkCode),
                          LearningProviderName =  nameof(ApprenticeshipVacancySummary.LearningProviderName),
                          VacancyReference = 2,
                          VacancyTitle =  nameof(ApprenticeshipVacancySummary.VacancyTitle),
                          VacancyUrl =  nameof(ApprenticeshipVacancySummary.VacancyUrl),
                          WageText =  nameof(ApprenticeshipVacancySummary.WageText)
                      }
                  }
            };
             
        }

       
    }
}