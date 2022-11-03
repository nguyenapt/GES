using System.Collections.Generic;
using GES.Common.Models;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Models;

namespace GES.Inside.Data.Services.Interfaces
{
    public interface IGesCaseReportSignUpService : IEntityService<GesCaseReportSignUp>
    {
        GesCaseReportSignUp GetGesCaseReportSignUpById(long gesCaseReportId, long individualId);

        bool RemoveGesCaseReportSignUp(long gesCaseReportId, long individualId);

        PaginatedResults<CaseReportSignUpListViewModel> GetCaseReportSignUpList(JqGridViewModel jqGridParams);
        string GetUserResigned(long caseReportId, long orgId);

        PaginatedResults<CaseReportSignUpUserListViewModel> GetCaseReportSignUpUserList(JqGridViewModel jqGridParams,
            long caseReportId);

        List<CaseReportSignUpListViewModel> ExportEndorsements();
    }
}
