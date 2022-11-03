using System.Data.Entity;
using System.Linq;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Repository.Interfaces;
using GES.Common.Logging;

namespace GES.Inside.Data.Repository
{
    public class I_ControversialActivitesPresetsItemsRepository : GenericRepository<I_ControversialActivitesPresetsItems>, II_ControversialActivitesPresetsItemsRepository
    {
        public I_ControversialActivitesPresetsItemsRepository(GesEntities context, IGesLogger logger)
           : base(context, logger)
        {

        }

        public I_ControversialActivitesPresetsItems GetById(long id)
        {
            return this.SafeExecute<I_ControversialActivitesPresetsItems>(() => _entities.Set<I_ControversialActivitesPresetsItems>().FirstOrDefault(d => d.I_ControversialActivitesPresetsItems_Id == id));
        }
        
        public IQueryable<I_ControversialActivitesPresetsItems> GetByControActivPresetId(long controActivPresetId)
        {
            return this.SafeExecute<IQueryable<I_ControversialActivitesPresetsItems>>(() => _entities.Set<I_ControversialActivitesPresetsItems>()
                    .Where(i => i.I_ControversialActivitesPresets_Id == controActivPresetId));
        }
    }
}
