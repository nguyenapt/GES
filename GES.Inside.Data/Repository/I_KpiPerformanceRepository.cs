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
    public class I_KpiPerformanceRepository : GenericRepository<I_KpiPerformance>, II_KpiPerformanceRepository
    {
        private readonly GesEntities _dbContext;
        public I_KpiPerformanceRepository(GesEntities context, IGesLogger logger)
           : base(context, logger)
        {
            _dbContext = context;
        }

        public IEnumerable<I_KpiPerformance> GetAllKpiPerformances()
        {
            return this.SafeExecute<IEnumerable<I_KpiPerformance>>(() => _entities.Set<I_KpiPerformance>());
        }
    }
}
