using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DFC.Integration.AVFeed.Repository.Sitefinity.Models;
using DFC.Integration.AVFeed.Repository.Sitefinity.Model;

namespace DFC.Integration.AVFeed.Repository.Sitefinity.Base
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

        //public override async Task<SfApprenticeshipVacancy> AddAsync(SfApprenticeshipVacancy entity)
        //{
        //    entity.SOCCode1 = $"{{\"@odata.id\":  \"{socEndpointConfig.GetSingleItemEndpoint(entity.SOCCode1)}\"}}";
        //    return await base.AddAsync(entity);
        //}

        public override async Task DeleteAsync(SfApprenticeshipVacancy entity)
        {
            using (var client = await OdataContext.GetHttpClientAsync())
            {
                await client.DeleteAsync(base.RepoEndpointConfig.GetSingleItemEndpoint(entity.Id.ToString()));
            }
        }

        public override async Task<IEnumerable<SfApprenticeshipVacancy>> GetManyAsync(Expression<Func<SfApprenticeshipVacancy, bool>> where)
        {
            var allVacancies = await GetAllAsync();
            return allVacancies.AsQueryable().Where(where).AsEnumerable();
        }
    }
}
