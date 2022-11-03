using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Models.Auth;

namespace GES.Inside.Data.Models
{
    public class CaseProfileDetailBaseCaseIssueModel
    {
        
        public long CaseProfileId { get; set; }

        // Base
        public long CompanyId { get; set; }
        [Display(Name = "Company")]
        public string CompanyName { get; set; }
        [Display(Name = "SEDOL")]
        public string Sedol { get; set; }
        [Display(Name = "ISIN")]
        public string Isin { get; set; }
        [Display(Name = "Industry")]
        public string Industry { get; set; }
        [Display(Name = "Domicile")]
        public string HomeCountry { get; set; }

        // Case
        [Display(Name = "Date")]
        public DateTime? AlertEntryDate { get; set; }
        public DateTime? StartingDate { get; set; }
        [Display(Name = "Heading")]
        public string Heading { get; set; }
        public string FocusTodayNorm { get; set; } // Not "In Focus List?", currently "Norm"?
        [Display(Name = "Location")]
        public string Location { get; set; }
        public string CaseConclusion { get; set; }
        public string Recommendation { get; set; }
        public bool ConfirmedSymbol { get; set; }

        // Issue
        [Display(Name = "Summary")]
        public string IssueSummary { get; set; }
        [Display(Name = "Guidelines")]
        public string Guidelines { get; set; }
        public string Conventions { get; set; }
        public string Description { get; set; }
        public string Confirmation { get; set; }
        [Display(Name = "Company dialogue summary")]
        public string CompanyDialogueSummary { get; set; }
        public string LogAndDialogReport { get; set; }
        [Display(Name = "Source dialogue summary")]
        public string SourceDialogueSummary { get; set; }
        public string IssueAndLog { get; set; }
        public string CompanyPreparedness { get; set; }
        public string LatestNews { get; set; }
        public string RevisionCriteria1And2 { get; set; }
        [Display(Name = "Sustainalytics commentary")]
        public string GesCommentary { get; set; }
        public string Conclusion { get; set; }
        public string References { get; set; }
        public string Endorsement { get; set; }
    }
}