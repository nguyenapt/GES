using GES.Inside.Data.DataContexts;
using System.Collections.Generic;
using System.Linq;
using GES.Inside.Data.Models;

namespace GES.Inside.Data.Repository.Interfaces
{
    public interface IG_OrganizationsG_ServicesRepository : IGenericRepository<G_OrganizationsG_Services>
    {
        G_OrganizationsG_Services GetById(long id);
        IEnumerable<long> GetSubscribedServicesOfIndividual(long individualId);

        IEnumerable<G_OrganizationsG_Services> GetOrganizationServices(long organizationId);
        IQueryable<OrganizationsServicesViewModel> GetOrganizationServicesViewModel(long orgId);
    }
}
