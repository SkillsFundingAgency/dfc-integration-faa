using DFC.Integration.AVFeed.Data.Models;
using System.Threading.Tasks;

namespace DFC.Integration.AVFeed.Data.Interfaces
{
    public interface IGetAvForSocFunc
    {
        MappedVacancySummary GetOutput();
        Task Execute(SocMapping mapping);
    }
}
