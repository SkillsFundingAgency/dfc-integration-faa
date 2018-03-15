﻿using AutoMapper;
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
        private IMapper mapper;

        public ProjectVacanciesFunction(IMapper mapper)
        {
            this.mapper = mapper;
        }

        public void Execute(MappedVacancyDetails allVacanciesForSOC)
        {
            if (allVacanciesForSOC != null)
            {
                projectedVacanciesForSOC = new ProjectedVacancySummary
                {
                    SocCode = allVacanciesForSOC.SocCode,
                    SocMappingId = allVacanciesForSOC.SocMappingId,
                    AccessToken = allVacanciesForSOC.AccessToken,
                };

                //if none were found for SOC
                if (allVacanciesForSOC.Vacancies == null)
                    return;

                var numberProvoidersFound = allVacanciesForSOC.Vacancies.Select(v => v.LearningProviderName).Distinct().Count();

                var projection = Enumerable.Empty<ApprenticeshipVacancyDetails>();
                if (numberProvoidersFound > 1)
                {
                    //have multipe providers
                    projection = allVacanciesForSOC.Vacancies
                        .OrderBy(v => v.PossibleStartDate)
                        .GroupBy(v => v.LearningProviderName)
                        .Select(g => g.First())
                        .Take(2);
                }
                else
                {
                    //just have a single or no provider 
                    projection = allVacanciesForSOC.Vacancies.OrderBy(v => v.PossibleStartDate).Take(2);
                }

                projectedVacanciesForSOC.Vacancies = mapper.Map<IEnumerable<ApprenticeshipVacancySummary>>(projection);
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
                throw new InvalidOperationException($"{nameof(Execute)} must be called before creating an audit record.");
            }
        }
    }
}


