using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GES.Inside.Data.DataContexts;

namespace GES.Inside.Data.Repository.Interfaces
{
    public interface II_PortfoliosI_CompaniesRepository : IGenericRepository<I_PortfoliosI_Companies>
    {
        void AddBatch(List<I_PortfoliosI_Companies> entities);
    }
}
