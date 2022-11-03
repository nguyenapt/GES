using System.Linq;
using GES.Inside.Data.DataContexts;

namespace GES.Inside.Data.Repository.Interfaces
{
    public interface II_ControversialActivitesPresetsItemsRepository : IGenericRepository<I_ControversialActivitesPresetsItems>
    {
        I_ControversialActivitesPresetsItems GetById(long id);
        IQueryable<I_ControversialActivitesPresetsItems> GetByControActivPresetId(long controActivPresetId);
    }
}    
