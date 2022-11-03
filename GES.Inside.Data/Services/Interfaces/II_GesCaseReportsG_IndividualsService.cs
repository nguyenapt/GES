using System.Collections.Generic;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Models;

namespace GES.Inside.Data.Services.Interfaces
{
    public interface II_GesCaseReportsG_IndividualsService : IEntityService<I_GesCaseReportsG_Individuals>
    {
        bool RemoveGesCaseReportsG_IndividualsService(long gesCaseReportId, long individualId);
        bool AddListGescaseReportToFocusList(List<long> gesCaseReportIds, long individualId);
        bool RemoveListGesCaseReportsG_IndividualsService(List<long> gesCaseReportIds, long individualId);
    }
}
