using System.Linq;
using GES.Common.Logging;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Repository.Interfaces;

namespace GES.Inside.Data.Repository
{
    public class GesCaseReportsManagedDocumentsRepository : GenericRepository<I_GesCaseReportsG_ManagedDocuments>, IGesCaseReportsManagedDocumentsRepository
    {
        private readonly GesEntities _dbContext;

        public GesCaseReportsManagedDocumentsRepository(GesEntities context, IGesLogger logger) : base(context, logger)
        {
            _dbContext = context;
        }

        public I_GesCaseReportsG_ManagedDocuments GetGesCaseReportsManagedDocuments(long documentIds)
        {
            return _dbContext.I_GesCaseReportsG_ManagedDocuments
                .FirstOrDefault(x => x.G_ManagedDocuments_Id == documentIds);
        }
    }
}
