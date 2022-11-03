using System.Collections.Generic;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Models;

namespace GES.Inside.Data.Services.Interfaces
{
    public interface IPermissionService : IEntityService<G_OrganizationsG_Services>
    {
        List<G_OrganizationsG_Services> GetOrganizationsG_ServicesByOrgId(long orgId);
    }
}
