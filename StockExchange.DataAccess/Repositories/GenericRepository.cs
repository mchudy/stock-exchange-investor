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
    public class GenericRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly StockExchangeModel Context;
        protected readonly DbSet<TEntity> DbSet;

        public GenericRepository()
        {
            Context = new StockExchangeModel();
            DbSet = Context.Set<TEntity>();
        }

        public GenericRepository(StockExchangeModel context)
        {
            Context = context;
            DbSet = context.Set<TEntity>();
        }

        public IQueryable<TEntity> GetQueryable() => DbSet;

        public void Insert(TEntity entity)
        {
            DbSet.Add(entity);
        }

        public void BulkInsert(IEnumerable<TEntity> entities)
        {
            Context.BulkInsert(entities);
        }

        public void Remove(TEntity entity)
        {
            DbSet.Remove(entity);
        }

        public Task<int> RemoveRange(IQueryable<TEntity> entities)
        {
            return entities.DeleteAsync();
        }

        public Task<int> Save()
        {
            return Context.SaveChangesAsync();
        }

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
        ~GenericRepository()
        {
            Dispose(false);
        }
    }
}
