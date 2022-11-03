using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Models;
using System.Collections.Generic;

namespace GES.Inside.Data.Repository.Interfaces
{
    public interface II_GesCaseReportSourcesRepository : IGenericRepository<I_GesCaseReportSources>
    {
        IList<CaseReportSourceReferenceViewModel> GetSourcesByCaseReportId(long caseReportId);

        I_GesCaseReportSources GetById(long Id);
    }
}
