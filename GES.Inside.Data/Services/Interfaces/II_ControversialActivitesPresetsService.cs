using System.Collections.Generic;
using GES.Common.Models;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Models;
using GES.Inside.Data.Models.Portfolio;

namespace GES.Inside.Data.Services.Interfaces
{
    public interface II_ControversialActivitesPresetsService : IEntityService<I_ControversialActivitesPresets>
    {
        I_ControversialActivitesPresets GetById(long presetId);

        PaginatedResults<PortfolioControActivityViewModel> GetControActivitiesPresetItem(JqGridViewModel jqGridParams, long presetId);
        List<IdNameModelPresetsExtendedModel> GetPresetsWithTerm(string term, int limit);

    }
}
