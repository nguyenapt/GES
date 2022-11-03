using System.Data.Entity;
using System.Linq;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Repository.Interfaces;
using GES.Common.Logging;
using GES.Inside.Data.Models.Portfolio;
using System.Collections.Generic;
using GES.Inside.Data.Models;

namespace GES.Inside.Data.Repository
{
    public class I_ControversialActivitesPresetsRepository : GenericRepository<I_ControversialActivitesPresets>, II_ControversialActivitesPresetsRepository
    {
        protected readonly GesEntities _dbContext;

        public I_ControversialActivitesPresetsRepository(GesEntities context, IGesLogger logger)
           : base(context, logger)
        {
            _dbContext = context;
        }

        public I_ControversialActivitesPresets GetById(long id)
        {
            return this.SafeExecute<I_ControversialActivitesPresets>(() => _entities.Set<I_ControversialActivitesPresets>().FirstOrDefault(d => d.I_ControversialActivitesPresets_Id == id));
        }

        public IEnumerable<PortfolioControActivityViewModel> GetPortfolioControActivityViewModelByPreset(long presetId)
        {
            return from c in _dbContext.I_ControversialActivites
                   join poc in _dbContext.I_ControversialActivitesPresetsItems on c.I_ControversialActivites_Id equals poc.I_ControversialActivites_Id
                   into co
                   from poc in (from poc in co where poc.I_ControversialActivitesPresets_Id == presetId select poc).DefaultIfEmpty()
                   select new PortfolioControActivityViewModel()
                   {
                       ControActivId = c.I_ControversialActivites_Id,
                       ControActivName = c.Name,
                       Threshold = poc.MinimumInvolvment
                   };
        }

        public IEnumerable<IdNameModelPresetsExtendedModel> SearchControversialActivitesPresetsWithItsItems(string term)
        {

            var query = from o in _dbContext.I_ControversialActivitesPresets
                                join i in _dbContext.I_ControversialActivitesPresetsItems on o.I_ControversialActivitesPresets_Id equals i.I_ControversialActivitesPresets_Id
                                select new IdNameModelPresetsExtendedModel()
                                {
                                    Id = o.I_ControversialActivitesPresets_Id,
                                    Name = o.Name,
                                    Items = o.I_ControversialActivitesPresetsItems.ToList().Select(x => new BasicControActivPresetItemModel()
                                    {
                                        Id = x.I_ControversialActivites_Id,
                                        Threshold = x.MinimumInvolvment
                                    })
                                }; 

            if(!string.IsNullOrWhiteSpace(term))
            {
                query.Where(item => item.Name.Contains(term));
            }

            return query;
        }
    }
}
