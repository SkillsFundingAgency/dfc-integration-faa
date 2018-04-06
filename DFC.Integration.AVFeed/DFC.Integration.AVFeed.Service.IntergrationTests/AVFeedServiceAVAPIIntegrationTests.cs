using DFC.Integration.AVFeed.Service;
using FakeItEasy;
using System.Web;
using FluentAssertions;
using Xunit;
using DFC.Integration.AVFeed.Data.Interfaces;
using DFC.Integration.AVFeed.Data.Models;

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

            var responseResult = await client.GetAsync(queryString.ToString(), RequestType.search);

            responseResult.Should().NotBeNull();

        }
    }
}
