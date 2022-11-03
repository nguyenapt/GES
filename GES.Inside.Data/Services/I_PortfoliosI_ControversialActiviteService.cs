using System.Collections.Generic;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Repository.Interfaces;
using GES.Inside.Data.Services.Interfaces;
using GES.Common.Logging;

namespace GES.Inside.Data.Services
{
    public class I_PortfoliosI_ControversialActiviteService : EntityService<GesEntities, I_PortfoliosI_ControversialActivites>, II_PortfoliosI_ControversialActiviteService
    {
        private readonly II_PortfoliosI_ControversialActivitesRepository _portfolioControversialRepository;

        public I_PortfoliosI_ControversialActiviteService(IUnitOfWork<GesEntities> unitOfWork,
            II_PortfoliosI_ControversialActivitesRepository portfolioControversialRepository, IGesLogger logger)
            : base(unitOfWork, logger, portfolioControversialRepository)
        {
            _portfolioControversialRepository = portfolioControversialRepository;
        }

        public IEnumerable<I_PortfoliosI_ControversialActivites> GetByPortfolioId(long portfolioId)
        {
            return this.SafeExecute(() => _portfolioControversialRepository.FindBy(x => x.I_Portfolios_Id == portfolioId));
        }
    }
}
