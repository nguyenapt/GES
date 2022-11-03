using System.Linq;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Repository.Interfaces;
using GES.Inside.Data.Services.Interfaces;
using GES.Common.Logging;

namespace GES.Inside.Data.Services
{
    public class I_ControversialActivitesPresetsItemsService : EntityService<GesEntities, I_ControversialActivitesPresetsItems>, II_ControversialActivitesPresetsItemsService
    {
        private readonly II_ControversialActivitesPresetsItemsRepository _controActivPresetsItemsRepository;

        public I_ControversialActivitesPresetsItemsService(IUnitOfWork<GesEntities> unitOfWork,
            II_ControversialActivitesPresetsItemsRepository controActivPresetsItemsRepository, IGesLogger logger)
            : base(unitOfWork, logger, controActivPresetsItemsRepository)
        {
            _controActivPresetsItemsRepository = controActivPresetsItemsRepository;
        }

        public IQueryable<I_ControversialActivitesPresetsItems> GetPresetItemsByPresetId(long controActivPresetId)
        {
            return this.SafeExecute<IQueryable<I_ControversialActivitesPresetsItems>>(() => _controActivPresetsItemsRepository.GetByControActivPresetId(controActivPresetId));
        }        
    }
}
