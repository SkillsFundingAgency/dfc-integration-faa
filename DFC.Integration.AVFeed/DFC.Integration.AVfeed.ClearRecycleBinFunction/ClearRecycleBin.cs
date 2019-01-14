using DFC.Integration.AVFeed.Data.Interfaces;
using DFC.Integration.AVFeed.Data.Models;
using System.Threading.Tasks;


namespace DFC.Integration.AVFeed.Function.ClearRecycleBin
{
    public class ClearRecycleBin : IClearRecycleBin
    {
        private readonly ICustomApiContextService customApiContextService;
        private readonly IApplicationLogger logger;
        private readonly IAuditService auditService;
        private readonly int numberToClear = 20;

        public ClearRecycleBin(IApplicationLogger logger, IAuditService auditService, ICustomApiContextService customApiContextService)
        {
            this.logger = logger;
            this.auditService = auditService;
            this.customApiContextService = customApiContextService;
        }

        public async Task ClearRecycleBinAsync()
        {
            logger.Info($"About to clear recycle bin with a batch size of {RecycleBinClearBatchSize} over {RecycleBinClearRequestLoops} requests ");
            for (int ii = 0; ii < RecycleBinClearRequestLoops; ii++)
            {
                logger.Info($"About to request delete of {RecycleBinClearBatchSize} vacancies from the recycle bin");
                customApiContextService.ClearSomeAVsRecycleBinRecordsAsync(RecycleBinClearBatchSize);
                auditService.AuditAsync($"Deleted upto {RecycleBinClearBatchSize} vacancies from the recycle bin");
            }
            logger.Info("Completed deleting vacancies from the recycle bin");
        }
    }
}
