using System;
using System.Data.Entity;
using System.Linq;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Repository.Interfaces;
using GES.Common.Logging;
using System.Collections.Generic;

namespace GES.Inside.Data.Repository
{
    public class GesUngpAssessmentScoresRepository : GenericRepository<GesUNGPAssessmentScores>, IGesUngpAssessmentScoresRepository
    {
        public GesUngpAssessmentScoresRepository(GesEntities context, IGesLogger logger)
           : base(context, logger)
        {

        }

        public GesUNGPAssessmentScores GetById(Guid id)
        {
            return this.SafeExecute<GesUNGPAssessmentScores>(() => _entities.Set<GesUNGPAssessmentScores>().FirstOrDefault(d => d.GesUNGPAssessmentScoresId == id));
        }

        public IEnumerable<GesUNGPAssessmentScores> GetAllGesUngpAssessmentScores()
        {
            return this.SafeExecute<List<GesUNGPAssessmentScores>>(()=> _entities.Set<GesUNGPAssessmentScores>().ToList());
        }

    }
}
