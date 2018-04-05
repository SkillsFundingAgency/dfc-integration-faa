using Microsoft.VisualStudio.TestTools.UnitTesting;
using DFC.Integration.AVFeed.Service.AVAPI;
using FakeItEasy;
using System.Web;
using FluentAssertions;
using Xunit;
using DFC.Integration.AVFeed.Data.Interfaces;

namespace DFC.Integration.AVFeed.Service.IntergrationTests
{
    [TestClass]
    public class AVFeedServiceAVAPIIntegrationTests
    {
        [TestMethod]
        public async System.Threading.Tasks.Task SearchApprenticeshipVacanciesAPITestAsync()
        {
            var fakeLogger = A.Fake<IApplicationLogger>();
            var client = new ClientProxy(fakeLogger);

            var queryString = HttpUtility.ParseQueryString(string.Empty);

            queryString["standardLarsCodes"] = "94,95";
            queryString["frameworkLarsCodes"] = "";
            queryString["pageSize"] = "100";
            queryString["pageNumber"] = "0";
            queryString["sortBy"] = "Age";

            var responseResult = await client.GetAsync(queryString.ToString());

           
            responseResult.Should().NotBeNull();

        }
    }
}
