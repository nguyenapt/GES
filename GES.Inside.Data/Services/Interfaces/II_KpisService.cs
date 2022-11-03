using System.Collections.Generic;
using GES.Common.Models;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Models;

namespace GES.Inside.Data.Services.Interfaces
{
    public interface II_KpisService : IEntityService<I_Kpis>
    {
        I_Kpis GetById(long id);
        IEnumerable<I_Kpis> GetKpisByEngagementTypesId(long engagementTypesId);
    }
}
