using AutoMapper;
using DFC.Integration.AVFeed.Data.Interfaces;
using DFC.Integration.AVFeed.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DFC.Integration.AVFeed.Repository.Sitefinity
{
    public class SocMappingRepository : ISocMappingRepository
    {
        private IMapper mapper;
        private ISocSitefinityOdataRepository sitefinityOdataRepository;
        private ITokenClient sitefinityTokenClient;

        public SocMappingRepository(IMapper mapper, ISocSitefinityOdataRepository sitefinityOdataRepository, ITokenClient sitefinityTokenClient)
        {
            this.mapper = mapper;
            this.sitefinityOdataRepository = sitefinityOdataRepository;
            this.sitefinityTokenClient = sitefinityTokenClient;
        }

        public async Task<IEnumerable<SocMapping>> GetSocMappingAsync()
        {
            var accessToken =  await sitefinityTokenClient.GetAccessTokenAsync();
            var result = await sitefinityOdataRepository.GetAllAsync();
            var output = mapper.Map<IEnumerable<SocMapping>>(result);
            foreach (var item in output)
            {
                item.AccessToken = accessToken;
            }

            return output;
        }
    }
}

