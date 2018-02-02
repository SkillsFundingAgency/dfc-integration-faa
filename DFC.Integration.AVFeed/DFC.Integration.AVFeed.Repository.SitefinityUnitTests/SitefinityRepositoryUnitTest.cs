using DFC.Integration.AVFeed.Repository.Sitefinity;
using DFC.Integration.AVFeed.Repository.Sitefinity.Model;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Net.Http;
using Xunit;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace DFC.Integration.AVFeed.Repository.SitefinityUnitTests
{
    public class SitefinityRepositoryUnitTest
    {
        OdataResult<SitefinitySocMapping> mappingData = null;

        [Fact]
        public void TestMethod1()
        {
        }

        [TestInitialize]
        public void InitializeDummyData()
        {
           mappingData = new OdataResult<SitefinitySocMapping>()
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
        }
        [Fact]
        public void GetSocMappingTest()
        {
            InitializeDummyData();
            var odataclient = A.Fake<IOdataContext<SitefinitySocMapping>>();

            Uri requestUri = new Uri("http://TestUri.gov.uk");
            var httpClient=A.CallTo(() => odataclient.GetHttpClient()).Returns(new HttpClient());
            A.CallTo(() => odataclient.GetResult(requestUri)).Returns(new OdataResult<SitefinitySocMapping>()
            {
                MetaData=
                    "http://local-beta.nationalcareersservice.org.uk/api/das-integration/$metadata#jobprofilesocs(NavigateToApprenticeshipStandard(UrlName),NavigateToApprenticeshipFramework(UrlName))",
                NextLink= "http://local-beta.nationalcareersservice.org.uk/api/das-integration/jobprofilesocs?$expand=NavigateToApprenticeshipStandard(%24select%3DUrlName)%2CNavigateToApprenticeshipFramework(%24select%3DUrlName)&$skip=50",
                Value=new List<SitefinitySocMapping>()
                {
                    new SitefinitySocMapping()
                    {
                        SocCode ="1234",
                        Frameworks =new List<NavigateToApprenticeshipFramework>()
                        {
                            new NavigateToApprenticeshipFramework() {UrlName = "1234"},
                            new NavigateToApprenticeshipFramework() {UrlName = "2222"},
                            new NavigateToApprenticeshipFramework() {UrlName = "3333"},
                            new NavigateToApprenticeshipFramework() {UrlName = "4444"},

                        },
                        Standards=new List<NavigateToApprenticeshipStandard>()
                        {
                            new NavigateToApprenticeshipStandard(){UrlName="22334"},
                            new NavigateToApprenticeshipStandard(){UrlName="3333"},
                            new NavigateToApprenticeshipStandard(){UrlName="4444"}
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
            });
            Assert.AreEqual(mappingData, odataclient.GetResult(requestUri));
        }
    }
}
