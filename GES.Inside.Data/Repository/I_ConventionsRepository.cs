using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Repository.Interfaces;
using GES.Common.Logging;
using System.Collections.Generic;
using System.Linq;

namespace GES.Inside.Data.Repository
{
    public class I_ConventionsRepository : GenericRepository<I_Conventions>, II_ConventionsRepository
    {
        public I_ConventionsRepository(GesEntities context, IGesLogger logger) : base(context, logger)
        {
        }

        public IEnumerable<I_Conventions> GetConvertionsByCaseReportId(long caseReportId)
        {
            return this.SafeExecute(() => (from conventions in _entities.Set<I_Conventions>()
                                           join conventionReport in _entities.Set<I_GesCaseReportsI_Conventions>()
                                           on conventions.I_Conventions_Id equals conventionReport.I_Conventions_Id into conventionsGroup
                                           from item in conventionsGroup.DefaultIfEmpty()
                                           where item.I_GesCaseReports_Id == caseReportId
                                           select conventions).AsEnumerable());
        }

        public IEnumerable<string> GetConventionsForCaseReport(long caseReportId)
        {

            return this.SafeExecute(() => (from conventions in _entities.Set<I_Conventions>()
                                           join conventionReport in _entities.Set<I_GesCaseReportsI_Conventions>()
                                           on conventions.I_Conventions_Id equals conventionReport.I_Conventions_Id into conventionsGroup
                                           from item in conventionsGroup.DefaultIfEmpty()
                                           where item.I_GesCaseReports_Id == caseReportId
                                           select conventions.Name)
                .Union(
                    from norm in _entities.Set<I_Norms>()
                    join rp in _entities.Set<I_GesCaseReportsI_Norms>() on norm.I_Norms_Id equals rp.I_Norms_Id
                    where rp.I_GesCaseReports_Id == caseReportId
                    select norm.SectionTitle
                    )
                .AsEnumerable());
        }

        public I_Conventions GetById(long conventionModelConventionsId)
        {
            return this.SafeExecute<I_Conventions>(() =>
                            _entities.Set<I_Conventions>().FirstOrDefault(d => d.I_Conventions_Id == conventionModelConventionsId));
        }
    }
}
