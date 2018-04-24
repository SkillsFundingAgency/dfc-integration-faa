using System;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using DFC.Integration.AVFeed.Repository.Sitefinity;
using FakeItEasy;
using FluentAssertions;
using RichardSzalay.MockHttp;
using Xunit;
using System.Threading.Tasks;

namespace DFC.Integration.AVFeed.Repository.SitefinityUnitTests.Base
{
    /// <summary>
    /// AVSitefinityRepository Tests
    /// </summary>
    public class AvSitefinityRepositoryTests
    {
        /// <summary>
        /// Avs the sitefinity repository test.
        /// </summary>
        [Fact]
        public void AvSitefinityRepositoryTest()
        { 
            //Arrange
            var fakeContext = A.Fake<IOdataContext<SfApprenticeshipVacancy>>();
            var fakeRepoEndPoint = A.Fake<IRepoEndpointConfig<SfApprenticeshipVacancy>>();
            var fakeSocRepoEndPointConfig = A.Fake<IRepoEndpointConfig<SitefinitySocMapping>>();
            
            //Act
            var sitefinityAvRepository = new AVSitefinityRepository(fakeContext, fakeRepoEndPoint, fakeSocRepoEndPointConfig);

            //Assert
            sitefinityAvRepository.Should().NotBe(null);
        }

        /// <summary>
        /// Adds the related asynchronous test.
        /// </summary>
        [Fact]
        public void AddRelatedAsyncTest()
        {
            //Arrange
            var fakeContext = A.Fake<IOdataContext<SfApprenticeshipVacancy>>();
            A.CallTo(() => fakeContext.PutAsync(A<Uri>._, A<string>._)).Returns(string.Empty);
            var fakeRepoEndPoint = A.Fake<IRepoEndpointConfig<SfApprenticeshipVacancy>>();
            var fakeSocRepoEndPointConfig = A.Fake<IRepoEndpointConfig<SitefinitySocMapping>>();
            var sitefinityAvRepository = new AVSitefinityRepository(fakeContext, fakeRepoEndPoint, fakeSocRepoEndPointConfig);

            //Act
            sitefinityAvRepository.AddRelatedAsync(string.Empty, new Guid()).GetAwaiter().GetResult();

            //Assert
            A.CallTo(() => fakeContext.PutAsync(A<Uri>._, A<string>._)).MustHaveHappened();
        }

        /// <summary>
        /// Deletes the asynchronous test.
        /// </summary>
        [Fact]
        public async Task DeleteAsyncTestAsync()
        {
            //Arrange
            var mockHttp = new MockHttpMessageHandler();
            
            // Setup a respond for the user api (including a wildcard in the URL)
            mockHttp.When("*")
                .Respond("application/json", "{'name' : 'Test McGee'}"); // Respond with JSON
            var entity = DataHelper.GetDummySfApprenticeshipVacancies(1).FirstOrDefault();
            var fakeContext = A.Fake<IOdataContext<SfApprenticeshipVacancy>>();
            A.CallTo(() => fakeContext.GetHttpClientAsync()).Returns(Task.FromResult(new HttpClient(mockHttp)));
            A.CallTo(() => fakeContext.PostAsync(A<Uri>._, A<SfApprenticeshipVacancy>._)).Returns(Task.FromResult(string.Empty));
            var fakeRepoEndPoint = A.Fake<IRepoEndpointConfig<SfApprenticeshipVacancy>>();
            A.CallTo(() => fakeRepoEndPoint.GetSingleItemEndpoint(A<string>._)).Returns("http://www.test.com");
          
            var fakeSocRepoEndPointConfig = A.Fake<IRepoEndpointConfig<SitefinitySocMapping>>();
            var sitefinityAvRepository = new AVSitefinityRepository(fakeContext, fakeRepoEndPoint, fakeSocRepoEndPointConfig);

            //Act
            await sitefinityAvRepository.DeleteAsync(entity);

            //Assert
            A.CallTo(() => fakeRepoEndPoint.GetSingleItemEndpoint(A<string>._)).MustHaveHappened();
            A.CallTo(() => fakeContext.GetHttpClientAsync()).MustHaveHappened();

        }

        /// <summary>
        /// Gets the many asynchronous test.
        /// </summary>
        [Fact]
        public void GetManyAsyncTest()
        {
            //Arrange
            //Arrange
            var mockHttp = new MockHttpMessageHandler();

            // Setup a respond for the user api (including a wildcard in the URL)
            mockHttp.When("*")
                .Respond("application/json", "{'name' : 'Test McGee'}");
            var pageResults = DataHelper.GetDummyOdataResultAppVacancies();
            var fakeContext = A.Fake<IOdataContext<SfApprenticeshipVacancy>>();
            Expression<Func<SfApprenticeshipVacancy, bool>>
                expression = vacancy => vacancy.UrlName.Equals(string.Empty);
            A.CallTo(() => fakeContext.GetHttpClientAsync()).Returns(new HttpClient(mockHttp));
            A.CallTo(() => fakeContext.GetResult(A<Uri>._)).Returns(pageResults);
            var fakeRepoEndPoint = A.Fake<IRepoEndpointConfig<SfApprenticeshipVacancy>>();
            A.CallTo(() => fakeRepoEndPoint.GetSingleItemEndpoint(A<string>._)).Returns(string.Empty);
            var fakeSocRepoEndPointConfig = A.Fake<IRepoEndpointConfig<SitefinitySocMapping>>();
            var sitefinityAvRepository = new AVSitefinityRepository(fakeContext, fakeRepoEndPoint, fakeSocRepoEndPointConfig);

            //Act
           
            sitefinityAvRepository.GetManyAsync(expression).GetAwaiter().GetResult();

            //Assert
            A.CallTo(() => fakeContext.GetResult(A<Uri>._)).MustHaveHappened();
            A.CallTo(() => fakeRepoEndPoint.GetAllItemsEndpoint()).MustHaveHappened();
        }
    }
}