using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DFC.Integration.AVFeed.Repository.Sitefinity;
using DFC.Integration.AVFeed.Repository.Sitefinity.Base;
using DFC.Integration.AVFeed.Repository.Sitefinity.Models;
using FakeItEasy;
using FluentAssertions;
using Xunit;
using DFC.Integration.AVFeed.Data.Interfaces;

namespace DFC.Integration.AVFeed.Repository.SitefinityUnitTests
{
    /// <summary>
    /// AVRepository Unit Tests
    /// </summary>
    public class AvRepositoryTests
    {
        /// <summary>
        /// Avs the repository test.
        /// </summary>
        [Fact]
        public void AvRepositoryTest()
        {
            //Arrange
            var fakeRepo = A.Fake<IAVSitefinityOdataRepository>();
            var fakeLogger = A.Fake<IApplicationLogger>();

            //Act
            var avRepository = new AVRepository(fakeRepo, fakeLogger);

            //Assert
            avRepository.Should().NotBe(null);
        }

        /// <summary>
        /// Deletes the existing asynchronous test.
        /// </summary>
        /// <param name="numberofVacancies">The numberof vacancies.</param>
        [Theory]
        [InlineData(1)]
        public void DeleteExistingAsyncTest(int numberofVacancies)
        {
            //Arrange
            var vacanciesToDelete = DataHelper.GetDummySfApprenticeshipVacancies(numberofVacancies);
            var fakeRepo = A.Fake<IAVSitefinityOdataRepository>();
            var fakeLogger = A.Fake<IApplicationLogger>();
            var deleteId = new Guid();
            A.CallTo(() => fakeRepo.GetManyAsync(A<Expression<Func<SfApprenticeshipVacancy, bool>>>._)).Returns(vacanciesToDelete);
            A.CallTo(() => fakeRepo.DeleteAsync(A<SfApprenticeshipVacancy>._)).Returns(Task.CompletedTask);
           
            var avRepository = new AVRepository(fakeRepo, fakeLogger);

            //Act
            avRepository.DeleteExistingAsync(deleteId).GetAwaiter().GetResult();

            //Assert
            A.CallTo(() => fakeRepo.GetManyAsync(A<Expression<Func<SfApprenticeshipVacancy, bool>>>._)).MustHaveHappened();
            A.CallTo(() => fakeRepo.DeleteAsync(A<SfApprenticeshipVacancy>._)).MustHaveHappened(Repeated.Exactly.Times(numberofVacancies));
        }

        /// <summary>
        /// Publishes the asynchronous test.
        /// </summary>
        [Fact]
        public void PublishAsyncTest()
        {
            //Arrange
            var vacancyDetails = DataHelper.GetDummyApprenticeshipVacancyDetails();
            var vacancyToPublish = DataHelper.GetDummySfApprenticeshipVacancies(1).FirstOrDefault();
            var fakeRepo = A.Fake<IAVSitefinityOdataRepository>();
            var fakeLogger = A.Fake<IApplicationLogger>();
            A.CallTo(() => fakeRepo.AddAsync(A<SfApprenticeshipVacancy>._)).Returns(vacancyToPublish);
            var avRepository = new AVRepository(fakeRepo, fakeLogger);

            //Act
            avRepository.PublishAsync(vacancyDetails, new Guid()).GetAwaiter().GetResult();

            //Assert
            A.CallTo(() => fakeRepo.AddAsync(A<SfApprenticeshipVacancy>._)).MustHaveHappened();
            A.CallTo(() => fakeRepo.AddRelatedAsync(A<string>._,A<Guid>._)).MustHaveHappened();

        }
    }
}