using GES.Inside.Data.DataContexts;

namespace GES.Inside.Data.Repository.Interfaces
{
    public interface IUploadRepository : IGenericRepository<G_Uploads>
    {
        G_Uploads GetByUploadId(long id);
        G_Uploads GetByDocumentId(long documentId);
    }
}
