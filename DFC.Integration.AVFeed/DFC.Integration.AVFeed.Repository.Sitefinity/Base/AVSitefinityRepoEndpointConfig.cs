using DFC.Integration.AVFeed.Repository.Sitefinity.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFC.Integration.AVFeed.Repository.Sitefinity.Base
{
    public class AVSitefinityRepoEndpointConfig : IRepoEndpointConfig<SfApprenticeshipVacancy>
    {
        private const string PROVIDER = "dynamicProvider2";
        private string _baseRequestEndpoint = ConfigurationManager.AppSettings.Get("Sitefinity.AVApiEndPoint");
        public Uri GetAllItemsEndpoint() => new Uri($"{_baseRequestEndpoint}?$expand=SOCCode&$orderby=Title&sf_provider={PROVIDER}");

        public string GetSingleItemEndpoint(string id) => $"{_baseRequestEndpoint}({id})?sf_provider={PROVIDER}";

        public Uri GetReferenceEndpoint(string id, string relatedField) => new Uri($"{_baseRequestEndpoint}({id})/{relatedField}/$ref?sf_provider={PROVIDER}");

        public Uri GetPostEndpoint() => new Uri($"{_baseRequestEndpoint}?sf_provider={PROVIDER}");
    }
}
