using DFC.Integration.AVFeed.Data.Interfaces;
using DFC.Integration.AVFeed.Data.Models;
using DFC.Integration.AVFeed.Function.GetAVDetailsForProjectedAV;
using FakeItEasy;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DFC.Integration.AVFeed.Service.AVAPIUnitTests
{
    public class AVFeedGetAVDetailsForProjectedTests
    {
        public async System.Threading.Tasks.Task GetProjectedVacanciesTestAsync(Scenario scenario, int expectedNumberDisplayed)
        {
            var fakeIAVService = A.Fake<IAVService>();
            var getVacanciesDetailsFunction = new GetVacanciesDetailsFunction(fakeIAVService);

            var dummyProjectedVacancySummary = new ProjectedVacancySummary
            {
                SocCode = "SOC123",
                AccessToken = "Access123",
                SocMappingId = Guid.NewGuid(),
                Vacancies = GetDummyVacanciesSummary()
            };

            A.CallTo(() => fakeIAVService.GetApprenticeshipVacancyDetailsAsync(A<string>._)).Returns(GetDummyVacanciesDetails(A<string>._));

            //Check for calling GetOutput before Execute
            Action test = () => {getVacanciesDetailsFunction.GetOutput(); };
            test.Should().Throw<InvalidOperationException>();

            await getVacanciesDetailsFunction.ExecuteAsync(dummyProjectedVacancySummary);

            var projectedVacancyDetails = getVacanciesDetailsFunction.GetOutput();

            projectedVacancyDetails.SocCode.Should().Be(dummyProjectedVacancySummary.SocCode);
            projectedVacancyDetails.SocMappingId.Should().Be(dummyProjectedVacancySummary.SocMappingId);
            projectedVacancyDetails.AccessToken.Should().Be(dummyProjectedVacancySummary.AccessToken);
            projectedVacancyDetails.Vacancies.Count().Should().Be(2);

        }

        private static IEnumerable<ApprenticeshipVacancySummary> GetDummyVacanciesSummary()
        {
            yield return new ApprenticeshipVacancySummary { TrainingProviderName = "Provider A", Title = "DummyTitle One", VacancyReference = 1 };
            yield return new ApprenticeshipVacancySummary { TrainingProviderName = "Provider B", Title = "DummyTitle Two", VacancyReference = 2 };
        }

        private static ApprenticeshipVacancyDetails GetDummyVacanciesDetails(string vacancyRef)
        {
            return new ApprenticeshipVacancyDetails { TrainingProviderName = "Provider A", Title = "DummyTitle One", VacancyReference = int.Parse(vacancyRef)};
        }
    }
}

