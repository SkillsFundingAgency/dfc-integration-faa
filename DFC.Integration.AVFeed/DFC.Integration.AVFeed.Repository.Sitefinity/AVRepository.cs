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

        public async Task DeleteExistingAsync(Guid socCodeGuid)
        {
            var existingAvCollection = await repository.GetManyAsync(av => av.SOCCode != null && av.SOCCode.Id == socCodeGuid);

            logger.Info($"Deleting '{existingAvCollection.Count()}' vacancies from sitefintiy for SocCode id '{socCodeGuid}'");
            foreach (var av in existingAvCollection)
            {
                await repository.DeleteAsync(av);
                logger.Info($"Deleted vacancy '{av.UrlName}-{av.Title}' from sitefintiy for SocCode '{av.SOCCode}'");
            }
        }

        public async Task<string> PublishAsync(ApprenticeshipVacancySummary avSummary, Guid socCodeId)
        {
            //Add vacancy
            var addedVacancyId = await repository.AddAsync(new SfApprenticeshipVacancy
            {
                PublicationDate = DateTime.UtcNow,
                UrlName = Guid.NewGuid().ToString(),
                URL = avSummary.VacancyUrl,
                Location = $"{avSummary.AddressDataTown} {avSummary.AddressDataPostCode}",
                WageUnitType = "Wage",
                WageAmount = avSummary.WageText,
                Title = avSummary.VacancyTitle,
                VacancyId = avSummary.VacancyReference.ToString(),
            });
            logger.Info($"Published vacancy '{avSummary.VacancyTitle}' to sitefintiy for SocCode id '{socCodeId}' with UrlName '{addedVacancyId.UrlName}'");

            await repository.AddRelatedAsync(addedVacancyId.Id.ToString(), socCodeId);
            logger.Info($"Added related field for vacancy '{avSummary.VacancyTitle}' to sitefintiy for SocCode id '{socCodeId}' with UrlName '{addedVacancyId.UrlName}'");

            return addedVacancyId.UrlName;
        }
    }
}
