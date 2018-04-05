using AutoMapper;
using DFC.Integration.AVFeed.Data.Interfaces;
using DFC.Integration.AVFeed.Data.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DFC.Integration.AVFeed.Service.AVAPI
{
    public class AVSoapWebService : IAVService
    {
        private IApprenticeshipVacancyApi apprenticeshipVacancyApi;
        private IMapper mapper;
        private IApplicationLogger logger;

        public AVSoapWebService(IApprenticeshipVacancyApi apprenticeshipVacancyApi, IMapper mapper, IApplicationLogger logger)
        {
            this.apprenticeshipVacancyApi = apprenticeshipVacancyApi;
            this.mapper = mapper;
            this.logger = logger;
        }
        public async Task<IEnumerable<ApprenticeshipVacancyDetails>> GetApprenticeshipVacancyDetails(SocMapping mapping)
        {
            List<ApprenticeshipVacancyDetails> avDetails = new List<ApprenticeshipVacancyDetails>();
            var frameworksAndStandards = mapping.Frameworks.Concat(mapping.Standards);
            foreach (var standardOrframework in frameworksAndStandards)
            {
                avDetails.AddRange(await GetAVDetailsForFramework(standardOrframework, 1));
            }

            return avDetails.AsEnumerable();
        }

        public Task<IEnumerable<ApprenticeshipVacancySummaryNew>> SearchApprenticeshipVacancies(SocMapping mapping)
        {
            throw new System.NotImplementedException();
        }

        private async Task<IEnumerable<ApprenticeshipVacancyDetails>> GetAVDetailsForFramework(string standardOrframework, int page)
        {
            logger.Trace($"Extracting AV for standardOrframework:'{standardOrframework}', page:'{page}'");
            throw new System.NotImplementedException();

        }
    }
}
