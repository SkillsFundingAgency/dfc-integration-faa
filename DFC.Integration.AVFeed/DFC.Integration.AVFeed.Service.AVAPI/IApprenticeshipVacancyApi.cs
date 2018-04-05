using System.Threading.Tasks;

namespace DFC.Integration.AVFeed.Service.AVAPI
{
    public interface IApprenticeshipVacancyApi
    {
        Task<string> GetAsync(string requestQueryString);
    }
}