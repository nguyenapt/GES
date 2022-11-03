using System.Linq;
using GES.Common.Logging;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Repository.Interfaces;

namespace GES.Inside.Data.Repository
{
    public class CompaniesManagedDocumentsRepository : GenericRepository<I_CompaniesG_ManagedDocuments>, ICompaniesManagedDocumentsRepository
    {
        private readonly GesEntities _dbContext;

        public CompaniesManagedDocumentsRepository(GesEntities context, IGesLogger logger) : base(context, logger)
        {
            _dbContext = context;
        }

        public long GetMaxId()
        {
            return SafeExecute(() => _dbContext.I_CompaniesG_ManagedDocuments.OrderByDescending(x => x.I_CompaniesG_ManagedDocuments_Id)
                                    .Select(x => x.I_CompaniesG_ManagedDocuments_Id).FirstOrDefault());
        }

        public I_CompaniesG_ManagedDocuments GetCompaniesGManagedDocuments(long documentIds)
        {
            return _dbContext.I_CompaniesG_ManagedDocuments
                    .FirstOrDefault(x => x.G_ManagedDocuments_Id == documentIds);
        }
    }
}
