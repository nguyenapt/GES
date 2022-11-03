using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Models.CaseProfiles;

namespace GES.Inside.Data.Models
{
    public class CaseProfileSrGovViewModel : CaseProfileCoreViewModel
    {
        public DateTime StatingDate { get; set; }

        public string CaseRecommendation { get; set; }

        public string Guidlines { get; set; }

        public string CompanyDialogueSummary { get; set; }

        public string SourceDialogueSummary { get; set; }

        public string LatestNews { get; set; }

        public string GesCommentary { get; set; }

        public string ChangeObjective { get; set; }

        public string Milestone { get; set; }

        [Display(Name = "Calendar")]
        public IEnumerable<EventListViewModel> Events { get; set; }

        public string Endorsement { get; set; }

        public ICaseProfileStatisticViewModel StatisticComponent { get; set; }

        public ICaseProfileEngagementInformationViewModel EngagementInformationComponent { get; set; }
        public IList<Sdg> Sdgs { get; set; }

    }
}
