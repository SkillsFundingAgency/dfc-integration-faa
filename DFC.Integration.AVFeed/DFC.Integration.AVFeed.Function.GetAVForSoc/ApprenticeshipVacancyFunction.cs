using AutoMapper;
using DFC.Integration.AVFeed.Data.Interfaces;
using DFC.Integration.AVFeed.Data.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DFC.Integration.AVFeed.Function.GetAVForSoc
{
    public class ApprenticeshipVacancyFunction : IGetAvForSocFunc
    {
        private SocMapping socMapping;
        private IMapper mapper;
        private IAVService avService;
        private IEnumerable<ApprenticeshipVacancySummary> vacancySummaries;

        public ApprenticeshipVacancyFunction(IMapper mapper, IAVService avService)
        {
            this.mapper = mapper;
            this.avService = avService;
        }

        public async Task Execute(SocMapping mapping)
        {
            socMapping = mapping ?? throw new ArgumentNullException(nameof(mapping));
            vacancySummaries = await avService.GetAVsForMultipleProvidersAsync(socMapping);
        }

        public MappedVacancySummary GetOutput()
        {
            Validate();
            return new MappedVacancySummary
            {
                Vacancies = vacancySummaries,
                SocCode = socMapping.SocCode,
                SocMappingId = socMapping.SocMappingId,
                AccessToken = socMapping.AccessToken
            };
        }

        private void Validate()
        {
            if (vacancySummaries == null)
            {
                throw new InvalidOperationException($"{nameof(Execute)} must be called before creating an audit record.");
            }
        }
    }
}
