using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFC.Integration.AVFeed.Repository.Sitefinity.Base
{
    public interface ICustomApiConfig
    {
        int GetRecycleBinClearBatchSize();
        Uri GetClearRequestUrl();
        Uri GetBaseAddressUrl();
        Uri GetAuthCookieEndpointUrl();
    }
}
