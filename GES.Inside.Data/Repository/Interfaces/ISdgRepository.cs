using System.Collections.Generic;
using GES.Inside.Data.DataContexts;

namespace GES.Inside.Data.Repository.Interfaces
{
    public interface ISdgRepository : IGenericRepository<Sdg>
    {
        IEnumerable<Sdg> GetSdgs();
        IEnumerable<Sdg> GetSdgsByOrder(IList<long> sdgIds);
        Sdg GetById(long id);
        bool TryDeleteSdg(long sdgId);
    }
}
