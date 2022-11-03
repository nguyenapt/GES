using System.Linq;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Models;

namespace GES.Inside.Data.Repository.Interfaces
{
    public interface IG_ServicesRepository : IGenericRepository<G_Services>
    {
        G_Services GetById(long id);
        IQueryable<ServicesModel> GetGesServices();
    }
}
