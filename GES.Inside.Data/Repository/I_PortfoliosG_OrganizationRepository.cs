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
    public class I_PortfoliosG_OrganizationRepository: GenericRepository<I_PortfoliosG_Organizations>, II_PortfoliosG_OrganizationRepository
    {

        public I_PortfoliosG_OrganizationRepository(GesEntities context, IGesLogger logger)
           : base(context, logger)
        {

        }

        public I_PortfoliosG_Organizations GetById(long id)
        {
            return this.SafeExecute<I_PortfoliosG_Organizations>(() => _entities.Set<I_PortfoliosG_Organizations>().FirstOrDefault(d => d.I_PortfoliosG_Organizations_Id == id));
        }

        public List<I_PortfoliosG_Organizations> GetByPortfolioIdAndOrganizationId(long portfolioId, long orgId)
        {
            return this.SafeExecute<List<I_PortfoliosG_Organizations>>(() => _entities.Set<I_PortfoliosG_Organizations>().Where(d => d.I_Portfolios_Id == portfolioId && d.G_Organizations_Id == orgId).ToList());
        }
    }
}
