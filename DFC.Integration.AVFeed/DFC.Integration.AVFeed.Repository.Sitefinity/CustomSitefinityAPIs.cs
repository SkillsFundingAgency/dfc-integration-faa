using DFC.Integration.AVFeed.Data.Interfaces;
using DFC.Integration.AVFeed.Repository.Sitefinity.Base;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;

namespace DFC.Integration.AVFeed.Repository.Sitefinity
{
    public class CustomSitefinityAPIs : ICustomSitefinityAPIs
    {
        private readonly IAPIContext APIContext;

        public CustomSitefinityAPIs(IAPIContext APIContext)
        {
            this.APIContext = APIContext;
        }
        public async Task ClearAVsFromRecycleBinAsync(int numberToRemove)
        {
            //local - beta.nationalcareersservice.org.uk / customapi / recyclebinclearappvacancies
            Uri ClearRequestURL = new Uri(ConfigurationManager.AppSettings.Get("Sitefinity.ClearAVBinURL"));
            var requestParameters = new Dictionary<string, string>();
            requestParameters.Add("itemCount", "1");
            await APIContext.PostAsync(ClearRequestURL, requestParameters);
        }
    }
}
