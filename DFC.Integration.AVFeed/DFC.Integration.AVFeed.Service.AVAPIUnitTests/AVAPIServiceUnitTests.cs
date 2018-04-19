using DFC.Integration.AVFeed.Data.Interfaces;
using DFC.Integration.AVFeed.Data.Models;
using FakeItEasy;
using FluentAssertions;
using System;
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
            var fakeAuditService = A.Fake<IAuditService>();
            var pageNumber = 1;
            var pageSize = 5;
            var returnDiffrentProvidersOnPage = 1;

            A.CallTo(() => fakeAPIService.GetAsync(A<string>._, RequestType.search)).Returns(FAAAPIDummyResposnes.GetDummyApprenticeshipVacancySummaryResponse(pageNumber, 50, pageSize, pageSize, returnDiffrentProvidersOnPage));

            var client = new ClientProxy(fakeLogger, fakeAuditService);

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

            A.CallTo(() => fakeAPIService.GetAsync(A<string>._, RequestType.search)).MustHaveHappened();
       
            //check null exception
            Func<Task> f = async () => { await aVAPIService.GetAVSumaryPageAsync(null, 1); };
            f.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public async System.Threading.Tasks.Task GetAVsForMultipleProvidersTestAsync()
        {
            var fakeLogger = A.Fake<IApplicationLogger>();
            var fakeAPIService = A.Fake<IApprenticeshipVacancyApi>();
            var fakeAuditService = A.Fake<IAuditService>();
            var pageNumber = 1;
            var pageSize = 5;
            var returnDiffrentProvidersOnPage = 2;

            A.CallTo(() => fakeAPIService.GetAsync(A<string>._, RequestType.search)).Returns(FAAAPIDummyResposnes.GetDummyApprenticeshipVacancySummaryResponse(pageNumber, 50, pageSize, pageSize, returnDiffrentProvidersOnPage)).Once().
                Then.Returns(FAAAPIDummyResposnes.GetDummyApprenticeshipVacancySummaryResponse((pageNumber + 1), 50, pageSize, pageSize, returnDiffrentProvidersOnPage));

            var client = new ClientProxy(fakeLogger, fakeAuditService);

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

            A.CallTo(() => fakeAPIService.GetAsync(A<string>._, RequestType.search)).MustHaveHappened(Repeated.Exactly.Twice);
  
            //check null exception
            Func<Task> f = async () => { await aVAPIService.GetAVsForMultipleProvidersAsync(null); };
            f.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public async System.Threading.Tasks.Task GetApprenticeshipVacancyDetailsTestAsync()
        {
            var fakeLogger = A.Fake<IApplicationLogger>();
            var fakeAPIService = A.Fake<IApprenticeshipVacancyApi>();
            var fakeAuditService = A.Fake<IAuditService>();

            A.CallTo(() => fakeAPIService.GetAsync(A<string>._, RequestType.apprenticeships)).Returns(FAAAPIDummyResposnes.GetDummyApprenticeshipVacancyDetailsResponse());

            var client = new ClientProxy(fakeLogger, fakeAuditService);

            var aVAPIService = new AVAPIService(fakeAPIService, fakeLogger);

            var vacancyDetails = await aVAPIService.GetApprenticeshipVacancyDetailsAsync("123");

            vacancyDetails.Should().NotBeNull();
            vacancyDetails.VacancyReference.Should().Be(123);

            A.CallTo(() => fakeAPIService.GetAsync(A<string>._, RequestType.apprenticeships)).MustHaveHappened();
         
            //check null exception
            Func<Task> f = async () => { await aVAPIService.GetApprenticeshipVacancyDetailsAsync(null); };
            f.Should().Throw<ArgumentNullException>();
        }
    }
}