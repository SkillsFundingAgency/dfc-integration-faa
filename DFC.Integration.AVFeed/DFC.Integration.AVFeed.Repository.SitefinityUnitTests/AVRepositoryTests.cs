using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DFC.Integration.AVFeed.Repository.Sitefinity;
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
            var dummySOC = nameof(SfSocCode.SOCCode);
            A.CallTo(() => fakeRepo.GetManyAsync(A<Expression<Func<SfApprenticeshipVacancy, bool>>>._)).Returns(vacanciesToDelete);
            A.CallTo(() => fakeRepo.DeleteAsync(A<SfApprenticeshipVacancy>._)).Returns(Task.CompletedTask);
           
            var avRepository = new AVRepository(fakeRepo, fakeLogger);

            //Act
            avRepository.DeleteExistingAsync(dummySOC).GetAwaiter().GetResult();

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

        [Theory]
        [InlineData("Weekly", "Weekly")]
        [InlineData("Monthly", "Monthly")]
        [InlineData("Annually", "Annually")]
        [InlineData("NotApplicable", "")]
        public void PublishAsyncWageUnitTest(string wageUnitText, string expectedText)
        {
            //Arrange
            var vacancyDetails = DataHelper.GetDummyApprenticeshipVacancyDetails();
            vacancyDetails.WageUnit = wageUnitText;
            var vacancyToPublish = DataHelper.GetDummySfApprenticeshipVacancies(1).FirstOrDefault();
            var fakeRepo = A.Fake<IAVSitefinityOdataRepository>();
            var fakeLogger = A.Fake<IApplicationLogger>();
            A.CallTo(() => fakeRepo.AddAsync(A<SfApprenticeshipVacancy>._)).Returns(vacancyToPublish);
            var avRepository = new AVRepository(fakeRepo, fakeLogger);

            //Act
            avRepository.PublishAsync(vacancyDetails, new Guid()).GetAwaiter().GetResult();

            //Assert
            A.CallTo(() => fakeRepo.AddAsync(A<SfApprenticeshipVacancy>.That.Matches(x => x.WageUnitType.Equals(expectedText)))).MustHaveHappened();
            A.CallTo(() => fakeRepo.AddRelatedAsync(A<string>._, A<Guid>._)).MustHaveHappened();

        }

        [Fact]
        public void DeleteByIdAsyncTest()
        {
            //Arrange
            var fakeRepo = A.Fake<IAVSitefinityOdataRepository>();
            var fakeLogger = A.Fake<IApplicationLogger>();
            var dummyDeleteGuid = new Guid();

            A.CallTo(() => fakeRepo.DeleteByIdAsync(dummyDeleteGuid)).Returns(Task.CompletedTask);
            var avRepository = new AVRepository(fakeRepo, fakeLogger);

            //Act
            avRepository.DeleteByIdAsync(dummyDeleteGuid).GetAwaiter().GetResult();

            //Assert
            A.CallTo(() => fakeRepo.DeleteByIdAsync(dummyDeleteGuid)).MustHaveHappened(Repeated.Exactly.Once);
        }


        [Fact]
        public void GetOrphanedApprenticeshipVacanciesAsyncTest()
        {
            var fakeRepo = A.Fake<IAVSitefinityOdataRepository>();
            var fakeLogger = A.Fake<IApplicationLogger>();
            var vacanciesToDelete = DataHelper.GetDummySfApprenticeshipVacancies(2);

            A.CallTo(() => fakeRepo.GetManyAsync(A<Expression<Func<SfApprenticeshipVacancy, bool>>>._)).Returns(vacanciesToDelete);

            var avRepository = new AVRepository(fakeRepo, fakeLogger);

            //Act
            var results = avRepository.GetOrphanedApprenticeshipVacanciesAsync();

            A.CallTo(() => fakeRepo.GetManyAsync(A<Expression<Func<SfApprenticeshipVacancy, bool>>>.That.Matches(m => LinqExpressionsTestHelper.IsExpressionEqual(m, v => v.SOCCode == null || (v.SOCCode.apprenticeshipstandards.Count() == 0 && v.SOCCode.apprenticeshipframeworks.Count() == 0))))).MustHaveHappened();
        }
    }
}