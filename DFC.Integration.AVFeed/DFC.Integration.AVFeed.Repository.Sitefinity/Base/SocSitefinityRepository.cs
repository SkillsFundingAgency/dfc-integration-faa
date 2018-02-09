using DFC.Integration.AVFeed.Repository.Sitefinity.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DFC.Integration.AVFeed.Data.Interfaces;
using DFC.Integration.AVFeed.Repository.Sitefinity.Base;

namespace DFC.Integration.AVFeed.Repository.Sitefinity
{
    public class SocSitefinityRepository : SitefinityRepository<SitefinitySocMapping>, ISocSitefinityOdataRepository
    {
        public SocSitefinityRepository(IOdataContext<SitefinitySocMapping> odataContext, IRepoEndpointConfig<SitefinitySocMapping> repoEndpointConfig) : base(odataContext, repoEndpointConfig) { }
    }
}
