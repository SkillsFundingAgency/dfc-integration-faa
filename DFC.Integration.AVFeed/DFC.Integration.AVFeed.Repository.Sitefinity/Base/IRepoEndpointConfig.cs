using System;

namespace DFC.Integration.AVFeed.Repository.Sitefinity
{
    public interface IRepoEndpointConfig<T> where T : class, new()
    {
        Uri GetAllItemsEndpoint();
        string GetSingleItemEndpoint(string id);
        Uri GetReferenceEndpoint(string id, string relatedField);
        Uri GetPostEndpoint();
    }
}