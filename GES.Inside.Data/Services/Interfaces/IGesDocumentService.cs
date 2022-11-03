using System;
using System.Collections.Generic;
using System.IO;
using GES.Common.Models;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Models;
using GES.Inside.Data.Models.Reports;

namespace GES.Inside.Data.Services.Interfaces
{
    public interface IGesDocumentService : IEntityService<DataContexts.GesDocument>
    {
        PaginatedResults<GesDocumentViewModel> GetDocumentsForGrid(JqGridViewModel jqGridParams);

        bool TryCreateUpdateGesDocument(DataContexts.GesDocument gesDocument);

        GesDocument GetGesDocumentById(Guid documentId);

        List<DataContexts.GesDocumentService> GetListGesDocumentServices();

        void DeleteRange(Guid[] documentIds);

        List<G_Organizations_GesDocument> GetOrgDocumentByDocumentId(Guid documentId);
        
        IEnumerable<ReportViewModel> GetDocumentByOrgId(long orgId, long serviceId);

        IEnumerable<ReportViewModel> GetAnualReport(long orgId);

        IEnumerable<ReportViewModel> GetPositionReport(long orgId);

        Dictionary<string, IEnumerable<ReportViewModel>> GetQuarterlyReport(long orgId);

        ReportViewModel GetDocumentById(long orgId, System.Guid documentId);

        bool SaveGesDocument(GesDocumentViewModel gesDocument, Stream fileStream, string fileName);
    }
}
