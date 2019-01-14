﻿using System;
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
         
            var clearRecycleBin = new ClearRecycleBin(fakeApplicationLogger, fakeAuditService, fakeCustomApiContextService);

            //Act
            await clearRecycleBin.ClearRecycleBinAsync();

            //Asserts
            A.CallTo(() => fakeCustomApiContextService.ClearSomeAVsRecycleBinRecordsAsync(RecycleBinClearBatchSize)).MustHaveHappened(RecycleBinClearRequestLoops, Times.Exactly);
            A.CallTo(() => fakeAuditService.AuditAsync(A<string>._, A<string>._)).MustHaveHappened(RecycleBinClearRequestLoops, Times.Exactly);

        }
    }
}
