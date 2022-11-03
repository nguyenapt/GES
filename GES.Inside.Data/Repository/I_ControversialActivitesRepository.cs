using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Repository.Interfaces;
using GES.Common.Logging;

namespace GES.Inside.Data.Repository
{
    public class I_ControversialActivitesRepository : GenericRepository<I_ControversialActivites>, II_ControversialActivitesRepository
    {
        public I_ControversialActivitesRepository(GesEntities context, IGesLogger logger)
           : base(context, logger)
        {

        }

        public I_ControversialActivites GetById(long id)
        {
            return this.SafeExecute<I_ControversialActivites>(() => _entities.Set<I_ControversialActivites>().FirstOrDefault(d => d.I_ControversialActivites_Id == id));
        }

    }
}
