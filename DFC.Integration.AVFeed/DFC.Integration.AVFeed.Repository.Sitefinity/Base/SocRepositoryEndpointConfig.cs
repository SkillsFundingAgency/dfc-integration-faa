using System;
using System.Configuration;

namespace DFC.Integration.AVFeed.Repository.Sitefinity
{
    public class SocRepositoryEndpointConfig : IRepoEndpointConfig<SitefinitySocMapping>
    {
        private const string PROVIDER = "dynamicProvider2";
        private string _baseRequestEndpoint = ConfigurationManager.AppSettings.Get("Sitefinity.SocApiEndPoint");

        public Uri GetAllItemsEndpoint() => new Uri(ConfigurationManager.AppSettings["Sitefinity.SocMappingEndpoint"]);

        public Uri GetPostEndpoint() => throw new NotImplementedException();

        public Uri GetReferenceEndpoint(string id, string relatedField) => throw new NotImplementedException();

        public string GetSingleItemEndpoint(string id) => $"{_baseRequestEndpoint}({id})?provider={PROVIDER}";
    }
}
