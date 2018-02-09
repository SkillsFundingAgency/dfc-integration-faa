using DFC.Integration.AVFeed.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DFC.Integration.AVFeed.Data.Models;

namespace DFC.Integration.AVFeed.Repository.CosmosDb
{
    public class AuditUsingCosmosDb : IAuditRepository
    {
        public async Task AddAvDetailsForSocAsync(SocMapping socMapping, IEnumerable<ApprenticeshipVacancyDetails> apprenticeshipVacancyDetails) => await Task.FromResult(0);
        public async Task AddProjectedAvSummaryForSocAsync(string socCode, IEnumerable<ApprenticeshipVacancySummary> apprenticeshipVacancySummary) => await Task.FromResult(0);
        public async Task AddSocMappingsAsync(IEnumerable<SocMapping> socMappings) => await Task.FromResult(0);
        public async Task CreateAuditRecordAsync(AppMetadata metadata) => await Task.FromResult(0);
    }
}
