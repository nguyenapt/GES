using System;
using System.Collections.Generic;
using System.IO;
using GES.Common.Models;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Models;
using GES.Inside.Data.Models.Reports;

namespace GES.Inside.Data.Services.Interfaces
{
    public interface IG_ManagedDocumentsService : IEntityService<DataContexts.G_ManagedDocuments>
    {
        PaginatedResults<DocumentViewModel> GetCompanyDocumentsForGrid(JqGridViewModel jqGridParams);

        DocumentViewModel GetDocumentById(long documentId);
        G_ManagedDocuments SaveGesDocument(DocumentViewModel document, Stream fileStream, string fileName);
        void DeleteRange(long[] documentIds);
    }
}
