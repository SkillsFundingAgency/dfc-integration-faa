using DFC.Integration.AVFeed.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using Newtonsoft.Json;
using System.Net.Http;

namespace DFC.Integration.AVFeed.Repository.Sitefinity.Base
{
    public class SitefinityRepository<T> : IRepository<T> where T : class, new()
    {
        protected IOdataContext<T> OdataContext { get; }
        protected IRepoEndpointConfig<T> RepoEndpointConfig { get; }

        public SitefinityRepository(IOdataContext<T> odataContext, IRepoEndpointConfig<T> repoEndpointConfig)
        {
            this.OdataContext = odataContext;
            this.RepoEndpointConfig = repoEndpointConfig;
        }
        public virtual T Add(T entity)
        {
            throw new NotImplementedException();
        }

        public virtual async Task<T> AddAsync(T entity)
        {
            var endpoint = RepoEndpointConfig.GetPostEndpoint();
            var jsonContent = await OdataContext.PostAsync(endpoint, entity);
            var addedEntity = JsonConvert.DeserializeObject<T>(jsonContent);
            return addedEntity;
        }

        public virtual void Delete(T entity)
        {
            throw new NotImplementedException();
        }

        public virtual void Delete(Expression<Func<T, bool>> where)
        {
            throw new NotImplementedException();
        }

        public virtual Task DeleteAsync(T entity)
        {
            throw new NotImplementedException();
        }

        public virtual Task DeleteAsync(Expression<Func<T, bool>> where)
        {
            throw new NotImplementedException();
        }

        public virtual T Get(Expression<Func<T, bool>> where)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<T> GetAll()
        {
            throw new NotImplementedException();
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            bool hasNextPage = false;
            Uri nextPage = RepoEndpointConfig.GetAllItemsEndpoint();
            List<T> sitefinitySocMapping = new List<T>();

            do
            {
                var result = await OdataContext.GetResult(nextPage);
                sitefinitySocMapping.AddRange(result.Value);
                hasNextPage = result.HasNextPage;
                if (hasNextPage)
                {
                    nextPage = new Uri(result.NextLink);
                }

            } while (hasNextPage);

            return sitefinitySocMapping;
        }

        public virtual Task<T> GetAsync(Expression<Func<T, bool>> where)
        {
            throw new NotImplementedException();
        }

        public virtual async Task<T> GetByIdAsync(string id)
        {
            using (var client = await OdataContext.GetHttpClientAsync())
            {
                var result = await client.GetStringAsync((RepoEndpointConfig.GetSingleItemEndpoint(id)));
                return JsonConvert.DeserializeObject<T>(result);
            }
        }

        public virtual IEnumerable<T> GetMany(Expression<Func<T, bool>> where)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IEnumerable<T>> GetManyAsync(Expression<Func<T, bool>> where)
        {
            throw new NotImplementedException();
        }

        public virtual string Update(T entity)
        {
            throw new NotImplementedException();
        }

        public virtual Task<string> UpdateAsync(T entity)
        {
            throw new NotImplementedException();
        }

        public T GetById(string id)
        {
            throw new NotImplementedException();
        }
    }
}
