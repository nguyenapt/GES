using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GES.Clients.Web.Helpers;
using GES.Common.Resources;
using GES.Common.Services.Interface;
using GES.Inside.Data.Models;
using GES.Inside.Data.Services.Interfaces;
using GES.Common.Enumeration;
using GES.Inside.Data.ExportModels;
using GES.Common.Logging;
using GES.Inside.Data.Models.CaseProfiles;
using GES.Clients.Web.Models;
using GES.Inside.Data.Repository.Interfaces;

namespace GES.Clients.Web.Controllers
{
    public class ExportController : GesControllerBase
    {

        private readonly II_CompaniesService _companiesService;
        private readonly ICalendarService _calendarService;
        private readonly IDocumentService _documentService;
        private readonly IAlertService _alertService;
        private readonly IApplicationSettingsService _applicationSettingsService;
        private readonly IGesFileStorageService _gesFileStorageService;
        private readonly II_GesCaseProfilesService _caseProfileService;
        private readonly IGesDocumentService _gesDocumentService;
        private readonly IGesCaseProfileTemplatesRepository _caseProfileTemplatesRepository;

        public ExportController(IGesLogger logger,
            II_CompaniesService companiesService,
            ICalendarService calendarService, IDocumentService documentService, IAlertService alertService, IApplicationSettingsService applicationSettingsService, IGesFileStorageService gesFileStorageService, II_GesCaseProfilesService caseProfileService, IGesDocumentService glossaryService, IGesCaseProfileTemplatesRepository caseProfileTemplatesRepository)
            : base(logger)
        {
            _companiesService = companiesService;
            _calendarService = calendarService;
            _documentService = documentService;
            _alertService = alertService;
            _applicationSettingsService = applicationSettingsService;
            _gesFileStorageService = gesFileStorageService;
            _caseProfileService = caseProfileService;
            _gesDocumentService = glossaryService;
            _caseProfileTemplatesRepository = caseProfileTemplatesRepository;

        }

        [AllowAnonymous]
        public ActionResult CompanyProfile(long companyId, long gesCompanyId, long orgId, long individualId, bool? showCompanyInfo, bool? showDialogue, bool? showCompanyOverview, bool? showCaseProfiles, bool? showAlerts, bool? showCompanyEvents, bool? showCorporateRatingInformation, bool? showCoverPage, bool? showDocuments, bool? showResolvedAndArchivedCases)
        {
            var exportCompanyDetail = new ExportCompanyDetailViewModel();

            var companyDetail = _companiesService.GetCompanyDetailViewModel(companyId, orgId, individualId, true) ?? new CompanyDetailViewModel();
            companyDetail.Events = _calendarService.GetCalendarEventsByCompanyId(companyId).Where(x => x.EventDate >= DateTime.Today)?.ToList();
            companyDetail.Documents = GetDocuments(companyId)?.ToList();
            companyDetail.OrganizationId = orgId;
            companyDetail.IndividualId = individualId;

            ViewBag.PageClass = "page-company page-company-profile";
            ViewBag.Title = $"<a target=\"_blank\" href=\"{Url.Action("Profile", "Company", new { id = companyId }, Request?.Url?.Scheme)}\">{companyDetail.CompanyName}</a>";
            ViewBag.SubTitle = Resources.CompanyProfile;

            var caseReports = _companiesService.GetCasesDataByCompanyId(gesCompanyId, companyId, orgId, !showResolvedAndArchivedCases.HasValue || !showResolvedAndArchivedCases.Value);

            foreach (var caseReportListViewModel in caseReports)
            {
                caseReportListViewModel.ServiceEngagementThemeNorm =
                    caseReportListViewModel.ServiceEngagementThemeNorm.Replace("Global Standards", "GS")
                        .Replace("Stewardship & Risk", "S&R");
            }

            if (caseReports != null && caseReports.Any())
                exportCompanyDetail.CaseReportListViewModels = caseReports.OrderBy(x => x.ServiceEngagementThemeNorm).ThenByDescending(d => d.SortOrderRecommendation).ToList();

            var alerts = _alertService.GetAlertsByCompanyId(companyId)?.ToList();
            if (alerts != null && alerts.Any())
                exportCompanyDetail.AlertListViewModels = alerts.OrderByDescending(x => x.Date).ThenByDescending(d => d.Heading).ToList();

            exportCompanyDetail.CompanyDetailViewModel = companyDetail;

            exportCompanyDetail.ShowAlerts = showAlerts.HasValue && showAlerts.Value;
            exportCompanyDetail.ShowCaseProfiles = showCaseProfiles.HasValue && showCaseProfiles.Value;
            exportCompanyDetail.ShowCompanyEvents = showCompanyEvents.HasValue && showCompanyEvents.Value;
            exportCompanyDetail.ShowCompanyInfo = showCompanyInfo.HasValue && showCompanyInfo.Value;
            exportCompanyDetail.ShowCompanyOverview = showCompanyOverview.HasValue && showCompanyOverview.Value;
            exportCompanyDetail.ShowCorporateRatingInformation = showCorporateRatingInformation.HasValue && showCorporateRatingInformation.Value;
            exportCompanyDetail.ShowDialogue = showDialogue.HasValue && showDialogue.Value;
            exportCompanyDetail.ShowCoverPage = showCoverPage.HasValue && showCoverPage.Value;
            exportCompanyDetail.ShowDocuments = showDocuments.HasValue && showDocuments.Value;

            return View("~/Views/Export/Export_Sustainalytics/CompanyProfile.cshtml", exportCompanyDetail);
        }

