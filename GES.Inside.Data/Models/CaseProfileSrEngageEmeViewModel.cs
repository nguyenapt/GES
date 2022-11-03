using GES.Inside.Data.Models.CaseProfiles;
using System;

namespace GES.Inside.Data.Models
{
    public class CaseProfileSrEngageEmeViewModel : CaseProfileCoreViewModel
    {
        public string MaterialEsgRiskAndOpportunities { get; set; }

        public DateTime StartingDate { get; set; }

        public string CaseRecommendation { get; set; }

        public string CompanyDialogueSummary { get; set; }

        public string LatestNews { get; set; }

        public string Endorsement { get; set; }

        //calendar

        public string ChangeObjective { get; set; }

        public string Milestones { get; set; }

        public string NextStep { get; set; }

        //status
        //statistic

        public string RecommendationForChange { get; set; }

    }
}