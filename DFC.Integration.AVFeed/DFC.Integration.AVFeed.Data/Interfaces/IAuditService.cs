using System.Collections.Generic;
using System.Threading.Tasks;

namespace DFC.Integration.AVFeed.Data.Interfaces
{
    public interface IAuditService
    {
        Task AuditAsync(string message);
        IEnumerable<string> GetAuditRecords();
    }
}
