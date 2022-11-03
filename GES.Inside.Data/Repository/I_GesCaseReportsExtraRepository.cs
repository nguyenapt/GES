using System.Collections.Generic;
using System.Data.Entity;
using EntityFramework.BulkInsert.Extensions;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Repository.Interfaces;
using GES.Common.Logging;

namespace GES.Inside.Data.Repository
{
    public class I_GesCaseReportsExtraRepository : GenericRepository<I_GesCaseReportsExtra>, II_GesCaseReportsExtraRepository
    {
        private DbContext _context;

        public I_GesCaseReportsExtraRepository(GesEntities context, IGesLogger logger)
           : base(context, logger)
        {
            _context = context;
        }

        public void AddBatch(List<I_GesCaseReportsExtra> entities)
        {
            this.SafeExecute(() =>
            {
                _context.BulkInsert(entities);
                _context.SaveChanges();
            });
        }
    }
}
