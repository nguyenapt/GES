using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GES.Inside.Data.DataContexts;

namespace GES.Inside.Data.Repository.Interfaces
{
    public interface II_PortfoliosG_OrganizationG_ServicesRepository : IGenericRepository<I_PortfoliosG_OrganizationsG_Services>
    {
        I_PortfoliosG_OrganizationsG_Services GetById(long id);

        IQueryable<I_PortfoliosG_OrganizationsG_Services> GetByPortfolioOrganizationId(long id);

    }
}
