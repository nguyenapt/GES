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
    public class I_GesCaseReportsKpisRepository : GenericRepository<I_GesCaseReportsI_Kpis>, II_GesCaseReportsKpisRepository
    {
        private readonly GesEntities _dbContext;
        public I_GesCaseReportsKpisRepository(GesEntities context, IGesLogger logger)
           : base(context, logger)
        {
            _dbContext = context;
        }


        public IEnumerable<GesCaseReportKpiViewModel> GetGesCaseReportsKpisByCaseReportID(long gesCaseReportsId){
     
             var engagementTypes = from e in _dbContext.I_GesCaseReportsI_Kpis
                                  where e.I_GesCaseReports_Id == gesCaseReportsId
                                  select new GesCaseReportKpiViewModel
                                  {
                                      I_GesCaseReports_Id = e.I_GesCaseReports_Id,
                                      I_GesCaseReportsI_Kpis_Id = e.I_GesCaseReportsI_Kpis_Id,
                                      I_KpiPerformance_Id = (long) e.I_KpiPerformance_Id,
                                      I_Kpis_Id = e.I_Kpis_Id,
                                      Created = e.Created
                                  };

            return engagementTypes;
        }

        public I_GesCaseReportsI_Kpis GetById(long id)
        {
            return this.SafeExecute<I_GesCaseReportsI_Kpis>(() => _entities.Set<I_GesCaseReportsI_Kpis>().FirstOrDefault(d => d.I_GesCaseReportsI_Kpis_Id == id));
        }
    }
}
