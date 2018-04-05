using DFC.Integration.AVFeed.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DFC.Integration.AVFeed.Data.Interfaces
{
    public interface IAVService
    {
        Task<IEnumerable<ApprenticeshipVacancyDetails>> GetApprenticeshipVacancyDetails(SocMapping mapping);
        Task<IEnumerable<ApprenticeshipVacancySummaryNew>> SearchApprenticeshipVacancies(SocMapping mapping);
    }
}
