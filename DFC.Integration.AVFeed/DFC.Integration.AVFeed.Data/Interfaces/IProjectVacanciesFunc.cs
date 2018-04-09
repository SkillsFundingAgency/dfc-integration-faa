using DFC.Integration.AVFeed.Data.Models;

namespace DFC.Integration.AVFeed.Data.Interfaces
{
    public interface IProjectVacanciesFunc
    {
        ProjectedVacancySummary GetOutput();
        void Execute(MappedVacancyDetails vacancySumariesForSOC);
    }
}

