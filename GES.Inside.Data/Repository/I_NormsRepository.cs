using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Repository.Interfaces;
using GES.Common.Logging;
using System.Collections.Generic;
using System.Linq;

namespace GES.Inside.Data.Repository
{
    public class I_NormsRepository : GenericRepository<I_Norms>, II_NormsRepository
    {
        public I_NormsRepository(GesEntities context, IGesLogger logger) : base(context, logger)
        {
        }


        public I_Norms GetById(long normModelNormsId)
        {
            return this.SafeExecute<I_Norms>(() =>
                _entities.Set<I_Norms>().FirstOrDefault(d => d.I_Norms_Id == normModelNormsId));
        }
    }
}
