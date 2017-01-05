using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockExchange.DataAccess.IRepositories
{
    public interface IRepository<TEntity> : IDisposable where TEntity : class
    {
        IQueryable<TEntity> GetQueryable();

        void Insert(TEntity entity);

        void BulkInsert(IEnumerable<TEntity> entities);

        void Remove(TEntity entity);

        Task<int> RemoveRange(IQueryable<TEntity> entities);

        Task<int> Save();
    }
}
