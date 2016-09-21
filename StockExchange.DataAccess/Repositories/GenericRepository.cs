using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using StockExchange.DataAccess.IRepositories;
using System.Linq.Expressions;

namespace StockExchange.DataAccess.Repositories
{
    public sealed class GenericRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly StockExchangeModel _context;
        private readonly IDbSet<TEntity> _dbSet;

        public GenericRepository()
        {
            _context = new StockExchangeModel();
            _dbSet = _context.Set<TEntity>();
        }

        public GenericRepository(StockExchangeModel context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        public IQueryable<TEntity> GetQueryable(Expression<Func<TEntity, bool>> filter = null,
           Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
           List<Expression<Func<TEntity, object>>> includeProperties = null,
           int? page = null, int? pageSize = null)
        {
            IQueryable<TEntity> query = _dbSet;
            includeProperties?.ForEach(i => { query = query.Include(i); });
            if (filter != null)
                query = query.Where(filter);
            if (orderBy != null)
                query = orderBy(query);
            if (page != null && pageSize != null)
                query = query.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);
            return query;
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
                _context.Dispose();
            }
        }

        ~GenericRepository()
        {
            Dispose(false);
        }
    }
}
