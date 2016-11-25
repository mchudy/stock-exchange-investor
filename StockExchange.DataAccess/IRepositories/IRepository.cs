using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace StockExchange.DataAccess.IRepositories
{
    public interface IRepository<TEntity> : IDisposable where TEntity : class
    {
        IQueryable<TEntity> GetQueryable(Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            List<Expression<Func<TEntity, object>>> includeProperties = null,
            int? page = null, int? pageSize = null);

        void Insert(TEntity entity);
        void BulkInsert(IEnumerable<TEntity> entities);

        int Save();
    }
}
