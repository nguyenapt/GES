using GES.Inside.Data.DataContexts;

namespace GES.Inside.Data.Services.Interfaces
{
    public interface IUploadService : IEntityService<G_Uploads>
    {
        G_Uploads GetByUploadId(long id);
        G_Uploads GetByDocumentId(long documentId);
        bool RemoveCompanyDocumentByDocumentId(long documentGManagedDocumentsId);
    }
}
