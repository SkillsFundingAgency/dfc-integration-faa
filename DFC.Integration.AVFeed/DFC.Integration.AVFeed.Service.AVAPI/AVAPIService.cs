using DFC.Integration.AVFeed.Data.Interfaces;
using DFC.Integration.AVFeed.Data.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace DFC.Integration.AVFeed.Service
{
    public class AVAPIService : IAVService
    {
        private IApprenticeshipVacancyApi apprenticeshipVacancyApi;
        private IApplicationLogger logger;

        public AVAPIService(IApprenticeshipVacancyApi apprenticeshipVacancyApi, IApplicationLogger logger)
        {
            this.apprenticeshipVacancyApi = apprenticeshipVacancyApi;
            this.logger = logger;
        }

        public Task<IEnumerable<ApprenticeshipVacancyDetails>> GetApprenticeshipVacancyDetails(string vacancyRef)
        {
            throw new System.NotImplementedException();
        }

       
        /// <summary>
        /// Get Apprenticeship Vacancy Summaries untill we have at leat two providers or all of them for the mapping
        /// </summary>
        /// <param name="mapping"></param>
        /// <returns></returns>
        /// 
        public async Task<IEnumerable<ApprenticeshipVacancySummaryNew>> GetAVsForMultipleProvidersAsync(SocMapping mapping)
        {
            if (mapping == null)
            {
                throw new ArgumentNullException(nameof(SocMapping));
            }

            List<ApprenticeshipVacancySummaryNew> avSummary = new List<ApprenticeshipVacancySummaryNew>();

            var pageOfVacancies = await GetAVSumaryPageAsync(mapping, 1);


            return avSummary;
        }

        public async Task<ApprenticeshipVacancySummaryResponse> GetAVSumaryPageAsync(SocMapping mapping, int pageNumber)
        {
            if (mapping == null)
            {
                throw new ArgumentNullException(nameof(SocMapping));
            }

            logger.Trace($"Extracting AV summaries for standardOrframework SOC {mapping.SocCode}");

           
            var queryString = HttpUtility.ParseQueryString(string.Empty);

            queryString["standardLarsCodes"] = string.Join(",", mapping.Standards);
            queryString["frameworkLarsCodes"] = string.Join(",", mapping.Frameworks);
            queryString["pageSize"] = "20";
            queryString["pageNumber"] = pageNumber.ToString();
            queryString["sortBy"] = "Age";

            var responseResult = await apprenticeshipVacancyApi.GetAsync(queryString.ToString(), RequestType.search);


            return JsonConvert.DeserializeObject<ApprenticeshipVacancySummaryResponse>(responseResult);

        }

    }
}
