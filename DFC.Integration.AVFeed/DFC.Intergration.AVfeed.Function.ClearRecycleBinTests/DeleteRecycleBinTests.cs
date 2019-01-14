using System;
using System.Configuration;
using DFC.Integration.AVFeed.Data.Interfaces;
using DFC.Integration.AVFeed.Function.ClearRecycleBin;
using FakeItEasy;
using Xunit;

namespace DFC.Integration.AVfeed.Function.ClearRecycleBinTests
{
    public class ClearRecycleBinTests
    {
        [Fact]
        public void ClearRecycleBinAsyncTest()
        {
            var RecycleBinClearBatchSize = int.Parse(ConfigurationManager.AppSettings.Get("Sitefinity.RecycleBinClearBatchSize"));
            var RecycleBinClearRequestLoops = int.Parse(ConfigurationManager.AppSettings.Get("Sitefinity.RecycleBinClearRequestLoops"));
        
            //Setup
            var fakeCustomApiContextService = A.Fake<ICustomApiContextService>();
            var fakeApplicationLogger = A.Fake<IApplicationLogger>();
            var fakeAuditService = A.Fake<IAuditService>();
         
            var clearRecycleBin = new ClearRecycleBin(fakeApplicationLogger, fakeAuditService, fakeCustomApiContextService);

            //Act
            clearRecycleBin.ClearRecycleBinAVs();

            //Asserts
            A.CallTo(() => fakeCustomApiContextService.ClearSomeAVsRecycleBinRecordsAsync(RecycleBinClearBatchSize)).MustHaveHappened(RecycleBinClearRequestLoops, Times.Exactly);
            A.CallTo(() => fakeAuditService.AuditAsync(A<string>._, A<string>._)).MustHaveHappened(RecycleBinClearRequestLoops, Times.Exactly);

        }
    }
}
