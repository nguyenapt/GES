using System.Collections.Generic;
using GES.Common.Models;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Models;

namespace GES.Inside.Data.Services.Interfaces
{
    public interface IG_OrganizationsG_ServicesService : IEntityService<G_OrganizationsG_Services>
    {
        G_OrganizationsG_Services GetById(long id);
        IEnumerable<OrganizationsServicesViewModel> GetOrganizationsServices(long orgId);
    }
}
