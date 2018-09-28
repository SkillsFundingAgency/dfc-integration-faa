using DFC.Integration.AVFeed.Data.Interfaces;
using DFC.Integration.AVFeed.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFC.Integration.AVFeed.Function.DeleteOrphanedAVs
{
    public class DeleteOrphanedAVs : IDeleteOrphanedAVs
    {
        private readonly IApprenticeshipVacancyRepository apprenticeshipVacancyRepository;
        private readonly ITokenClient sitefinityTokenClient;
        private readonly IApplicationLogger logger;

        public DeleteOrphanedAVs(IApprenticeshipVacancyRepository apprenticeshipVacancyRepository, ITokenClient sitefinityTokenClient, IApplicationLogger logger)
        {
            this.apprenticeshipVacancyRepository = apprenticeshipVacancyRepository;
            this.sitefinityTokenClient = sitefinityTokenClient;
            this.logger = logger;
        }

        public async Task ExecuteAsync()
        {
            var accessToken = await sitefinityTokenClient.GetAccessTokenAsync();
            logger.Info("Getting orphaned apprenticeship vacancies");
            var orphanedApprenticeshipVacancies = apprenticeshipVacancyRepository.GetOrphanedApprenticeshipVacanciesAsync();
            logger.Info($"Got {orphanedApprenticeshipVacancies.Count()} orphaned apprenticeship vacancies");
            foreach (OrphanedVacancySummary o in orphanedApprenticeshipVacancies)
            {
                 logger.Info($"Got orphaned apprenticeship and about to delete vacancies id= {o.Id} published = {o.PublicationDate.ToString()} title = {o.Title} vacancyId = {o.VacancyId}" );
                 //await apprenticeshipVacancyRepository.DeleteByIdAsync(o.Id);
            }
            logger.Info("Completed deleting orphaned apprenticeship vacancies");
        }
    }
}
