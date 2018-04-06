using DFC.Integration.AVFeed.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DFC.Integration.AVFeed.Data.Interfaces
{
    public interface IAVService_ToDelete
    {
        Task<IEnumerable<ApprenticeshipVacancyDetails>> GetApprenticeshipVacancyDetails(SocMapping mapping);
       
    }

    public interface IAVService
    {
        Task<IEnumerable<ApprenticeshipVacancyDetails>> GetApprenticeshipVacancyDetails(string vacancyRef);
        Task<IEnumerable<ApprenticeshipVacancySummaryNew>> GetAVsForMultipleProvidersAsync(SocMapping mapping);

        Task<ApprenticeshipVacancySummaryResponse> GetAVSumaryPageAsync(SocMapping mapping, int pageNumber);
    }


}
