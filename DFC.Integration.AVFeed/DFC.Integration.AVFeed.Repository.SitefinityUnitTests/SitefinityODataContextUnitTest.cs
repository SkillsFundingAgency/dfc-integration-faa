﻿using System;
using DFC.Integration.AVFeed.Repository.Sitefinity;
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
            A.CallTo(() => idatacontext.GetResult(new Uri("http://test.com"), A<bool>._))
                .Returns(DataHelper.GetDummyOdataResultSocMapping());
        }
    }
}