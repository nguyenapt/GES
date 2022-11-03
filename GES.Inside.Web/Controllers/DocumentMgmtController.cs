using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GES.Common.Enumeration;
using GES.Common.Helpers;
using GES.Common.Models;
using GES.Common.Services.Interface;
using GES.Inside.Data.Models;
using GES.Inside.Data.Services.Interfaces;
using GES.Common.Logging;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Repository.Interfaces;

namespace GES.Inside.Web.Controllers
{
    public class DocumentMgmtController : GesControllerBase
    {

        private readonly IGesDocumentService _gesdocumentService;
        private readonly IOrganizationService _organizationService;
        private readonly IGesFileStorageService _fileStorageService;
        private readonly IG_ManagedDocumentsService _gManagedDocumentsService;
        private IApplicationSettingsService _applicationSettingsService;

        public DocumentMgmtController(IGesLogger logger, IGesDocumentService gesdocumentService, IOrganizationService organizationService, 
            IApplicationSettingsService applicationSettingsService, IGesFileStorageService fileStorageService, IG_ManagedDocumentsService gManagedDocumentsService)
            : base(logger)
        {
            _gesdocumentService = gesdocumentService;
            _organizationService = organizationService;
            _applicationSettingsService = applicationSettingsService;
            _fileStorageService = fileStorageService;
            _gManagedDocumentsService = gManagedDocumentsService;
        }

        [CustomAuthorize(FormKey = "ConfigDocument", Action = ActionEnum.Read)]
        public ActionResult List()
        {
            return View();
        }        
        [CustomAuthorize(FormKey = "ConfigCompanyDocument", Action = ActionEnum.Read)]
        public ActionResult CompanyDocumentList()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetDataForGesDocumentJqGrid(JqGridViewModel jqGridParams)
        {
            var documents = this.SafeExecute(() => _gesdocumentService.GetDocumentsForGrid(jqGridParams), "Error when getting the document with criteria {@JqGridViewModel}", jqGridParams);

            return Json(documents);
        }        
        
        [HttpPost]
        public JsonResult GetDataForCompanyDocumentJqGrid(JqGridViewModel jqGridParams)
        {
            var documents = this.SafeExecute(() => _gManagedDocumentsService.GetCompanyDocumentsForGrid(jqGridParams), "Error when getting the document with criteria {@JqGridViewModel}", jqGridParams);

            return Json(documents);
        }

        public ActionResult CreateForm_Document(string documentId)
        {
            var documentservices = this.SafeExecute(() => _gesdocumentService.GetListGesDocumentServices(), $"Error when getting list of document services");
            var services = new List<SelectListItem>
                             {
//                                 new SelectListItem { Text = "None", Value = ""}
                             };
            services.AddRange(documentservices.Select(i => new SelectListItem { Text = i.Name, Value = i.GesDocumentServiceId.ToString(), Selected = i.GesDocumentServiceId == 3? true: false}));
            ViewBag.gesDocumentService = services;

            var gesDocumentViewModel = new GesDocumentViewModel();

            var gDocumentId = string.IsNullOrEmpty(documentId) ? Guid.Empty : new Guid(documentId);

            var orgs = _gesdocumentService.GetOrgDocumentByDocumentId(gDocumentId);
            var selectedOrgId = new List<string>();

            if (orgs != null && orgs.Any())
            {
                selectedOrgId = orgs.Select(d => d.G_Organizations_Id.ToString()).ToList();
            }

            gesDocumentViewModel.Organizations = GetOrganizationSelectListItems();
            gesDocumentViewModel.SelectedOrganizations = selectedOrgId.ToArray();
            gesDocumentViewModel.ServiceId = 0;
            var document = _gesdocumentService.GetGesDocumentById(gDocumentId);

            if (document != null)
            {
                gesDocumentViewModel.Id = document.DocumentId;
                gesDocumentViewModel.Name = document.Name;
                gesDocumentViewModel.FileName = document.FileName;
                gesDocumentViewModel.ServiceId = document.GesDocumentServiceId;
                gesDocumentViewModel.Source = document.Source;
                gesDocumentViewModel.Comment = document.Comment;
                gesDocumentViewModel.Created = document.Created;
                gesDocumentViewModel.Metadata01 = document.Metadata01;
                gesDocumentViewModel.Metadata02 = document.Metadata02;
                gesDocumentViewModel.Metadata03 = document.Metadata03;
                gesDocumentViewModel.HashCodeDocument = document.HashCodeDocument;
            }

            return PartialView("_CreateDocument", gesDocumentViewModel);
        }        
        
        public ActionResult CreateForm_CompanyDocument(string documentId)
        {
            
            var documentViewModel = new DocumentViewModel();
       
            long docId;
            var result = Int64.TryParse(documentId, out docId);

            if (result)
            {
                documentViewModel = _gManagedDocumentsService.GetDocumentById(docId);;
            }

            return PartialView("_CreateCompanyDocument", documentViewModel);
        }

