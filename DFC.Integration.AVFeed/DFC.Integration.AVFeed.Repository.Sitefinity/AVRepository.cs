using System;
using System.Linq;
using System.Threading.Tasks;
using DFC.Integration.AVFeed.Data.Interfaces;
using DFC.Integration.AVFeed.Data.Models;
using DFC.Integration.AVFeed.Repository.Sitefinity.Base;
using DFC.Integration.AVFeed.Repository.Sitefinity.Models;

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

        public async Task DeleteExistingAsync(Guid socCodevalue)
        {
            var existingAvCollection = await repository.GetManyAsync(av => av.SOCCode != null && av.SOCCode.Id == socCodevalue);

            logger.Info($"Deleting '{existingAvCollection.Count()}' vacancies from sitefintiy for SocCode id '{socCodevalue}'");
            foreach (var av in existingAvCollection)
            {
                await repository.DeleteAsync(av);
                logger.Info($"Deleted vacancy '{av.UrlName}-{av.Title}' from sitefintiy for SocCode '{av.SOCCode}'");
            }
        }

        public async Task<string> PublishAsync(ApprenticeshipVacancySummary apprenticeshipVacancySummary, Guid socCodeId)
        {
            //Add vacancy
            var addedVacancyId = await repository.AddAsync(new SfApprenticeshipVacancy
            {
                PublicationDate = DateTime.UtcNow,
                UrlName = Guid.NewGuid().ToString(),
                URL = apprenticeshipVacancySummary.VacancyUrl,
                Location = $"{apprenticeshipVacancySummary.AddressDataTown} {apprenticeshipVacancySummary.AddressDataPostCode}",
                WageUnitType = "Wage",
                WageAmount = apprenticeshipVacancySummary.WageText,
                Title = apprenticeshipVacancySummary.VacancyTitle,
                VacancyId = apprenticeshipVacancySummary.VacancyReference.ToString(),
            });
            logger.Info($"Published vacancy '{apprenticeshipVacancySummary.VacancyTitle}' to sitefintiy for SocCode id '{socCodeId}' with UrlName '{addedVacancyId.UrlName}'");

            await repository.AddRelatedAsync(addedVacancyId.Id.ToString(), socCodeId);
            logger.Info($"Added related field for vacancy '{apprenticeshipVacancySummary.VacancyTitle}' to sitefintiy for SocCode id '{socCodeId}' with UrlName '{addedVacancyId.UrlName}'");

            return addedVacancyId.UrlName;
        }
    }
}
