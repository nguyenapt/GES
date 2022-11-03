using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using GES.Common.Enumeration;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Models;
using GES.Inside.Data.Repository.Interfaces;
using GES.Inside.Data.Services.Interfaces;
using GES.Common.Logging;
using GES.Common.Exceptions;
using GES.Common.Helpers;

namespace GES.Inside.Data.Services
{
    public class DocumentService : EntityService<GesEntities, G_ManagedDocuments>, IDocumentService
    {
        private readonly GesEntities _dbContext;
        private readonly IG_ManagedDocumentsRepository _documentsReporsitory;
        private readonly IUploadRepository _uploadRepository;
        private readonly ICompaniesManagedDocumentsRepository _companiesManagedDocumentsRepository;
        private readonly II_GesCompanyDialogueRepository _companyDialogueRepository;
        private readonly II_GesSourceDialogueRepository _sourceDialogueRepository;
        private readonly IUnitOfWork<GesEntities> _unitOfWork;
        

        public DocumentService(IUnitOfWork<GesEntities> unitOfWork, IG_ManagedDocumentsRepository documentsReporsitory, IGesLogger logger, IUploadRepository uploadRepository, ICompaniesManagedDocumentsRepository companiesManagedDocumentsRepository, II_GesCompanyDialogueRepository companyDialogueRepository, II_GesSourceDialogueRepository sourceDialogueRepository) : base(unitOfWork,logger, documentsReporsitory)
        {
            _dbContext = unitOfWork.DbContext;
            _unitOfWork = unitOfWork;
            _documentsReporsitory = documentsReporsitory;
            _uploadRepository = uploadRepository;
            _companiesManagedDocumentsRepository = companiesManagedDocumentsRepository;
            _companyDialogueRepository = companyDialogueRepository;
            _sourceDialogueRepository = sourceDialogueRepository;
        }

        public List<DocumentViewModel> GetDocumentsByCompanyId(long companyId)
        {
            var result = this._documentsReporsitory.GetUploadedDocuments(companyId);

            var orderedDocuments = this.SafeExecute<List<DocumentViewModel>>(() => result.OrderByDescending(x => x.Created).ToList());
            foreach (var item in orderedDocuments)
            {
                item.FileExtension = UtilHelper.GetFileExtension(item.FileName);
            }

            return orderedDocuments;
        }

        public DocumentViewModel GetUploadedDocumentById(long documentId)
        {
            return _documentsReporsitory.GetUploadedDocumentById(documentId);
        }

        public DocumentViewModel GetDocumentByCompanySourceDialogId(long companySourceDialogId, string dialogType)
        {
            return _documentsReporsitory.GetDocumentByCompanySourceDialogId(companySourceDialogId, dialogType);
        }

        public G_ManagedDocuments GetDocumentById(long documentId)
        {
            return _documentsReporsitory.GetById(documentId);
        }