        [HttpPost]
        public JsonResult CreateUpdateGesDocument(GesDocumentViewModel gesDocument, HttpPostedFileBase file)
        {
            if (!ModelState.IsValid)
            {
                return Json(new
                {
                    success = false,
                    message = "",
                    error = "Something wrong. Invalid model! Kindly check again."
                });
            }


            bool isEditing = true;
            try
            {
                string fileName = "";

                
                if (gesDocument.Id == new Guid())
                {
                    gesDocument.Id = Guid.NewGuid();
                    isEditing = false;
                }
                else
                {
                    fileName = gesDocument.FileName;
                }

                Stream fileStream = null;

                if (file != null && file.ContentLength > 0)
                {
                    var fileExtension = Path.GetExtension(file.FileName);
                    var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(file.FileName);

                    fileName = UtilHelper.CreateSafeFileName(fileNameWithoutExtension, fileExtension);
                    fileStream = file.InputStream;
                }

                gesDocument.ServiceId = 3;

                _gesdocumentService.SaveGesDocument(gesDocument, fileStream, fileName);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    error = ex.Message,
                    editing = isEditing
                });
            }

            return Json(new
            {
                success = true,
                editing = isEditing
            });
        }

        [HttpPost]
        public JsonResult AddOrUpdateCompanyDocument(long managedDocumentsId,  long companiesId, long caseProfileId, string name, string comment, HttpPostedFileBase file)
        {
            var documentViewModel = new DocumentViewModel
            {
                G_ManagedDocuments_Id = managedDocumentsId,
                I_Companies_Id = companiesId,
                I_GesCaseReports_Id = caseProfileId,
                Name = name,
                Comment = comment
            };

            return CreateUpdateCompanyDocument(documentViewModel, file);
        }       

        [HttpPost]
        public JsonResult CreateUpdateCompanyDocument(DocumentViewModel document, HttpPostedFileBase file)
        {
            if (!ModelState.IsValid)
            {
                return Json(new
                {
                    success = false,
                    message = "",
                    error = "Something wrong. Invalid model! Kindly check again."
                });
            }
            var fileName = "";

            var isEditing = true;
            G_ManagedDocuments managedDocument;
            try
            {
                

                
                if (document.G_ManagedDocuments_Id == 0)
                {
                    isEditing = false;
                }
                else
                {
                    fileName = document.FileName;
                }

                Stream fileStream = null;

                if (file != null && file.ContentLength > 0)
                {
                    var fileExtension = Path.GetExtension(file.FileName);
                    var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(file.FileName);

                    fileName = UtilHelper.CreateSafeFileName(fileNameWithoutExtension, fileExtension);
                    fileStream = file.InputStream;
                }

                managedDocument = _gManagedDocumentsService.SaveGesDocument(document, fileStream, fileName);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    error = ex.Message,
                    editing = isEditing
                });
            }

            return Json(new
            {
                success = true,
                editing = isEditing,
                filePath = managedDocument != null && managedDocument.G_ManagedDocuments_Id != 0?"/documentmgmt/CompanyDocDownload/?documentId=" + managedDocument.G_ManagedDocuments_Id:"",
                fileName = fileName,
                docId = managedDocument?.G_ManagedDocuments_Id ?? 0
            });
        }

        [HttpPost]
        public JsonResult DeleteDocuments(Guid[] documentIds)
        {
            try
            {
                _gesdocumentService.DeleteRange(documentIds);
                return Json(new { success = true });
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "Error." });
            }
        }        
        
        [HttpPost]
        public JsonResult DeleteCompanyDocuments(long[] documentIds)
        {
            try
            {
                _gManagedDocumentsService.DeleteRange(documentIds);
                
                return Json(new { success = true });
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "Error." });
            }
        }

        private IEnumerable<SelectListItem> GetOrganizationSelectListItems()
        {
            var claim = _organizationService.GetAllClients();

            var roles = claim.Select(x =>
                                new SelectListItem
                                {
                                    Value = x.Id.ToString(),
                                    Text = x.Name.ToString()
                                });

            var list = new SelectList(roles, "Value", "Text");
            return list;
        }

        [Route("Download/{documentId}")]
        public ActionResult Download(Guid? documentId)
        {
            if (!documentId.HasValue)
                return HttpNotFound();

            var document = this._gesdocumentService.GetGesDocumentById(documentId.Value);

            if (document != null)
            {
                var fileStream = this._fileStorageService.GetStream(documentId.Value);

                if (fileStream != null && fileStream.CanRead)
                {
                    return File(fileStream, System.Web.MimeMapping.GetMimeMapping(document.FileName), document.FileName);
                }
            }

            return HttpNotFound();
        }       
        
        [Route("CompanyDocDownload/{documentId}")]
        public ActionResult CompanyDocDownload(long documentId)
        {
            if (documentId == 0)
                return HttpNotFound();

            var document = this._gManagedDocumentsService.GetDocumentById(documentId);

            if (document != null)
            {
                var fileStream = _fileStorageService.GetStreamFromOldSystem(document.FileName);

                if (fileStream != null && fileStream.CanRead)
                {
                    return File(fileStream, System.Web.MimeMapping.GetMimeMapping(document.FileName), document.FileName);
                }
            }

            return HttpNotFound();
        }


    }
}
