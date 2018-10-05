using DFC.Integration.AVFeed.Data.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DFC.Integration.AVFeed.Data.Interfaces
{
    public interface IApprenticeshipVacancyRepository
    {
        Task DeleteExistingAsync(string  SOC);

        Task DeleteByIdAsync(Guid Id);

        Task<string> PublishAsync(ApprenticeshipVacancyDetails apprenticeshipVacancyDetails, Guid socCodeId);

        Task<IEnumerable<OrphanedVacancySummary>> GetOrphanedApprenticeshipVacanciesAsync();
      
    }
}
