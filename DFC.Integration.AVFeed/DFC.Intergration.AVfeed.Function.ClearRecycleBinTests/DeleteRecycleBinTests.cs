using System;
using DFC.Integration.AVFeed.Data.Interfaces;
using DFC.Integration.AVFeed.Function.ClearRecycleBin;
using FakeItEasy;
using Xunit;

namespace DFC.Integration.AVfeed.Function.ClearRecycleBinTests
{
    public class ClearRecycleBinTests
    {
        [Fact]
        public async void ClearRecycleBinAsyncTest()
        {
            //Setup
            var fakeCustomApiContextService = A.Fake<ICustomApiContextService>();
            var fakeTokenClient = A.Fake<ITokenClient>();
            var fakeApplicationLogger = A.Fake<IApplicationLogger>();
            var fakeAuditService = A.Fake<IAuditService>();

            //A.CallTo(() => fakeCustomApiContextService.ClearAVsRecycleBinAsync()).
         
            var clearRecycleBin = new ClearRecycleBin(fakeApplicationLogger, fakeAuditService, fakeCustomApiContextService);

            //Act
            await clearRecycleBin.ClearRecycleBinAsync();

            //Asserts
            A.CallTo(() => fakeCustomApiContextService.ClearAVsRecycleBinAsync(A<int>._)).MustHaveHappened();

        }
    }
}
