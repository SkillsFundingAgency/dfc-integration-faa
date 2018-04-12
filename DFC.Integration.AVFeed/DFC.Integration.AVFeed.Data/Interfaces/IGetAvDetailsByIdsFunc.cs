using DFC.Integration.AVFeed.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFC.Integration.AVFeed.Data.Interfaces
{
    public interface IGetAvDetailsByIdsFunc
    {
        ProjectedVacancyDetails GetOutput();
        Task ExecuteAsync(ProjectedVacancySummary projectedVacancies);
    }
}
