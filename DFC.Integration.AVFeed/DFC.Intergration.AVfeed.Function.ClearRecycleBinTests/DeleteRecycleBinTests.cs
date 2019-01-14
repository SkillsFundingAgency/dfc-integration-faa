using System;
using System.Net;
using DFC.Integration.AVFeed.Data.Interfaces;
using DFC.Integration.AVFeed.Function.ClearRecycleBin;
using DFC.Integration.AVFeed.Repository.Sitefinity.Base;
using FakeItEasy;
using Xunit;

namespace DFC.Integration.AVfeed.Function.ClearRecycleBinTests
{
    public class ClearRecycleBinTests
    {
        [Theory]
        [InlineData (1, 20)]
        public async void ClearRecycleBinAsyncTest(int numberRequestRequiredToClear, int recycleBinClearBatchSize)
        {
            //Setup
            var fakeCustomApiContextService = A.Fake<ICustomApiContextService>();
            var fakeTokenClient = A.Fake<ITokenClient>();
            var fakeApplicationLogger = A.Fake<IApplicationLogger>();
            var fakeAuditService = A.Fake<IAuditService>();
            var fakeCustomApiConfig = A.Fake<ICustomApiConfig>();

          
            A.CallTo(() => fakeCustomApiConfig.GetRecycleBinClearBatchSize()).Returns(recycleBinClearBatchSize);

            A.CallTo(() => fakeCustomApiContextService.DeleteAVsRecycleBinRecordsAsync(A<int>._)).Returns(HttpStatusCode.Continue).NumberOfTimes(numberRequestRequiredToClear).Then.Returns(HttpStatusCode.OK);

            var clearRecycleBin = new ClearRecycleBin(fakeApplicationLogger, fakeAuditService, fakeCustomApiContextService, fakeCustomApiConfig);

            //Act
            await clearRecycleBin.ClearRecycleBinAsync();

            //Asserts
            A.CallTo(() => fakeCustomApiContextService.DeleteAVsRecycleBinRecordsAsync(recycleBinClearBatchSize)).MustHaveHappened(numberRequestRequiredToClear + 1, Times.Exactly);
            A.CallTo(() => fakeAuditService.AuditAsync(A<string>._, A<string>._)).MustHaveHappened(numberRequestRequiredToClear + 1, Times.Exactly);
        }
    }
}
