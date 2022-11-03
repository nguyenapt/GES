using GES.Inside.Data.DataContexts;
using System.Collections;
using System.Collections.Generic;
using GES.Inside.Data.Models;

namespace GES.Inside.Data.Repository.Interfaces
{
    public interface II_KpisRepository : IGenericRepository<I_Kpis>
    {
        I_Kpis GetById(long id);

        IEnumerable<I_Kpis> GetKpisByEngagementTypeId(long engagementTypeId);
    }
}
