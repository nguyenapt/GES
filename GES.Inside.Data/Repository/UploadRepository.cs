using System.Data.Entity;
using System.Linq;
using GES.Common.Logging;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Repository.Interfaces;

namespace GES.Inside.Data.Repository
{
    public class UploadRepository : GenericRepository<G_Uploads>, IUploadRepository
    {
        private readonly GesEntities _dbContext;

        public UploadRepository(GesEntities context, IGesLogger logger) : base(context, logger)
        {
            _dbContext = context;
        }

        public G_Uploads GetByUploadId(long id)
        {
            return SafeExecute(() => _dbContext.G_Uploads.FirstOrDefault(x => x.G_Uploads_Id == id));
        }

        public G_Uploads GetByDocumentId(long documentId)
        {
            return SafeExecute(() => _dbContext.G_Uploads.FirstOrDefault(x => x.G_ManagedDocuments_Id == documentId));
        }
    }
}
