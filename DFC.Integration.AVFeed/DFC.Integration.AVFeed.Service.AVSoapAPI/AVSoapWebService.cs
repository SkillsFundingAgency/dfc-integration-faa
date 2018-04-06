using AutoMapper;
using DFC.Integration.AVFeed.Data.Interfaces;
using DFC.Integration.AVFeed.Data.Models;
using DFC.Integration.AVFeed.Service.AVSoapAPI.FAA;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DFC.Integration.AVFeed.Service.AVSoapAPI
{
    public class AVSoapWebService : IAVService_ToDelete
    {
        private IVacancyDetailsSoapApi soapApi;
        private IMapper mapper;
        private IApplicationLogger logger;

        public AVSoapWebService(IVacancyDetailsSoapApi soapApi, IMapper mapper, IApplicationLogger logger)
        {
            this.soapApi = soapApi;
            this.mapper = mapper;
            this.logger = logger;
        }
        public async Task<IEnumerable<ApprenticeshipVacancyDetails>> GetApprenticeshipVacancyDetails(SocMapping mapping)
        {
            List<ApprenticeshipVacancyDetails> avDetails = new List<ApprenticeshipVacancyDetails>();
            var frameworksAndStandards = mapping.Frameworks.Concat(mapping.Standards);
            foreach (var standardOrframework in frameworksAndStandards)
            {
                avDetails.AddRange(await GetAVDetailsForFramework(standardOrframework, 1, VacancyDetailsSearchLocationType.National));
                avDetails.AddRange(await GetAVDetailsForFramework(standardOrframework, 1, VacancyDetailsSearchLocationType.NonNational));
            }

            return avDetails.AsEnumerable();
        }

        public Task<IEnumerable<ApprenticeshipVacancySummaryNew>> SearchApprenticeshipVacancies(SocMapping mapping)
        {
            throw new System.NotImplementedException();
        }

        private async Task<IEnumerable<ApprenticeshipVacancyDetails>> GetAVDetailsForFramework(string standardOrframework, int page, VacancyDetailsSearchLocationType locationType)
        {
            logger.Trace($"Extracting AV for standardOrframework:'{standardOrframework}', page:'{page}', locationType: {locationType}");

            var result = await soapApi.GetAsync(new VacancyDetailsRequest
            {
                VacancySearchCriteria = new VacancySearchData
                {
                    FrameworkCode = standardOrframework,
                    VacancyLocationType = locationType,
                    PageIndex = page,
                }
            });

            var details = mapper.Map<IEnumerable<FAA.VacancyFullData>, IEnumerable<ApprenticeshipVacancyDetails>>(result.SearchResults.SearchResults,
                opt => opt.AfterMap(
                    (src, dst) =>
                    {
                        foreach (var item in dst)
                        {
                            item.MessageId = result.MessageId;
                            item.VacancyLocationType = locationType.ToString();
                            item.FrameworkCode = standardOrframework;
                            item.ResultPage = page;

                        }
                    }));

            logger.Trace($"Extracted page:'{page}' of totalPages:'{result.SearchResults.TotalPages}' for standardOrframework:'{standardOrframework}', locationType: {locationType}");
            if (result.SearchResults.TotalPages >= page)
            {
                return Enumerable.Concat(details, await GetAVDetailsForFramework(standardOrframework, ++page, locationType));
            }
            else
            {
                return details;
            }
        }
    }
}
