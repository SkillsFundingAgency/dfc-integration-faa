using DFC.Integration.AVFeed.Data.Interfaces;
using DFC.Integration.AVFeed.Data.Models;
using DFC.Integration.AVFeed.Service;
using FakeItEasy;
using FluentAssertions;
using System;
using System.Linq;
using Xunit;

namespace DFC.Integration.AVFeed.Service.AVAPIUnitTests
{
    public class AVAPIServiceUnitTests
    {
        [Fact]
        public async System.Threading.Tasks.Task GetAVSumaryPageTestAsync()
        {
            var fakeLogger = A.Fake<IApplicationLogger>();

            var client = new ClientProxy(fakeLogger);
            var aVAPIService = new AVAPIService(client, fakeLogger);

            var mapping = new SocMapping
            {
                SocCode = "1234",
                SocMappingId = Guid.NewGuid(),
                Standards = new string[] { "225" },
                Frameworks = new string[] { "512" }
            };

            var pageSumary = await aVAPIService.GetAVSumaryPageAsync(mapping, 1);
        }

        [Fact]
        public async System.Threading.Tasks.Task GetAVsForMultipleProvidersTestAsync()
        {
            var fakeLogger = A.Fake<IApplicationLogger>();

            var client = new ClientProxy(fakeLogger);
            var aVAPIService = new AVAPIService(client, fakeLogger);

            var mapping = new SocMapping
            {
                SocCode = "1234",
                SocMappingId = Guid.NewGuid(),
                Standards = new string[] { "225" },
                Frameworks = new string[] { "512" }
            };

            var aVSumaryList = await aVAPIService.GetAVsForMultipleProvidersAsync(mapping);

            var numberProviders = aVSumaryList.Select(v => v.TrainingProviderName).Distinct().Count();

            numberProviders.Should().BeGreaterThan(1);
        }

        private ApprenticeshipVacancySummaryResponse GetDummyApprenticeshipVacancySummaryResponse(int currentPage, int totalMatches, int pageSize)
        {

            var r = new ApprenticeshipVacancySummaryResponse();
            r.CurrentPage = currentPage;
            r.TotalMatched = totalMatches;
            r.TotalPages = (totalMatches / pageSize);
            r.TotalReturned = pageSize;

            return r;

        }
    }
}
