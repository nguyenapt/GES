using System;
using System.Collections.Generic;
using GES.Inside.Data.DataContexts;

namespace GES.Inside.Data.Repository.Interfaces
{
    public interface IGesUngpAssessmentScoresRepository : IGenericRepository<GesUNGPAssessmentScores>
    {
        GesUNGPAssessmentScores GetById(Guid id);
        IEnumerable<GesUNGPAssessmentScores> GetAllGesUngpAssessmentScores();
        
    }
}
