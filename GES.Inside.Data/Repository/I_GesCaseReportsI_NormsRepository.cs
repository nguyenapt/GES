using System;
using System.Data.Entity;
using System.Linq;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Repository.Interfaces;
using GES.Common.Logging;
using System.Collections.Generic;
using GES.Inside.Data.Models;

namespace GES.Inside.Data.Repository
{
    public class I_GesCaseReportsI_NormsRepository : GenericRepository<I_GesCaseReportsI_Norms>, II_GesCaseReportsI_NormsRepository
    {
        public I_GesCaseReportsI_NormsRepository(GesEntities dbContext, IGesLogger logger)
            : base(dbContext, logger)
        {
        }

        public IEnumerable<I_GesCaseReportsI_Norms> GetGesCaseReportsNormByCaseReportId(long gesCaseReportsId)
        {
            return this.SafeExecute<IEnumerable<I_GesCaseReportsI_Norms>>(() => _entities.Set<I_GesCaseReportsI_Norms>().Where(i => i.I_GesCaseReports_Id == gesCaseReportsId));
        }

        public I_GesCaseReportsI_Norms GetById(long id)
        {
            return this.SafeExecute<I_GesCaseReportsI_Norms>(() => _entities.Set<I_GesCaseReportsI_Norms>().Find(id));
        }

    }
}
