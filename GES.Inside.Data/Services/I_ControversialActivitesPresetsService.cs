using System.Collections.Generic;
using System.Linq;
using GES.Common.Models;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Models;
using GES.Inside.Data.Models.Portfolio;
using GES.Inside.Data.Repository.Interfaces;
using GES.Inside.Data.Services.Interfaces;
using GES.Common.Logging;

namespace GES.Inside.Data.Services
{
    public class I_ControversialActivitesPresetsService : EntityService<GesEntities, I_ControversialActivitesPresets>, II_ControversialActivitesPresetsService
    {
        private readonly GesEntities _dbContext;
        private readonly II_ControversialActivitesPresetsRepository _controActivPresetsRepository;

        public I_ControversialActivitesPresetsService(IUnitOfWork<GesEntities> unitOfWork,
            II_ControversialActivitesPresetsRepository controActivPresetsRepository, IGesLogger logger)
            : base(unitOfWork, logger, controActivPresetsRepository)
        {
            _dbContext = unitOfWork.DbContext;
            _controActivPresetsRepository = controActivPresetsRepository;
        }

        public I_ControversialActivitesPresets GetById(long presetId)
        {
            return SafeExecute(() => _controActivPresetsRepository.GetById(presetId));
        }

        public PaginatedResults<PortfolioControActivityViewModel> GetControActivitiesPresetItem(JqGridViewModel jqGridParams, long presetId)
        {
            var query = this._controActivPresetsRepository.GetPortfolioControActivityViewModelByPreset(presetId);

            query = query.OrderByDescending(d => d.Threshold).ThenBy(d => d.ControActivId);

            var items = this.SafeExecute<List<PortfolioControActivityViewModel>>(query.ToList);

            return new PaginatedResults<PortfolioControActivityViewModel>(items, items.Count, jqGridParams.rows, jqGridParams.page);
        }
        
        public List<IdNameModelPresetsExtendedModel> GetPresetsWithTerm(string term, int limit)
        {
            var query = this._controActivPresetsRepository.SearchControversialActivitesPresetsWithItsItems(term);

            return this.SafeExecute(() => query.GroupBy(x => x.Id).Select(x => x.FirstOrDefault()).OrderBy(x => x.Name).Take(limit).ToList());
        }

    }
}
