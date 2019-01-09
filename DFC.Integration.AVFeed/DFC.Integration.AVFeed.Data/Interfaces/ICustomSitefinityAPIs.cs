using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFC.Integration.AVFeed.Data.Interfaces
{
    public interface ICustomSitefinityAPIs
    {
        Task ClearAVsFromRecycleBinAsync(int numberToRemove);
    }
}
