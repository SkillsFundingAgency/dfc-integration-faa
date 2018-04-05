using DFC.Integration.AVFeed.Data.Interfaces;
using DFC.Integration.AVFeed.Data.Models;
using Microsoft.Azure.WebJobs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DFC.Integration.AVFeed.AuditService
{
    public class CosmosAuditService : IAuditService
    {
        private readonly IAsyncCollector<AuditRecord<object, object>> auditRecordCollector;
        private readonly AuditRecord<object, object> masterRecord;

        public CosmosAuditService(IAsyncCollector<AuditRecord<object, object>> auditRecordCollector, AuditRecord<object, object> record)
        {
            this.auditRecordCollector = auditRecordCollector;
            this.masterRecord = record;
        }
        public async Task AuditAsync(string message)
        {
            await auditRecordCollector.AddAsync(new AuditRecord<object, object>
            {
                CorrelationId = masterRecord.CorrelationId,
                StartedAt = masterRecord.StartedAt,
                EndedAt = DateTime.Now,
                Function = masterRecord.Function,
                Input = string.Empty,
                Output = message
            });
        }

        public IEnumerable<string> GetAuditRecords() => throw new NotImplementedException();
    }
}
