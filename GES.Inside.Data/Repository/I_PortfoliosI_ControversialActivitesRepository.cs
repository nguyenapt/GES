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
    public class I_PortfoliosI_ControversialActivitesRepository : GenericRepository<I_PortfoliosI_ControversialActivites>, II_PortfoliosI_ControversialActivitesRepository
    {
        public I_PortfoliosI_ControversialActivitesRepository(GesEntities context, IGesLogger logger)
           : base(context, logger)
        {

        }

        public I_PortfoliosI_ControversialActivites GetById(long id)
        {
            return this.SafeExecute<I_PortfoliosI_ControversialActivites>(() => _entities.Set<I_PortfoliosI_ControversialActivites>().FirstOrDefault(d => d.I_PortfoliosI_ControversialActivites_Id == id));
        }
    }
}