        public long TrySaveManagedDocument(DocumentViewModel document, RelatedDocumentMng relatedType)
        {
            try
            {
                return SafeExecute(() => document.G_ManagedDocuments_Id != 0 ? EditManagedDocument(document) : AddManagedDocument(document, relatedType));
                
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public bool TryDeleteManagedDocument(long documentId)
        {
            return SafeExecute(() =>
            {
                var managedDocument =
                    _dbContext.G_ManagedDocuments.FirstOrDefault(x => x.G_ManagedDocuments_Id == documentId);
                var upload = _dbContext.G_Uploads.FirstOrDefault(x => x.G_ManagedDocuments_Id == documentId);
                var companyManagedDocument =
                    _dbContext.I_CompaniesG_ManagedDocuments.FirstOrDefault(x => x.G_ManagedDocuments_Id == documentId);

                var companyDialog =
                    _dbContext.I_GesCompanyDialogues.FirstOrDefault(d => d.G_ManagedDocuments_Id == documentId);

                var sourceDialog =
                    _dbContext.I_GesSourceDialogues.FirstOrDefault(d => d.G_ManagedDocuments_Id == documentId);


                if (managedDocument != null)
                {
                    _documentsReporsitory.Delete(managedDocument);
                }
                if (upload != null)
                {
                    _uploadRepository.Delete(upload);
                }
                if (companyManagedDocument != null)
                {
                    _companiesManagedDocumentsRepository.Delete(companyManagedDocument);
                }
                if (companyDialog != null)
                {
                    companyDialog.G_ManagedDocuments_Id = null;
                    _companyDialogueRepository.Edit(companyDialog);
                    _companyDialogueRepository.Save();
                }
                if (sourceDialog != null)
                {
                    sourceDialog.G_ManagedDocuments_Id = null;
                    _sourceDialogueRepository.Edit(sourceDialog);
                    _sourceDialogueRepository.Save();
                }

                _documentsReporsitory.Save();
                _uploadRepository.Save();
                _companiesManagedDocumentsRepository.Save();

                return true;
            });
        }

        private long AddManagedDocument(DocumentViewModel document, RelatedDocumentMng relatedType)
        {
            var newDocument = Mapper.Map<G_ManagedDocuments>(document);
            newDocument.Created = DateTime.UtcNow;

            var newSavedDocument = _documentsReporsitory.Add(newDocument);
            _documentsReporsitory.Save();

            var newUploadDocument = new G_Uploads
            {
                FileName = document.FileName,
                Created = DateTime.UtcNow,
                G_ManagedDocuments_Id = newSavedDocument.G_ManagedDocuments_Id
            };
            _uploadRepository.Add(newUploadDocument);
            _uploadRepository.Save();

            if (relatedType == RelatedDocumentMng.Company)
            {
                _companiesManagedDocumentsRepository.Add(new I_CompaniesG_ManagedDocuments
                {
                    G_ManagedDocuments_Id = newSavedDocument.G_ManagedDocuments_Id,
                    I_Companies_Id = (long) document.I_Companies_Id,
                    I_CompaniesG_ManagedDocuments_Id = _companiesManagedDocumentsRepository.GetMaxId() + 1 //Don't have primary key for table
                });
                _companiesManagedDocumentsRepository.Save();
            }

            if (relatedType == RelatedDocumentMng.CompanyDialog)
            {
                var companyDialog = _companyDialogueRepository.GetById(document.I_GesCompanyDialogues_Id);
                if (companyDialog != null)
                {
                    companyDialog.G_ManagedDocuments_Id = newSavedDocument.G_ManagedDocuments_Id;
                }
                _companyDialogueRepository.Edit(companyDialog);
                _companyDialogueRepository.Save();
            }

            if (relatedType == RelatedDocumentMng.SourceDialog)
            {
                var sourceDialog = _sourceDialogueRepository.GetById(document.I_GesCompanyDialogues_Id);
                if (sourceDialog != null)
                {
                    sourceDialog.G_ManagedDocuments_Id = newSavedDocument.G_ManagedDocuments_Id;
                }
                _sourceDialogueRepository.Edit(sourceDialog);
                _sourceDialogueRepository.Save();
            }

            return newSavedDocument.G_ManagedDocuments_Id;
        }

        private long EditManagedDocument(DocumentViewModel document)
        {
            var oldDocument = GetDocumentById(document.G_ManagedDocuments_Id);
            oldDocument.Name = document.Name;
            oldDocument.G_DocumentManagementTaxonomies_Id = document.G_DocumentManagementTaxonomies_Id;
            oldDocument.G_ManagedDocumentServices_Id = document.G_ManagedDocumentServices_Id;
            oldDocument.Comment = document.Comment;
            oldDocument.G_ManagedDocumentApprovalStatuses_Id = document.G_ManagedDocumentApprovalStatuses_Id;
            oldDocument.Modified = DateTime.UtcNow;

            var oldUpload = _uploadRepository.GetByDocumentId(document.G_ManagedDocuments_Id);
            oldUpload.FileName = document.FileName;

            _documentsReporsitory.Edit(oldDocument);
            _uploadRepository.Edit(oldUpload);
            _documentsReporsitory.Save();
            _uploadRepository.Save();
            return document.G_ManagedDocuments_Id;
        }
    }
}