        [AllowAnonymous]
        public ActionResult CaseProfile(GesCaseReportType reportType, long caseReportId, long orgId, bool? showCompanyInfo, bool? showCaseInfoBusinessConduct, bool? showStatistic, bool? showCompanyEvents, bool? showSummary, bool? showAlerts, bool? showDescription, bool? showConclusion, bool? showGesCommentary, bool? showLatestNews, bool? showEngagementInformation, bool? showDiscussionPoint, bool? showOtherStakeholder, bool? showKPI, bool? showGuidelinesAndConventions, bool? showConfirmationDetails, bool? showReferences, bool? showCompanyDialogue, bool? showSourceDialogue, bool? showCompanyRelatedItems, bool? showGesContactInformation, bool? showAdditionalDocuments, bool? showSummaryMaterialRisk, bool? showClosingDetail)
        {
            var caseReport = _caseProfileService.GetCaseProfileCoreModel(reportType, caseReportId, orgId);
            if (caseReport == null)
            {
                return View($"~/Views/Company/CaseProfiles/CaseProfileImplementing.cshtml");
            }

            var caseReportTitle = SafeExecute(() => _caseProfileService.GetCaseReportTitle(caseReportId), $"Exception when getting the case report title of case ({caseReportId}).");
            ViewBag.CaseReportTitle = caseReport.CaseComponent?.NormId != 0
                ? $"{caseReportTitle?.Value} <img src='/Content/img/icons/norm_area_{caseReport.CaseComponent?.NormId}_32.png' title='{caseReport.CaseComponent?.Norm}' class='norm-icon' alt='norm area'/>"
                : caseReportTitle?.Value;

            var engagementType = _caseProfileService.GetEngagementType(caseReportId);

            var recommendationValue = caseReport?.NewI_GesCaseReportStatuses_Id ?? 0;

            if (recommendationValue == (long)RecommendationType.Resolved || recommendationValue == (long)RecommendationType.ResolvedIndicationOfViolation)
            {
                recommendationValue = (long)RecommendationType.Resolved;
            }            

            switch (reportType)
            {
                case GesCaseReportType.StConfirmed:
                case GesCaseReportType.StIndicationOfViolation:
                case GesCaseReportType.StAlert:
                case GesCaseReportType.StResolved:
                case GesCaseReportType.StArchived:
                    ViewBag.CaseProfileInvisibleEntities = new List<GesCaseProfileTemplatesViewModel>();
                    break;
                default:
                    ViewBag.CaseProfileInvisibleEntities = _caseProfileTemplatesRepository.GetGesCaseProfileInvisibleEntitiesClientViews(engagementType, recommendationValue); ;
                    break;
            }            

            ViewBag.CaseProfileType = reportType;
            ViewBag.Title = $"<a class=\"content-export-header-sus\" target=\"_blank\" href=\"{Url.Action("Profile", "Company", new { id = caseReport.BaseComponent.CompanyId }, Request?.Url?.Scheme)}\">{caseReport.BaseComponent.CompanyName}</a>";

            ViewBag.CaseProfileLink = $"{Url.Action("CaseReport", "Company", new { id = caseReportId }, Request?.Url?.Scheme)}";

            var additionalincidents = _caseProfileService.GetAdditionalCaseReports(caseReportId, orgId);
            var alerts = _alertService.GetAlertsByCompanyId(caseReport.BaseComponent.CompanyId)?.ToList();
            // Add more settings later
            var model = new GesExportModel<CaseProfileCoreViewModel> { Data = caseReport };

            model.ShowCompanyInfo = showCompanyInfo.HasValue && showCompanyInfo.Value;
            model.ShowCaseInfoBusinessConduct = showCaseInfoBusinessConduct.HasValue && showCaseInfoBusinessConduct.Value;
            model.ShowStatistic = showStatistic.HasValue && showStatistic.Value;
            model.ShowCompanyEvents = showCompanyEvents.HasValue && showCompanyEvents.Value;
            model.ShowSummary = showSummary.HasValue && showSummary.Value;

            model.ShowAlerts = showAlerts.HasValue && showAlerts.Value;
            model.ShowDescription = showDescription.HasValue && showDescription.Value;
            model.ShowConclusion = showConclusion.HasValue && showConclusion.Value;
            model.ShowGesCommentary = showGesCommentary.HasValue && showGesCommentary.Value;
            model.ShowLatestNews = showLatestNews.HasValue && showLatestNews.Value;
            model.ShowEngagementInformation = showEngagementInformation.HasValue && showEngagementInformation.Value;
            model.ShowDiscussionPoint = showDiscussionPoint.HasValue && showDiscussionPoint.Value;
            model.ShowOtherStakeholder = showOtherStakeholder.HasValue && showOtherStakeholder.Value;
            model.ShowKPI = showKPI.HasValue && showKPI.Value;
            model.ShowGuidelinesAndConventions = showGuidelinesAndConventions.HasValue && showGuidelinesAndConventions.Value;
            model.ShowConfirmationDetails = showConfirmationDetails.HasValue && showConfirmationDetails.Value;
            model.ShowReferences = showReferences.HasValue && showReferences.Value;
            model.ShowCompanyDialogue = showCompanyDialogue.HasValue && showCompanyDialogue.Value;
            model.ShowSourceDialogue = showSourceDialogue.HasValue && showSourceDialogue.Value;
            model.ShowCompanyRelatedItems = showCompanyRelatedItems.HasValue && showCompanyRelatedItems.Value;

            model.ShowGesContactInformation = showGesContactInformation.HasValue && showGesContactInformation.Value;
            model.ShowAdditionalDocuments = showAdditionalDocuments.HasValue && showAdditionalDocuments.Value;

            model.ShowSummaryMaterialRisk = showSummaryMaterialRisk.HasValue && showSummaryMaterialRisk.Value;
            model.ShowClosingDetail = showClosingDetail.HasValue && showClosingDetail.Value;

            if (additionalincidents != null && additionalincidents.Any())
            {
                model.AdditionalIncidents = additionalincidents.OrderBy(x => x.ServiceEngagementThemeNorm).ThenByDescending(d => d.SortOrderRecommendation).ToList();
            }

            model.UpcommingEvents = _calendarService.GetCalendarEventsByCompanyId(caseReport.BaseComponent.CompanyId).Where(x => x.EventDate >= DateTime.Today)?.ToList();
            if (alerts != null && alerts.Any())
                model.Alerts = alerts.OrderByDescending(x => x.Date).ThenByDescending(d => d.Heading).ToList();

            if (reportType == GesCaseReportType.BcEngage)
            {
                try
                {
                    var viewModel = (CaseProfileFullAttributeViewModel)caseReport;
                    var sdgs = viewModel?.SdgAndGuidelineConventionComponent?.Sdgs;
                    var sdgsViewModel = sdgs.Select(x => new CaseProfileExportSDGViewModel() { Name = x.Sdg_Name, Link = x.Sdg_Link, Content = GetDownLoadContent(x.DocumentId, orgId) }).ToList();
                    model.SDGImages = sdgsViewModel;
                }
                catch { }
            }
            switch (reportType)
            {
                case GesCaseReportType.StConfirmed:
                case GesCaseReportType.StIndicationOfViolation:
                case GesCaseReportType.StAlert:                    
                case GesCaseReportType.StResolved:                    
                case GesCaseReportType.StArchived:
                    return View("~/Views/Export/Export_Sustainalytics/CaseProfileStandard.cshtml", model);
                default:
                    return View("~/Views/Export/Export_Sustainalytics/CaseProfile.cshtml", model);
            }
            
        }

