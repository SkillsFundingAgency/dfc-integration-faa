namespace DFC.Integration.AVFeed.Function.GetServiceHEalthStatusTest
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using Data.Models;

    public class HelperHealthStatusDataSet
    {
        public static IEnumerable<object[]> HealthStatus()
        {
            yield return new object[]
            {
                new ServiceHealthCheckStatus
                {
                    ApplicationName = "http://local-beta.nationalcareersservice.org.uk/api/das-integration/jobprofilesocs",
                    ApplicationStatus = HttpStatusCode.OK,
                    IsApplicationRunning = true,
                    IsApplicationExternal = true,
                    ApplicationStatusDescription = "Application endpoint at :{url} is in healthy state.",
                    FailedAt = null
                },
                "http://local-beta.nationalcareersservice.org.uk/api/das-integration/jobprofilesocs"
            };
            yield return new object[]
            {
                new ServiceHealthCheckStatus
                {
                    ApplicationName = "http://local-beta.nationalcareersservice.org.uk/api/das-integration/jobprofilesocs",
                    ApplicationStatus = HttpStatusCode.Gone,
                    IsApplicationRunning = false,
                    IsApplicationExternal = true,
                    ApplicationStatusDescription = "Application endpoint at :http://local-beta.nationalcareersservice.org.uk/api/das-integration/jobprofilesocs failed to respond.Check endpoint configuration.",
                    FailedAt = DateTime.UtcNow
                },
                "http://local-beta.nationalcareersservice.org.uk/api/das-integration/jobprofilesocs"
            };
            yield return new object[]
            {
                new ServiceHealthCheckStatus
                {
                    ApplicationName = "https://soapapi.findapprenticeship.service.gov.uk/services/VacancyDetails/VacancyDetails51.svc",
                    ApplicationStatus = HttpStatusCode.OK,
                    IsApplicationRunning = true,
                    IsApplicationExternal = true,
                    ApplicationStatusDescription = "Application endpoint at :{url} is in healthy state.",
                    FailedAt = null
                },
                "https://soapapi.findapprenticeship.service.gov.uk/services/VacancyDetails/VacancyDetails51.svc"
            };
            yield return new object[]
            {
                new ServiceHealthCheckStatus
                {
                    ApplicationName = "https://soapapi.findapprenticeship.service.gov.uk/services/VacancyDetails/VacancyDetails51.svc",
                    ApplicationStatus = HttpStatusCode.Gone,
                    IsApplicationRunning = false,
                    IsApplicationExternal = true,
                    ApplicationStatusDescription = "Application endpoint at :https://soapapi.findapprenticeship.service.gov.uk/services/VacancyDetails/VacancyDetails51.svc failed to respond.Check endpoint configuration.",
                    FailedAt = DateTime.UtcNow
                },
                "https://soapapi.findapprenticeship.service.gov.uk/services/VacancyDetails/VacancyDetails51.svc"
            };
        }
    }
}