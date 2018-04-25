using AutoMapper;
using DFC.Integration.AVFeed.Data.Interfaces;
using DFC.Integration.AVFeed.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DFC.Integration.AVFeed.Function.ProjectVacanciesForSoc
{
    public class ProjectVacanciesFunction : IProjectVacanciesFunc
    {

        private ProjectedVacancySummary projectedVacanciesForSOC;

        public ProjectVacanciesFunction()
        {
           
        }

        public void Execute(MappedVacancySummary mappedVacancySummary)
        {
            if (mappedVacancySummary != null)
            {
                projectedVacanciesForSOC = new ProjectedVacancySummary
                {
                    SocCode = mappedVacancySummary.SocCode,
                    SocMappingId = mappedVacancySummary.SocMappingId,
                    AccessToken = mappedVacancySummary.AccessToken,
                };

                //if none were found for SOC
                if (mappedVacancySummary.Vacancies == null)
                    return;

                var numberProvoidersFound = mappedVacancySummary.Vacancies.Select(v => v.TrainingProviderName).Distinct().Count();

                var projection = Enumerable.Empty<ApprenticeshipVacancySummary>();
                if (numberProvoidersFound > 1)
                {
                    //have multipe providers
                    projection = mappedVacancySummary.Vacancies
                        .GroupBy(v => v.TrainingProviderName)
                        .Select(g => g.First())
                        .Take(2);
                }
                else
                {
                    //just have a single or no provider 
                    projection = mappedVacancySummary.Vacancies.Take(2);
                }

                projectedVacanciesForSOC.Vacancies = projection;
            }
        }

        public ProjectedVacancySummary GetOutput()
        {
            Validate();
            return projectedVacanciesForSOC;
        }

        private void Validate()
        {
            if (projectedVacanciesForSOC == null)
            {
                throw new InvalidOperationException($"{nameof(Execute)} must be called before projecting vacancies.");
            }
        }
    }
}


