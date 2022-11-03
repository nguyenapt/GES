using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using EntityFramework.BulkInsert.Extensions;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Repository.Interfaces;
using GES.Common.Logging;

namespace GES.Inside.Data.Repository
{
    public class I_GesCompanyWatcherRepository : GenericRepository<I_GesCompanyWatcher>, II_GesCompanyWatcherRepository
    {
        private DbContext _context;
        public I_GesCompanyWatcherRepository(GesEntities context, IGesLogger logger)
           : base(context, logger)
        {
            _context = context;
        }

        public I_GesCompanyWatcher GetById(long id)
        {
            return this.SafeExecute<I_GesCompanyWatcher>(() => _entities.Set<I_GesCompanyWatcher>().FirstOrDefault(d => d.I_GesCompaniesG_Individuals_Id == id));
        }

        public void AddBatch(List<I_GesCompanyWatcher> entities)
        {
            this.SafeExecute(() =>
            {
                _context.BulkInsert(entities);
                _context.SaveChanges();
            });
        }

    }
}
