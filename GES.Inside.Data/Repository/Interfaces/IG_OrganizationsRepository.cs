using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Models;
using System.Collections.Generic;

namespace GES.Inside.Data.Repository.Interfaces
{
    public interface IG_OrganizationsRepository : IGenericRepository<G_Organizations>
    {
        G_Organizations GetById(long id);

        IEnumerable<ClientViewModel> GetAllClients();

        IEnumerable<ClientViewModel> GetAllOrganizations();
    }
}
