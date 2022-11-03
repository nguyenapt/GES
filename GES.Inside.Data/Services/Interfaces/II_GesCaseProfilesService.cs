using System.Collections.Generic;
using GES.Common.Enumeration;
using GES.Common.Models;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Models;
using GES.Inside.Data.Models.Anonymous;

namespace GES.Inside.Data.Services.Interfaces
{
    public interface II_GesCaseProfilesService : IEntityService<I_GesCaseReports>
    {
        int ExtractKeywordsForAllCaseProfiles(int limit);

        IEnumerable<I_EngagementActivityOptions> GetActivityOptions();

        I_ActivityForms GetActivityForm(long caseReportId, long orgId);

        I_ActivityForms UpdateEndorsement(I_ActivityForms activityForm);

        KeyValueObject<string,string> GetCaseReportTitle(long caseReportId);

        GesCaseReportType GetReportType(long reportId, long orgId);

        int GetEngagementType(long reportId);

        CaseProfileBcEvaluateViewModel GetBusinessConductEvaluate(long caseReportId, long orgId);

        CaseProfileBcEngageViewModel GetBusinessConductEngage(long caseReportId, long orgId);

        CaseProfileBcDisengageViewModel GetBusinessConductDisEngage(long caseReportId, long orgId);
        CaseProfileBcArchivedViewModel GetBusinessConductArchived(long caseReportId, long orgId);
        CaseProfileBcResolvedViewModel GetBusinessConductResolved(long caseReportId, long orgId);
        CaseProfileCoreViewModel GetCaseProfileCoreModel(GesCaseReportType reportType, long caseReportId, long orgId);

        CaseProfileSrEvaluateViewModel GetSrEvaluate(long caseReportId, long orgId);

        CaseProfileSrViewModel GetSrEngageOrArchived(long caseReportId, long orgId);

        CaseProfileSrEmeViewModel GetSrEmeEngageOrArchived(long caseReportId, long orgId);

        CaseProfileSrGovViewModel GetSrGovEngageOrArchived(long caseReportId, long orgId);

        CaseProfileSrEngageEmeViewModel GetSrEngageEme(long caseReportId, long orgId);

        CaseProfileSrEngageCropGovernance GetSrEngageCropGovernance(long caseReportId, long orgId);

        CaseReportViewModel GetCaseReportViewModel(long caseReportId, long orgId);

        List<CaseReportListViewModel> GetAdditionalCaseReports(long caseProfileId, long orgId);

        PaginatedResults<CaseReportListViewModel> GetCaseProfiles(JqGridViewModel jqGridParams, long? companyId);

        IEnumerable<TimeLineModel> GetTimeLines(long caseProfileId);

        List<GesCaseAuditLogsViewModel> GetRecommendationHistory(long caseReportId);
        List<GesCaseAuditLogsViewModel> GetConclusionHistory(long caseReportId);

    }
}
