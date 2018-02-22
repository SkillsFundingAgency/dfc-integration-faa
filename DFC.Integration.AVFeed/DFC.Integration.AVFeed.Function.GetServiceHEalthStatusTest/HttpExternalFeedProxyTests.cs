namespace DFC.Integration.AVFeed.Function.GetServiceHEalthStatusTest
{
    using System.Net;
    using FluentAssertions;
    using GetServiceHealthStatus;
    using GetServiceHealthStatus.Interfaces;
    using Xunit;

    public class HttpExternalFeedProxyTests
    {
        [Theory]
        [InlineData("http://local-beta.nationalcareersservice.org.uk",HttpStatusCode.OK)]
        [InlineData("https://soapapi.findapprenticeship.service.gov.uk/services/VacancyDetails/VacancyDetails51.svc", HttpStatusCode.OK)]
        public void GetResponseFromUriTest(string uri,HttpStatusCode code)
        {
            IHttpExternalFeedProxy proxy=new HttpExternalFeedProxy();

            var respone=proxy.GetResponseFromUri(uri).Result;
            //Assert
            respone.Should().Match<HttpWebResponse>(x => x.StatusCode == code);
        }
    }
}