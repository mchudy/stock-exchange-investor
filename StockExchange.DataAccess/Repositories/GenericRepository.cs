﻿using EntityFramework.BulkInsert.Extensions;
using StockExchange.DataAccess.IRepositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

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

        public IQueryable<TEntity> GetQueryable() => DbSet;

        public void Insert(TEntity entity)
        {
            DbSet.Add(entity);
        }

        public void BulkInsert(IEnumerable<TEntity> entities)
        {
            Context.BulkInsert(entities);
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
