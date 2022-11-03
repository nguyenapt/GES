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
    public class G_ManagedDocumentsService : EntityService<GesEntities, G_ManagedDocuments>, IG_ManagedDocumentsService
    {
        private readonly GesEntities _dbContext;
        private readonly IG_ManagedDocumentsRepository _gManagedDocumentsRepository;
        private readonly IOrganizationDocumentService _organizationDocumentService;
        private readonly IGesFileStorageService _gesFileStorageService;
        private readonly IUploadRepository _uploadRepository;
        private readonly ICompaniesManagedDocumentsRepository _companiesManagedDocumentsRepository;
        private readonly IGesCaseReportsManagedDocumentsRepository _caseReportsManagedDocumentsRepository;
        private readonly IUploadService _uploadService;

        //public IGesFileStorageService GesFileStorageService { get; set; }//http://autofac.readthedocs.io/en/latest/advanced/circular-dependencies.html

        public G_ManagedDocumentsService(IUnitOfWork<GesEntities> unitOfWork,
            IG_ManagedDocumentsRepository gManagedDocumentsRepository,
            IOrganizationDocumentService organizationDocumentService, IGesFileStorageService gesFileStorageService,
            IUploadService uploadService, ICompaniesManagedDocumentsRepository companiesManagedDocumentsRepository, 
            IGesCaseReportsManagedDocumentsRepository caseReportsManagedDocumentsRepository, IUploadRepository uploadRepository,
            IGesLogger logger)
            : base(unitOfWork, logger, gManagedDocumentsRepository)
        {
            _dbContext = unitOfWork.DbContext;
            _gManagedDocumentsRepository = gManagedDocumentsRepository;
            _organizationDocumentService = organizationDocumentService;
            _gesFileStorageService = gesFileStorageService;
            _uploadService = uploadService;
            _companiesManagedDocumentsRepository = companiesManagedDocumentsRepository;
            _uploadRepository = uploadRepository;
            _caseReportsManagedDocumentsRepository = caseReportsManagedDocumentsRepository;
        }

        public PaginatedResults<DocumentViewModel> GetCompanyDocumentsForGrid(JqGridViewModel jqGridParams)
        {
            var documents = this._gManagedDocumentsRepository.GetDocumentViewModel().AsQueryable();

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
                    case "companyname":
                        documents = sortedDirection == "asc"
                            ? documents.OrderBy(x => x.CompanyName)
                            : documents.OrderByDescending(x => x.CompanyName);
                        break;     
                    case "reportincident":
                        documents = sortedDirection == "asc"
                            ? documents.OrderBy(x => x.ReportIncident)
                            : documents.OrderByDescending(x => x.ReportIncident);
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

        public DocumentViewModel GetDocumentById(long documentId)
        {
           return this._gManagedDocumentsRepository.GetUploadedDocumentById(documentId);
        }

        public G_ManagedDocuments SaveGesDocument(DocumentViewModel document, Stream fileStream, string fileName)
        {
            
            var uploaded = _gesFileStorageService.StoreFile(fileStream, fileName);
            var docServiceId = document.I_GesCaseReports_Id != 0
                ? GManagedDocumentService.CaseProfile
                : GManagedDocumentService.ExternalDocument;

            var managedDocuments = new G_ManagedDocuments
            {
                G_ManagedDocuments_Id = document.G_ManagedDocuments_Id,
                Name = document.Name,
                Comment = document.Comment,
                Source = "",
                G_ManagedDocumentServices_Id = (long?) docServiceId,

            };
                //Save to file infor to database
            var id = TryCreateUpdateCompanyDocument(managedDocuments);
      
            if ( uploaded && id != 0)
            {
                _uploadRepository.BatchDelete(d => d.G_ManagedDocuments_Id == document.G_ManagedDocuments_Id);
                _uploadRepository.Save();

                try
                {
                    _uploadRepository.Add(new G_Uploads()
                    {
                        G_ManagedDocuments_Id = id,
                        FileName = fileName,
                        Created = DateTime.UtcNow
                    });
                    _uploadRepository.Save();

                    if (document.I_Companies_Id != null && document.I_Companies_Id != 0)
                    {
                        _companiesManagedDocumentsRepository.Add(new I_CompaniesG_ManagedDocuments()
                        {
                            I_Companies_Id = (long) document.I_Companies_Id,
                            G_ManagedDocuments_Id = id

                        });
                        _companiesManagedDocumentsRepository.Save();
                    }
                    
                    if (document.I_GesCaseReports_Id != null && document.I_GesCaseReports_Id != 0)
                    {
                        _caseReportsManagedDocumentsRepository.Add(new I_GesCaseReportsG_ManagedDocuments()
                        {
                            I_GesCaseReports_Id = (long) document.I_GesCaseReports_Id,
                            G_ManagedDocuments_Id = id

                        });
                        _companiesManagedDocumentsRepository.Save();
                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }

            return managedDocuments;
        }
        
        
        private long TryCreateUpdateCompanyDocument(G_ManagedDocuments document)
        {
            var id = UpdateDocument(document);
             
            return id;
        }
        
        private long UpdateDocument(G_ManagedDocuments document)
        {
            var oldDocument = _gManagedDocumentsRepository.GetById(document.G_ManagedDocuments_Id);
            long id = 0;
            if (oldDocument == null)
            {
                document.Created = DateTime.UtcNow;
                _gManagedDocumentsRepository.Add(document);
                _gManagedDocumentsRepository.Save();
                id =  document.G_ManagedDocuments_Id;
            }
            else
            {
                if (oldDocument.Comment != null && oldDocument.Name != null && oldDocument.Comment.Equals(document.Comment) && oldDocument.Name.Equals(document.Name)) return id;
                
                oldDocument.G_ManagedDocuments_Id = document.G_ManagedDocuments_Id;
                oldDocument.Name = document.Name;
                oldDocument.Comment = document.Comment;
                oldDocument.Source = document.Source;
                oldDocument.G_ManagedDocumentServices_Id = document.G_ManagedDocumentServices_Id;
                oldDocument.Modified = DateTime.UtcNow;

                _gManagedDocumentsRepository.Edit(oldDocument);
                _gManagedDocumentsRepository.Save();
                id =  oldDocument.G_ManagedDocuments_Id;

            }

            return id;
        }
        
        public void DeleteRange(long[] documentIds)
        {
            var docId = documentIds[0];
            _uploadRepository.BatchDelete(d => d.G_ManagedDocuments_Id == docId);
            _uploadRepository.Save();
            
            var caseReportsManagedDocuments =
                _caseReportsManagedDocumentsRepository.GetGesCaseReportsManagedDocuments(documentIds[0]);
            
            if (caseReportsManagedDocuments != null)
            {
                _caseReportsManagedDocumentsRepository.Delete(caseReportsManagedDocuments);
                _caseReportsManagedDocumentsRepository.Save();
            }
            
            _gManagedDocumentsRepository.DeleteRange(documentIds);
            _gManagedDocumentsRepository.Save();
            
            var companiesGManagedDocuments = _companiesManagedDocumentsRepository.GetCompaniesGManagedDocuments(documentIds[0]);

            if (companiesGManagedDocuments != null)
            {
                _companiesManagedDocumentsRepository.Delete(companiesGManagedDocuments);
                _companiesManagedDocumentsRepository.Save();
            }
           


        }
        
    }
}