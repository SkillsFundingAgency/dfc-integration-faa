using System.Collections.Generic;

namespace DFC.Integration.AVFeed.Data.Models
{
    public class FunctionResult<T>
    {
        public T Output { get; set; }
        public IEnumerable<string> AuditMessages { get; set; }
    }
}
