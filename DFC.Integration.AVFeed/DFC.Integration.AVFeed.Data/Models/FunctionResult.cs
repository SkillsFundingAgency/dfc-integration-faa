using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFC.Integration.AVFeed.Data.Models
{
    public class FunctionResult<T>
    {
        public T Output { get; set; }
        public IEnumerable<string> AuditMessages { get; set; }
    }
}
