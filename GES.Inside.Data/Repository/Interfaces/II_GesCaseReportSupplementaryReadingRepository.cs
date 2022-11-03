using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Models;
using System.Collections.Generic;

namespace GES.Inside.Data.Repository.Interfaces
{
    public interface II_GesCaseReportSupplementaryReadingRepository : IGenericRepository<I_GesCaseReportSupplementaryReading>
    {
        IList<CaseReportSupplementaryViewModel> GetSupplementaryReadingsByCaseReportId(long caseReportId);

        I_GesCaseReportSupplementaryReading GetById(long Id);
    }
}
