using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Models;
using GES.Inside.Data.Repository.Interfaces;
using GES.Inside.Data.Services.Interfaces;
using Z.EntityFramework.Plus;
using GES.Common.Logging;

namespace GES.Inside.Data.Services
{
    public class GesUngpAssessmentScoresService : EntityService<GesEntities, GesUNGPAssessmentScores>, IGesUngpAssessmentScoresService
    {
        private readonly GesEntities _dbContext;
        private readonly IGesUngpAssessmentScoresRepository _assessmentScoresRepository;

        public GesUngpAssessmentScoresService(IUnitOfWork<GesEntities> unitOfWork, IGesUngpAssessmentScoresRepository gesUngpAssessmentScoresRepository, IGesLogger logger) : base(unitOfWork, logger, gesUngpAssessmentScoresRepository)
        {
            _dbContext = unitOfWork.DbContext;
            _assessmentScoresRepository = gesUngpAssessmentScoresRepository;
        }

        public IEnumerable<GesUngpAssessmentScoresViewModel> GetAllGesUngpAssessmentScores()
        {
            return this.SafeExecute<IEnumerable<GesUngpAssessmentScoresViewModel>>(() => (from x in _dbContext.GesUNGPAssessmentScores
                select new GesUngpAssessmentScoresViewModel
                {
                    GesUngpAssessmentScoresId = x.GesUNGPAssessmentScoresId,
                    Name = x.Name,
                    Description = x.Description,
                    ScoreType = x.ScoreType,
                    Order = x.Order,
                    Score = x.Score,
                    Created = x.Created
                }).FromCache().ToList());
        }

        public string GetScoreDescription(Guid? id)
        {
            return this.SafeExecute<string>(() => (from x in _dbContext.GesUNGPAssessmentScores where x.GesUNGPAssessmentScoresId == id
                select x.Name).FromCache().FirstOrDefault());
        }
    }
}