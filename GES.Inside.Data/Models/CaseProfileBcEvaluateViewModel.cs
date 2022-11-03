using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GES.Inside.Data.Models
{
    public class CaseProfileBcEvaluateViewModel : CaseProfileStandardBcCoreViewModel
    {
        // Case
        [Display(Name = "Recommendation")]
        public string CaseRecommendation { get; set; } // Recommendation dropdown

        // Issue
        // log (Source Dialogue Summary)

        public string Endorsement { get; set; }
        
        public IEnumerable<EventListViewModel> Events { get; set; }

        // Engagement Information

        // Discussion points to the Company

        // Status (Visual - high up in the profile)

        // Statistics

        // Pictures
    }
}