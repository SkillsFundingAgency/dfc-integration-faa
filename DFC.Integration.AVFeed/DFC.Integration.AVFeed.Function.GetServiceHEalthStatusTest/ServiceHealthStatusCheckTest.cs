namespace DFC.Integration.AVFeed.Function.GetServiceHEalthStatusTest
{
    using System.Configuration;
    using System.Net;
    using Data.Interfaces;
    using GetServiceHealthStatus.Interfaces;
    using FakeItEasy;
    using FluentAssertions;
    using GetServiceHealthStatus;
    using Xunit;

    public class ServiceHealthStatusCheckTest
    {
        [Fact()]
        public void GetAvFeedHealthStatusInfo()
        {
            //Arrange
            var httpWReq =(HttpWebRequest)WebRequest.Create("https://soapapi.findapprenticeship.service.gov.uk/services/VacancyDetails/VacancyDetails51.svc");
            var response = (HttpWebResponse)httpWReq.GetResponse();

            var externalFeedProxy = A.Fake<IHttpExternalFeedProxy>();
            var serviceHealthStatus = A.Fake<IGetServiceHealthStatus>();
            A.CallTo(() => externalFeedProxy.GetResponseFromUri("https://soapapi.findapprenticeship.service.gov.uk/services/VacancyDetails/VacancyDetails51.svc")).Returns(response);

            var serviceHealthCheckStatus=new GetAvServiceHealthStatus(externalFeedProxy);
            var result= serviceHealthCheckStatus.GetAvFeedHealthStatusInfoAsync().Result;
            result.Should().NotBe(null);

            A.CallTo(() => externalFeedProxy.GetResponseFromUri(A<string>._)).MustHaveHappened();

        }
        [Fact()]
        public void GetSitefinityHealthStatusInfo()
        {
            //Arrange
            var httpWReq = (HttpWebRequest)WebRequest.Create("http://local-beta.nationalcareersservice.org.uk");
            var response = (HttpWebResponse)httpWReq.GetResponse();
            var endpoint = ConfigurationManager.AppSettings.Get("Sitefinity.SocApiEndPoint");
            var externalFeedProxy = A.Fake<IHttpExternalFeedProxy>();
            var serviceHealthStatus = A.Fake<IGetServiceHealthStatus>();
            A.CallTo(() => externalFeedProxy.GetResponseFromUri("http://local-beta.nationalcareersservice.org.uk")).Returns(response);

            var serviceHealthCheckStatus = new GetAvServiceHealthStatus(externalFeedProxy);
            var result = serviceHealthCheckStatus.GetAvFeedHealthStatusInfoAsync().Result;
            result.Should().NotBe(null);

            A.CallTo(() => externalFeedProxy.GetResponseFromUri(A<string>._)).MustHaveHappened();

        }
    }
}


