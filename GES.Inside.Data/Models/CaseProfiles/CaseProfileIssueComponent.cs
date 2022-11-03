using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GES.Inside.Data.DataContexts;

namespace GES.Inside.Data.Models.CaseProfiles
{
    public interface ICaseProfileIssueComponent
    {
        long CaseProfileId { get; set; }
        string Summary { get; set; }
        string Description { get; set; }
        string Guidelines { get; set; }
        string CompanyDialogueSummary { get; set; }
        string SourceDialogueSummary { get; set; }
        string CompanyPreparedness { get; set; }
        string LatestNews { get; set; }
        string GesCommentary { get; set; }
        string Conclusion { get; set; }
        DateTime GesCommentaryModified { get; set; }
        IList<I_GesLatestNews> LatestNewsArchive { get; set; }
        DateTime LatestNewsModified { get; set; }
        ICaseProfileIncidentAnalysisComponent OpeningIncidentAnalysisComponent { get; set; }
        ICaseProfileIncidentAnalysisComponent ClosingIncidentAnalysisComponent { get; set; }

        [Display(Name = "Alert")]
        string AlertText { get; set; }
        DateTime? AlertDate { get; set; }

        [Display(Name = "Sources")]
        string AlertSource { get; set; }
        IList<DialogueModel> CompanyDialogues { get; set; }
        IList<DialogueModel> SourceDialogues { get; set; }
    }

    public class CaseProfileIssueComponent : ICaseProfileIssueComponent
    {
        // Issue
        public long CaseProfileId { get; set; }

        [Display(Name = "Summary")]
        public string Summary { get; set; }

        [Display(Name = "Full description")]
        public string Description { get; set; }

        [Display(Name = "Guidelines")]
        public string Guidelines { get; set; }

        [Display(Name = "New dialogue")]
        public string CompanyDialogueNew { get; set; }

        public DateTime? CompanyDialogueNewReviewed { get; set; }

        [Display(Name = "Company dialogue summary")]
        public string CompanyDialogueSummary { get; set; }
        public IList<DialogueModel> CompanyDialogues { get; set; }
        public IList<DialogueModel> SourceDialogues { get; set; }

        [Display(Name = "New dialogue")]
        public string SourceDialogueNew { get; set; }

        [Display(Name = "Source dialogue summary")]
        public string SourceDialogueSummary { get; set; }

        [Display(Name = "Company preparedness")]
        public string CompanyPreparedness { get; set; }

        [Display(Name = "Latest news")]
        public string LatestNews { get; set; }

        public IList<I_GesLatestNews> LatestNewsArchive { get; set; }

        public DateTime LatestNewsModified { get; set; }
        public ICaseProfileIncidentAnalysisComponent OpeningIncidentAnalysisComponent { get; set; }
        public ICaseProfileIncidentAnalysisComponent ClosingIncidentAnalysisComponent { get; set; }

        [Display(Name = "Sustainalytics commentary")]
        public string GesCommentary { get; set; }        

        public DateTime GesCommentaryModified { get; set; }

        [Display(Name = "GSS Link")]
        public string GSSLink { get; set; }

        public DateTime GSSLinkModified { get; set; }

        [Display(Name = "Conclusion")]
        public string Conclusion { get; set; }

        [Display(Name = "Conclusion")]
        public string ConclusionObs { get; set; }

        [Display(Name = "Alert")]
        public string AlertText { get; set; }
        public DateTime? AlertDate { get; set; }
        [Display(Name = "Sources")]
        public string AlertSource { get; set; }
    }
    
    public class BusinessConductEngageOrDisEngageOrResolveCaseProfileIssueComponent : CaseProfileIssueComponent
    {
        // TruongP: Remove this class later
    }

    public class SrEmeCaseProfileIssueComponent : CaseProfileIssueComponent
    {
        [Display(Name = "Material ESG risks")]
        public string MostMaterialRisk { get; set; }
    }

    public class FullAttributeCaseProfileIssueComponent : CaseProfileIssueComponent
    {
        [Display(Name = "Summary of material risks")]
        public string MostMaterialRisk { get; set; }
    }
}
