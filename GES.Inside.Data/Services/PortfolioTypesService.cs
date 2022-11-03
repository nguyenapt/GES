using System.Linq;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Repository.Interfaces;
using GES.Inside.Data.Services.Interfaces;
using GES.Common.Logging;

namespace GES.Inside.Data.Services
{
    public class PortfolioTypesService: EntityService<GesEntities, I_PortfolioTypes>, IPortfolioTypesService
    {
        private readonly GesEntities _dbContext;
        private readonly II_PortfolioTypesRepository _portfolioTypesRepository;

        public PortfolioTypesService(IUnitOfWork<GesEntities> unitOfWork, II_PortfolioTypesRepository portfolioTypesRepository, IGesLogger logger)
            : base(unitOfWork, logger, portfolioTypesRepository)
        {
            _dbContext = unitOfWork.DbContext;
            _portfolioTypesRepository = portfolioTypesRepository;
        }

        public I_PortfolioTypes GetByName(string name)
        {
            return this.SafeExecute(() => _portfolioTypesRepository.FindBy(x => x.Name == name).FirstOrDefault());
        }

    }
}
