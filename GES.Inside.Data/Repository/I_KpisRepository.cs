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
    public class I_KpisRepository : GenericRepository<I_Kpis>, II_KpisRepository
    {
        private readonly GesEntities _dbContext;
        public I_KpisRepository(GesEntities context, IGesLogger logger)
           : base(context, logger)
        {
            _dbContext = context;
        }

        public I_Kpis GetById(long id)
        {
            return this.SafeExecute<I_Kpis>(() => _entities.Set<I_Kpis>().FirstOrDefault(d => d.I_Kpis_Id == id));
        }

        public IEnumerable<I_Kpis> GetKpisByEngagementTypeId(long engagementTypeId)
        {
            return this.SafeExecute<IEnumerable<I_Kpis>>(() => _entities.Set<I_Kpis>().Where(i => i.I_EngagementTypes_Id == engagementTypeId));
        }
    }
}
