using DFC.Integration.AVFeed.Data.Interfaces;
using DFC.Integration.AVFeed.Data.Models;
using System;
using System.Threading.Tasks;

namespace DFC.Intergration.AVFeed.Function.GetAVDetailsForProjected
{
    public class GetVacanciesDetailsFunction : IGetAvDetailsByIdsFunc
    {
        public Task Execute(ProjectedVacancySummary projectedVacancies)
        {
            throw new NotImplementedException();
        }

        public ProjectedVacancyDetails GetOutput()
        {
            throw new NotImplementedException();
        }
    }
}
