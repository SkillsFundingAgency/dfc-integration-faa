using System.Threading.Tasks;

namespace DFC.Integration.AVFeed.Service.AVAPI
{
    public interface IApprenticeshipVacancyApi
    {
        Task<JsonResult> GetAsync(VacancyDetailsRequest vacancyDetailsRequest);
    }
}