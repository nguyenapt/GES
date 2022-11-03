using System.Collections.Generic;
using GES.Inside.Data.DataContexts;

namespace GES.Inside.Data.Repository.Interfaces
{
    public interface II_GesCaseReportsG_IndividualsRepository : IGenericRepository<I_GesCaseReportsG_Individuals>
    {
        I_GesCaseReportsG_Individuals GetById(long id);
        void AddBatch(List<I_GesCaseReportsG_Individuals> entities);
    }
}
