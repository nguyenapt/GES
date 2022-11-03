using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GES.Inside.Data.Models.CaseProfiles;

namespace GES.Inside.Data.Models
{
    public class CaseProfileBcArchivedViewModel : CaseProfileBcViewModel
    {
        // Case
        [Display(Name = "Recommendation")]
        public string CaseRecommendation { get; set; } // Recommendation dropdown
        //public bool CaseConfirmedCases { get; set; } // Confirmed case or cases???

        // Issue
        [Display(Name = "Conventions")]
        public List<CaseProfileConventionViewModel> IssueConventions { get; set; }
        [Display(Name = "Description")]
        public string IssueDescription { get; set; }
        // log and Dialogue report (Company Dialogue Summary)
        
        // Endorsement
        
        // Calendar
        
        // Engagement Information
     
        // Other Investors/Stakeholder Initiatives

        // Discussion points to the Company

        // Status (Visual - high up in the profile)
        [Display(Name = "Response rate")]
        public double StatusResponseRate { get; set; }
        [Display(Name = "Progress rate")]
        public double StatusProgressRate { get; set; }

        // visual timeline

        public ICaseProfileStatisticViewModel StatisticComponent { get; set; }

        //[Display(Name = "Meetings in person")]
        //public List<> StatMeetingsInPerson { get; set; } // int or List?
        // Date of latest meeting

        // Pictures
        // Possibility to add pictures and infographics (maybe later if difficult and not nice for case profile layout)

        public ICaseProfileSdgAndGuidelineConventionComponent SdgAndGuidelineConventionComponent { get; set; }
    }
}