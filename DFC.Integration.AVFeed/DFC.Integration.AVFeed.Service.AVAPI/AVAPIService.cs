using DFC.Integration.AVFeed.Data.Interfaces;
using DFC.Integration.AVFeed.Data.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Net;
using DFC.Integration.AVFeed.Core;

namespace DFC.Integration.AVFeed.Service
{
    public class AVAPIService : IAVService
    {
        private IApprenticeshipVacancyApi apprenticeshipVacancyApi;
        private IApplicationLogger logger;
        private string _sortBy = ConfigurationManager.AppSettings.Get("FAA.SortBy");

        public AVAPIService(IApprenticeshipVacancyApi apprenticeshipVacancyApi, IApplicationLogger logger)
        {
            this.apprenticeshipVacancyApi = apprenticeshipVacancyApi;
            this.logger = logger;
        }

        public async Task<ApprenticeshipVacancyDetails> GetApprenticeshipVacancyDetailsAsync(string vacancyRef)
        {
            if (vacancyRef == null)
            {
                throw new ArgumentNullException(nameof(vacancyRef));
            }

            var responseResult = await apprenticeshipVacancyApi.GetAsync($"{vacancyRef}", RequestType.apprenticeships);
            logger.Trace($"Got details for vacancy ref : {vacancyRef}");
            return JsonConvert.DeserializeObject<ApprenticeshipVacancyDetails>(responseResult);
        }

        /// <summary>
        /// Get Apprenticeship Vacancy Summaries untill we have at leat two providers or all of them for the mapping
        /// </summary>
        /// <param name="mapping"></param>
        /// <returns></returns>
        ///
        public async Task<IEnumerable<ApprenticeshipVacancySummary>> GetAVsForMultipleProvidersAsync(SocMapping mapping)
        {
            if (mapping == null)
            {
                throw new ArgumentNullException(nameof(SocMapping));
            }

            var avSummary = new List<ApprenticeshipVacancySummary>();
            var pageNumber = 0;
            var maxPagesToTry =  int.Parse(ConfigurationManager.AppSettings.Get("FAA.MaxPagesToTryPerMapping"));

            logger.Trace($"Getting vacancies for mapping {JsonConvert.SerializeObject(mapping)}");
            //Allways break after a given number off loops
            while (maxPagesToTry > pageNumber)
            {
                    var apprenticeshipVacancySummaryResponse = await GetAVSumaryPageAsync(mapping, ++pageNumber);

                    logger.Trace(
                        $"Got {apprenticeshipVacancySummaryResponse?.TotalReturned} vacancies of {apprenticeshipVacancySummaryResponse?.TotalMatched} on page: {pageNumber} of {apprenticeshipVacancySummaryResponse?.TotalPages} for SOC {mapping.SocCode}");

                if (apprenticeshipVacancySummaryResponse?.Results != null)
                    avSummary.AddRange(apprenticeshipVacancySummaryResponse.Results);

                //stop when there are no more pages or we have more then multiple supplier
                    if (apprenticeshipVacancySummaryResponse?.TotalPages < pageNumber ||
                        avSummary.Select(v => v.TrainingProviderName).Distinct().Count() > 1)
                        break;
            }

            return avSummary;
        }

        public async Task<ApprenticeshipVacancySummaryResponse> GetAVSumaryPageAsync(SocMapping mapping, int pageNumber)
        {
            if (mapping == null)
            {
                throw new ArgumentNullException(nameof(SocMapping));
            }

            logger.Trace($"Extracting AV summaries for SOC {mapping.SocCode} page : {pageNumber}");

            var queryString = HttpUtility.ParseQueryString(string.Empty);
           
            if (!mapping.Standards.Where(s=>!string.IsNullOrEmpty(s)).Distinct().Any() && !mapping.Frameworks.Where(f => !string.IsNullOrEmpty(f)).Distinct().Any())
                return null;
            queryString["standardLarsCodes"] = string.Join("," , mapping.Standards.Where(std => !string.IsNullOrEmpty(std)));
            queryString["frameworkLarsCodes"] = string.Join("," , mapping.Frameworks.Where(fwrk => !string.IsNullOrEmpty(fwrk)));
            queryString["pageSize"] = ConfigurationManager.AppSettings.Get("FAA.PageSize");
            queryString["pageNumber"] = pageNumber.ToString();
            queryString["sortBy"] = _sortBy;

            var responseResult = await apprenticeshipVacancyApi.GetAsync(queryString.ToString() , RequestType.search);

            return JsonConvert.DeserializeObject<ApprenticeshipVacancySummaryResponse>(responseResult);
        }

    }
}
