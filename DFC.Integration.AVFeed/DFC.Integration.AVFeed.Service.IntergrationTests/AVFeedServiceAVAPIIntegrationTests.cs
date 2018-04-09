using DFC.Integration.AVFeed.Service;
using FakeItEasy;
using System.Web;
using FluentAssertions;
using Xunit;
using DFC.Integration.AVFeed.Data.Interfaces;
using DFC.Integration.AVFeed.Data.Models;
using Newtonsoft.Json;
using System.Linq;

namespace DFC.Integration.AVFeed.Service.IntergrationTests
{
   
    public class AVFeedServiceAVAPIIntegrationTests
    {
        [Fact]
        public async System.Threading.Tasks.Task SearchApprenticeshipVacanciesAPITestAsync()
        {
            var fakeLogger = A.Fake<IApplicationLogger>();
            var client = new ClientProxy(fakeLogger);

            var queryString = HttpUtility.ParseQueryString(string.Empty);

            //Standard "Construction > Plumbing and Domestic Heating Technician"
            queryString["standardLarsCodes"] = "225";

            //Frameworl Plumbing and Domestic Heating Technician
            queryString["frameworkLarsCodes"] = "512";

            queryString["pageSize"] = "5";
            queryString["pageNumber"] = "1";
            queryString["sortBy"] = "Age";

            //test the search call
            var responseResult = await client.GetAsync(queryString.ToString(), RequestType.search);

            responseResult.Should().NotBeNull();

            var apprenticeshipVacancySummaryResponse = JsonConvert.DeserializeObject<ApprenticeshipVacancySummaryResponse>(responseResult);

            apprenticeshipVacancySummaryResponse.Results.Count().Should().BeGreaterThan(0);

            var vacancyId = apprenticeshipVacancySummaryResponse.Results[0].VacancyReference;
            responseResult = await client.GetAsync(vacancyId.ToString(), RequestType.apprenticeships);

            responseResult.Should().NotBeNull();

            var apprenticeshipVacancyDetails = JsonConvert.DeserializeObject<ApprenticeshipVacancyDetails>(responseResult);

            apprenticeshipVacancyDetails.VacancyReference.Should().Be(vacancyId);
         

        }
    }
}
