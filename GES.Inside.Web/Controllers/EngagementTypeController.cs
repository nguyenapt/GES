using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GES.Common.Enumeration;
using GES.Common.Helpers;
using GES.Common.Logging;
using GES.Common.Models;
using GES.Common.Services.Interface;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Models;
using GES.Inside.Data.Repository.Interfaces;
using GES.Inside.Data.Services.Interfaces;
using GES.Inside.Web.Extensions;

namespace GES.Inside.Web.Controllers
{
    public class EngagementTypeController : GesControllerBase
    {
        #region Declaration

        private readonly II_EngagementTypesService _engagementTypesService;
        private readonly IG_IndividualsService _gIndividualsService;
        private readonly II_EngagementTypesRepository _engagementTypesRepository;
        private readonly II_EngagementTypeNewsService _engagementTypeNewsService;
        private readonly II_EngagementTypeNewsRepository _engagementTypeNewsRepository;
        private readonly II_TimelineItemsService _timelineItemsService;
        private readonly II_TimeLineItemsRepository _timeLineItemsRepository;
        private readonly IGesDocumentService _gesdocumentService;
        private readonly II_KpisService _kpisService;
        private readonly II_KpisRepository _kpisRepository;
        private readonly IApplicationSettingsService _applicationSettingsService;
        private const string UploadHashcode = "Theme";

        public IGesFileStorageService GesFileStorageService { get; set; }

        #endregion

        #region Constructor

        public EngagementTypeController(IGesLogger logger, II_EngagementTypesService engagementTypesService,
            IG_IndividualsService gIndividualsService, II_EngagementTypesRepository engagementTypesRepository,
            IGesDocumentService gesdocumentService, II_EngagementTypeNewsService engagementTypeNewsService,
            II_EngagementTypeNewsRepository engagementTypeNewsRepository, II_TimelineItemsService timelineItemsService,
            II_TimeLineItemsRepository timeLineItemsRepository, II_KpisService kpisService,
            II_KpisRepository kpisRepository,
            IApplicationSettingsService applicationSettingsService) : base(logger)
        {
            _engagementTypesService = engagementTypesService;
            _gIndividualsService = gIndividualsService;
            _engagementTypesRepository = engagementTypesRepository;
            _gesdocumentService = gesdocumentService;
            _engagementTypeNewsService = engagementTypeNewsService;
            _engagementTypeNewsRepository = engagementTypeNewsRepository;
            _timelineItemsService = timelineItemsService;
            _timeLineItemsRepository = timeLineItemsRepository;
            _kpisService = kpisService;
            _kpisRepository = kpisRepository;
            _applicationSettingsService = applicationSettingsService;
        }

        #endregion

        #region ActionResult

        [CustomAuthorize(FormKey = "ConfigEngagementType", Action = ActionEnum.Read)]
        public ActionResult List()
        {
            return View();
        }

        [CustomAuthorize(FormKey = "ConfigEngagementType", Action = ActionEnum.Read)]
        public ActionResult EngagementTypeDetails(string id)
        {
            long engagementTypeId;
            var engagementTypeDetails = new EngagementTypeViewModel();

            long orgId = 0, individualId = 0;
            this.GetIndividualInfo(_gIndividualsService, ref individualId, ref orgId);

            if (long.TryParse(id, out engagementTypeId))
            {
                engagementTypeDetails = _engagementTypesService.GetEngagementTypeModel(engagementTypeId, orgId) ??
                                        new EngagementTypeViewModel();
            }

            ViewBag.NgController = "EngagementTypeController";

            return View(engagementTypeDetails);
        }

        #endregion

        #region JsonResult

        [HttpPost]
        public JsonResult GetDataForEngagementTypesJqGrid(JqGridViewModel jqGridParams)
        {
            long orgId = 0, individualId = 0;
            this.GetIndividualInfo(_gIndividualsService, ref individualId, ref orgId);

            var engagementTypes = SafeExecute(() => _engagementTypesService.GetEngagementTypes(jqGridParams, orgId), "Error when getting the Engagement types {@JqGridViewModel}", jqGridParams);

            return Json(engagementTypes);
        }

        [HttpPost]
        public JsonResult UploadFiles(HttpPostedFileBase file)
        {
            var oldSystemFilePath = UtilHelper.GetFilePath(_applicationSettingsService.GesReportFileUploadPathForOldSystem,
                file.FileName, null);

            var newSystemFilePath =
                UtilHelper.GetFilePath(_applicationSettingsService.BlobPath, file.FileName, UploadHashcode);
            
            SaveFile(file);

            CopyFileToOldSystem(newSystemFilePath, oldSystemFilePath);
            

            return Json(new {Status = "Success"});
        }

