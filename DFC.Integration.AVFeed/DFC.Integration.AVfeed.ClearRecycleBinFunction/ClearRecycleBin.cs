using DFC.Integration.AVFeed.Data.Interfaces;
using DFC.Integration.AVFeed.Data.Models;
using System.Threading.Tasks;


namespace DFC.Integration.AVFeed.Function.ClearRecycleBin
{
    public class ClearRecycleBin : IClearRecycleBin
    {
        private readonly ICustomSitefinityAPIs customSitefinityAPIs;
        private readonly IApplicationLogger logger;
        private readonly IAuditService auditService;
        private readonly int numberToClear = 1;

        public ClearRecycleBin(ICustomSitefinityAPIs customSitefinityAPIs, IApplicationLogger logger, IAuditService auditService)
        {
            this.logger = logger;
            this.auditService = auditService;
            this.customSitefinityAPIs = customSitefinityAPIs;
        }

        public async Task ClearRecycleBinAsync()
        {
            logger.Info($"About to delete {numberToClear} vacancies from the recycle bin");

            await customSitefinityAPIs.ClearAVsFromRecycleBinAsync(1);

            await auditService.AuditAsync($"Deleted {numberToClear} vacancies from the recycle bin");
          
            logger.Info("Completed deleting vacancies from the recycle bin");
        }
    }
}
