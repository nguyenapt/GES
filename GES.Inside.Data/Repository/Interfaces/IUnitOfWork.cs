using System;
using System.Data.Entity;
using GES.Inside.Data.Models;

namespace GES.Inside.Data.Repository.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Saves all pending changes
        /// </summary>
        /// <returns>The number of objects in an Added, Modified, or Deleted state</returns>
        int Commit();
    }

    public interface IUnitOfWork<TContext> : IUnitOfWork
        where TContext : DbContext, IDisposable
    {        
        TContext DbContext { get; }
    }
}
