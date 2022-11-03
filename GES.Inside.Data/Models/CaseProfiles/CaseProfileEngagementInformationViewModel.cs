using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GES.Inside.Data.DataContexts;

namespace GES.Inside.Data.Models.CaseProfiles
{
    public interface ICaseProfileEngagementInformationViewModel
    {
        [Display(Name = "GAP analysis")]
        string GapAnalysis { get; set; }

        [Display(Name = "Change objective")]
        string ChangeObjective { get; set; }

        [Display(Name = "Milestones")]
        string LatestMilestone { get; set; }

        [Display(Name = "Next step")]
        string NextStep { get; set; }

        IList<I_Milestones> Milestones { get; set; }

        DateTime ChangeObjectiveDateTime { get; set; }

        DateTime NextStepDateTime { get; set; }

        DateTime LatestMilestoneDateTime { get; set; }
        int? MileStone { get; set; }
    }

    [MetadataType(typeof(ICaseProfileEngagementInformationViewModel))]
    public class CaseProfileEngagementInformationViewModel : ICaseProfileEngagementInformationViewModel
    {
        public string GapAnalysis { get; set; }
        public string ChangeObjective { get; set; }
        public string LatestMilestone { get; set; }
        public int LatestMilestoneLevel { get; set; }
        public string NextStep { get; set; }
        public IList<I_Milestones> Milestones { get; set; }
        public DateTime ChangeObjectiveDateTime { get; set; }
        public DateTime NextStepDateTime { get; set; }
        public DateTime LatestMilestoneDateTime { get; set; }
        public int? MileStone { get; set; }
    }
}
