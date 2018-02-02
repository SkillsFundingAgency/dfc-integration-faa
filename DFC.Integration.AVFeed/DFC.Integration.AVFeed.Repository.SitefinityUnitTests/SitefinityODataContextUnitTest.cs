using System;
using DFC.Integration.AVFeed.Repository.Sitefinity;
using DFC.Integration.AVFeed.Repository.Sitefinity.Model;
using FakeItEasy;
using Xunit;

namespace DFC.Integration.AVFeed.Repository.SitefinityUnitTests
{
    public class SitefinityODataContextUnitTest
    {
        [Fact]
        public void get_odatacontext_result_for_socmappings()
        {
            var idatacontext = A.Fake<IOdataContext<SitefinitySocMapping>>();

            A.CallTo(() => idatacontext.GetHttpClientAsync()).Returns(new System.Net.Http.HttpClient());
            A.CallTo(() => idatacontext.GetResult(new Uri("http://test.com")))
                .Returns(DataHelper.GetDummyOdataResultSocMapping());

            //var v = idatacontext.GetResult(new Uri("http://test.com")).Result;
            //    Assert.Equal(DataHelper.GetDummyOdataResultSocMapping(),idatacontext.GetResult(new Uri("http://test.com")).Result);
        }
    }
}