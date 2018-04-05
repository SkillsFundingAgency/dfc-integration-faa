using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DFC.Integration.AVFeed.Data.Models;
using DFC.Integration.AVFeed.Repository.Sitefinity.Model;
using DFC.Integration.AVFeed.Repository.Sitefinity.Models;

namespace DFC.Integration.AVFeed.Repository.SitefinityUnitTests
{
    public static class DataHelper
    {
        public static PagedOdataResult<SitefinitySocMapping> GetDummyOdataResultSocMapping()
        {

            PagedOdataResult<SitefinitySocMapping> mappingData = new PagedOdataResult<SitefinitySocMapping>()
                {
                
                    MetaData =
                        "http://local-beta.nationalcareersservice.org.uk/api/das-integration/$metadata#jobprofilesocs(NavigateToApprenticeshipStandard(UrlName),NavigateToApprenticeshipFramework(UrlName))",
                    NextLink =
                        "http://local-beta.nationalcareersservice.org.uk/api/das-integration/jobprofilesocs?$expand=NavigateToApprenticeshipStandard(%24select%3DUrlName)%2CNavigateToApprenticeshipFramework(%24select%3DUrlName)&$skip=50",
                    Value = new List<SitefinitySocMapping>()
                    {
                        new SitefinitySocMapping()
                        {
                            Id= new Guid("aaad48af-6b0a-4ec2-857c-6b96ce1ae151"),
                            SocCode = "1234",
                            Frameworks = new List<NavigateToApprenticeshipFramework>()
                            {
                                new NavigateToApprenticeshipFramework() {UrlName = "1234"},
                                new NavigateToApprenticeshipFramework() {UrlName = "2222"},
                                new NavigateToApprenticeshipFramework() {UrlName = "3333"},
                                new NavigateToApprenticeshipFramework() {UrlName = "4444"},

                            },
                            Standards = new List<NavigateToApprenticeshipStandard>()
                            {
                                new NavigateToApprenticeshipStandard() {UrlName = "22334"},
                                new NavigateToApprenticeshipStandard() {UrlName = "3333"},
                                new NavigateToApprenticeshipStandard() {UrlName = "4444"}
                            }
                        },

                        new SitefinitySocMapping()
                        {
                            Id= new Guid("8a330e9d-2418-40fd-bb13-0b2108ee37e8"),
                            SocCode = "3333",
                            Frameworks = new List<NavigateToApprenticeshipFramework>()
                            {
                                new NavigateToApprenticeshipFramework() {UrlName = "1234"},
                                new NavigateToApprenticeshipFramework() {UrlName = "2222"},
                                new NavigateToApprenticeshipFramework() {UrlName = "3333"},
                                new NavigateToApprenticeshipFramework() {UrlName = "4444"},

                            },
                            Standards = new List<NavigateToApprenticeshipStandard>()
                            {
                                new NavigateToApprenticeshipStandard() {UrlName = "22334"},
                                new NavigateToApprenticeshipStandard() {UrlName = "3333"},
                                new NavigateToApprenticeshipStandard() {UrlName = "4444"}
                            }
                        }
                    }
                };
            return mappingData;
        }

        public static PagedOdataResult<SfApprenticeshipVacancy> GetDummyOdataResultAppVacancies()
        {
            var mappingData = new PagedOdataResult<SfApprenticeshipVacancy>()
            {
                  Value =  GetDummySfApprenticeshipVacancies(1)
            };
            return mappingData;
        }

