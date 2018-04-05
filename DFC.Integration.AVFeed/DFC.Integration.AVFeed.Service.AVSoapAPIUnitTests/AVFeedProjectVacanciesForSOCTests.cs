using AutoMapper;
using DFC.Integration.AVFeed.Data.Models;
using DFC.Integration.AVFeed.Function.ProjectVacanciesForSoc;
using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit;

namespace DFC.Integration.AVFeed.Service.AVSoapAPIUnitTests
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
        [InlineData(Scenario.MultipeProvidersMoreThanTwoStartDatesMatch, 2)]
        [InlineData(Scenario.OnlyOneAvailable, 1)]
        [InlineData(Scenario.NoneAvailable, 0)]
        public void GetProjectedVacanciesTestAsync(Scenario scenario, int expectedNumberDisplayed)
        {

            var mapperConfig = new MapperConfiguration(cfg => cfg.CreateMap<ApprenticeshipVacancyDetails, ApprenticeshipVacancySummary>());
            var projectVacanciesFunction = new ProjectVacanciesFunction(mapperConfig.CreateMapper());

            //run the execute method
            var testVacancies = GetTestMappedVacancySummary(scenario);
            projectVacanciesFunction.Execute(testVacancies);

            //get the results.
            var projectedVacancies = projectVacanciesFunction.GetOutput();
            CheckResultIsAsExpected(testVacancies.SocCode, projectedVacancies, expectedNumberDisplayed);

        }

        private static IEnumerable<ApprenticeshipVacancyDetails> GetTestVacanciesMultipeProvidersMoreThanTwo()
        {
            yield return new ApprenticeshipVacancyDetails() { LearningProviderName = "Provider A", PossibleStartDate = DateTime.Now.AddDays(20), VacancyTitle = "Not Displayed" };
            yield return new ApprenticeshipVacancyDetails() { LearningProviderName = "Provider B", PossibleStartDate = DateTime.Now.AddDays(1), VacancyTitle = "Displayed" };
            yield return new ApprenticeshipVacancyDetails() { LearningProviderName = "Provider B", PossibleStartDate = DateTime.Now.AddDays(5), VacancyTitle = "Not Displayed" };
            yield return new ApprenticeshipVacancyDetails() { LearningProviderName = "Provider B", PossibleStartDate = DateTime.Now.AddDays(2), VacancyTitle = "Not Displayed" };
            yield return new ApprenticeshipVacancyDetails() { LearningProviderName = "Provider A", PossibleStartDate = DateTime.Now.AddDays(10), VacancyTitle = "Not Displayed" };
            yield return new ApprenticeshipVacancyDetails() { LearningProviderName = "Provider C", PossibleStartDate = DateTime.Now.AddDays(15), VacancyTitle = "Not Displayed" };
            yield return new ApprenticeshipVacancyDetails() { LearningProviderName = "Provider D", PossibleStartDate = DateTime.Now.AddDays(3), VacancyTitle = "Displayed" };
            yield return new ApprenticeshipVacancyDetails() { LearningProviderName = "Provider A", PossibleStartDate = DateTime.Now.AddDays(3), VacancyTitle = "Not Displayed" };
        }

        private static IEnumerable<ApprenticeshipVacancyDetails> GetTestVacanciesMultipeProvidersMoreThanTwoStartDatesMatch()
        {
            yield return new ApprenticeshipVacancyDetails() { LearningProviderName = "Provider A", PossibleStartDate = DateTime.Now.AddDays(20), VacancyTitle = "Not Displayed" };
            yield return new ApprenticeshipVacancyDetails() { LearningProviderName = "Provider B", PossibleStartDate = DateTime.Now.AddDays(1), VacancyTitle = "Displayed" };
            yield return new ApprenticeshipVacancyDetails() { LearningProviderName = "Provider B", PossibleStartDate = DateTime.Now.AddDays(5), VacancyTitle = "Not Displayed" };
            yield return new ApprenticeshipVacancyDetails() { LearningProviderName = "Provider B", PossibleStartDate = DateTime.Now.AddDays(1), VacancyTitle = "Not Displayed" };
            yield return new ApprenticeshipVacancyDetails() { LearningProviderName = "Provider A", PossibleStartDate = DateTime.Now.AddDays(10), VacancyTitle = "Not Displayed" };
            yield return new ApprenticeshipVacancyDetails() { LearningProviderName = "Provider C", PossibleStartDate = DateTime.Now.AddDays(15), VacancyTitle = "Not Displayed" };
            yield return new ApprenticeshipVacancyDetails() { LearningProviderName = "Provider D", PossibleStartDate = DateTime.Now.AddDays(1), VacancyTitle = "Displayed" };
            yield return new ApprenticeshipVacancyDetails() { LearningProviderName = "Provider A", PossibleStartDate = DateTime.Now.AddDays(3), VacancyTitle = "Not Displayed" };
        }

        private static IEnumerable<ApprenticeshipVacancyDetails> GetTestVacanciesOnlyOneAvailable()
        {
            yield return new ApprenticeshipVacancyDetails() { LearningProviderName = "Provider A", PossibleStartDate = DateTime.Now.AddDays(1), VacancyTitle = "Displayed" };
        }

        private static IEnumerable<ApprenticeshipVacancyDetails> GetTestVacanciesSingleProviderMoreThanTwo()
        {
            yield return new ApprenticeshipVacancyDetails() { LearningProviderName = "Provider A", PossibleStartDate = DateTime.Now.AddDays(1), VacancyTitle = "Displayed" };
            yield return new ApprenticeshipVacancyDetails() { LearningProviderName = "Provider A", PossibleStartDate = DateTime.Now.AddDays(10), VacancyTitle = "Not Displayed" };
            yield return new ApprenticeshipVacancyDetails() { LearningProviderName = "Provider A", PossibleStartDate = DateTime.Now.AddDays(20), VacancyTitle = "Not Displayed" };
            yield return new ApprenticeshipVacancyDetails() { LearningProviderName = "Provider A", PossibleStartDate = DateTime.Now.AddDays(2), VacancyTitle = "Displayed" };
            yield return new ApprenticeshipVacancyDetails() { LearningProviderName = "Provider A", PossibleStartDate = DateTime.Now.AddDays(3), VacancyTitle = "Not Displayed" };
        }

        private void CheckResultIsAsExpected(string SocCode, ProjectedVacancySummary projectedVacancies, int expectedCount)
        {
            projectedVacancies.SocCode.Should().Be(SocCode);

            int numberOfProjectedVacanices = 0;
            if (expectedCount > 0)
            {
                foreach (ApprenticeshipVacancySummary v in projectedVacancies.Vacancies)
                {
                    numberOfProjectedVacanices++;
                    v.VacancyTitle.Should().Be("Displayed");
                }
            }
            numberOfProjectedVacanices.Should().Be(expectedCount);
        }

        private MappedVacancyDetails GetTestMappedVacancySummary(Scenario scenario)
        {
            var v = new MappedVacancyDetails() { SocCode = "T01" };

            switch (scenario)
            {
                case Scenario.OnlySingleProviderMoreThanTwo:
                    v.Vacancies = GetTestVacanciesSingleProviderMoreThanTwo();
                    break;

                case Scenario.MultipeProvidersMoreThanTwo:
                    v.Vacancies = GetTestVacanciesMultipeProvidersMoreThanTwo();
                    break;

                case Scenario.MultipeProvidersMoreThanTwoStartDatesMatch:
                    v.Vacancies = GetTestVacanciesMultipeProvidersMoreThanTwoStartDatesMatch();
                    break;

                case Scenario.OnlyOneAvailable:
                    v.Vacancies = GetTestVacanciesOnlyOneAvailable();
                    break;

                case Scenario.NoneAvailable:
                    ;
                    break;

                default:
                    throw new Exception("Test Scenarios not supported");

            }
            return v;
        }

    }
}
