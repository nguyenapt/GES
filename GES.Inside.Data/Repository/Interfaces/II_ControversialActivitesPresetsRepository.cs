using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Models;
using System.Collections.Generic;

namespace GES.Inside.Data.Repository.Interfaces
{
    public interface II_ControversialActivitesPresetsRepository : IGenericRepository<I_ControversialActivitesPresets>
    {
        I_ControversialActivitesPresets GetById(long id);

        IEnumerable<Models.Portfolio.PortfolioControActivityViewModel> GetPortfolioControActivityViewModelByPreset(long presetId);

        IEnumerable<IdNameModelPresetsExtendedModel> SearchControversialActivitesPresetsWithItsItems(string term);
    }
}
