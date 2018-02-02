using DFC.Integration.AVFeed.Data.Interfaces;
using DFC.Integration.AVFeed.Data.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DFC.Integration.AVFeed.Function.GetMappings
{
    public class SitefinitySocMappings : IGetSocMappingFunc
    {
        private IEnumerable<SocMapping> socMappingoutput;
        private ISocMappingRepository socMappingRepository;

        public SitefinitySocMappings(ISocMappingRepository socMappingRepository)
        {
            this.socMappingRepository = socMappingRepository;
        }
        public IEnumerable<SocMapping> GetOutput()
        {
            return socMappingoutput;
        }

        public async Task Execute()
        {
            socMappingoutput = await socMappingRepository.GetSocMappingAsync();
        }

        public async Task CreateAuditRecordAsync()
        {
            throw new NotImplementedException();
        }
    }
}
