using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GES.Inside.Data.DataContexts;

namespace GES.Inside.Data.Repository.Interfaces
{
    public interface II_PortfoliosG_OrganizationRepository : IGenericRepository<I_PortfoliosG_Organizations>
    {
        I_PortfoliosG_Organizations GetById(long id);

        List<I_PortfoliosG_Organizations> GetByPortfolioIdAndOrganizationId(long portfolioId, long orgId);
    }
}
