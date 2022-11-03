using System;
using System.Collections.Generic;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Models.CaseProfiles;

namespace GES.Inside.Data.Repository.Interfaces
{
    public interface IGesUngpAssessmentFormRepository : IGenericRepository<GesUNGPAssessmentForm>
    {
        GesUNGPAssessmentForm GetById(Guid id);
        GesUNGPAssessmentForm GetGesUngpAssessmentFormByCaseProfileId(long caseProfileId);
        GesUngpAuditViewModel GetGesUngpAssessmentFormHistoryByUngpId(long caseProfileId);
    }
}
