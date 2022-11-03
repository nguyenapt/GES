using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Models.Auth;
using GES.Inside.Data.Models.CaseProfiles;

namespace GES.Inside.Data.Models
{
    public class CaseProfileBcViewModel : CaseProfileCoreViewModel
    {
        // Case
        [Display(Name = "Conclusion")]
        public string CaseConclusion { get; set; } // Conclusion dropdown
        [Display(Name = "Conclusion date")]
        public DateTime CaseConclusionDate { get; set; }

        // Issue
        [Display(Name = "Company preparedness")]
        public string IssueCompanyPreparedness { get; set; }
        //public string Conclusion { get; set; } // What's the difference with "Conclusion dropdown"?

        // Additional Incidents
        [Display(Name = "Additional incident")]
        public List<CaseProfileBcViewModel> AdditionalIncidents { get; set; }
    }
}