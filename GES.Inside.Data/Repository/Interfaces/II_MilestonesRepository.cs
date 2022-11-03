using System.Collections;
using System.Collections.Generic;
using GES.Inside.Data.DataContexts;

namespace GES.Inside.Data.Repository.Interfaces
{
    public interface II_MilestonesRepository : IGenericRepository<I_Milestones>
    {
        bool UpdateMilestoneForCaseProfile(I_Milestones milestones);
        I_Milestones GetMilestone(long caseProfileId, long milestoneId);
        I_Milestones GetById(long milestoneId);
    }
}
