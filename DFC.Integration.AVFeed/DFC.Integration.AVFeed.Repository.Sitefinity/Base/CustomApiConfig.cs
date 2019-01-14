using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFC.Integration.AVFeed.Repository.Sitefinity.Base
{
    public class CustomApiConfig : ICustomApiConfig
    {
        public Uri GetAuthCookieEndpointUrl() => new Uri(ConfigurationManager.AppSettings.Get("Sitefinity.AuthCookieEndpoint"));
    
        public Uri  GetBaseAddressUrl() => new Uri(ConfigurationManager.AppSettings.Get("Sitefinity.BaseAddress"));
       
        public Uri GetClearRequestUrl() => new Uri(ConfigurationManager.AppSettings.Get("Sitefinity.ClearAVBinURL"));

        public int GetRecycleBinClearBatchSize() => int.Parse(ConfigurationManager.AppSettings.Get("Sitefinity.RecycleBinClearBatchSize"));
    }
}
