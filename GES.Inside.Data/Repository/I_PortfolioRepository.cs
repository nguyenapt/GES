using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Repository.Interfaces;
using GES.Common.Logging;

namespace GES.Inside.Data.Repository
{
    public class I_PortfolioRepository : GenericRepository<I_Portfolios>, II_PortfolioRepository
    {
        public I_PortfolioRepository(GesEntities context, IGesLogger logger)
           : base(context, logger)
        {

        }

        public I_Portfolios GetById(long id)
        {
            return this.SafeExecute<I_Portfolios>(() => _entities.Set<I_Portfolios>().FirstOrDefault(d => d.I_Portfolios_Id == id)) ;
        }
    }
}
