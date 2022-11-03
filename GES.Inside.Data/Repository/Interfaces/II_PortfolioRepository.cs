using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GES.Inside.Data.DataContexts;

namespace GES.Inside.Data.Repository.Interfaces
{
    public interface II_PortfolioRepository:IGenericRepository<I_Portfolios>
    {
        I_Portfolios GetById(long id);
    }
}
