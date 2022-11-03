using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GES.Inside.Data.DataContexts;

namespace GES.Inside.Data.Repository.Interfaces
{
    public interface II_GesCompanyWatcherRepository : IGenericRepository<I_GesCompanyWatcher>
    {
        I_GesCompanyWatcher GetById(long id);
        void AddBatch(List<I_GesCompanyWatcher> entities);
    }
}
