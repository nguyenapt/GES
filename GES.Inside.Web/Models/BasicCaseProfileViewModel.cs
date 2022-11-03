using System;
using System.Collections.Generic;
using GES.Inside.Data.Models;
using GES.Inside.Data.DataContexts;

namespace GES.Inside.Web.Models
{
    public class BasicCaseProfileViewModel
    {
        public long I_GesCaseReports_Id { get; set; }
        public string Summary { get; set; }
        public string ReportIncident { get; set; }
        public string Guidelines { get; set; }
        public string Description { get; set; }
        public string DescriptionNew { get; set; }
        public string Conclusion { get; set; }
        public string CompanyDialogueSummary { get; set; }
        public string CompanyDialogueNew { get; set; }
        public string SourceDialogueSummary { get; set; }
        public string SourceDialogueNew { get; set; }
        public string ProcessGoal { get; set; }
        public string ProcessStep { get; set; }
        public string EntryDate { get; set; }
        public Guid? CountryId { get; set; }
        public long? NewI_GesCaseReportStatuses_Id { get; set; }
        public long? I_GesCaseReportStatuses_Id { get; set; }
        public long? I_NormAreas_Id { get; set; }
        public long? AnalystG_Users_Id { get; set; }
        public long? I_ResponseStatuses_Id { get; set; }
        public long? I_ProgressStatuses_Id { get; set; }
        public int? MileStone { get; set; }
        public long I_GesCompanies_Id { get; set; }
        public long I_Companies_Id { get; set; }
        public long? I_Engagement_Type_Id { get; set; }
        public bool ShowInClient { get; set; }
        public string CompanyName { get; set; }
        public string CompanyIsin { get; set; }
        public string CompanyIndustry { get; set; }
        public string CompanyHomeCountry { get; set; }
        public string UnGlobalCompact { get; set; }
        public string GriAlignedDisclosure { get; set; }
        public IList<long> SdgIds  { get; set; }
        public IEnumerable<CaseReportKpiViewModel> CaseReportKpiViewModels { get; set; }
        public MilestoneModel MileStoneModel { get; set; }
        public IEnumerable<GesCommentaryViewModel> CommentaryViewModels { get; set; }
        public IEnumerable<GSSLinkViewModel> GSSLinkViewModels { get; set; }
        public IEnumerable<GesLatestNewsViewModel> GesLatestNewsViewModels { get; set; }
        public IList<DialogueEditModel> CompanyDialogueLogs { get; set; }
        public IList<DialogueEditModel> SourceDialogueLogs { get; set; }
        public IEnumerable<ConventionsViewModel> ConventionsViewModels { get; set; }
        public IEnumerable<NormViewModel> NormViewModels { get; set; }
        public IList<DiscussionPointsViewModel> DiscussionPoints { get; set; }        
        public string InvestorInitiatives { get; set; }
        public IList<OtherStakeholderViewModel> StakeholderViews { get; set; }

        public IList<AssociatedCorporationsViewModel> AssociatedCorporations { get; set; }

        public IList<CaseReportSourceReferenceViewModel> SourcesViewModels { get; set; }

        public IList<CaseReportSourceReferenceViewModel> ReferencesViewModels { get; set; }

        public IList<CaseReportSupplementaryViewModel> SupplementaryReadingViewModels { get; set; }

        public IList<DocumentViewModel> Documents { get; set; }
        
        public string IncidentAnalysisSummary { get; set; }
        public string IncidentAnalysisDialogueAndAnalysis { get; set; }
        public string IncidentAnalysisConclusion { get; set; }
        public string IncidentAnalysisGuidelines { get; set; }
        public string IncidentAnalysisSources { get; set; }

        public string ClosingIncidentAnalysisSummary { get; set; }
        public string ClosingIncidentAnalysisDialogueAndAnalysis { get; set; }
        public string ClosingIncidentAnalysisConclusion { get; set; }
        public long? IssueHeadingId { get; set; }
        public string Comment { get; set; }
    }
}