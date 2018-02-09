using DFC.Integration.AVFeed.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFC.Integration.AVFeed.AuditService
{
    public class InMemoryAuditService : IAuditService
    {
        private List<string> _auditService = new List<string>();
        public async Task AuditAsync(string message)
        {
            await Task.FromResult(0);
            _auditService.Add($"{DateTime.Now}|{message}");
        }

        public IEnumerable<string> GetAuditRecords() => _auditService;
    }
}
