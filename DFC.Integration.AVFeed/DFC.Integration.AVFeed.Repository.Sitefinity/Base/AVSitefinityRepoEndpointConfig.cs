﻿
using System;
using System.Configuration;

namespace DFC.Integration.AVFeed.Repository.Sitefinity
{
    public class AVSitefinityRepoEndpointConfig : IRepoEndpointConfig<SfApprenticeshipVacancy>
    {
        private const string PROVIDER = "dynamicProvider2";
        private string _baseRequestEndpoint = ConfigurationManager.AppSettings.Get("Sitefinity.AVApiEndPoint");
        public Uri GetAllItemsEndpoint() => new Uri($"{_baseRequestEndpoint}?$expand=SOCCode&$orderby=Title&sf_provider={PROVIDER}");

        public string GetSingleItemEndpoint(string id) => $"{_baseRequestEndpoint}({id})?sf_provider={PROVIDER}";

        public Uri GetReferenceEndpoint(string id, string relatedField) => new Uri($"{_baseRequestEndpoint}({id})/{relatedField}/$ref?sf_provider={PROVIDER}");

        public Uri GetPostEndpoint() => new Uri($"{_baseRequestEndpoint}?sf_provider={PROVIDER}");

        public Uri GetPublishEndpoint(string id) => new Uri($"{_baseRequestEndpoint}({id})/operation?sf_provider={PROVIDER}");
    }
}
