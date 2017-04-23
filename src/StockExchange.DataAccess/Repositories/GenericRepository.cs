using EntityFramework.BulkInsert.Extensions;
using EntityFramework.Extensions;
using StockExchange.DataAccess.IRepositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace StockExchange.DataAccess.Repositories
{
    /// <summary>
    /// A generic class for operations on a database
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class GenericRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// Database context
        /// </summary>
        protected readonly StockExchangeModel Context;

        /// <summary>
        /// Database table representation
        /// </summary>
        protected readonly DbSet<TEntity> DbSet;

        /// <summary>
        /// Creates a new instance of <see cref="GenericRepository{TEntity}"/>
        /// </summary>
        public GenericRepository()
        {
            Context = new StockExchangeModel();
            DbSet = Context.Set<TEntity>();
        }

        /// <summary>
        /// Creates a new instance of <see cref="GenericRepository{TEntity}"/>
        /// </summary>
        /// <param name="context"></param>
        public GenericRepository(StockExchangeModel context)
        {
            Context = context;
            DbSet = context.Set<TEntity>();
        }

        /// <inheritdoc />
        public IQueryable<TEntity> GetQueryable() => DbSet;

        /// <inheritdoc />
        public void Insert(TEntity entity)
        {
            DbSet.Add(entity);
        }

        /// <inheritdoc />
        public void BulkInsert(IEnumerable<TEntity> entities)
        {
            Context.BulkInsert(entities);
        }

        /// <inheritdoc />
        public void Remove(TEntity entity)
        {
            DbSet.Remove(entity);
        }

        /// <inheritdoc />
        public Task<int> RemoveRange(IQueryable<TEntity> entities)
        {
            return entities.DeleteAsync();
        }

        /// <inheritdoc />
        public Task<int> Save()
        {
            return Context.SaveChangesAsync();
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                Context.Dispose();
            }
        }

        /// <inheritdoc />
        ~GenericRepository()
        {
            Dispose(false);
        }
    }
}
