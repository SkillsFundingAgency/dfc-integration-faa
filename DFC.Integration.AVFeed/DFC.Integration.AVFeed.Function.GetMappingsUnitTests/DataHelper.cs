using System;
using System.Collections.Generic;
using DFC.Integration.AVFeed.Data.Models;
using DFC.Integration.AVFeed.Repository.Sitefinity;

namespace DFC.Integration.AVFeed.Function.GetMappingsUnitTests
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
                    }
                };
            return mappingData;
        }

        public static IEnumerable<SitefinitySocMapping> GetListOfSiteFinitySocMapping()
        {
            IEnumerable<SitefinitySocMapping> value = new List<SitefinitySocMapping>()
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

        public static IEnumerable<SocMapping> GetFinalSocMappingData()
        {
            IEnumerable<SocMapping> mappingData = new List<SocMapping>()
            {
                new SocMapping()
                {
                    CorrelationId= new Guid("aaad48af-6b0a-4ec2-857c-6b96ce1ae151"),
                    SocMappingId = new Guid("aaad48af-6b0a-4ec2-857c-6b96ce1ae151"),
                    SocCode = "1234",
                    Standards = new List<string>() {"1234", "2345", "3456"},
                    Frameworks = new List<string>() {"1234", "2345", "3456"}
                },
                new SocMapping()
                {
                    CorrelationId = new Guid("8a330e9d-2418-40fd-bb13-0b2108ee37e8"),
                    SocMappingId = new Guid("8a330e9d-2418-40fd-bb13-0b2108ee37e8"),
                    SocCode = "2222",
                    Standards = new List<string>() {"1234", "2345", "3456"},
                    Frameworks =
                        new List<string>() {"1234", "2345", "3456"}
                },
                new SocMapping()
                {
                    CorrelationId = new Guid("40a123ef-c759-4da6-8b4f-42cb70d31257"),
                    SocMappingId = new Guid("40a123ef-c759-4da6-8b4f-42cb70d31257"),
                    SocCode = "3333",
                    Standards = new List<string>() {"1234", "2345", "3456"},
                    Frameworks =
                        new List<string>() {"1234", "2345", "3456"}
                },
                new SocMapping()
                {
                    CorrelationId = new Guid("6079230a-3fb2-4a25-9848-b8f497459033"),
                    SocMappingId = new Guid("6079230a-3fb2-4a25-9848-b8f497459033"),
                    SocCode = "4444",
                    Standards = new List<string>() {"1234", "2345", "3456"},
                    Frameworks =
                        new List<string>() {"1234", "2345", "3456"}
                },
                new SocMapping()
                {
                    CorrelationId = new Guid("5172143a-b750-4284-b973-e1e0f8bacce2"),
                    SocMappingId = new Guid("5172143a-b750-4284-b973-e1e0f8bacce2"),
                    SocCode = "55555",
                    Standards = new List<string>() {"1234", "2345", "3456"},
                    Frameworks =
                        new List<string>() {"1234", "2345", "3456"}
                },
                new SocMapping()
                {
                    CorrelationId = new Guid("1ca7fa0d-5b91-4dcd-a772-d8604b1ce52f"),
                    SocMappingId = new Guid("1ca7fa0d-5b91-4dcd-a772-d8604b1ce52f"),
                    SocCode = "55556",
                    Standards = new List<string>() {"1234", "2345", "3456"},
                    Frameworks =
                        new List<string>() {"1234", "2345", "3456"}
                },
                new SocMapping()
                {
                    CorrelationId = new Guid("b0494ebd-6357-4b74-8ec3-f55770903d97"),
                    SocMappingId = new Guid("b0494ebd-6357-4b74-8ec3-f55770903d97"),
                    SocCode = "6666",
                    Standards = new List<string>() {"1234", "2345", "3456"},
                    Frameworks =
                        new List<string>() {"1234", "2345", "3456"}
                },
                new SocMapping()
                {
                    CorrelationId = new Guid("5197c47d-8911-4b59-9926-6bb662353d00"),
                    SocMappingId = new Guid("5197c47d-8911-4b59-9926-6bb662353d00"),
                    SocCode = "77777",
                    Standards = new List<string>() {"1234", "2345", "3456"},
                    Frameworks =
                        new List<string>() {"1234", "2345", "3456"}
                },

            };
            return mappingData;
        }
    }
}