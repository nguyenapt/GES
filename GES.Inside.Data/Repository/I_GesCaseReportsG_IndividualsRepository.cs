using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using EntityFramework.BulkInsert.Extensions;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Repository.Interfaces;
using GES.Common.Logging;

namespace GES.Inside.Data.Repository
{
    public class I_GesCaseReportsG_IndividualsRepository : GenericRepository<I_GesCaseReportsG_Individuals>, II_GesCaseReportsG_IndividualsRepository
    {
        private DbContext _context;
        public I_GesCaseReportsG_IndividualsRepository(GesEntities context, IGesLogger logger)
           : base(context, logger)
        {
            _context = context;
        }

        public I_GesCaseReportsG_Individuals GetById(long id)
        {
            return this.SafeExecute<I_GesCaseReportsG_Individuals>(() => _entities.Set<I_GesCaseReportsG_Individuals>().FirstOrDefault(d => d.I_GesCaseReportsG_Individuals_Id == id));
        }


        public void AddBatch(List<I_GesCaseReportsG_Individuals> entities)
        {
            this.SafeExecute(() =>
            {
                _context.BulkInsert(entities);
                _context.SaveChanges();
            });
        }
    }
}
