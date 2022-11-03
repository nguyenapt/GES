using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Threading.Tasks;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Repository.Interfaces;
using GES.Common.Logging;

namespace GES.Inside.Data.Repository
{
    public class I_PortfoliosG_OrganizationG_ServicesRepository : GenericRepository<I_PortfoliosG_OrganizationsG_Services>, II_PortfoliosG_OrganizationG_ServicesRepository
    {

        public I_PortfoliosG_OrganizationG_ServicesRepository(GesEntities context, IGesLogger logger)
           : base(context, logger)
        {

        }

        public I_PortfoliosG_OrganizationsG_Services GetById(long id)
        {
            return this.SafeExecute<I_PortfoliosG_OrganizationsG_Services>(() => _entities.Set<I_PortfoliosG_OrganizationsG_Services>().FirstOrDefault(d => d.I_PortfoliosG_OrganizationsG_Services_Id == id));
        }

        public IQueryable<I_PortfoliosG_OrganizationsG_Services> GetByPortfolioOrganizationId(long portfolioOrganizationId)
        {
            return
                this.SafeExecute<IQueryable<I_PortfoliosG_OrganizationsG_Services>>(() => _entities.Set<I_PortfoliosG_OrganizationsG_Services>()
                    .Where(i => i.I_PortfoliosG_Organizations_Id == portfolioOrganizationId));
        }

    }
}
