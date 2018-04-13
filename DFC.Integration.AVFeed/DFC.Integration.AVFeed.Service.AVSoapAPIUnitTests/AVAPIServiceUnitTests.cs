using DFC.Integration.AVFeed.Data.Interfaces;
using DFC.Integration.AVFeed.Data.Models;
using DFC.Integration.AVFeed.Service;
using FakeItEasy;
using FluentAssertions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace DFC.Integration.AVFeed.Service.AVAPIUnitTests
{
    public class AVAPIServiceUnitTests
    {
        [Fact]
        public async System.Threading.Tasks.Task GetAVSumaryPageTestAsync()
        {
            var fakeLogger = A.Fake<IApplicationLogger>();
            var fakeAPIService = A.Fake<IApprenticeshipVacancyApi>();
            var pageNumber = 1;
            var pageSize = 5;
            var returnDiffrentProvidersOnPage = 1;

            A.CallTo(() => fakeAPIService.GetAsync(A<string>._, RequestType.search)).Returns(GetDummyApprenticeshipVacancySummaryResponse(pageNumber, 50, pageSize, pageSize, returnDiffrentProvidersOnPage));

            var client = new ClientProxy(fakeLogger);

            var aVAPIService = new AVAPIService(fakeAPIService, fakeLogger);

            var mapping = new SocMapping
            {
                SocCode = "1234",
                SocMappingId = Guid.NewGuid(),
                Standards = new string[] { "225" },
                Frameworks = new string[] { "512" }
            };

            var pageSumary = await aVAPIService.GetAVSumaryPageAsync(mapping, 1);

            pageSumary.CurrentPage.Should().Be(pageNumber);
            pageSumary.Results.Count().Should().Be(pageSize);

            //check null exception
            Func<Task> f = async () => { await aVAPIService.GetAVSumaryPageAsync(null, 1); };
            f.Should().Throw<ArgumentNullException>();

        }

        [Fact]
        public async System.Threading.Tasks.Task GetAVsForMultipleProvidersTestAsync()
        {
            var fakeLogger = A.Fake<IApplicationLogger>();
            var fakeAPIService = A.Fake<IApprenticeshipVacancyApi>();
            var pageNumber = 1;
            var pageSize = 5;
            var returnDiffrentProvidersOnPage = 2;

            A.CallTo(() => fakeAPIService.GetAsync(A<string>._, RequestType.search)).Returns(GetDummyApprenticeshipVacancySummaryResponse(pageNumber, 50, pageSize, pageSize, returnDiffrentProvidersOnPage)).Once().
                Then.Returns(GetDummyApprenticeshipVacancySummaryResponse((pageNumber + 1), 50, pageSize, pageSize, returnDiffrentProvidersOnPage));

            var client = new ClientProxy(fakeLogger);

            var aVAPIService = new AVAPIService(fakeAPIService, fakeLogger);

            var mapping = new SocMapping
            {
                SocCode = "1234",
                SocMappingId = Guid.NewGuid(),
                Standards = new string[] { "225" },
                Frameworks = new string[] { "512" }
            };

            var aVSumaryList = await aVAPIService.GetAVsForMultipleProvidersAsync(mapping);

            //must have got more then 1 page to get multipe supplier
            aVSumaryList.Count().Should().BeGreaterThan(pageSize);

            var numberProviders = aVSumaryList.Select(v => v.TrainingProviderName).Distinct().Count();
            numberProviders.Should().BeGreaterThan(1);

            //check null exception
            Func<Task> f = async () => { await aVAPIService.GetAVsForMultipleProvidersAsync(null); };
            f.Should().Throw<ArgumentNullException>();

        }

        [Fact]
        public async System.Threading.Tasks.Task GetApprenticeshipVacancyDetailsTestAsync()
        {
            var fakeLogger = A.Fake<IApplicationLogger>();
            var fakeAPIService = A.Fake<IApprenticeshipVacancyApi>();

            A.CallTo(() => fakeAPIService.GetAsync(A<string>._, RequestType.apprenticeships)).Returns(GetDummyApprenticeshipVacancyDetailsResponse());

            var client = new ClientProxy(fakeLogger);

            var aVAPIService = new AVAPIService(fakeAPIService, fakeLogger);

            var vacancyDetails = await aVAPIService.GetApprenticeshipVacancyDetailsAsync("123");
            
            vacancyDetails.Should().NotBeNull();
            vacancyDetails.VacancyReference.Should().Be(123);

            //check null exception
            Func<Task> f = async () => { await aVAPIService.GetApprenticeshipVacancyDetailsAsync(null); };
            f.Should().Throw<ArgumentNullException>();

        }

        private string GetDummyApprenticeshipVacancySummaryResponse(int currentPage, int totalMatches, int nunmberToReturn, int pageSize, int diffrentProvidersPage)
        {

            var r = new ApprenticeshipVacancySummaryResponse();
            r.CurrentPage = currentPage;
            r.TotalMatched = totalMatches;
            r.TotalPages = (totalMatches / pageSize);
            r.TotalReturned = nunmberToReturn;

            var recordsToReturn = new List<ApprenticeshipVacancySummary>();

            for (int ii = 0; ii < nunmberToReturn; ii++)
            {
                recordsToReturn.Add(new ApprenticeshipVacancySummary()
                {
                    VacancyReference = ii,
                    Title = $"Title {ii}",
                    TrainingProviderName = $"TrainingProviderName {((currentPage == diffrentProvidersPage) ? ii : currentPage)}"
                });
            }

            r.Results = recordsToReturn.ToArray();

            return JsonConvert.SerializeObject(r);
        }

        private string GetDummyApprenticeshipVacancyDetailsResponse()
        {

            var r = new ApprenticeshipVacancyDetails()
            {
                VacancyReference = 123
            };

            return JsonConvert.SerializeObject(r);
        }
    }
}
