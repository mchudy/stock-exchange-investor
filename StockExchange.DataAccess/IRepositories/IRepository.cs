using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockExchange.DataAccess.IRepositories
{
    /// <summary>
    /// A generic interface for operations on a database
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IRepository<TEntity> : IDisposable where TEntity : class
    {
        /// <summary>
        /// Returns the query 
        /// </summary>
        /// <returns></returns>
        IQueryable<TEntity> GetQueryable();

        /// <summary>
        /// Inserts entity into the database
        /// </summary>
        /// <param name="entity">The entity to insert</param>
        void Insert(TEntity entity);
        
        /// <summary>
        /// Inserts multiple entities in a single SQL statement
        /// </summary>
        /// <param name="entities">Entities to insert</param>
        void BulkInsert(IEnumerable<TEntity> entities);

        /// <summary>
        /// Removes entity from the database
        /// </summary>
        /// <param name="entity">Entity to remove</param>
        void Remove(TEntity entity);

        /// <summary>
        /// Removes multiple entities from the database
        /// </summary>
        /// <param name="entities">Entities to remove</param>
        /// <returns>Number of records removed</returns>
        Task<int> RemoveRange(IQueryable<TEntity> entities);

        /// <summary>
        /// Saves changes
        /// </summary>
        Task<int> Save();
    }
}