        public static ICollection<SitefinitySocMapping> GetListOfSiteFinitySocMapping()
        {
            var value = new List<SitefinitySocMapping>()
            {
                new SitefinitySocMapping()
                {
                    SocCode = "1234",
                    Frameworks = new List<NavigateToApprenticeshipFramework>()
                    {
                        new NavigateToApprenticeshipFramework() {UrlName = "1234"},
                        new NavigateToApprenticeshipFramework() {UrlName = "2222"},
                        new NavigateToApprenticeshipFramework() {UrlName = "3333"},
                        new NavigateToApprenticeshipFramework() {UrlName = "4444"},

                    },
                    Standards = new List<NavigateToApprenticeshipStandard>()
                    {
                        new NavigateToApprenticeshipStandard() {UrlName = "22334"},
                        new NavigateToApprenticeshipStandard() {UrlName = "3333"},
                        new NavigateToApprenticeshipStandard() {UrlName = "4444"}
                    }
                },

                new SitefinitySocMapping()
                {
                    SocCode = "3333",
                    Frameworks = new List<NavigateToApprenticeshipFramework>()
                    {
                        new NavigateToApprenticeshipFramework() {UrlName = "1234"},
                        new NavigateToApprenticeshipFramework() {UrlName = "2222"},
                        new NavigateToApprenticeshipFramework() {UrlName = "3333"},
                        new NavigateToApprenticeshipFramework() {UrlName = "4444"},

                    },
                    Standards = new List<NavigateToApprenticeshipStandard>()
                    {
                        new NavigateToApprenticeshipStandard() {UrlName = "22334"},
                        new NavigateToApprenticeshipStandard() {UrlName = "3333"},
                        new NavigateToApprenticeshipStandard() {UrlName = "4444"}
                    }
                }
            };
            return value;
        }

        public static IEnumerable<SfApprenticeshipVacancy> GetDummySfApprenticeshipVacancies(int numberOfVacancies)
        {
            for (var i = 0; i < numberOfVacancies; i++)
            {
                yield return new SfApprenticeshipVacancy
                {
                    UrlName = nameof(SfApprenticeshipVacancy.UrlName),
                    URL = nameof(SfApprenticeshipVacancy.URL),
                    WageAmount = nameof(SfApprenticeshipVacancy.WageAmount),
                    WageUnitType = nameof(SfApprenticeshipVacancy.WageUnitType),
                     Location= nameof(SfApprenticeshipVacancy.Location),
                    Title = nameof(SfApprenticeshipVacancy.Title),
                    VacancyId = nameof(SfApprenticeshipVacancy.VacancyId),
                    PublicationDate = DateTime.UtcNow,

                };
            }
        }

        public static string GetDummyAppVacancies()
        {
            const string dir = "..\\..\\..\\DFC.Integration.AVFeed.Repository.SitefinityUnitTests\\jsonData\\";

            var directoryInfo = new DirectoryInfo(dir);
            var firstFile = directoryInfo.GetFiles("dummyAppVacancies.json").FirstOrDefault();
            if (firstFile != null)
            {
                return File.ReadAllText(firstFile.FullName);
            }

            return string.Empty;
        }

        public static string GetPostResultStringContent()
        {
            const string dir = "..\\..\\..\\DFC.Integration.AVFeed.Repository.SitefinityUnitTests\\jsonData\\";

            var directoryInfo = new DirectoryInfo(dir);
            var firstFile = directoryInfo.GetFiles("dummyCreatedVacancy.json").FirstOrDefault();
            if (firstFile != null)
            {
                return File.ReadAllText(firstFile.FullName);
            }

            return string.Empty;
        }

        public static ApprenticeshipVacancySummary GetDummyApprenticeshipVacancySummary()
        {
            return
                new ApprenticeshipVacancySummary
                {
                    AddressDataPostCode = nameof(ApprenticeshipVacancySummary.AddressDataPostCode),
                    AddressDataTown = nameof(ApprenticeshipVacancySummary.AddressDataTown),
                    PossibleStartDate = DateTime.Now,
                    CreatedDateTime = DateTime.Now,
                    FrameworkCode = nameof(ApprenticeshipVacancySummary.FrameworkCode),
                    LearningProviderName = nameof(ApprenticeshipVacancySummary.LearningProviderName),
                    VacancyReference = 1,
                    VacancyTitle = nameof(ApprenticeshipVacancySummary.VacancyTitle),
                    VacancyUrl = nameof(ApprenticeshipVacancySummary.VacancyUrl),
                    WageText = nameof(ApprenticeshipVacancySummary.WageText)
                };
        }
    }
}