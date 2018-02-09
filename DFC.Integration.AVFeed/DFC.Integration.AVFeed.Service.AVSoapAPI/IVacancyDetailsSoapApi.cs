using DFC.Integration.AVFeed.Service.AVSoapAPI.FAA;
using System.Threading.Tasks;

namespace DFC.Integration.AVFeed.Service.AVSoapAPI
{
    public interface IVacancyDetailsSoapApi
    {
        Task<VacancyDetailsResponse> GetAsync(VacancyDetailsRequest vacancyDetailsRequest);
    }
}