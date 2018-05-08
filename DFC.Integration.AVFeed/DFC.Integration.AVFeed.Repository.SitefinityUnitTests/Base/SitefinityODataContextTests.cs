using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using DFC.Integration.AVFeed.Data.Interfaces;
using DFC.Integration.AVFeed.Repository.Sitefinity;
using FakeItEasy;
using FluentAssertions;
using Newtonsoft.Json;
using RichardSzalay.MockHttp;
using Xunit;

namespace DFC.Integration.AVFeed.Repository.SitefinityUnitTests
{
    public class SitefinityODataContextTests
    {
        [Fact]
        public void SitefinityODataContextTest()
        {
            //Arrange
            var fakeTokenService = A.Fake<ITokenClient>();
            var fakeHttpClientService = A.Fake<IHttpClientService>();
            var fakeLogger = A.Fake<IApplicationLogger>();
            var fakeAudit = A.Fake<IAuditService>();

            //Act
            var sitefinityODataContext = new SitefinityODataContext<SfApprenticeshipVacancy>(fakeTokenService, fakeHttpClientService, fakeLogger, fakeAudit);

            //Assert
            sitefinityODataContext.Should().NotBe(null);
        }

        [Fact]
        public void GetHttpClientAsyncTest()
        {
            //Arrange
            var mockHttp = new MockHttpMessageHandler();
            var fakeLogger = A.Fake<IApplicationLogger>();
            var fakeAudit = A.Fake<IAuditService>();

            var fakeTokenService = A.Fake<ITokenClient>();
            A.CallTo(() => fakeTokenService.GetAccessTokenAsync())
                .Returns(nameof(fakeTokenService.GetAccessTokenAsync));
            var fakeHttpClientService = A.Fake<IHttpClientService>();
            A.CallTo(() => fakeHttpClientService.GetHttpClient()).Returns(new HttpClient(mockHttp));
            var sitefinityODataContext = new SitefinityODataContext<SfApprenticeshipVacancy>(fakeTokenService, fakeHttpClientService, fakeLogger, fakeAudit);

            //Act
            var result = sitefinityODataContext.GetHttpClientAsync().GetAwaiter().GetResult();

            //Assert
            A.CallTo(() => fakeHttpClientService.GetHttpClient()).MustHaveHappened();
            result.DefaultRequestHeaders.Contains("Authorization");
            result.DefaultRequestHeaders.Contains("X-SF-Service-Request");
            result.DefaultRequestHeaders.Contains("Accept");
        }
        
        

        [Fact]
        public void GetResultTest()
        {
            //Arrange
            var dummyUri = new Uri("http://www.test.com");
            var mockHttp = new MockHttpMessageHandler();
            var fakeLogger = A.Fake<IApplicationLogger>();
            var fakeAudit = A.Fake<IAuditService>();
            mockHttp.When(dummyUri.AbsoluteUri)
                .Respond("application/json", DataHelper.GetDummyAppVacancies()); // Respond with JSON
         
            var fakeTokenService = A.Fake<ITokenClient>();
            A.CallTo(() => fakeTokenService.GetAccessTokenAsync())
                .Returns(nameof(fakeTokenService.GetAccessTokenAsync));
            var fakeHttpClientService = A.Fake<IHttpClientService>();
            A.CallTo(() => fakeHttpClientService.GetHttpClient()).Returns(new HttpClient(mockHttp));
             
            var sitefinityODataContext = new SitefinityODataContext<SfApprenticeshipVacancy>(fakeTokenService, fakeHttpClientService, fakeLogger, fakeAudit);

            //Act
            var result = sitefinityODataContext.GetResult(dummyUri, false).GetAwaiter().GetResult();

            //Assert
            A.CallTo(() => fakeHttpClientService.GetHttpClient()).MustHaveHappened();
            result.Value.Count().Should().BeGreaterThan(0);

        }

        [Fact]
        public void PostAsyncTest()
        {
            //Arrange
            var data = DataHelper.GetDummySfApprenticeshipVacancies(1).FirstOrDefault();
            var dummyUri = new Uri("http://www.test.com");
            var mockHttp = new MockHttpMessageHandler();
            var mockResponse = new HttpResponseMessage(HttpStatusCode.Created)
            {
                Content = new StringContent(DataHelper.GetPostResultStringContent())
            };
            mockHttp.When(HttpMethod.Post, dummyUri.AbsoluteUri)
                .Respond(req => mockResponse);// Respond with JSON

            var fakeTokenService = A.Fake<ITokenClient>();
            var fakeLogger = A.Fake<IApplicationLogger>();
            var fakeAudit = A.Fake<IAuditService>();
            A.CallTo(() => fakeTokenService.GetAccessTokenAsync())
                .Returns(nameof(fakeTokenService.GetAccessTokenAsync));
            var fakeHttpClientService = A.Fake<IHttpClientService>();
            A.CallTo(() => fakeHttpClientService.GetHttpClient()).Returns(new HttpClient(mockHttp));

            var sitefinityODataContext = new SitefinityODataContext<SfApprenticeshipVacancy>(fakeTokenService, fakeHttpClientService, fakeLogger, fakeAudit);

            //Act
            var result = sitefinityODataContext.PostAsync(dummyUri, data).GetAwaiter().GetResult();
            var addedEntity = JsonConvert.DeserializeObject<SfApprenticeshipVacancy>(result);

            //Assert
            A.CallTo(() => fakeHttpClientService.GetHttpClient()).MustHaveHappened();
            addedEntity.Title.Should().NotBeNullOrEmpty();

        }

        [Fact]
        public void PutAsyncTest()
        {
            //Arrange
            var fakeLogger = A.Fake<IApplicationLogger>();
            var fakeAudit = A.Fake<IAuditService>();
            var data = "relatedData";
            var dummyUri = new Uri("http://www.test.com");
            var mockHttp = new MockHttpMessageHandler();
            var mockResponse = new HttpResponseMessage(HttpStatusCode.NoContent){Content = new StringContent(string.Empty)};
            mockHttp.When(HttpMethod.Put, dummyUri.AbsoluteUri)
                .Respond(req => mockResponse);// Respond with JSON

            var fakeTokenService = A.Fake<ITokenClient>();
            A.CallTo(() => fakeTokenService.GetAccessTokenAsync())
                .Returns(nameof(fakeTokenService.GetAccessTokenAsync));
            var fakeHttpClientService = A.Fake<IHttpClientService>();
            A.CallTo(() => fakeHttpClientService.GetHttpClient()).Returns(new HttpClient(mockHttp));

            var sitefinityODataContext = new SitefinityODataContext<SfApprenticeshipVacancy>(fakeTokenService, fakeHttpClientService, fakeLogger, fakeAudit);

            //Act
            var result = sitefinityODataContext.PutAsync(dummyUri, data).GetAwaiter().GetResult();

            //Assert
            A.CallTo(() => fakeHttpClientService.GetHttpClient()).MustHaveHappened();
            result.Should().BeNullOrEmpty();
        }
    }
}