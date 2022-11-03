using System.Collections.Generic;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Models;

namespace GES.Inside.Data.Services.Interfaces
{
    public interface II_GesCompanyWatcherService : IEntityService<I_GesCompanyWatcher>
    {
        bool RemoveGesCompanyWatcher(long gesCompanyId, long individualId);
        bool AddListCompanyToFocusList(List<long> gesCompanyIds, long individualId);
        bool RemoveListGesCompanyWatcher(List<long> gesCompanyIds, long individualId);
    }
}
