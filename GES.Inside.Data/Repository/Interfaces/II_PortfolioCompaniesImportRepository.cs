using System.Collections.Generic;
using GES.Inside.Data.DataContexts;

namespace GES.Inside.Data.Repository.Interfaces
{
    public interface II_PortfolioCompaniesImportRepository : IGenericRepository<I_PortfolioCompaniesImport>
    {
        void AddBatch(List<I_PortfolioCompaniesImport> entities);
    }
}
