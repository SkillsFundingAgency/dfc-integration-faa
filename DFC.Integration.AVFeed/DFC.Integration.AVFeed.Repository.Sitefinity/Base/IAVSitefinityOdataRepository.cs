using DFC.Integration.AVFeed.Data.Interfaces;
using System;
using System.Threading.Tasks;

namespace DFC.Integration.AVFeed.Repository.Sitefinity
{
    public interface IAVSitefinityOdataRepository : IRepository<SfApprenticeshipVacancy>
    {
        Task AddRelatedAsync(string addedVacancyId, Guid socCodeId);
    }
}
