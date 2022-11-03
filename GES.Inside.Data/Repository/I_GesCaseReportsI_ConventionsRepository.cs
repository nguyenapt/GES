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
    public class I_GesCaseReportsI_ConventionsRepository : GenericRepository<I_GesCaseReportsI_Conventions>, II_GesCaseReportsI_ConventionsRepository
    {
        private readonly GesEntities _dbContext;
        public I_GesCaseReportsI_ConventionsRepository(GesEntities dbContext, IGesLogger logger)
            : base(dbContext, logger)
        {
            _dbContext = dbContext;
        }

        public IList<ConventionModel> GetAllConventions()
        {
            var conventions = (from c in _dbContext.I_Conventions
                              select new ConventionModel
                              {
                                  I_Conventions_Id = c.I_Conventions_Id,
                                  Name = c.Name
                              }).ToList();
            return conventions;
        }

        public IEnumerable<I_GesCaseReportsI_Conventions> GetGesCaseReportsConventionsByCaseReportId(long gesCaseReportsId)
        {
            return this.SafeExecute<IEnumerable<I_GesCaseReportsI_Conventions>>(() => _entities.Set<I_GesCaseReportsI_Conventions>().Where(i => i.I_GesCaseReports_Id == gesCaseReportsId));
        }

        I_GesCaseReportsI_Conventions II_GesCaseReportsI_ConventionsRepository.GetById(long id)
        {
            return this.SafeExecute<I_GesCaseReportsI_Conventions>(() => _entities.Set<I_GesCaseReportsI_Conventions>().Find(id));
        }

    }
}
