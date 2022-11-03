using GES.Inside.Data.Models.CaseProfiles;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GES.Inside.Data.Models
{
    public class CaseProfileStandardViewModel : CaseProfileCoreViewModel
    {
        public string Guidlines { get; set; }

        public string CompanyDialogueSummary { get; set; }

        public string SourceDialogueSummary { get; set; }

        public string LatestNews { get; set; }

        public string Conclusion { get; set; }

        [Display(Name = "Calendar")]
        public IEnumerable<EventListViewModel> Events { get; set; }

        public ICaseProfileStatisticViewModel StatisticComponent { get; set; }

        public ICaseProfileEngagementInformationViewModel EngagementInformationComponent { get; set; }

        public ICaseProfileSdgAndGuidelineConventionComponent GuidelineConventionComponent { get; set; }
    }
}