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
        private readonly IAuditService auditService;

        public DeleteOrphanedAVs(IApprenticeshipVacancyRepository apprenticeshipVacancyRepository, ITokenClient sitefinityTokenClient, IApplicationLogger logger, IAuditService auditService)
        {
            this.apprenticeshipVacancyRepository = apprenticeshipVacancyRepository;
            this.sitefinityTokenClient = sitefinityTokenClient;
            this.logger = logger;
            this.auditService = auditService;
        }

        public async Task DeleteOrphanedAvsAsync()
        {
            logger.Info("Getting orphaned apprenticeship vacancies");
            var orphanedApprenticeshipVacancies = await apprenticeshipVacancyRepository.GetOrphanedApprenticeshipVacanciesAsync();

            await auditService.AuditAsync($"Got {orphanedApprenticeshipVacancies.Count()} orphaned apprenticeship vacancies");

            foreach (OrphanedVacancySummary o in orphanedApprenticeshipVacancies)
            {
                 await auditService.AuditAsync($"Got orphaned apprenticeship deleting vacancies id= {o.Id} published = {o.PublicationDate.ToString()} title = {o.Title} vacancyId = {o.VacancyId}");
                 await apprenticeshipVacancyRepository.DeleteByIdAsync(o.Id);
            }

            logger.Info("Completed deleting orphaned apprenticeship vacancies");
        }
    }
}
