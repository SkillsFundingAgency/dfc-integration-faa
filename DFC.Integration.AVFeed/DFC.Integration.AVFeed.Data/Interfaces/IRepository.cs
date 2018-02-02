using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DFC.Integration.AVFeed.Data.Interfaces
{
    public interface IRepository<T> where T : class
    {
        // Marks an entity as new
        T Add(T entity);
        Task<T> AddAsync(T entity);
        // Marks an entity as modified
        string Update(T entity);
        Task<string> UpdateAsync(T entity);
        // Marks an entity to be removed
        void Delete(T entity);
        Task DeleteAsync(T entity);
        void Delete(Expression<Func<T, bool>> where);
        Task DeleteAsync(Expression<Func<T, bool>> where);
        // Get an entity by int id
        T GetById(string id);
        Task<T> GetByIdAsync(string id);
        // Get an entity using delegate
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Get", Justification = "Default repository pattern, changing it doesnt make sense.")]
        T Get(Expression<Func<T, bool>> where);
        Task<T> GetAsync(Expression<Func<T, bool>> where);
        // Gets all entities of type T
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        IEnumerable<T> GetAll();
        Task<IEnumerable<T>> GetAllAsync();
        // Gets entities using delegate
        IEnumerable<T> GetMany(Expression<Func<T, bool>> where);
        Task<IEnumerable<T>> GetManyAsync(Expression<Func<T, bool>> where);
    }
}
