using System.Collections.Generic;
using GES.Inside.Data.DataContexts;

namespace GES.Inside.Data.Services.Interfaces
{
    public interface II_GesCaseReportsExtraService : IEntityService<I_GesCaseReportsExtra>
    {
        int BatchUpdateCaseReportKeywords(IEnumerable<I_GesCaseReportsExtra> list);
    }
}