        [AllowAnonymous]
        public ActionResult CompanyDialogue(GesCaseReportType reportType, long caseReportId, long orgId, DateTime? fromDate, DateTime? toDate)
        {
            var caseReport = _caseProfileService.GetCaseProfileCoreModel(reportType, caseReportId, orgId);
            if (caseReport == null)
            {
                return View($"~/Views/Company/CaseProfiles/CaseProfileImplementing.cshtml");
            }

            var caseReportTitle = SafeExecute(() => _caseProfileService.GetCaseReportTitle(caseReportId), $"Exception when getting the case report title of case ({caseReportId}).");
            ViewBag.CaseReportTitle = caseReport.CaseComponent?.NormId != 0
                ? $"{caseReportTitle?.Value} <img src='/Content/img/icons/norm_area_{caseReport.CaseComponent?.NormId}_32.png' title='{caseReport.CaseComponent?.Norm}' class='norm-icon' alt='norm area'/>"
                : caseReportTitle?.Value;

            ViewBag.CaseProfileType = reportType;
            ViewBag.Title = $"<a target=\"_blank\" href=\"{Url.Action("Profile", "Company", new { id = orgId }, Request?.Url?.Scheme)}\">{caseReport.BaseComponent.CompanyName}</a>";

            var companyDialogues = caseReport.IssueComponent.CompanyDialogues.ToList();

            if (fromDate.HasValue)
            {
                companyDialogues = companyDialogues.Where(x => x.ContactDate >= fromDate.Value).ToList();
            }

            if (toDate.HasValue)
            {
                companyDialogues = companyDialogues.Where(x => x.ContactDate <= toDate.Value).ToList();
            }

            // Add more settings later
            var model = new CompanyDialogueViewModel { Data = companyDialogues, BaseComponent = caseReport.BaseComponent };

            model.FromDate = fromDate;
            model.ToDate = toDate;

            return View("~/Views/Export/Export_Sustainalytics/CompanyDialogue.cshtml", model);
        }

