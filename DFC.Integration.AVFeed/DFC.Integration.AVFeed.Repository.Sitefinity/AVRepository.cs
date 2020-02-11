using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DFC.Integration.AVFeed.Data.Interfaces;
using DFC.Integration.AVFeed.Data.Models;


namespace DFC.Integration.AVFeed.Repository.Sitefinity
{
    public class AVRepository : IApprenticeshipVacancyRepository
    {
        private readonly IAVSitefinityOdataRepository repository;
        private readonly IApplicationLogger logger;

        public AVRepository(IAVSitefinityOdataRepository repository, IApplicationLogger logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        public async Task DeleteExistingAsync(string SOC)
        {
            var existingAvCollection = await repository.GetManyAsync(av => av.SOCCode != null && av.SOCCode.SOCCode == SOC);

            logger.Info($"Deleting '{existingAvCollection.Count()}' vacancies from sitefintiy for SOC '{SOC}'");
            foreach (var av in existingAvCollection)
            {
                await repository.DeleteAsync(av);
                logger.Info($"Deleted vacancy '{av.UrlName}-{av.Title}' from sitefintiy for SocCode '{av.SOCCode.SOCCode}'");
            }
        }

        public async Task DeleteByIdAsync(Guid Id)
        {
            await repository.DeleteByIdAsync(Id);
            logger.Info($"Deleted apprenticeship vacancy with Sitefinity Id '{Id}");
        }

        public async Task<string> PublishAsync(ApprenticeshipVacancyDetails apprenticeshipVacancyDetails,
            Guid socCodeId)
        {
            //Add vacancy
            var addedVacancyId = await repository.AddAsync(new SfApprenticeshipVacancy
            {
                PublicationDate = DateTime.UtcNow,
                UrlName = Guid.NewGuid().ToString(),
                URL = apprenticeshipVacancyDetails.VacancyUrl.ToString(),
                Location =
                    $"{apprenticeshipVacancyDetails.Location.Town} {apprenticeshipVacancyDetails.Location.PostCode}",
                WageUnitType = GetWageUnitText(apprenticeshipVacancyDetails.WageUnit),
                WageAmount = apprenticeshipVacancyDetails.WageText,
                Title = apprenticeshipVacancyDetails.Title,
                VacancyId = apprenticeshipVacancyDetails.VacancyReference.ToString(),
            });
            logger.Info(
                $"Published vacancy '{apprenticeshipVacancyDetails.Title}' to sitefinity for SocCode id '{socCodeId}' with UrlName '{addedVacancyId.UrlName}'");

            await repository.AddRelatedAsync(addedVacancyId.Id.ToString(), socCodeId);
            logger.Info(
                $"Added related field for vacancy '{apprenticeshipVacancyDetails.Title}' to sitefinity for SocCode id '{socCodeId}' with UrlName '{addedVacancyId.UrlName}'");

            await repository.UnlockAndPublishAsync(addedVacancyId.Id.ToString());

            logger.Info(
            $"Published vacancy item '{apprenticeshipVacancyDetails.Title}' to sitefinity with UrlName '{addedVacancyId.UrlName}'");


            return addedVacancyId.UrlName;

        }

        public async Task<IEnumerable<OrphanedVacancySummary>> GetOrphanedApprenticeshipVacanciesAsync()
        {
            var op = await repository.GetManyAsync(av => av.SOCCode == null || (av.SOCCode.apprenticeshipstandards.Count() == 0 && av.SOCCode.apprenticeshipframeworks.Count() == 0));
            return op.Select(v => new OrphanedVacancySummary
            {
                Id = v.Id,
                PublicationDate = v.PublicationDate,
                Title = v.Title,
                VacancyId = v.VacancyId
            });
        }

        private static string GetWageUnitText(string wageUnit)
        {
            switch (wageUnit?.ToUpperInvariant())
            {
                case "WEEKLY":
                case "ANNUALLY":
                case "MONTHLY":
                    return wageUnit;
                default:
                    return string.Empty;

            }
        }
    }
}
