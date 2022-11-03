using GES.Inside.Data.DataContexts;
using System.Collections.Generic;

namespace GES.Inside.Data.Repository.Interfaces
{
    public interface II_NormsRepository : IGenericRepository<I_Norms>
    {
        I_Norms GetById(long normModelNormsId);
    }
}
