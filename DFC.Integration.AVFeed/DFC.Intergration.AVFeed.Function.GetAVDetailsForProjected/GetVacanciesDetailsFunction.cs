using DFC.Integration.AVFeed.Data.Interfaces;
using DFC.Integration.AVFeed.Data.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DFC.Intergration.AVFeed.Function.GetAVDetailsForProjected
{
    public class GetVacanciesDetailsFunction : IGetAvDetailsByIdsFunc
    {
        private ProjectedVacancyDetails projectedVacancyDetails;

        private IAVService avService;

        public GetVacanciesDetailsFunction(IAVService avService)
        {
            this.avService = avService;
        }
        public async Task Execute(ProjectedVacancySummary projectedVacancySummary)
        {
            if (projectedVacancySummary == null)
            {
                throw new ApplicationException(nameof(projectedVacancySummary));
            }

            projectedVacancyDetails = new ProjectedVacancyDetails() { SocCode = projectedVacancySummary.SocCode, AccessToken = projectedVacancySummary.AccessToken, SocMappingId = projectedVacancySummary.SocMappingId };

            var vacancyDetailsList  = new List<ApprenticeshipVacancyDetails>();

            foreach (var v in projectedVacancySummary.Vacancies)
            {
                vacancyDetailsList.Add(await avService.GetApprenticeshipVacancyDetailsAsync(v.VacancyReference.ToString()));
            }
            projectedVacancyDetails.Vacancies = vacancyDetailsList;
        }

        public ProjectedVacancyDetails GetOutput()
        {
            Validate();
            return projectedVacancyDetails;
        }

        private void Validate()
        {
            if (projectedVacancyDetails == null)
            {
                throw new InvalidOperationException($"{nameof(Execute)} must be called before geting output");
            }
        }
    }
}
