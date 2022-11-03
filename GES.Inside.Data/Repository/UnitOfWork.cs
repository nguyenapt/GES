using System;
using GES.Common.Exceptions;
using GES.Common.Logging;
using GES.Inside.Data.Repository.Interfaces;
using System.Data;
using System.Data.Entity;
using GES.Inside.Data.Exceptions;

namespace GES.Inside.Data.Repository
{
    /// <summary>
    /// The Entity Framework implementation of IUnitOfWork
    /// </summary>
    public class UnitOfWork<TContext> : IUnitOfWork<TContext> where TContext : DbContext, new()
    {
        /// <summary>
        /// The DbContext
        /// </summary>
        private TContext _dbContext;

        protected readonly IGesLogger Logger;

        /// <summary>
        /// Initializes a new instance of the UnitOfWork class.
        /// </summary>
        public UnitOfWork(TContext dbContext, IGesLogger logger)
        {
            _dbContext = dbContext;
            Logger = logger;
            _dbContext.Configuration.AutoDetectChangesEnabled = false;
            _dbContext.Configuration.ValidateOnSaveEnabled = false;
        }


        public TContext DbContext
        {
            get
            {
                return _dbContext;
            }
        }

        /// <summary>
        /// Saves all pending changes
        /// </summary>
        /// <returns>The number of objects in an Added, Modified, or Deleted state</returns>
        public int Commit()
        {
            try
            {
                // Save changes with the default options
                return this.Execute<int, DataException, GesDbException>(Logger, _dbContext.SaveChanges);
            }
            catch(Exception ex)
            {
                Logger?.Error(ex, "Error when commit the transaction that save the changes");

                // Write log
                throw new GesDbException("Can not be commit the changes. Check inner exception for detail.", ex);
            }
        }

        public void Dispose()
        {
       
        }
    }
}
