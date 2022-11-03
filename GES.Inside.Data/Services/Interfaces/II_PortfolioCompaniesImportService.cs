using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GES.Inside.Data.DataContexts;

namespace GES.Inside.Data.Services.Interfaces
{
    public interface II_PortfolioCompaniesImportService : IEntityService<I_PortfolioCompaniesImport>
    {
        IEnumerable<I_PortfolioCompaniesImport> GetPortfolioCompaniesImportByPortfolioId(long portfolioId);

        int RemovePortfolioCompaniesImportByListId(List<long> ids);
        void RemovePortfolioCompaniesImportByPortfolioId(long portfolioId);
        List<I_PortfolioCompaniesImport> GetPortfolioCompaniesImportByListId( List<long> ids);
        void AddBatch(List<I_PortfolioCompaniesImport> entities);
    }
}
