using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DFC.Integration.AVFeed.Repository.Sitefinity
{
    public class AVSitefinityRepository : SitefinityRepository<SfApprenticeshipVacancy>, IAVSitefinityOdataRepository
    {
        private IRepoEndpointConfig<SitefinitySocMapping> socEndpointConfig;

        public AVSitefinityRepository(IOdataContext<SfApprenticeshipVacancy> odataContext, IRepoEndpointConfig<SfApprenticeshipVacancy> repoEndpointConfig, IRepoEndpointConfig<SitefinitySocMapping> socEndpointConfig) : base(odataContext, repoEndpointConfig)
        {
            this.socEndpointConfig = socEndpointConfig;
        }

        public async Task AddRelatedAsync(string addedVacancyId, Guid socCodeId)
        {
            var relatedSocLink = $"{{\"@odata.id\":  \"{socEndpointConfig.GetSingleItemEndpoint(socCodeId.ToString())}\"}}";
            await OdataContext.PutAsync(base.RepoEndpointConfig.GetReferenceEndpoint(addedVacancyId, "SOCCode"), relatedSocLink);
        }

        public override async Task DeleteAsync(SfApprenticeshipVacancy entity)
        {
            await OdataContext.DeleteAsync(base.RepoEndpointConfig.GetSingleItemEndpoint(entity.Id.ToString()));
        }

        public async Task DeleteByIdAsync(Guid Id)
        {
            await OdataContext.DeleteAsync(base.RepoEndpointConfig.GetSingleItemEndpoint(Id.ToString()));
        }

        public override async Task<IEnumerable<SfApprenticeshipVacancy>> GetManyAsync(Expression<Func<SfApprenticeshipVacancy, bool>> where)
        {
            var allVacancies = await GetAllAsync(false);
            return allVacancies.AsQueryable().Where(where).AsEnumerable();
        }
    }
}
