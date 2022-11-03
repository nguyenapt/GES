using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using GES.Inside.Data.Repository.Interfaces;
using GES.Common.Logging;
using EntityFramework.BulkInsert.Extensions;
using GES.Common.Exceptions;
using System.Linq.Expressions;
using EntityFramework.Utilities;

namespace GES.Inside.Data.Repository
{
    public abstract class GenericRepository<T> : IGenericRepository<T>
      where T : class
    {
        protected DbContext _entities;
        protected readonly IDbSet<T> _dbset;
        protected readonly IGesLogger Logger;

        protected GenericRepository(DbContext context, IGesLogger logger)
        {
            _entities = context;
            _dbset = context.Set<T>();
            Logger = logger;
        }

        public virtual IEnumerable<T> GetAll()
        {

            return _dbset.AsEnumerable<T>();
        }

        public IEnumerable<T> FindBy(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {

            return _dbset.Where(predicate).AsEnumerable();
        }

        public virtual T Add(T entity)
        {
            return _dbset.Add(entity);
        }

        public virtual T Delete(T entity)
        {
            return _dbset.Remove(entity);
        }

        public virtual void Edit(T entity)
        {
            _entities.Entry(entity).State = EntityState.Modified;
        }

        public virtual void Save()
        {
            _entities.SaveChanges();
        }

        public void Add(IEnumerable<T> entities)
        {
            this._entities.BulkInsert(entities);
        }

        public T FindOn(Expression<Func<T, bool>> predicate)
        {
            return this._dbset.Where(predicate).FirstOrDefault();
        }

        /// <summary>
        /// Execute function and handle/throw the wrapper error
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="func">The function need to be invoked</param>
        /// <returns></returns>
        /// <exception cref="GesDbException"></exception>
        /// <exception cref="ArgumentNullException">Thrown when the func parameter is null</exception>
        protected virtual TResult SafeExecute<TResult>(Func<TResult> func) => this.Execute<TResult, System.Data.DataException, GesDbException>(Logger, func);

        /// <summary>
        /// Execute function with try/catch to throw the wrapper error if it happen
        /// </summary>
        /// <param name="action">The action need to be invoked</param>
        /// <exception cref="GesDbException"></exception>
        /// <exception cref="ArgumentNullException">Thrown when the action parameter is null</exception>
        protected virtual void SafeExecute(Action action) => this.Execute<System.Data.DataException, GesDbException>(Logger, action);

        public virtual void BatchDelete(Expression<Func<T, bool>> predicate)
        {
            EFBatchOperation.For(DbContext, DbContext.Set<T>())
                    .Where(predicate)
                    .Delete();
        }

        public DbContext DbContext => _entities;
    }
}
