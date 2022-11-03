using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Models.CaseProfiles;

namespace GES.Inside.Data.Models
{
    public class CaseProfileFullAttributeViewModel : CaseProfileCoreViewModel
    {        
        //BC - Engage
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

        //Bc - Archived        
        [Display(Name = "Recommendation")]
        public string CaseRecommendation { get; set; } // Recommendation dropdown
        
        [Display(Name = "Conventions")]
        public List<CaseProfileConventionViewModel> IssueConventions { get; set; }
        [Display(Name = "Description")]
        public string IssueDescription { get; set; }

        //Bc - disengage

        //Bc - evaluate

        //Bc - Resolved

        //Bc - Viewmodel        
        [Display(Name = "Conclusion")]
        public string CaseConclusion { get; set; } // Conclusion dropdown
        [Display(Name = "Conclusion date")]
        public DateTime CaseConclusionDate { get; set; }
        
        [Display(Name = "Company preparedness")]
        public string IssueCompanyPreparedness { get; set; }        
        
        [Display(Name = "Additional incident")]
        public List<CaseProfileBcViewModel> AdditionalIncidents { get; set; }

        //Bespoke
        public DateTime StatingDate { get; set; }        

        public string Guidlines { get; set; }

        public string CompanyDialogueSummary { get; set; }

        public string SourceDialogueSummary { get; set; }

        public string LatestNews { get; set; }

        public string GesCommentary { get; set; }

        public string ChangeObjective { get; set; }

        public string Milestone { get; set; }

        public string NextStep { get; set; }
        
        public IList<Sdg> Sdgs { get; set; }

        //Standard
        public string Conclusion { get; set; }        

        public ICaseProfileSdgAndGuidelineConventionComponent GuidelineConventionComponent { get; set; }

        //SrEme

        //StandardCore
        public DateTime AlertEntryDate { get; set; }        

        public string SrourceDialogueSummary { get; set; }

        public string CompanyPrepareness { get; set; }        

        public string IssueConclusion { get; set; }

        //SrGov

        //Sr-ViewModel        
        public IList<KpiViewModel> KpiViewModels { get; set; }

        //Generation-Viewmodel

        //Sr-Engage
        public DateTime StartingDate { get; set; }        

        public string Description { get; set; }

        //SrEngageEme
        public string MaterialEsgRiskAndOpportunities { get; set; }        

        public string Milestones { get; set; }
        
        public string RecommendationForChange { get; set; }

        //SrEvaluate
       
        public string SourceDialogSummary { get; set; }
    }
}