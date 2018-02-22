namespace DFC.Integration.AVFeed.Function.GetServiceHEalthStatusTest
{
    using System.Net;
    using Data.Models;
    using GetServiceHealthStatus.Interfaces;
    using FakeItEasy;
    using FluentAssertions;
    using GetServiceHealthStatus;
    using Xunit;

    public class ServiceHealthStatusCheckTest : HelperHealthStatusDataSet
    {
        [Fact()]
        public void GetAvFeedHealthStatusInfo()
        {
            //Arrange
            var httpWReq =(HttpWebRequest)WebRequest.Create("https://soapapi.findapprenticeship.service.gov.uk/services/VacancyDetails/VacancyDetails51.svc");
            var response = (HttpWebResponse)httpWReq.GetResponse();

            var externalFeedProxy = A.Fake<IHttpExternalFeedProxy>();
            A.CallTo(() => externalFeedProxy.GetResponseFromUri("https://soapapi.findapprenticeship.service.gov.uk/services/VacancyDetails/VacancyDetails51.svc")).Returns(response);

            var serviceHealthCheckStatus=new GetAvServiceHealthStatus(externalFeedProxy);
            var result= serviceHealthCheckStatus.GetApprenticeshipFeedHealthStatusAsync().Result;

            result.Should().NotBe(null);
            A.CallTo(() => externalFeedProxy.GetResponseFromUri(A<string>._)).MustHaveHappened();
        }
       
        [Theory]
        [MemberData(nameof(HealthStatus))]
        public void GetServiceHealthStatus(ServiceHealthCheckStatus status,string uri)
        {
            //Arrange
            var externalFeedProxy = new HttpExternalFeedProxy();

            var serviceHealthCheckStatus = new GetAvServiceHealthStatus(externalFeedProxy);
            var result = serviceHealthCheckStatus.GetExternalFeedStatusAsync(uri).Result;
            status.FailedAt = result.FailedAt;
            //Assert
            result.Should().BeEquivalentTo(status);
            result.Should().NotBe(null);
        }
    }
}


