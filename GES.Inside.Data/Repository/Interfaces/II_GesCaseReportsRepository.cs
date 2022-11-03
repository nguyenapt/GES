using System.Collections.Generic;
using System.Linq;
using GES.Common.Enumeration;
using GES.Common.Models;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Models;
using GES.Inside.Data.Models.Anonymous;
using GES.Inside.Data.Models.CaseProfiles;

namespace GES.Inside.Data.Repository.Interfaces
{
    public interface II_GesCaseReportsRepository : IGenericRepository<I_GesCaseReports>
    {
        I_GesCaseReports GetById(long id);
        KeyValueObject<string,string> GetCaseReportTitle(long caseReportId);
        GesCaseReportType GetBCCaseReportType(EngagementTypeEnum engagementType, RecommendationType recommandationType);

        GesCaseReportType GetSRCaseReportType(EngagementTypeEnum engagementType, RecommendationType recommandationType);
        TResult GetCaseProfileCoreViewModel<TResult>(long caseReportId, long orgId) where TResult : ICaseProfileCoreViewModel, new();
        ICaseProfileStatisticViewModel GetStatisticComponent(long orgId, long caseProfileId);
        GesContact GetGesContact(long caseProfileId);
        ICaseProfileEngagementInformationViewModel GetEngagementInformationComponent(long caseReportId);
        ICaseProfileIncidentAnalysisComponent GetOpeningIncidentAnalysisComponent(long caseProfileId);
        ICaseProfileIncidentAnalysisComponent GetClosingIncidentAnalysisComponent(long caseProfileId, RecommendationType recommendationType);
        IEnumerable<I_GesCaseReportStatuses> GetRecommendations();
        IEnumerable<IssueHeading> GetIssueHeadings();
        IEnumerable<I_GesCaseReportStatuses> GetConclusions();
        IEnumerable<I_NormAreas> GetNormAreas();
        IEnumerable<GesContact> GetUsers();
        IQueryable<CaseReportListViewModel> GetCaseProfiles(long? companyId);
        IEnumerable<I_ResponseStatuses> GetResponseStatuses();
        IEnumerable<I_ProgressStatuses> GetProgressStatuses();
        IEnumerable<G_DocumentManagementTaxonomies> GetDocumentManagementTaxonomies();
        IEnumerable<G_ManagedDocumentServices> GetManagedDocumentServices();
        IEnumerable<IdNameModel> GetEngagementTypes();
        ICaseProfileSdgAndGuidelineConventionComponent GetCaseProfileSdgAndGuidelineConventionComponent(long caseProfileId);
        IEnumerable<Sdg> GetCaseProfileSdgs(long caseProfileId);
        List<ReferenceViewModel> GetCaseReportReferenceses(long caseProfileId);
        List<RevisionViewModel> GetRevisionCriterials(long caseProfileId);
        ConfirmationInformationViewModel GetConfirmationInformation(long caseProfileId);
        IEnumerable<TimeLineModel> GetTimeLines(long caseProfileId);
        AlertViewModel GetAlert(long caseProfileId);
        bool HasConfirmedConclusion(long caseProfileId);
        void UpdateCaseReportConfirmed(IList<CaseReportListViewModel> caseProfiles);
        void UpdateCaseReportInformation(IList<CaseReportListViewModel> caseProfiles, long orgId, long gesCompanyId);
        void UpdateCaseReportSubscribed(IList<CaseReportListViewModel> caseProfiles, long orgId, long gesCompanyId);
        IEnumerable<KpiViewModel> GetCaseProfileKpis(long caseProfileId);
        bool HasSubscribed(long orgId, long gesCompanyId, long caseProfileId);

        void UpdateCaseReportKPI(IList<CaseReportListViewModel> caseProfiles);
        IEnumerable<GesMilestoneTypes> AllMilestoneTypes();
        MilestoneModel GetLatestMilestoneModelByCaseProfileId(long id);

        ICaseProfileUNGPAssessmentComponent GetCaseProfileUNGPAssessmentComponent(long caseProfileId);

        IList<DialogueEditModel> GetCompanyDialogueLogsByCaseProfileId(long caseProfileId);

        IList<DialogueEditModel> GetSourceDialogueLogsByCaseProfileId(long caseProfileId);

        PaginatedResults<GesContact> GetContacts(JqGridViewModel jqGridParams, long orgId);

        GesCaseReportType GetBpCaseReportType(RecommendationType recommandationType);

        GesCaseReportType GetGovCaseReportType(EngagementTypeEnum engagementType, RecommendationType recommandationType);

        List<GesCaseAuditLogsViewModel> GetRecommendationsHistory(long caseReportId);
        List<MilestoneModel> GetMilestonesByCasereportId(long caseReportId);
        List<GesCaseAuditLogsViewModel> GetConclusionsHistory(long caseReportId);

    }
}
