using DFC.Integration.AVFeed.Data.Interfaces;
using DFC.Integration.AVFeed.Data.Models;
using System;
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
        public Task Execute(ProjectedVacancySummary projectedVacancies)
        {
            if (projectedVacancies == null)
            {
                throw new ApplicationException(nameof(projectedVacancies));
            }

            projectedVacancyDetails = new ProjectedVacancyDetails() {SocCode = projectedVacancies.SocCode, AccessToken =  projectedVacancies.AccessToken, SocMappingId = projectedVacancies.SocMappingId}

            projectedVacancyDetails.Vacancies = new List<ApprenticeshipVacancyDetails>()
;




        }

        public ProjectedVacancyDetails GetOutput()
        {
            throw new NotImplementedException();
        }
    }
}
