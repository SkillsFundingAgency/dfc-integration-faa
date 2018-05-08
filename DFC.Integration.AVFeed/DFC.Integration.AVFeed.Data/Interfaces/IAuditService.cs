using System.Collections.Generic;
using System.Threading.Tasks;

namespace DFC.Integration.AVFeed.Data.Interfaces
{
    public interface IAuditService
    {
        Task AuditAsync(string outputMessage, string inputMessage = null);
        IEnumerable<string> GetAuditRecords();
    }
}
