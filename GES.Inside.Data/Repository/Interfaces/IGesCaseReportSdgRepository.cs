using System.Collections;
using System.Collections.Generic;
using GES.Inside.Data.DataContexts;

namespace GES.Inside.Data.Repository.Interfaces
{
    public interface IGesCaseReportSdgRepository : IGenericRepository<GesCaseReportSdg>
    {
        bool TryUpdateSdgsForCaseProfile(long caseProfileId, IList<long> sdgIds);
        IList<long> GetSdgIdsByCaseProfile(long caseProfileId);
        GesCaseReportSdg GetCaseReportSdg(long caseProfileId, long sdgId);
        GesUNGPAssessmentForm GesUngpAssessmentByCaseProfile(long? id);
    }
}
