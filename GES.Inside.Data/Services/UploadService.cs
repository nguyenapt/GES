using GES.Common.Logging;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Repository.Interfaces;
using GES.Inside.Data.Services.Interfaces;

namespace GES.Inside.Data.Services
{
    public class UploadService : EntityService<GesEntities, G_Uploads>, IUploadService
    {
        private readonly IUploadRepository _uploadRepository;
        public UploadService(IUnitOfWork unitOfWork, IGesLogger logger, IGenericRepository<G_Uploads> repository, IUploadRepository uploadRepository) : base(unitOfWork, logger, repository)
        {
            _uploadRepository = uploadRepository;
        }

        public G_Uploads GetByUploadId(long id)
        {
            return _uploadRepository.GetByUploadId(id);
        }

        public G_Uploads GetByDocumentId(long documentId)
        {
            return _uploadRepository.GetByDocumentId(documentId);
        }

        public bool RemoveCompanyDocumentByDocumentId(long documentGManagedDocumentsId)
        {
            this.SafeExecute(() => this._uploadRepository.BatchDelete(d => d.G_ManagedDocuments_Id == documentGManagedDocumentsId));

            return true;
        }
    }
}
