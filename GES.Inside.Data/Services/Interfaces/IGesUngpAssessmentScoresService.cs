using System;
using System.Collections.Generic;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Models;

namespace GES.Inside.Data.Services.Interfaces
{
    public interface IGesUngpAssessmentScoresService : IEntityService<GesUNGPAssessmentScores>
    {
        IEnumerable<GesUngpAssessmentScoresViewModel> GetAllGesUngpAssessmentScores();
        string GetScoreDescription(Guid? id);
    }
}
