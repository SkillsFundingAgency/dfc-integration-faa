using DFC.Integration.AVFeed.Data.Interfaces;
using DFC.Integration.AVFeed.Data.Models;
using System.Threading.Tasks;


namespace DFC.Integration.AVFeed.Function.ClearRecycleBin
{
    public class ClearRecycleBin : IClearRecycleBin
    {
        private readonly ICustomSitefinityAPIs customSitefinityAPIs;
        private readonly ICustomApiContextService customApiContextService;
        private readonly IApplicationLogger logger;
        private readonly IAuditService auditService;
        private readonly int numberToClear = 20;

        public ClearRecycleBin(ICustomSitefinityAPIs customSitefinityAPIs, IApplicationLogger logger, IAuditService auditService, ICustomApiContextService customApiContextService)
        {
            this.logger = logger;
            this.auditService = auditService;
            this.customSitefinityAPIs = customSitefinityAPIs;
            this.customApiContextService = customApiContextService;
        }

        public async Task ClearRecycleBinAsync()
        {
            logger.Info($"About to delete {numberToClear} vacancies from the recycle bin");

            //await customSitefinityAPIs.ClearAVsFromRecycleBinAsync(1);

            await customApiContextService.ClearAVsRecycleBinAsync(numberToClear);

            await auditService.AuditAsync($"Deleted {numberToClear} vacancies from the recycle bin");
          
            logger.Info("Completed deleting vacancies from the recycle bin");
        }
    }
}
