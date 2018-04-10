using System;
using System.Linq;
using DFC.Integration.AVFeed.Data.Interfaces;
using DFC.Integration.AVFeed.Data.Models;
using DFC.Integration.AVFeed.Function.PublishSfVacancy;
using FakeItEasy;
using FluentAssertions;
using Xunit;

namespace DFC.Integration.AVFeed.Function.PublishSfVacancyUnitTests
{
    public class PublishSfVacancyFunctionTests
    {
        [Fact]
        public void PublishSfVacancyFunctionTest()
        {
            //Arrange
            var fakeRepo = A.Fake<IApprenticeshipVacancyRepository>();
            var fakeLogger = A.Fake<IApplicationLogger>();

            //Act
            var publishFunc = new PublishSfVacancyFunction(fakeRepo, null, fakeLogger);

            //Assert
            publishFunc.Should().NotBe(null);
        }

        [Fact]
        public void ExecuteAsyncTest()
        {
            //Arrange
            const string testUrl = "testURL";
            var queueItem = DataHelper.GetDummyProjectedVacancyDetails();
            var fakeRepo = A.Fake<IApprenticeshipVacancyRepository>();
            var fakeTokenClient = A.Fake<ITokenClient>();
            var fakeLogger = A.Fake<IApplicationLogger>();
            A.CallTo(() => fakeRepo.PublishAsync(A<ApprenticeshipVacancyDetails>._,A<Guid>._)).Returns(testUrl);
            var publishFunc = new PublishSfVacancyFunction(fakeRepo, fakeTokenClient, fakeLogger);

            //Act
            publishFunc.ExecuteAsync(queueItem).GetAwaiter().GetResult();

            //Assert
            A.CallTo(() => fakeRepo.DeleteExistingAsync(A<Guid>._)).MustHaveHappened();
            A.CallTo(() => fakeRepo.PublishAsync(A<ApprenticeshipVacancyDetails>._,A<Guid>._)).MustHaveHappened(Repeated.Exactly.Times(queueItem.Vacancies.Count()));
            A.CallTo(() => fakeTokenClient.SetAccessToken(A<string>._)).MustHaveHappened();
        }

        [Fact]
        public void GetOutputTest()
        {
            //Assert
            const string testUrl = "testURL";
            var queueItem = DataHelper.GetDummyProjectedVacancyDetails();
            var fakeRepo = A.Fake<IApprenticeshipVacancyRepository>();
            var fakeLogger = A.Fake<IApplicationLogger>();
            A.CallTo(() => fakeRepo.PublishAsync(A<ApprenticeshipVacancyDetails>._, A<Guid>._)).Returns(testUrl);
            var fakeTokenClient = A.Fake<ITokenClient>();
            var publishFunc = new PublishSfVacancyFunction(fakeRepo, fakeTokenClient, fakeLogger);

            //Act
            publishFunc.ExecuteAsync(queueItem).GetAwaiter().GetResult();

            //Assert
            publishFunc.GetOutput().Vacancies.FirstOrDefault()?.UrlName.Should().BeEquivalentTo(testUrl);
        }
    }
}