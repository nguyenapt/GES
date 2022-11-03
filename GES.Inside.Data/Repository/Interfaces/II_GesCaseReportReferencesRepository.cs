using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Models;
using System.Collections.Generic;

namespace GES.Inside.Data.Repository.Interfaces
{
    public interface II_GesCaseReportReferencesRepository : IGenericRepository<I_GesCaseReportReferences>
    {
        IList<CaseReportSourceReferenceViewModel> GetReferencesByCaseReportId(long caseReportId);

        I_GesCaseReportReferences GetById(long Id);
    }
}
