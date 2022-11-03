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
    public class I_PortfoliosG_OrganizationsI_ControversialActivitesRepository : GenericRepository<I_PortfoliosG_OrganizationsI_ControversialActivites>, II_PortfoliosG_OrganizationsI_ControversialActivitesRepository
    {
        public I_PortfoliosG_OrganizationsI_ControversialActivitesRepository(GesEntities context, IGesLogger logger)
           : base(context, logger)
        {

        }

        public I_PortfoliosG_OrganizationsI_ControversialActivites GetById(long id)
        {
            return this.SafeExecute<I_PortfoliosG_OrganizationsI_ControversialActivites>(() => _entities.Set<I_PortfoliosG_OrganizationsI_ControversialActivites>().FirstOrDefault(d => d.I_PortfoliosG_OrganizationsI_ControversialActivites_Id == id));
        }
    }
}
