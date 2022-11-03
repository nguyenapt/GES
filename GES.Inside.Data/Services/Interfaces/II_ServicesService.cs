using System.Collections.Generic;
using GES.Common.Models;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Models;

namespace GES.Inside.Data.Services.Interfaces
{
    public interface II_ServicesService : IEntityService<G_Services>
    {
        G_Services GetById(long id);
        PaginatedResults<ServicesModel> GetGesServices(JqGridViewModel jqGridParams);
        IEnumerable<ServicesModel> GetGesServices();
    }
}
