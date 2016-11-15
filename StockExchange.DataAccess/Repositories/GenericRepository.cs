using StockExchange.DataAccess.IRepositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace StockExchange.DataAccess.Repositories
{
    public class GenericRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly StockExchangeModel Context;
        protected readonly IDbSet<TEntity> DbSet;

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

        public IQueryable<TEntity> GetQueryable(Expression<Func<TEntity, bool>> filter = null,
           Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
           List<Expression<Func<TEntity, object>>> includeProperties = null,
           int? page = null, int? pageSize = null)
        {
            IQueryable<TEntity> query = DbSet;
            includeProperties?.ForEach(i => { query = query.Include(i); });
            if (filter != null)
                query = query.Where(filter);
            if (orderBy != null)
                query = orderBy(query);
            if (page != null && pageSize != null)
                query = query.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);
            return query;
        }

        public void Insert(TEntity entity)
        {
            DbSet.Add(entity);
        }

        public int Save()
        {
            return Context.SaveChanges();
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
