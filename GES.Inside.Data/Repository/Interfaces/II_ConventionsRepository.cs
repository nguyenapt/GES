using GES.Inside.Data.DataContexts;
using System.Collections.Generic;

namespace GES.Inside.Data.Repository.Interfaces
{
    public interface II_ConventionsRepository : IGenericRepository<I_Conventions>
    {
        IEnumerable<I_Conventions> GetConvertionsByCaseReportId(long caseReportId);

        IEnumerable<string> GetConventionsForCaseReport(long caseReportId);
        I_Conventions GetById(long conventionModelConventionsId);
    }
}
