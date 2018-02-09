using DFC.Integration.AVFeed.Data.Models;
using System;
using System.Threading.Tasks;

namespace DFC.Integration.AVFeed.Data.Interfaces
{
    public interface IApprenticeshipVacancyRepository
    {
        Task DeleteExistingAsync(Guid socCodeGuid);

        Task<string> PublishAsync(ApprenticeshipVacancySummary apprenticeshipVacancySummary, Guid socCodeId);
    }
}
