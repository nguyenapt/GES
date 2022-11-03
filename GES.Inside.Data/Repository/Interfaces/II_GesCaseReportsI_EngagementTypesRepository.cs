using System.Collections.Generic;
using GES.Inside.Data.DataContexts;

namespace GES.Inside.Data.Repository.Interfaces
{
    public interface II_GesCaseReportsI_EngagementTypesRepository : IGenericRepository<I_GesCaseReportsI_EngagementTypes>
    {
        I_GesCaseReportsI_EngagementTypes FindById(long id);

        IEnumerable<I_GesCaseReportsI_EngagementTypes> GetByCaseProfileId(long caseProfileId);
    }
}