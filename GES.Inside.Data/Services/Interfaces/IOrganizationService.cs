using System;
using System.Collections.Generic;
using GES.Inside.Data.DataContexts;
using GES.Common.Models;
using GES.Inside.Data.Models;

namespace GES.Inside.Data.Services.Interfaces
{
    public interface IOrganizationService : IEntityService<G_Organizations>
    {
        PaginatedResults<ClientViewModel> GetClients(JqGridViewModel jqGridParams, bool isAllOrganizations = false);

        G_Organizations GetById(long id);

        string[] GetClientStatuses();

        string[] GetClientProgressStatuses();

        string[] GetIndustries();
        
        IEnumerable<G_Industries> GetIndustriesObject();

        string[] GetCountries();

        long[] GetServicesAgreementByOrganizationId(long organizationId);

        List<ClientViewModel> GetAllClients();
    }
}