        public string GetDownLoadContent(Guid? documentId, long orgId)
        {
            if (!documentId.HasValue)
                return "";

            var document = this.SafeExecute(() => this._gesDocumentService.GetDocumentById(orgId, documentId.Value), $"Exception when getting the document with criteria organization({orgId}) and document({documentId.Value})");

            if (document != null)
            {
                var fileStream = this.SafeExecute(() => this._gesFileStorageService.GetStream(orgId, documentId.Value), $"Exception when read the stream of document with criteria organization({orgId}) and document({documentId.Value})");

                if (fileStream != null && fileStream.CanRead)
                {
                    var fileStreamResult = File(fileStream, System.Web.MimeMapping.GetMimeMapping(document.FileName), document.FileName);
                    if (fileStreamResult != null)
                    {
                        byte[] fileBytes = new byte[fileStreamResult.FileStream.Length];
                        int byteCount = fileStreamResult.FileStream.Read(fileBytes, 0, (int)fileStreamResult.FileStream.Length);
                        string fileContent = Convert.ToBase64String(fileBytes);
                        fileStreamResult.FileStream.Close();
                        return "data:image/png;base64," + fileContent;
                    }
                }
            }

            return "";
        }

        [AllowAnonymous]
        public ActionResult CoverPage(long companyId, long gesCompanyId, long orgId, long individualId)
        {
            var companyDetail = _companiesService.GetCompanyDetailViewModel(companyId, orgId, individualId, true) ?? new CompanyDetailViewModel();
            ViewBag.Title = $"<a target=\"_blank\" href=\"{Url.Action("Profile", "Company", new { id = companyId }, Request?.Url?.Scheme)}\">{companyDetail.CompanyName}</a>";
            return View("~/Views/Export/Export_Sustainalytics/CoverPage.cshtml");
        }

