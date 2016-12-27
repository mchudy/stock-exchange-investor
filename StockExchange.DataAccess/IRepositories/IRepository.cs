using System;
using System.Collections.Generic;
using System.Linq;
using StockExchange.DataAccess.Models;

namespace StockExchange.DataAccess.IRepositories
{
    public interface IRepository<TEntity> : IDisposable where TEntity : class
    {
        IQueryable<TEntity> GetQueryable();

        void Insert(TEntity entity);

        void BulkInsert(IEnumerable<TEntity> entities);

        void Remove(TEntity entity);

        int Save();
    }
}
