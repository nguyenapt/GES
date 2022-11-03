using System.ComponentModel.DataAnnotations;
using GES.Common.Enumeration;

namespace GES.Inside.Data.Models.CaseProfiles
{
    public interface ICaseProfileIncidentAnalysisComponent
    {
        string IncidentAnalysisSummary { get; set; }
        string IncidentAnalysisDialogueAndAnalysis { get; set; } 
        string IncidentAnalysisConclusion { get; set; }
        string IncidentAnalysisGuidelines { get; set; }
        string IncidentAnalysisSources { get; set; }
        RecommendationType RecommendationType { get; set; }
    }

    public class CaseProfileIncidentAnalysisComponent : ICaseProfileIncidentAnalysisComponent
    {
        [Display(Name = "Incident summary")]
        public string IncidentAnalysisSummary { get; set; }

        [Display(Name = "Dialogue and analysis")]
        public string IncidentAnalysisDialogueAndAnalysis { get; set; }

        [Display(Name = "Conclusion")]
        public string IncidentAnalysisConclusion { get; set; }

        [Display(Name = "Guidelines and conventions")]
        public string IncidentAnalysisGuidelines { get; set; }

        [Display(Name = "Sources")]
        public string IncidentAnalysisSources { get; set; }
        public RecommendationType RecommendationType { get; set; }
    }
}
