using System.Collections.Generic;
using GES.Inside.Data.DataContexts;

namespace GES.Inside.Data.Repository.Interfaces
{
    public interface II_GesCaseReportsExtraRepository : IGenericRepository<I_GesCaseReportsExtra>
    {
        void AddBatch(List<I_GesCaseReportsExtra> entities);
    }
}
