using System.Collections.Generic;
using System.Linq;
using GES.Common.Models;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Models;

namespace GES.Inside.Data.Services.Interfaces
{
    public interface II_ControversialActivitesPresetsItemsService : IEntityService<I_ControversialActivitesPresetsItems>
    {
        IQueryable<I_ControversialActivitesPresetsItems> GetPresetItemsByPresetId(long controActivPresetId);
    }
}