        [HttpGet]
        public JsonResult GetEngagementTypeById(long id)
        {
            long orgId = 0, individualId = 0;
            this.GetIndividualInfo(_gIndividualsService, ref individualId, ref orgId);

            var engagementTypeDetails = new EngagementTypeViewModel();

            if (id != 0)
            {
                if (_engagementTypesService.GetEngagementTypeModel(id, orgId) != null)
                {
                    engagementTypeDetails = _engagementTypesService.GetEngagementTypeModel(id, orgId);

                    if (!string.IsNullOrWhiteSpace(engagementTypeDetails.ThemeImagePath))
                    {
                        var themeBanner = UtilHelper.ImageToByte(
                            GesFileStorageService.GetFilePath(engagementTypeDetails.ThemeImagePath, UploadHashcode));
                        if (themeBanner!= null)
                        {
                            engagementTypeDetails.ThemeImage = Convert.ToBase64String(themeBanner);
                        } 
                    }
                }
                else
                {
                    engagementTypeDetails = new EngagementTypeViewModel();
                }
            }

            ViewBag.NgController = "EngagementTypeController";

            return Json(engagementTypeDetails, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateEngagementType(EngagementTypeViewModel engagementTypeDetails)
        {
            I_EngagementTypes updateEngagementType;
            var documents = new List<I_EngagementTypes_GesDocument>();

            // engagementTypeDetails.I_EngagementTypes_Id = 0;
            if (engagementTypeDetails.I_EngagementTypes_Id == 0)
            {
                updateEngagementType = new I_EngagementTypes();
            }
            else
            {
                updateEngagementType = _engagementTypesRepository.GetById(engagementTypeDetails.I_EngagementTypes_Id);
                if (updateEngagementType == null)
                    return null;
            }

            updateEngagementType.I_EngagementTypeCategories_Id = engagementTypeDetails.I_EngagementTypeCategories_Id;
            updateEngagementType.Name = engagementTypeDetails.Name;
            updateEngagementType.Description = engagementTypeDetails.Description;
            updateEngagementType.Goal = engagementTypeDetails.Goal;
            updateEngagementType.NextStep = engagementTypeDetails.NextStep;
            updateEngagementType.LatestNews = engagementTypeDetails.EngagementTypeNews != null
                ? (engagementTypeDetails.EngagementTypeNews.OrderByDescending(e => e.Created).FirstOrDefault()
                    ?.EngagementTypeNewsDescription)
                : "";
            updateEngagementType.OtherInitiatives = engagementTypeDetails.OtherInitiatives;
            //updateEngagementType.Sources = engagementTypeDetails.Sources;
            updateEngagementType.GesReports = engagementTypeDetails.GesReports;
            updateEngagementType.ContactG_Users_Id = engagementTypeDetails.ContactG_Users_Id;
            updateEngagementType.Participants = engagementTypeDetails.Participants;
            updateEngagementType.NonSubscriberInformation = engagementTypeDetails.NonSubscriberInformation;
            updateEngagementType.SortOrder = engagementTypeDetails.SortOrder;
            updateEngagementType.I_EngagementTypes_GesDocument = documents;
            updateEngagementType.ThemeImage = engagementTypeDetails.ThemeImagePath;
            updateEngagementType.IsShowInClientMenu = engagementTypeDetails.IsShowInClientMenu;
            updateEngagementType.IsShowInCaseProfileTemplate = engagementTypeDetails.IsShowInCaseProfileTemplate;
            

            updateEngagementType.GesReports =
                GenerateGesReport(
                    (List<EngagementTypeGesDocumentViewModel>) engagementTypeDetails.EngagementTypeDocuments);


            if (updateEngagementType.I_EngagementTypes_Id == 0)
            {
                updateEngagementType.Created = DateTime.UtcNow;
                _engagementTypesRepository.Add(updateEngagementType);
            }
            else
            {
                _engagementTypesRepository.Edit(updateEngagementType);
            }

            _engagementTypesRepository.Save();

            //Save News items

            var updateEngagementTypesNewsResult = UpdateEngagementTypesNews(
                (List<EngagementTypeNewsViewModel>) engagementTypeDetails.EngagementTypeNews,
                updateEngagementType.I_EngagementTypes_Id);

            if (!updateEngagementTypesNewsResult)
            {
                return Json(new
                {
                    success = false,
                    message = "Failed updating Engagement Types News."
                });
            }

            //Save Timelines
            var updateEngagementTypeTimelineResult = UpdateEngagementTypeTimeline(
                (List<EventListViewModel>) engagementTypeDetails.TimeLine, updateEngagementType.I_EngagementTypes_Id);

            if (!updateEngagementTypeTimelineResult)
            {
                return Json(new
                {
                    success = false,
                    message = "Failed updating Engagement Type Timelines."
                });
            }

            //Save Kpis
            var updateEngagementTypeKpIsResult =
                UpdateEngagementTypeKpIs((List<KpiViewModel>) engagementTypeDetails.KPIs,
                    updateEngagementType.I_EngagementTypes_Id);

            if (!updateEngagementTypeKpIsResult)
            {
                return Json(new
                {
                    success = false,
                    message = "Failed updating Engagement Type Timelines."
                });
            }

           var updateGesReportsResult = SaveUploadGesReports(
                (List<EngagementTypeGesDocumentViewModel>) engagementTypeDetails.EngagementTypeDocuments,
                (List<I_EngagementTypes_GesDocument>) updateEngagementType.I_EngagementTypes_GesDocument,
                updateEngagementType.I_EngagementTypes_Id);

            if (!updateGesReportsResult)
            {
                return Json(new
                {
                    success = false,
                    message = "Failed updating Ges Reports."
                });
            }

            return Json(new {Status = "Success"});
        }

        [HttpPost]
        public JsonResult DeleteEngagementType(EngagementTypeViewModel engagementTypeDetails)
        {
            var engagementTypesId = engagementTypeDetails.I_EngagementTypes_Id;

            //Delete timelines
            var oldTimeLines = _timelineItemsService.GetTimelineItemsByEngagementTypesId(engagementTypesId).ToList();
            foreach (var deleteItem in oldTimeLines)
            {
                _timelineItemsService.Delete(deleteItem);
            }

            //Delete KPIS
            var oldKpis = _kpisService.GetKpisByEngagementTypesId(engagementTypesId).ToList();
            foreach (var deleteItem in oldKpis)
            {
                _kpisService.Delete(deleteItem);
            }

            //Delete News
            var oldNews = _engagementTypeNewsService.GetEngagementTypesNewsByEngagementTypesId(engagementTypesId)
                .ToList();

            foreach (var deleteItem in oldNews)
            {
                _engagementTypeNewsService.Delete(deleteItem);
            }

            //Delete Docs
            if (engagementTypeDetails.EngagementTypeDocuments != null)
            {
                foreach (var existingGesDocument in engagementTypeDetails.EngagementTypeDocuments)
                {
                    if (existingGesDocument.DocumentId != null)
                    {
                        var oldDoc = _gesdocumentService.GetGesDocumentById((Guid)existingGesDocument.DocumentId);
                        _gesdocumentService.Delete(oldDoc);
                    }

                    _engagementTypesRepository.DeleteEngagementTypesGesDocument(existingGesDocument.Id);
                }
            }

            var oldEngagementType = _engagementTypesRepository.GetById(engagementTypeDetails.I_EngagementTypes_Id);

            _engagementTypesRepository.Delete(oldEngagementType);

            _engagementTypesRepository.Save();
            _engagementTypesRepository.Save();
            _gesdocumentService.Save();
            _timelineItemsService.Save();
            _kpisService.Save();
            _engagementTypeNewsService.Save();


            return Json(new {Status = "Success"});
        }
        
        [HttpPost]
        public JsonResult ActiveOrDeactiveEngagementType(EngagementTypeViewModel engagementTypeDetails)
        {
            var updateEngagementType = _engagementTypesRepository.GetById(engagementTypeDetails.I_EngagementTypes_Id);
                if (updateEngagementType == null)
                    return Json(new {Status = "Unsuccess"});
            
            updateEngagementType.Deactive = engagementTypeDetails.Deactive;
            
            _engagementTypesRepository.Edit(updateEngagementType);
            _engagementTypesRepository.Save();

            return Json(new {Status = "Success"});
        }
        

        [HttpGet]
        public JsonResult GetEngagementTypeCategories()
        {
            var engagementTypeCategories = _engagementTypesService.AllEngagementTypeCategories().Select(x => new { x.I_EngagementTypeCategories_Id, x.Name});
            return Json(engagementTypeCategories, JsonRequestBehavior.AllowGet);
        }
        
        [HttpPost]
        public JsonResult GetEngagementTypeContactsJqGrid(JqGridViewModel jqGridParams)
        {
            long orgId = 0, individualId = 0;
            this.GetIndividualInfo(_gIndividualsService, ref individualId, ref orgId);

            var engagementTypeContacts = SafeExecute(() => _engagementTypesService.GetGesContacts(jqGridParams, orgId), "Error when getting the Contacts {@JqGridViewModel}", jqGridParams);

            return Json(engagementTypeContacts);
        }
        
        [HttpGet]
        public JsonResult GetDocumentTypes()
        {
            var documentTypes = _engagementTypesService.GetGesDocTypes().Select(x => new { x.GesDocTypesId, x.Name});
            return Json(documentTypes, JsonRequestBehavior.AllowGet);
        }
        

        #endregion

        #region Private methods

        private bool UpdateEngagementTypesNews(IReadOnlyCollection<EngagementTypeNewsViewModel> newNews, long engagementTypesId)
        {
            var oldNews = _engagementTypeNewsService.GetEngagementTypesNewsByEngagementTypesId(engagementTypesId)
                .ToList();

            if (newNews == null)
            {
                //Remove all news
                foreach (var deleteItem in oldNews)
                {
                    _engagementTypeNewsService.Delete(deleteItem);
                }

                return true;
            }

            var listNewsId = newNews.ToList().Select(d => d.EngagementTypeNewsId);

            try
            {
                //Delete Item
                var listDelete = oldNews.Where(d => !listNewsId.Contains(d.I_EngagementTypeNews_Id));

                foreach (var deleteItem in listDelete)
                {
                    _engagementTypeNewsService.Delete(deleteItem);
                }

                //Add newItem
                foreach (var newNewsItem in newNews)
                {
                    if (newNewsItem.EngagementTypeNewsId == 0)
                    {
                        _engagementTypeNewsService.Add(new I_EngagementTypeNews()
                        {
                            I_EngagementTypes_Id = engagementTypesId,
                            Description = newNewsItem.EngagementTypeNewsDescription,
                            Created = newNewsItem.Created
                        });
                    }
                    else
                    {
                        var updateNews = _engagementTypeNewsRepository.GetById(newNewsItem.EngagementTypeNewsId);
                        updateNews.Description = newNewsItem.EngagementTypeNewsDescription;

                        _engagementTypeNewsService.Update(updateNews);
                    }
                }

                _engagementTypeNewsService.Save();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        private bool UpdateEngagementTypeTimeline(List<EventListViewModel> newTimelines, long engagementTypesId)
        {
            
            var oldTimeLines = _timelineItemsService.GetTimelineItemsByEngagementTypesId(engagementTypesId).ToList();

            if (newTimelines == null)
            {
                //Remove all old Timelines
                foreach (var deleteItem in oldTimeLines)
                {
                    _timelineItemsService.Delete(deleteItem);
                }

                return true;
            }

            try
            {
                //Delete Item

                var deleteList =
                    oldTimeLines.Where(c => newTimelines.All(d => c.I_TimelineItems_Id != d.Id)).ToList();

                foreach (var deleteItem in deleteList)
                {
                    _timelineItemsService.Delete(deleteItem);
                }

                //Add timeline
                foreach (var timeline in newTimelines)
                {
                    if (timeline.Id == 0)
                    {
                        _timelineItemsService.Add(new I_TimelineItems()
                        {
                            I_EngagementTypes_Id = engagementTypesId,
                            Description = timeline.Heading,
                            Date = timeline.EventDate,
                            Created = DateTime.UtcNow
                        });
                    }
                    else
                    {
                        var updateTimeline = _timeLineItemsRepository.GetById(timeline.Id);
                        updateTimeline.Description = timeline.Heading;
                        updateTimeline.Date = timeline.EventDate;

                        _timelineItemsService.Update(updateTimeline);
                    }
                }

                _timelineItemsService.Save();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        private bool UpdateEngagementTypeKpIs(List<KpiViewModel> newKpis, long engagementTypesId)
        {
           
            var oldKpis = _kpisService.GetKpisByEngagementTypesId(engagementTypesId).ToList();

            if (newKpis == null)
            {
                //Remove all KPIs
                foreach (var deleteItem in oldKpis)
                {
                    _kpisService.Delete(deleteItem);
                }
                _kpisService.Save();
                return true;
            }

            var listTimeLineId = newKpis.ToList().Select(d => d.KpiId);

            try
            {
                //Delete Item
                var listDelete = oldKpis.Where(d => !listTimeLineId.Contains(d.I_Kpis_Id));

                foreach (var deleteItem in listDelete)
                {
                    _kpisService.Delete(deleteItem);
                }

                //Add timeline
                foreach (var kpi in newKpis)
                {
                    if (kpi.KpiId == 0)
                    {
                        _kpisService.Add(new I_Kpis()
                        {
                            I_EngagementTypes_Id = engagementTypesId,
                            Description = kpi.KpiDescription,
                            Name = kpi.KpiName,
                            Created = DateTime.UtcNow
                        });
                    }
                    else
                    {
                        var updateKpi = _kpisRepository.GetById(kpi.KpiId);
                        updateKpi.Description = kpi.KpiDescription;
                        updateKpi.Name = kpi.KpiName;

                        _kpisService.Update(updateKpi);
                    }
                }

                _kpisService.Save();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        private bool SaveUploadGesReports(IReadOnlyCollection<EngagementTypeGesDocumentViewModel> newEngagementTypeGesDocument,
            IEnumerable<I_EngagementTypes_GesDocument> oldEngagementTypesGesDocuments, long engagementTypesId)
        {
            try
            {
                //Delete uploaded docs
                var deleteExistingGesDocumentGuidList = (from existingGesDocument in oldEngagementTypesGesDocuments
                    where newEngagementTypeGesDocument == null ||
                          newEngagementTypeGesDocument.All(c => c.DocumentId != existingGesDocument.DocumentId)
                    where existingGesDocument.DocumentId != null
                    select (Guid) existingGesDocument.DocumentId).ToList();

                if (deleteExistingGesDocumentGuidList.Count > 0)
                {
                    _gesdocumentService.DeleteRange(deleteExistingGesDocumentGuidList.ToArray());

                    foreach (var deleteId in deleteExistingGesDocumentGuidList)
                    {
                        _engagementTypesRepository.DeleteEngagementTypesGesDocument(deleteId);
                    }

                    _gesdocumentService.Save();
                }

                if (newEngagementTypeGesDocument == null)
                {
                    return true;
                }

                //Add and update doc
                foreach (var doc in newEngagementTypeGesDocument)
                {
                    if (doc.DocumentId == null || doc.DocumentId == Guid.Empty)
                    {
                        var entity = new GesDocument
                        {
                            DocumentId = Guid.NewGuid(),
                            Name = doc.Name,
                            FileName = doc.FileName,
                            Created = DateTime.UtcNow,
                            GesDocumentServiceId = (long?) DocumentServices.EngTyp,
                            HashCodeDocument = UploadHashcode
                        };

                        _gesdocumentService.Add(entity);
                        _gesdocumentService.Save();

                        _engagementTypesRepository.AddEngagementTypesGesDocument(new I_EngagementTypes_GesDocument()
                        {
                            Id = Guid.NewGuid(),
                            EngagementTypeId = engagementTypesId,
                            DocumentTypeId = doc.DocumentTypeId,
                            DocumentId = entity.DocumentId,
                            Created = DateTime.UtcNow
                        });
                    }
                    else
                    {
                        if (doc.DocumentId == null) continue;
                        var updateDoc = _gesdocumentService.GetGesDocumentById((Guid)doc.DocumentId);
                        updateDoc.Name = doc.Name;

                        _gesdocumentService.Update(updateDoc);
                    }
                }

                _gesdocumentService.Save();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        private string GenerateGesReport(List<EngagementTypeGesDocumentViewModel> newEngagementTypeGesDocuments)
        {
            var gesReport = "";
            var folderPath = _applicationSettingsService.GesReportFileUploadPathForOldSystem.Split(Convert.ToChar("/")).Last();
            if (newEngagementTypeGesDocuments == null) return gesReport;
            
            foreach (var engagementTypeDocument in newEngagementTypeGesDocuments)
            {
                gesReport += "<a href=\"/" + folderPath + "/" + engagementTypeDocument.FileName + "\">";

                switch (engagementTypeDocument.FileName.Split(Convert.ToChar(".")).Last())
                {
                    case "pdf":
                        gesReport += "<img src=\"/graphics/icons/pdf.gif\">&nbsp;";
                        break;
                    case "xls":
                        gesReport += "<img src=\"/graphics/icons/icon_excel.gif\">&nbsp;";
                        break;
                    case "xlsx":
                        gesReport += "<img src=\"/graphics/icons/icon_excel.gif\">&nbsp;";
                        break;
                    default:
                        gesReport += "<img src=\"/graphics/icons/items.gif\">&nbsp;";
                        break;
                }

                gesReport += engagementTypeDocument.FileName + "</a><br>";

            }

            return gesReport;
        }
        
        private void SaveFile(HttpPostedFileBase file)
        {
            if (file == null || file.ContentLength <= 0) return;

            var fileStream = file.InputStream;
            GesFileStorageService.StoreFile(fileStream, file.FileName, UploadHashcode);
       
        }
        
        private static void CopyFileToOldSystem(string nesSystemPath, string oldSystemPath)
        {
            UtilHelper.CopyMultiple(nesSystemPath, oldSystemPath);
        }
        
        #endregion

    }
}