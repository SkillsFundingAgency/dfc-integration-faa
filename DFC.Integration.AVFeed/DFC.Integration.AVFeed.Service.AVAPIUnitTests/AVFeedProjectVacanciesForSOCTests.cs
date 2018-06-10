using DFC.Integration.AVFeed.Data.Models;
using DFC.Integration.AVFeed.Function.ProjectVacanciesForSoc;
using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit;

namespace DFC.Integration.AVFeed.Service.AVAPIUnitTests
{

    public enum Scenario
    {
        OnlySingleProviderMoreThanTwo,
        MultipeProvidersMoreThanTwo,
        MultipeProvidersMoreThanTwoStartDatesMatch,
        OnlyOneAvailable,
        NoneAvailable
    }
    public class AVFeedProjectVacanciesForSOCTests
    {
        [Theory]
        [InlineData(Scenario.OnlySingleProviderMoreThanTwo, 2)]
        [InlineData(Scenario.MultipeProvidersMoreThanTwo, 2)]
        [InlineData(Scenario.OnlyOneAvailable, 1)]
        [InlineData(Scenario.NoneAvailable, 0)]
        public void GetProjectedVacanciesTestAsync(Scenario scenario, int expectedNumberDisplayed)
        {

            var projectVacanciesFunction = new ProjectVacanciesFunction();

            //run the execute method
            var testVacancies = GetTestMappedVacancySummary(scenario);
            projectVacanciesFunction.Execute(testVacancies);

            //get the results.
            var projectedVacancies = projectVacanciesFunction.GetOutput();
            CheckResultIsAsExpected(testVacancies.SocCode, projectedVacancies, expectedNumberDisplayed);

        }

        private static IEnumerable<ApprenticeshipVacancySummary> GetTestVacanciesMultipeProvidersMoreThanTwo()
        {
            yield return new ApprenticeshipVacancySummary { TrainingProviderName = "Provider A", Title = "Displayed" };
            yield return new ApprenticeshipVacancySummary { TrainingProviderName = "Provider A", Title = "Not Displayed" };
            yield return new ApprenticeshipVacancySummary { TrainingProviderName = "Provider B", Title = "Displayed" };
        }

        private static IEnumerable<ApprenticeshipVacancySummary> GetTestVacanciesOnlyOneAvailable()
        {
            yield return new ApprenticeshipVacancySummary { TrainingProviderName = "Provider A", Title = "Displayed" };
        }

        private static IEnumerable<ApprenticeshipVacancySummary> GetTestVacanciesSingleProviderMoreThanTwo()
        {
            yield return new ApprenticeshipVacancySummary { TrainingProviderName = "Provider A", Title = "Displayed" };
            yield return new ApprenticeshipVacancySummary { TrainingProviderName = "Provider A", Title = "Displayed" };
            yield return new ApprenticeshipVacancySummary { TrainingProviderName = "Provider A", Title = "Not Displayed" };
        }

        private static void CheckResultIsAsExpected(string SocCode, ProjectedVacancySummary projectedVacancies, int expectedCount)
        {
            projectedVacancies.SocCode.Should().Be(SocCode);

            var numberOfProjectedVacanices = 0;
            if (expectedCount > 0)
            {
                foreach (ApprenticeshipVacancySummary v in projectedVacancies.Vacancies)
                {
                    numberOfProjectedVacanices++;
                    v.Title.Should().Be("Displayed");
                }
            }
            numberOfProjectedVacanices.Should().Be(expectedCount);
        }

        private static MappedVacancySummary GetTestMappedVacancySummary(Scenario scenario)
        {
            var v = new MappedVacancySummary { SocCode = "T01" };

            switch (scenario)
            {
                case Scenario.OnlySingleProviderMoreThanTwo:
                    v.Vacancies = GetTestVacanciesSingleProviderMoreThanTwo();
                    break;

                case Scenario.MultipeProvidersMoreThanTwo:
                    v.Vacancies = GetTestVacanciesMultipeProvidersMoreThanTwo();
                    break;

                case Scenario.OnlyOneAvailable:
                    v.Vacancies = GetTestVacanciesOnlyOneAvailable();
                    break;

                case Scenario.NoneAvailable:
                    ;
                    break;

                default:
                    throw new InvalidOperationException("Test Scenarios not supported");

            }
            return v;
        }

    }
}
