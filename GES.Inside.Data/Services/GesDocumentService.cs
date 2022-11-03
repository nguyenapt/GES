using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Dynamic;
using GES.Common.Configurations;
using GES.Common.Models;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Models;
using GES.Inside.Data.Repository.Interfaces;
using GES.Inside.Data.Services.Interfaces;
using GES.Common.Enumeration;
using GES.Inside.Data.Models.Reports;
using GES.Common.Logging;
using GES.Common.Exceptions;
using GES.Inside.Data.Configs;
using GES.Inside.Data.Extensions;
using GES.Inside.Data.Helpers;

namespace GES.Inside.Data.Services
{
    public class GesDocumentService : EntityService<GesEntities, GesDocument>, IGesDocumentService
    {
        private readonly GesEntities _dbContext;
        private readonly IGesDocumentsRepository _documentsReporsitory;
        private readonly IOrganizationDocumentService _organizationDocumentService;

        public IGesFileStorageService GesFileStorageService { get; set; }//http://autofac.readthedocs.io/en/latest/advanced/circular-dependencies.html

        public GesDocumentService(IUnitOfWork<GesEntities> unitOfWork, IGesDocumentsRepository documentsReporsitory,
            IOrganizationDocumentService organizationDocumentService, IGesLogger logger)
            : base(unitOfWork, logger, documentsReporsitory)
        {
            _dbContext = unitOfWork.DbContext;
            _documentsReporsitory = documentsReporsitory;
            _organizationDocumentService = organizationDocumentService;
        }

        public PaginatedResults<GesDocumentViewModel> GetDocumentsForGrid(JqGridViewModel jqGridParams)
        {
            var documents = this._documentsReporsitory.GetDocumentViewModel().AsQueryable();

            var sortedColumn = jqGridParams.sidx.ToLower();
            var sortedDirection = jqGridParams.sord.ToLower();
            if (!(string.IsNullOrEmpty(sortedColumn) && string.IsNullOrEmpty(sortedDirection)))
            {
                switch (sortedColumn)
                {
                    case "name":
                        documents = sortedDirection == "asc"
                            ? documents.OrderBy(x => x.Name).ThenByDescending(d => d.Created)
                            : documents.OrderByDescending(x => x.Name).ThenBy(d => d.Created);
                        break;
                    case "comment":
                        documents = sortedDirection == "asc"
                            ? documents.OrderBy(x => x.Comment)
                            : documents.OrderByDescending(x => x.Comment);
                        break;
                    case "servicename":
                        documents = sortedDirection == "asc"
                            ? documents.OrderBy(x => x.ServiceName)
                            : documents.OrderByDescending(x => x.ServiceName);
                        break;
                    case "created":
                        documents = sortedDirection == "asc"
                            ? documents.OrderBy(x => x.Created)
                            : documents.OrderByDescending(x => x.Created);
                        break;
                    default:
                        documents = documents.OrderByDescending(d => d.Created);
                        break;
                }
            }

            if (jqGridParams._search)
            {
                var finalRules = JqGridHelper.GetFilterRules<GesDocumentViewModel>(jqGridParams);
                documents = String.IsNullOrEmpty(finalRules) ? documents : documents.Where(finalRules);
            }

            var result = documents.ToPagedList(Logger, jqGridParams.page, jqGridParams.rows);
            
            return result;
        }

