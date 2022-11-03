using GES.Inside.Data.DataContexts;
using System.Collections;
using System.Collections.Generic;
using GES.Inside.Data.Models;

namespace GES.Inside.Data.Repository.Interfaces
{
    public interface II_GesCaseReportsI_NormsRepository : IGenericRepository<I_GesCaseReportsI_Norms>
    {
        IEnumerable<I_GesCaseReportsI_Norms> GetGesCaseReportsNormByCaseReportId(long gesCaseReportsId);

        I_GesCaseReportsI_Norms GetById(long id);
    }
}
