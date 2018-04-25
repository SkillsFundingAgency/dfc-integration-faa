
namespace DFC.Integration.AVFeed.Repository.Sitefinity
{
    public class SocSitefinityRepository : SitefinityRepository<SitefinitySocMapping>, ISocSitefinityOdataRepository
    {
        public SocSitefinityRepository(IOdataContext<SitefinitySocMapping> odataContext, IRepoEndpointConfig<SitefinitySocMapping> repoEndpointConfig) : base(odataContext, repoEndpointConfig) { }
    }
}
