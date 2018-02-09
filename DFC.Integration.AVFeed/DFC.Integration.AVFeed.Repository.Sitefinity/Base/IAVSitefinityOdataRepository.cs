using DFC.Integration.AVFeed.Data.Interfaces;
using DFC.Integration.AVFeed.Repository.Sitefinity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFC.Integration.AVFeed.Repository.Sitefinity.Base
{
    public interface IAVSitefinityOdataRepository : IRepository<SfApprenticeshipVacancy>
    {
        Task AddRelatedAsync(string addedVacancyId, Guid socCodeId);
    }
}
