using DFC.Integration.AVFeed.Data.Interfaces;
using DFC.Integration.AVFeed.Data.Models;
using DFC.Integration.AVFeed.Repository.Sitefinity.Base;
using System.Configuration;
using System.Net;
using System.Threading.Tasks;


namespace DFC.Integration.AVFeed.Function.ClearRecycleBin
{
    public class ClearRecycleBin : IClearRecycleBin
    {
        private readonly ICustomApiContextService customApiContextService;
        private readonly IApplicationLogger logger;
        private readonly IAuditService auditService;
        private readonly ICustomApiConfig customApiConfig;

        public ClearRecycleBin(IApplicationLogger logger, IAuditService auditService, ICustomApiContextService customApiContextService, ICustomApiConfig customApiConfig)
        {
            this.logger = logger;
            this.auditService = auditService;
            this.customApiContextService = customApiContextService;
            this.customApiConfig = customApiConfig;
        }

        public async Task ClearRecycleBinAsync()
        {
            var RecycleBinClearBatchSize = customApiConfig.GetRecycleBinClearBatchSize();
            logger.Trace($"About to clear all vacancies from the recycle bin with a batch size of {RecycleBinClearBatchSize}");
            var continueDeleting = HttpStatusCode.PartialContent;
            while (continueDeleting == HttpStatusCode.PartialContent)
            {
                logger.Trace($"About to request delete of {RecycleBinClearBatchSize} vacancies from the recycle bin");
                continueDeleting =  await customApiContextService.DeleteAVsRecycleBinRecordsAsync(RecycleBinClearBatchSize);
                logger.Trace($"Deleted {RecycleBinClearBatchSize} vacancies from the recycle bin return status {continueDeleting}.");
            }
            logger.Trace("Completed deleting all vacancies from the recycle bin");
        }
    }
}
