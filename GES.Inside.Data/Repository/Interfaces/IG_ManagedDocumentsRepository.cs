using System.Collections.Generic;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Models;

namespace GES.Inside.Data.Repository.Interfaces
{
    public interface IG_ManagedDocumentsRepository : IGenericRepository<G_ManagedDocuments>
    {
        G_ManagedDocuments GetById(long id);

        IEnumerable<DocumentViewModel> GetAdditionalDocuments(long caseProfileId);

        IEnumerable<DocumentViewModel> GetUploadedDocuments(long companyId);
        DocumentViewModel GetUploadedDocumentById(long documentId);
        DocumentViewModel GetDocumentByCompanySourceDialogId(long companySourceDialogId, string dialogType);
        IEnumerable<DocumentViewModel> GetDocumentViewModel();
        void DeleteRange(long[] documentIds);
    }
}