        public bool SaveGesDocument(GesDocumentViewModel gesDocument, Stream fileStream, string fileName)
        {
            var hashCode = GesFileStorageService.StoreFile(fileStream, fileName, gesDocument.HashCodeDocument);

            if (!string.IsNullOrEmpty(hashCode))
            {
                //Save to file infor to database
                var isSavedSuccess = TryCreateUpdateGesDocument(new GesDocument
                {
                    DocumentId = gesDocument.Id,
                    Name = gesDocument.Name,
                    FileName = fileName,
                    GesDocumentServiceId = gesDocument.ServiceId,
                    Source = gesDocument.Source,
                    Comment = gesDocument.Comment,
                    Created = gesDocument.Created,
                    Metadata01 = gesDocument.Metadata01,
                    Metadata02 = gesDocument.Metadata02,
                    Metadata03 = gesDocument.Metadata03,
                    HashCodeDocument = hashCode
                });

                if (isSavedSuccess)
                {
                    _organizationDocumentService.RemoveOrganizationDocumentByDocumentId(gesDocument.Id);

                    if (gesDocument.SelectedOrganizations != null && gesDocument.SelectedOrganizations.Any())
                    {
                        var newOrgDocuments =
                            gesDocument.SelectedOrganizations.Select(
                                d =>
                                    new G_Organizations_GesDocument()
                                    {
                                        GesDocumentId = gesDocument.Id,
                                        G_Organizations_Id = long.Parse(d)
                                    }).ToList();
                        _organizationDocumentService.AddBatch(newOrgDocuments);
                    }
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

        public bool TryCreateUpdateGesDocument(GesDocument gesDocument)
        {
            UpdateDocument(gesDocument);
            _documentsReporsitory.Save();
            return true;
        }

        public GesDocument GetGesDocumentById(Guid documentId)
        {
            return _documentsReporsitory.GetById(documentId);
        }

        public List<DataContexts.GesDocumentService> GetListGesDocumentServices()
        {
            var query = from m in _dbContext.GesDocumentService
                        where !Settings.HiddenDocumentServices.Contains(m.Name)
                        select m;

            return query.OrderBy(d => d.SortOrder).ToList();
        }

        public void DeleteRange(Guid[] documentIds)
        {
            _documentsReporsitory.DeleteRange(documentIds);
            _documentsReporsitory.Save();
        }

        public List<G_Organizations_GesDocument> GetOrgDocumentByDocumentId(Guid documentId)
        {
            var query = from m in _dbContext.G_Organizations_GesDocument
                        where m.GesDocumentId != null && m.GesDocumentId == documentId
                        select m;

            return query.ToList();
        }

        #region Clients

        public IEnumerable<ReportViewModel> GetDocumentByOrgId(long orgId, long serviceId)
        {
            var query = from d in _dbContext.GesDocument
                        join s in _dbContext.GesDocumentService on d.GesDocumentServiceId equals s.GesDocumentServiceId
                        join od in _dbContext.G_Organizations_GesDocument on d.DocumentId equals od.GesDocumentId into god
                        from od in god.DefaultIfEmpty()

                        where d.GesDocumentServiceId != null && d.GesDocumentServiceId.Value == serviceId
                              && (od == null || (od.G_Organizations_Id != null && od.G_Organizations_Id.Value == orgId))

                        select new ReportViewModel()
                        {
                            DocumentId = d.DocumentId,
                            FileName = d.FileName,
                            Name = d.Name,
                            GesDocumentServiceId = d.GesDocumentServiceId,
                            Metadata01 = d.Metadata01,
                            Created = d.Created,
                            Modified = d.Modified
                        };

            return this.SafeExecute<IEnumerable<ReportViewModel>>(() => query != null ? query.ToList() : Enumerable.Empty<ReportViewModel>());
        }

        public IEnumerable<ReportViewModel> GetAnualReport(long orgId)
        {
            return this.GetDocumentByOrgId(orgId, (long)ReportEnum.Anual).OrderByDescending(r => r.Metadata01);
        }

        public IEnumerable<ReportViewModel> GetPositionReport(long orgId)
        {
            return this.GetDocumentByOrgId(orgId, (long)ReportEnum.Position).OrderByDescending(r => r.Modified).ThenByDescending(r => r.Created);
        }

        public Dictionary<string, IEnumerable<ReportViewModel>> GetQuarterlyReport(long orgId)
        {
            var anualReports = this.GetDocumentByOrgId(orgId, (long)ReportEnum.Quarterly).OrderByDescending(r => r.Metadata01);

            return anualReports != null && anualReports.Any()
                ? anualReports.GroupBy(x => x.Metadata01)
                              .ToDictionary(k => k.Key, x => x != null ? x.OrderByDescending(r => r.Name).AsEnumerable() : Enumerable.Empty<ReportViewModel>())
                : new Dictionary<string, IEnumerable<ReportViewModel>>();
        }

        public ReportViewModel GetDocumentById(long orgId, Guid documentId)
        {
            var query = from d in _dbContext.GesDocument
                        join od in _dbContext.G_Organizations_GesDocument on d.DocumentId equals od.GesDocumentId into god
                        from od in god.DefaultIfEmpty()

                        where d.GesDocumentServiceId != null && d.DocumentId == documentId
                              && (od == null || (od.G_Organizations_Id != null && od.G_Organizations_Id.Value == orgId))

                        select new ReportViewModel()
                        {
                            DocumentId = d.DocumentId,
                            FileName = d.FileName,
                            Name = d.Name,
                            GesDocumentServiceId = d.GesDocumentServiceId,
                            Metadata01 = d.Metadata01,
                            Created = d.Created,
                            Modified = d.Modified,
                            HashCodeDocument = d.HashCodeDocument
                        };

            var result = this.SafeExecute<ReportViewModel>(() => query.FirstOrDefault());
            if (result == null)
            {
                if (_documentsReporsitory.GetById(documentId) != null)
                {
                    throw new GesServiceException("You have not permission.");
                }
                else
                {
                    throw new GesServiceException("File not found.");
                }
            }

            return result;
        }

        #endregion

        #region Private & Sub-methods

        private void UpdateDocument(GesDocument gesDocument)
        {
            var oldgesdocument = _documentsReporsitory.GetById(gesDocument.DocumentId);
            if (oldgesdocument == null)
            {
                gesDocument.Created = DateTime.UtcNow;
                _documentsReporsitory.Add(gesDocument);
            }
            else
            {
                oldgesdocument.DocumentId = gesDocument.DocumentId;
                oldgesdocument.Name = gesDocument.Name;

                oldgesdocument.Comment = gesDocument.Comment;
                oldgesdocument.Source = gesDocument.Source;
                oldgesdocument.GesDocumentServiceId = gesDocument.GesDocumentServiceId;
                oldgesdocument.Modified = DateTime.UtcNow;
                oldgesdocument.Metadata01 = gesDocument.Metadata01;
                oldgesdocument.Metadata02 = gesDocument.Metadata02;
                oldgesdocument.Metadata03 = gesDocument.Metadata03;
                oldgesdocument.HashCodeDocument = gesDocument.HashCodeDocument;

                if (!string.IsNullOrWhiteSpace(gesDocument.FileName))
                {
                    oldgesdocument.FileName = gesDocument.FileName;
                }

                _documentsReporsitory.Edit(oldgesdocument);
            }
        }

        #endregion
    }
}