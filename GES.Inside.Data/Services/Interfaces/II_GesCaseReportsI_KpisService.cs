using System.Collections.Generic;
using GES.Common.Models;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Models;

namespace GES.Inside.Data.Services.Interfaces
{
    public interface II_GesCaseReportsI_KpisService : IEntityService<I_GesCaseReportsI_Kpis>
    {
        I_GesCaseReportsI_Kpis GetById(long id);
        IEnumerable<I_GesCaseReportsI_Kpis> GetGesCaseReportsKpisByCaseReportID(long gesCaseReportsId);
    }
}
