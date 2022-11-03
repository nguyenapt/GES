using System;
using System.Collections.Generic;
using System.Linq;
using GES.Common.Logging;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Repository.Interfaces;

namespace GES.Inside.Data.Repository
{
    public class I_MilestonesRepository : GenericRepository<I_Milestones>, II_MilestonesRepository
    {
        private readonly GesEntities _dbContext;

        public I_MilestonesRepository(GesEntities context, IGesLogger logger) : base(context, logger)
        {
            _dbContext = context;
        }

        public bool UpdateMilestoneForCaseProfile(I_Milestones milestones)
        {
            throw new NotImplementedException();
        }

        public I_Milestones GetMilestone(long caseProfileId, long milestoneId)
        {
            return _dbContext.I_Milestones.FirstOrDefault(x =>
                x.I_Milestones_Id == milestoneId && x.I_GesCaseReports_Id == caseProfileId);
        }

        public I_Milestones GetById(long milestoneId)
        {
            return _dbContext.I_Milestones.FirstOrDefault(x =>
                x.I_Milestones_Id == milestoneId);
        }
    }
}
