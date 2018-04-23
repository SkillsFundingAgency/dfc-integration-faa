using DFC.Integration.AVFeed.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DFC.Integration.AVFeed.Data.Interfaces
{
    public interface IAVService
    {
        Task<ApprenticeshipVacancyDetails> GetApprenticeshipVacancyDetailsAsync(string vacancyRef);
        Task<IEnumerable<ApprenticeshipVacancySummary>> GetAVsForMultipleProvidersAsync(SocMapping mapping);

        Task<ApprenticeshipVacancySummaryResponse> GetAVSumaryPageAsync(SocMapping mapping, int pageNumber);
    }


}
