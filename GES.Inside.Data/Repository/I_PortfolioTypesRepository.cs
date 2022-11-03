using System.Data.Entity;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Repository.Interfaces;
using GES.Common.Logging;

namespace GES.Inside.Data.Repository
{
    public class I_PortfolioTypesRepository : GenericRepository<I_PortfolioTypes>, II_PortfolioTypesRepository
    {
        public I_PortfolioTypesRepository(GesEntities context, IGesLogger logger)
           : base(context, logger)
        {

        }
    }
}
