using System.Collections.Generic;
using GES.Inside.Data.DataContexts;

namespace GES.Inside.Data.Services.Interfaces
{
    public interface II_PortfoliosG_OrganizationG_ServicesService : IEntityService<I_PortfoliosG_OrganizationsG_Services>
    {
        I_PortfoliosG_OrganizationsG_Services GetById(long id);

        bool UpdatePortfolioOrganizationServices(long portfolioOrganizationId, long[] serviceIds);
    }
}