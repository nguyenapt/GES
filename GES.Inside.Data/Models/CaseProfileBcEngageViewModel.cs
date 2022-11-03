using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Models.CaseProfiles;

namespace GES.Inside.Data.Models
{
    public class CaseProfileBcEngageViewModel : CaseProfileCoreViewModel
    {        
        // Engagement Information
        [Display(Name = "GAP analysis")]
        public string EngagementGapAnalysis { get; set; }

        [Display(Name = "Change objective")]
        public string EngagementChangeObjective { get; set; }

        [Display(Name = "Milestones")]
        public List<CaseProfileSimpleNoteViewModel> EngagementMilestones { get; set; }

        [Display(Name = "Next step")]
        public CaseProfileSimpleNoteViewModel EngagementNextStep { get; set; }

        [Display(Name = "Response rate")]
        public double StatusResponseRate { get; set; }

        [Display(Name = "Progress rate")]
        public double StatusProgressRate { get; set; }

        public string Endorsement { get; set; }

        [Display(Name = "Discussion points")]
        public IList<I_EngagementDiscussionPoints> DiscussionPoints { get; set; }

        [Display(Name = "Investor initiatives")]
        public string InvestorInitiatives { get; set; }

        [Display(Name = "Calendar")]
        public IEnumerable<EventListViewModel> Events { get; set; }

        public IList<I_EngagementOtherStakeholderViews> StakeholderViews { get; set; }

        public ICaseProfileStatisticViewModel StatisticComponent { get; set; }

        public ICaseProfileEngagementInformationViewModel EngagementInformationComponent { get; set; }
        
        [Display(Name = "Guidlines")]
        public IEnumerable<string> Guidelines { get; set; }

        public ICaseProfileSdgAndGuidelineConventionComponent SdgAndGuidelineConventionComponent { get; set; }


        public long ConclusionId { get; set; }
    }
}