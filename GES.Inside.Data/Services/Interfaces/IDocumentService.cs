using System.Collections.Generic;
using GES.Common.Enumeration;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Models;

namespace GES.Inside.Data.Services.Interfaces
{
    public interface IDocumentService : IEntityService<G_ManagedDocuments>
    {
        List<DocumentViewModel> GetDocumentsByCompanyId(long companyId);
        DocumentViewModel GetUploadedDocumentById(long documentId);
        G_ManagedDocuments GetDocumentById(long documentId);
        long TrySaveManagedDocument(DocumentViewModel document, RelatedDocumentMng relatedType);
        bool TryDeleteManagedDocument(long documentId);
        DocumentViewModel GetDocumentByCompanySourceDialogId(long companySourceDialogId, string dialogType);
    }
}
