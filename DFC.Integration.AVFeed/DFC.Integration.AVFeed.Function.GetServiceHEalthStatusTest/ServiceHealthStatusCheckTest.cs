﻿namespace DFC.Integration.AVFeed.Function.GetServiceHEalthStatusTest
{
    using System;
    using System.Net;
    using Data.Models;
    using DFC.Integration.AVFeed.Data.Interfaces;
    using DFC.Integration.AVFeed.Repository.Sitefinity;
    using FakeItEasy;
    using FluentAssertions;
    using GetServiceHealthStatus;
    using Xunit;


    public class ServiceHealthStatusCheckTest 
    {
        [Fact()]
        public async System.Threading.Tasks.Task GetSitefinityHealthStatusAsync()
        {
            var fakeSocSitefinityOdataRepository = A.Fake<ISocSitefinityOdataRepository>();
            var fakeAVService = A.Fake<IAVService>();
            var fakeApplicationLogger = A.Fake<IApplicationLogger>();

            var getAvServiceHealthStatus = new GetAvServiceHealthStatus(fakeSocSitefinityOdataRepository, fakeAVService, fakeApplicationLogger);

            var result = await  getAvServiceHealthStatus.GetSitefinityHealthStatusAsync();
            result.Status.Should().Be(ServiceState.Green);

            A.CallTo(() => fakeSocSitefinityOdataRepository.GetAllAsync()).ThrowsAsync(new Exception("Fake Exception"));
            result = await getAvServiceHealthStatus.GetSitefinityHealthStatusAsync();
            result.Status.Should().Be(ServiceState.Red);
        }

        [Fact()]
        public async System.Threading.Tasks.Task GetAVFeedHealthStatusAsync()
        {
            var fakeSocSitefinityOdataRepository = A.Fake<ISocSitefinityOdataRepository>();
            var fakeAVService = A.Fake<IAVService>();
            var fakeApplicationLogger = A.Fake<IApplicationLogger>();

            var getAvServiceHealthStatus = new GetAvServiceHealthStatus(fakeSocSitefinityOdataRepository, fakeAVService, fakeApplicationLogger);

            var result = await getAvServiceHealthStatus.GetApprenticeshipFeedHealthStatusAsync();
            result.Status.Should().Be(ServiceState.Green);
            
            A.CallTo(() => fakeAVService.GetApprenticeshipVacancyDetails(A<SocMapping>._)).ThrowsAsync(new Exception("Fake Exception"));
            result = await getAvServiceHealthStatus.GetApprenticeshipFeedHealthStatusAsync();
            result.Status.Should().Be(ServiceState.Red);
        }


        [Fact()]
        public async System.Threading.Tasks.Task GetServiceHealthStateAsync()
        {
            var fakeSocSitefinityOdataRepository = A.Fake<ISocSitefinityOdataRepository>();
            var fakeAVService = A.Fake<IAVService>();
            var fakeApplicationLogger = A.Fake<IApplicationLogger>();

            var getAvServiceHealthStatus = new GetAvServiceHealthStatus(fakeSocSitefinityOdataRepository, fakeAVService, fakeApplicationLogger);

            var result = await getAvServiceHealthStatus.GetServiceHealthStateAsync();
            result.ApplicationStatus.Should().Be(HttpStatusCode.OK);

            A.CallTo(() => fakeAVService.GetApprenticeshipVacancyDetails(A<SocMapping>._)).ThrowsAsync(new Exception("Fake Exception"));
            result = await getAvServiceHealthStatus.GetServiceHealthStateAsync();
            result.ApplicationStatus.Should().Be(HttpStatusCode.BadGateway);
        }
    }
}

