using System;
using System.Collections.Generic;

namespace DFC.Integration.AVFeed.Data.Models
{
    public class AuditRecord<TIn, TOut>
    {
        public Guid Id => Guid.NewGuid();
        public Guid CorrelationId { get; set; }
        public string Application => "DFC.Integration.AVFeed";
        public string Function { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime EndedAt { get; set; }
        public TIn Input { get; set; }
        public TOut Output { get; set; }
        public IEnumerable<string> Audit { get; set; }
    }
}
