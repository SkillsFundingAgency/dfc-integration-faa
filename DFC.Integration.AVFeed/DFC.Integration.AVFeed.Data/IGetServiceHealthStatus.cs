using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFC.Integration.AVFeed.Data
{
    public interface IGetServiceHealthStatus
    {

        Task<dynamic> GetHealthStatusInfo();
    }
}
