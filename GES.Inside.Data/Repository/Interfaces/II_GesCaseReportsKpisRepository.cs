using GES.Inside.Data.DataContexts;
using System.Collections;
using System.Collections.Generic;
using GES.Inside.Data.Models;

namespace GES.Inside.Data.Repository.Interfaces
{
    public interface II_GesCaseReportsKpisRepository : IGenericRepository<I_GesCaseReportsI_Kpis>
    {
        IEnumerable<GesCaseReportKpiViewModel> GetGesCaseReportsKpisByCaseReportID(long gesCaseReportsId);

        I_GesCaseReportsI_Kpis GetById(long id);
    }
}
