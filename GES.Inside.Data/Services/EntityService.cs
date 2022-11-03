using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Runtime.Caching;
using GES.Inside.Data.Configs;
using GES.Inside.Data.Repository.Interfaces;
using GES.Inside.Data.Services.Interfaces;
using Z.EntityFramework.Plus;
using GES.Common.Logging;
using GES.Common.Exceptions;

namespace GES.Inside.Data.Services
{
    public abstract class EntityService<TU, T> : IEntityService<T> where TU : DbContext where T : class  
    {
        protected readonly IUnitOfWork UnitOfWork;
        protected readonly IGenericRepository<T> _repository;
        protected readonly IGesLogger Logger;

        public EntityService(IUnitOfWork unitOfWork, IGesLogger logger, IGenericRepository<T> repository)
        {
            UnitOfWork = unitOfWork;
            _repository = repository;
            this.Logger = logger;

            // Cache expiration rule
            var options = new CacheItemPolicy() { AbsoluteExpiration = DateTimeOffset.UtcNow.AddMinutes(Settings.CacheExpiredAfterMin)};
            QueryCacheManager.DefaultCacheItemPolicy = options;
            
        }

        public virtual void Add(T entity, bool isExecuteImmediate = false)
        {
            Guard.AgainstNullArgument(nameof(entity), entity);

            _repository.Add(entity);
            if (isExecuteImmediate)
            {
                this.Save();
            }
        }

        public virtual void Update(T entity, bool isExecuteImmediate = false)
        {
            Guard.AgainstNullArgument(nameof(entity), entity);

            _repository.Edit(entity);
            if (isExecuteImmediate)
            {
                this.Save();
            }
        }

        public virtual void Delete(T entity,bool isExecuteImmediate = false)
        {
            Guard.AgainstNullArgument(nameof(entity), entity);

            _repository.Delete(entity);
            if (isExecuteImmediate)
            {
                this.Save();
            }
        }

        public virtual IEnumerable<T> GetAll()
        {
            return this.SafeExecute<IEnumerable<T>>(() => _repository.GetAll());
        }

        public virtual int Save()
        {
            return this.SafeExecute<int>(UnitOfWork.Commit);
        }

        /// <summary>
        /// Safe handle exception when 
        /// </summary>
        /// <param name="action"></param>
        protected virtual void SafeExecute(Action action)
        {
            Guard.AgainstNullArgument(nameof(action), action);
            Guard.AgainstNullArgumentProperty(nameof(action), "Method", action.Method);

            try
            {
                action();
            }
            catch (GesDbException ex)
            {
                // Write log if need
                Logger?.Error(ex, $"Exception when invoke action {action.Method.Name} in {this.GetType().Name}.");

                // Re-throw exception
                throw new GesServiceException($"Error when execute function {action.Method.Name}", ex);
            }
            catch (System.Data.DataException ex) // Catch when executing the linq happen the error
            {
                // Write log if need
                Logger?.Error(ex, $"Exception when invoke action {action.Method.Name} in {this.GetType().Name}.");

                // Re-throw exception
                throw new GesServiceException($"Error when execute function {action.Method.Name}", ex);
            }
        }
        protected virtual TResult SafeExecute<TResult>(Func<TResult> func)
        {
            Guard.AgainstNullArgument(nameof(func), func);
            Guard.AgainstNullArgumentProperty(nameof(func), "Method", func.Method);

            try
            {
                return func();
            }
            catch (GesDbException ex)
            {
                // Write log if need
                Logger?.Error(ex, $"Exception when invoke action {func.Method.Name} in {this.GetType().Name}.");

                // Re-throw exception
                throw new GesServiceException($"Error when execute function {func.Method.Name}", ex);
            }
            catch(System.Data.DataException ex)
            {
                // Write log if need
                Logger?.Error(ex, $"Exception when invoke action {func.Method.Name} in {this.GetType().Name}.");

                // Re-throw exception
                throw new GesServiceException($"Error when execute function {func.Method.Name}", ex);
            }
        }
    }
}