        [AllowAnonymous]
        public ActionResult CoverPageCaseProfile(GesCaseReportType reportType, long caseProfileId, long orgId)
        {
            var caseProfileViewModel = _caseProfileService.GetCaseProfileCoreModel(reportType, caseProfileId, orgId);

            ViewBag.Title = $"<a target=\"_blank\" href=\"{Url.Action("Profile", "Company", new { id = caseProfileViewModel.BaseComponent.CompanyId }, Request?.Url?.Scheme)}\">{caseProfileViewModel.BaseComponent.CompanyName}</a>";
            ViewBag.SubTitle = $"<a target=\"_blank\" href=\"{Url.Action("CaseReport", "Company", new { id = caseProfileId }, Request?.Url?.Scheme)}\">{caseProfileViewModel.CaseComponent.Heading}</a>";
            ViewBag.EngagementTheme = caseProfileViewModel.CaseComponent.EngagementTheme + (!string.IsNullOrEmpty(caseProfileViewModel.CaseComponent?.Norm) ? " - " + caseProfileViewModel.CaseComponent?.Norm : "");
            return View("~/Views/Export/Export_Sustainalytics/CoverPageCaseProfile.cshtml");
        }

        [AllowAnonymous]
        public ActionResult CoverPageDialogue(long companyId, long gesCompanyId, long orgId, long individualId, string fromDate, string toDate, GesCaseReportType reportType, long caseProfileId)
        {
            var caseProfileViewModel = _caseProfileService.GetCaseProfileCoreModel(reportType, caseProfileId, orgId);

            var companyDetail = _companiesService.GetCompanyDetailViewModel(companyId, orgId, individualId, true) ?? new CompanyDetailViewModel();
            ViewBag.Title = $"<a target=\"_blank\" href=\"{Url.Action("Profile", "Company", new { id = companyId }, Request?.Url?.Scheme)}\">{companyDetail.CompanyName}</a>";

            string dateRangeTitle = "";

            if (string.IsNullOrEmpty(fromDate) && string.IsNullOrEmpty(toDate))
            {
                dateRangeTitle = "ALL DIALOGUES";
            }
            else if (string.IsNullOrEmpty(fromDate) && !string.IsNullOrEmpty(toDate))
            {
                dateRangeTitle += "DIALOGUES: --> " + toDate;
            }
            else if (! string.IsNullOrEmpty(fromDate) && string.IsNullOrEmpty(toDate))
            {
                dateRangeTitle += "DIALOGUES: " + fromDate + " --> NOW";
            }
            else if (!string.IsNullOrEmpty(fromDate) && !string.IsNullOrEmpty(toDate))
            {
                dateRangeTitle += "DIALOGUES: " + fromDate + " --> " + toDate;
            }           

            ViewBag.DateRange = dateRangeTitle;

            ViewBag.EngagementTheme = caseProfileViewModel.CaseComponent.EngagementTheme + (!string.IsNullOrEmpty(caseProfileViewModel.CaseComponent?.Norm) ? " - " + caseProfileViewModel.CaseComponent?.Norm : "");
            return View("~/Views/Export/Export_Sustainalytics/CoverPageDialogue.cshtml");
        }

        [HttpGet]
        public ActionResult Download(string fileName, string fileDownloadName)
        {
            var fileStream = _gesFileStorageService.GetFileStream(fileName);

            if (fileStream != null && fileStream.CanRead)
            {
                return File(fileStream, MimeMapping.GetMimeMapping(fileName), fileDownloadName);
            }
            return HttpNotFound();
        }


        private IEnumerable<DocumentViewModel> GetDocuments(long companyId)
        {
            return _documentService.GetDocumentsByCompanyId(companyId).Select(x =>
            {
                x.DownloadUrl = CommonHelper.GetDocDownloadUrl(x.FileName);
                return x;
            });
        }
    }
}