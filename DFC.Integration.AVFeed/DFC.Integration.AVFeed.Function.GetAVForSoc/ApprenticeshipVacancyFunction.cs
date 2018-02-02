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
        private IEnumerable<ApprenticeshipVacancyDetails> details;

        public ApprenticeshipVacancyFunction(IMapper mapper, IAVService avService)
        {
            this.mapper = mapper;
            this.avService = avService;
        }

        public async Task Execute(SocMapping mapping)
        {
            socMapping = mapping ?? throw new ArgumentNullException(nameof(mapping));
            details = await avService.GetApprenticeshipVacancyDetails(socMapping);
        }

        public MappedVacancyDetails GetOutput()
        {
            Validate();
            return new MappedVacancyDetails
            {
                Vacancies = mapper.Map<IEnumerable<ApprenticeshipVacancyDetails>>(details),
                SocCode = socMapping.SocCode,
                SocMappingId = socMapping.SocMappingId,
                AccessToken = socMapping.AccessToken
            };
        }

        private void Validate()
        {
            if (details == null)
            {
                throw new InvalidOperationException($"{nameof(Execute)} must be called before creating an audit record.");
            }
        }
    }
}
