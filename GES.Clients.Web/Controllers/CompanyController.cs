using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;
using GES.Clients.Web.Configs;
using GES.Clients.Web.Helpers;
using GES.Clients.Web.Models;
using GES.Common.Enumeration;
using GES.Common.Extensions;
using GES.Common.Models;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Models;
using GES.Inside.Data.Services.Auth;
using GES.Inside.Data.Services.Interfaces;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Z.EntityFramework.Plus;
using GES.Clients.Web.Extensions;
using GES.Clients.Web.PhantomJs;
using GES.Common.Configurations;
using GES.Common.Services.Interface;
using GES.Common.Logging;
using GES.Common.Exceptions;
using GES.Common.Helpers;
using GES.Inside.Data.Extensions;
using GES.Inside.Data.Models.Anonymous;
using GES.Common.Resources;
using GES.Inside.Data.Models.Auth;
using Ical.Net;
using Ical.Net.CalendarComponents;
using Ical.Net.DataTypes;
using Ical.Net.Serialization;
using Calendar = Ical.Net.Calendar;
using Exception = System.Exception;
using Guard = GES.Common.Exceptions.Guard;
using GES.Inside.Data.Repository.Interfaces;

namespace GES.Clients.Web.Controllers
{
    public class CompanyController : GesControllerBase
    {
        #region Declaration
        private readonly II_CompaniesService _companiesService;
        private readonly ICalendarService _calendarService;
        private readonly IDocumentService _documentService;
        private readonly IOldUserService _oldUserService;
        private readonly IGesUserService _gesUserService;
        private readonly IG_IndividualsService _gIndividualsService;
        private readonly IGesCaseReportSignUpService _gesCaseReportSignUpService;
        private readonly II_GesCompanyWatcherService _gesCompanyWatcherService;
        private readonly II_GesCaseReportsG_IndividualsService _gesCaseReportsG_IndividualsService;
        private readonly IExportProcessor _exportProcessor;
        private readonly II_GesCaseProfilesService _gesCaseProfilesService;
        private readonly IExportExcelService _exportExcelService;
        private readonly II_EngagementTypesService _engagementTypesService;
        private readonly IPhantomJsRunner _phantomJsRunner;
        private const long BussinessConductServiceId = 30;
        private readonly IGesFileStorageService _gesFileStorageService;
        private readonly IPdfFilesMerger _pdfFilesMerger;
        private readonly IGesDocumentService _gesDocumentService;
        private readonly IPdfFileDownload _pdfFileDownload;
        private readonly I_GesEventCalendarUserAcceptService _gesEventCalendarUserAcceptService;
        private readonly IGesCaseProfileTemplatesRepository _caseProfileTemplatesRepository;

        public IGesFileStorageService GesFileStorageService { get; set; }

        #endregion

        #region Constructor
        public CompanyController(IGesLogger logger, II_CompaniesService companiesService, ICalendarService calendarService, IDocumentService documentService, IOldUserService oldUserService, IGesUserService gesUserService, IG_IndividualsService gIndividualsService,
            IGesCaseReportSignUpService gesCaseReportSignUpService, II_GesCompanyWatcherService gesCompanyWatcherService, II_GesCaseReportsG_IndividualsService gesCaseReportsG_IndividualsService,
            IExportProcessor exportProcessor, II_GesCaseProfilesService gesCaseProfilesService, IExportExcelService exportExcelService, IPhantomJsRunner phantomJsRunner, II_EngagementTypesService engagementTypesService, IGesFileStorageService gesFileStorageService, IPdfFilesMerger pdfFilesMerger, IGesDocumentService gesDocumentService, IPdfFileDownload pdfFileDownload, I_GesEventCalendarUserAcceptService gesEventCalendarUserAcceptService, IGesCaseProfileTemplatesRepository caseProfileTemplatesRepository)
            : base(logger)
        {
            _companiesService = companiesService;
            _calendarService = calendarService;
            _documentService = documentService;
            _oldUserService = oldUserService;
            _gesUserService = gesUserService;
            _gIndividualsService = gIndividualsService;
            _gesCaseReportSignUpService = gesCaseReportSignUpService;
            _gesCompanyWatcherService = gesCompanyWatcherService;
            _gesCaseReportsG_IndividualsService = gesCaseReportsG_IndividualsService;
            _exportProcessor = exportProcessor;
            _gesCaseProfilesService = gesCaseProfilesService;
            _exportExcelService = exportExcelService;
            _phantomJsRunner = phantomJsRunner;
            _engagementTypesService = engagementTypesService;
            _gesFileStorageService = gesFileStorageService;
            _pdfFilesMerger = pdfFilesMerger;
            _gesDocumentService = gesDocumentService;
            _pdfFileDownload = pdfFileDownload;
            _gesEventCalendarUserAcceptService = gesEventCalendarUserAcceptService;
            _caseProfileTemplatesRepository = caseProfileTemplatesRepository;
        }

        #endregion

        #region ActionResult

        public ActionResult List(
            string q, string qrCompanyIdSelected,
            string portfolioIds, string recommendationIds, string countries, string homeCountries,
            string normAreaIds, string serviceIds, string sectorIds, string continentIds,
            bool? exportAllHoldings, bool? isFocusList, bool? isnew, bool? clearCache,
            bool showKeywords = false, bool onlyCompaniesWithActiveCases = false)
        {

            if (clearCache ?? false)
            {
                // get list of individualIds and orgIds for clearing cache
                var recentLoggedInEntities = CommonHelper.GetRecentLoggedInIndividualIdsAndOrgIds(_gesUserService, _oldUserService);
                var toBeClearedIndividualIds = recentLoggedInEntities.Select(i => i.Id).ToList();
                var toBeClearedOrgIds = recentLoggedInEntities.Where(i => i.ForeignKey != null).Select(i => i.ForeignKey.Value).Distinct().ToList();
                CacheHelper.ClearAllCache(toBeClearedIndividualIds, toBeClearedOrgIds);
            }

            var searchName = q ?? string.Empty;
            var companyIdSelected = qrCompanyIdSelected ?? string.Empty;
            countries = countries ?? string.Empty;
            homeCountries = homeCountries ?? string.Empty;
            recommendationIds = recommendationIds ?? string.Empty;
            portfolioIds = portfolioIds ?? string.Empty;
            normAreaIds = normAreaIds ?? string.Empty;
            serviceIds = serviceIds ?? string.Empty;
            sectorIds = sectorIds ?? string.Empty;
            var industryGroupIds = string.Join(",", _companiesService.GetIndustryGroupIdsFromSectorIds(sectorIds));
            continentIds = continentIds ?? string.Empty;
            countries = string.IsNullOrEmpty(countries)
                ? string.Join(",", _companiesService.GetCountryCodesFromContinentIds(continentIds))
                : countries;

            long orgId = 0, individualId = 0;
            this.GetIndividualInfo(_gIndividualsService, ref individualId, ref orgId);

            var boxViewModel = new CompanySearchBoxViewModel
            {
                PortfolioIndexes = GetPortfolioIndexDropdown(orgId, portfolioIds),
                ShowClosedCases = true,
                OnlyCompaniesWithActiveCases = onlyCompaniesWithActiveCases,
                Recommendations = GetRecommendationDropdown(recommendationIds),
                Conclusions = _companiesService.GetConclusions().Select(ToSelectListItem),
                EngagementAreas = GetEngagementAreaDropdown(serviceIds, normAreaIds, orgId),
                Locations = GetLocationDropdown(countries),
                Responses = _companiesService.GetResponses().Select(ToSelectListItem),
                Progresses = _companiesService.GetProgresses().Select(ToSelectListItem),
                Industries = GetIndustryGroupDropdown(industryGroupIds),
                Name = searchName,
                //onlySearchCompany = !string.IsNullOrEmpty(searchName),
                onlySearchCompany = false,
                onlyShowFocusList = isFocusList ?? false,
                HomeCountries = GetHomeCountriesDropdown(homeCountries),
                ClientType = _companiesService.GetClientType(orgId),
                CompanyId = companyIdSelected
            };
            ViewBag.IsNew = isnew ?? false;

            ViewBag.Title = Resources.SearchAndAnalysis;
            ViewBag.IsFocusList = false;
            if (isFocusList != null && isFocusList == true)
            {
                ViewBag.Title = Resources.FocusList;
                ViewBag.IsFocusList = true;
                boxViewModel.OnlyCompaniesWithActiveCases = false;
            }

            ViewBag.ExportAllHoldings = false;
            if (exportAllHoldings != null && exportAllHoldings == true)
            {
                ViewBag.ExportAllHoldings = true;
                boxViewModel.OnlyCompaniesWithActiveCases = false;
            }

            return View(boxViewModel);
        }

        private SelectListItem ToSelectListItem(IdNameModel item)
        {
            return new SelectListItem
            {
                Text = item.Name,
                Value = item.Id.ToString()
            };
        }

        public ActionResult Profile(string id, bool? isnew)
        {
            var companyDetail = new CompanyDetailViewModel();

            long orgId = 0, individualId = 0;
            this.GetIndividualInfo(_gIndividualsService, ref individualId, ref orgId);

            long companyId;
            if (long.TryParse(id, out companyId))
            {
                var checkCompany = _companiesService.CheckCompanyInPortfolios(companyId, orgId);

                switch (checkCompany)
                {
                        case SearchCompanyResult.OutsidePortfolio:
                            return View("~/Views/Company/CompanyOutsidePortfolio.cshtml");
                        case SearchCompanyResult.NotFound:
                            return View("~/Views/Company/CompanyNotFound.cshtml");
                }

                companyDetail = _companiesService.GetCompanyDetailViewModel(companyId, orgId, individualId) ?? new CompanyDetailViewModel();

                // get events > then push into the Model
                companyDetail.Events = _calendarService.GetCalendarEventsByCompanyId(companyId)?.ToList();

                // documents
                companyDetail.Documents = GetDocuments(companyId)?.ToList();
            }

            // add support info to ViewModel

            companyDetail.OrganizationId = orgId;
            companyDetail.IndividualId = individualId;
            ViewBag.IsNew = isnew ?? false;
            ViewBag.PageClass = "page-company page-company-profile";
            ViewBag.ClientType = _companiesService.GetClientType(orgId).ToString();
            return View(companyDetail);
        }

        public ActionResult CaseReport(string id)
        {
            long caseReportId;

            if (!long.TryParse(id, out caseReportId))
            {
                return View(new CaseReportViewModel());
            }

            long orgId = 0, individualId = 0;

            SafeExecute(() => this.GetIndividualInfo(_gIndividualsService, ref individualId, ref orgId), "Exception when getting the individual/organization id");

            var reportType = _gesCaseProfilesService.GetReportType(caseReportId, orgId);

            ViewBag.CaseProfileType = reportType;
            var engagementType = _gesCaseProfilesService.GetEngagementType(caseReportId);

            ViewBag.EngagementType = engagementType;

            var caseReportTitle = SafeExecute(() => _gesCaseProfilesService.GetCaseReportTitle(caseReportId), $"Exception when getting the case report title of case ({caseReportId}).");
            ViewBag.CaseReportId = caseReportId;

            ViewBag.ClientType = _companiesService.GetClientType(orgId).ToString();

            return SafeExecute(() =>
            {
                var caseReport = _gesCaseProfilesService.GetCaseProfileCoreModel(reportType, caseReportId, orgId);

                if (caseReport == null || caseReport.BaseComponent == null)
                {
                    if (reportType == GesCaseReportType.Unsubscribed)
                        return View("~/Views/Company/CaseProfiles/CaseProfileUnsubscribed.cshtml");
                    return View("~/Views/Company/CaseProfiles/CaseProfileImplementing.cshtml");
                }

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

                var normId = caseReport.CaseComponent?.NormId;

                ViewBag.Title = $"{caseReportTitle?.Key} > {caseReportTitle?.Value}";

                ViewBag.SubTitle = normId != null && normId != 0 ? $"{caseReportTitle?.Value} <img src='/Content/img/icons/norm_area_{normId}_32.png' title='{caseReport.CaseComponent?.Norm}' class='norm-icon' alt='norm area'/>"
                    : caseReportTitle?.Value;

                ViewBag.InitialHeading = $"<a href='{Url.Action("Profile", new { id = caseReport.BaseComponent?.CompanyId })}'>{caseReportTitle?.Key}</a>";

                ViewBag.CompanyId = caseReport.BaseComponent?.CompanyId;
                ViewBag.CompanyName = caseReportTitle?.Key;

                return View("~/Views/Company/CaseProfiles/CaseProfile.cshtml", caseReport);
            }, $"Exception when getting the case report data of case ({caseReportId}).");
        }

        public ActionResult ExportView(long id)
        {
            // Fake just return the normal view
            return RedirectToAction("CaseReport", new { id = id });
        }

        public ActionResult Export(long id)
        {
            var pageUrl = Url.Action("ExportView", "Company", new { id }, Request.Url.Scheme);
            try
            {
                var pdf = this._exportProcessor.Export(pageUrl);

                return File(pdf.DataFile, pdf.MimeType);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        
        public ActionResult PositionPaperForConventions()
        {
            var orgId = this.GetOrganizationId(_gIndividualsService);

            try
            {
                var positionReports = this._gesDocumentService.GetPositionReport(this.GetOrganizationId(_gIndividualsService));
                
                return View(positionReports);
            }
            catch (GesServiceException ex)
            {
                Logger.Error(ex, $"Error when getting the anual report of organization({orgId})");

                throw;
            }
        }        
        public ActionResult PositionPaperForGlobalEthicalStandard()
        {
            var orgId = this.GetOrganizationId(_gIndividualsService);

            try
            {
                var positionReports = this._gesDocumentService.GetPositionReport(this.GetOrganizationId(_gIndividualsService));
                
                return View(positionReports);
            }
            catch (GesServiceException ex)
            {
                Logger.Error(ex, $"Error when getting the anual report of organization({orgId})");

                throw;
            }
        }

        [Route("PositionPaper/Download/{documentId}")]
        public ActionResult Download(Guid? documentId)
        {
            if (!documentId.HasValue)
                return HttpNotFound();

            var orgId = this.GetOrganizationId(_gIndividualsService);

            var document = this.SafeExecute(() => this._gesDocumentService.GetDocumentById(orgId, documentId.Value), $"Exception when getting the document with criteria organization({orgId}) and document({documentId.Value})");

            if(document != null)
            {
                var fileStream = this.SafeExecute(() => this._gesFileStorageService.GetStream(orgId, documentId.Value), $"Exception when read the stream of document with criteria organization({orgId}) and document({documentId.Value})");

                if(fileStream != null && fileStream.CanRead)
                {
                    return File(fileStream, System.Web.MimeMapping.GetMimeMapping(document.FileName), document.FileName);
                } 
            }

            return HttpNotFound();
        }
        #endregion

        [HttpPost]
        public JsonResult GetDataForCompaniesJqGrid(JqGridViewModel jqGridParams, string name, string isin, List<long> portfolioIds, bool? onlyShowFocusList, bool? notshowclosecase, bool? onlycompanieswithactivecase,
            List<long?> recommendationId, List<long?> conclusionId, string engagementAreaIds, List<Guid?> locationId, List<long?> responseId, List<long?> progressId, List<long?> industryId, bool? onlySearchCompanyName, List<Guid?> homeCountryIds, string companyId, string sustainalyticsId)
        {

            long orgId = 0, individualId = 0;
            this.GetIndividualInfo(_gIndividualsService, ref individualId, ref orgId);

            if (recommendationId == null || recommendationId.All(i => i == null))
            {
                recommendationId = new List<long?>();
            }

            if (conclusionId == null || conclusionId.All(i => i == null))
            {
                conclusionId = new List<long?>();
            }

            if (responseId == null || responseId.All(i => i == null))
            {
                responseId = new List<long?>();
            }

            if (progressId == null || progressId.All(i => i == null))
            {
                progressId = new List<long?>();
            }

            if (locationId == null || locationId.All(i => i == null))
            {
                locationId = new List<Guid?>();
            }
            if (industryId == null || industryId.All(i => i == null))
            {
                industryId = new List<long?>();
            }
            if (homeCountryIds == null || homeCountryIds.All(i => i == null))
            {
                homeCountryIds = new List<Guid?>();
            }

            if (portfolioIds == null || (portfolioIds.Count == 1 && portfolioIds.Contains(0)))
            {
                portfolioIds = new List<long>();
            }

            var CompanyId = ConvertStringToLong(companyId);
            var SustainalyticsId = ConvertStringToLong(sustainalyticsId);

            var serviceId = new List<long?>();
            long? engagementTypeId = null;
            long? normId = null;
            if (!string.IsNullOrEmpty(engagementAreaIds) && engagementAreaIds.Contains("-"))
            {
                var array = engagementAreaIds.Trim().Split('-');
                long temp;

                if (array[0].Trim().Length > 0)
                {
                    serviceId = ExtractStringToListLong(array[0]);
                }

                if (long.TryParse(array[1], out temp))
                {
                    engagementTypeId = temp;
                }

                if (long.TryParse(array[2], out temp))
                {
                    normId = temp;

                }
            }

            var companyCaseReport = this.SafeExecute(() => _companiesService.GetCompanyCasesGrid(jqGridParams, orgId, individualId, onlyShowFocusList ?? false, name, isin, portfolioIds, notshowclosecase ?? false, onlycompanieswithactivecase ?? false,
                recommendationId, conclusionId, serviceId, normId, locationId, responseId, progressId, industryId, engagementTypeId, homeCountryIds, CompanyId, SustainalyticsId),
                $"Exception when get the company case grid.");

            return Json(companyCaseReport, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetCaseReports(JqGridViewModel jqGridParams, long gesCompanyId, long companyId, bool notshowclosecase, string name,
            string recommendationIdString, string conclusionIdString, string engagementAreaIds, string locationIdString, string responseIdString, string progressIdString, string industryIdString,
            bool? onlyShowFocusList, bool? companyInfocusList)
        {
            var recommendationIds = ExtractStringToListLong(recommendationIdString);
            var conclusionIds = ExtractStringToListLong(conclusionIdString);
            var responseIds = ExtractStringToListLong(responseIdString);
            var progressIds = ExtractStringToListLong(progressIdString);
            var locationIds = ExtractStringToListGuid(locationIdString);
            var industryIds = ExtractStringToListLong(industryIdString);

            var serviceId = new List<long?>();
            long? engagementTypeId = null;
            long? normId = null;
            if (!string.IsNullOrEmpty(engagementAreaIds) && engagementAreaIds.Contains("-"))
            {
                var array = engagementAreaIds.Trim().Split('-');
                long temp;

                if (array[0].Trim().Length > 0)
                {
                    var serviceIdstr = array[0].Trim();
                    if (serviceIdstr == "20") serviceIdstr = "30";
                    serviceId = ExtractStringToListLong(serviceIdstr);
                }

                if (long.TryParse(array[1], out temp))
                {
                    engagementTypeId = temp;
                }

                if (long.TryParse(array[2], out temp) && !(serviceId.Count == 1 && serviceId.Contains(Configurations.GovernanceServiceId)))
                {
                    normId = temp;
                }
            }

            long orgId = 0, individualId = 0;
            this.GetIndividualInfo(_gIndividualsService, ref individualId, ref orgId);

            bool cansignUp = CheckClaimSignUp();

            var caseReports = this.SafeExecute(() => _companiesService.GetCaseReportsData(gesCompanyId, companyId, notshowclosecase, orgId, individualId, onlyShowFocusList ?? false, companyInfocusList ?? false, name, recommendationIds, conclusionIds, serviceId, normId, locationIds, responseIds, progressIds, industryIds, engagementTypeId)
            , $"Exception when getting the case report data");

            var clientType = _companiesService.GetClientType(orgId);
            string standforBc = clientType == ClientType.BusinessConduct? "GS":"GES";

            var caseProfileTemp = new List<CaseReportListViewModel>();
            foreach (var caseReportListViewModel in caseReports)
            {
                caseReportListViewModel.ServiceEngagementThemeNorm =
                    caseReportListViewModel.ServiceEngagementThemeNorm.Replace("Global Standards", standforBc)
                        .Replace("Stewardship & Risk", "S&R");

                caseReportListViewModel.CanSignUp = cansignUp;

                if (caseReportListViewModel.EngagementTypeId == (long)EngagementTypeEnum.Conventions)
                {
                    if ((ServiceSelectListItemConfiguration.NormIds_Group1 != null &&
                         caseReportListViewModel.NormId != null && ServiceSelectListItemConfiguration.NormIds_Group1.Contains((long)caseReportListViewModel.NormId)) ||
                        (ServiceSelectListItemConfiguration.NormIds_Group2 != null && caseReportListViewModel.NormId != null && ServiceSelectListItemConfiguration.NormIds_Group2.Contains((long)caseReportListViewModel.NormId)))
                    {
                        caseProfileTemp.Add(caseReportListViewModel);
                    }
                    
                }
                else
                {
                    caseProfileTemp.Add(caseReportListViewModel);
                }
            }

            caseReports = caseProfileTemp;

            var sortCol = jqGridParams.sidx.ToLower();
            var sortDir = jqGridParams.sord.ToLower();
            if (!(string.IsNullOrEmpty(sortCol) && string.IsNullOrEmpty(sortDir)))
            {
                switch (sortCol)
                {
                    case "reportincident":
                        caseReports = sortDir == "asc"
                            ? caseReports.OrderBy(x => x.IssueName).ThenBy(d => d.SortOrderEngagementType).ToList()
                            : caseReports.OrderByDescending(x => x.IssueName).ThenByDescending(d => d.SortOrderEngagementType).ToList();
                        break;
                    case "location":
                        caseReports = sortDir == "asc"
                            ? caseReports.OrderBy(x => x.Location).ThenBy(d => d.SortOrderEngagementType).ToList()
                            : caseReports.OrderByDescending(x => x.Location).ThenByDescending(d => d.SortOrderEngagementType).ToList();
                        break;
                    case "serviceengagementthemenorm":
                        caseReports = sortDir == "asc"
                            ? caseReports.OrderBy(x => x.ServiceEngagementThemeNorm).ThenBy(x => x.SortOrderEngagementType).ToList()
                            : caseReports.OrderByDescending(x => x.ServiceEngagementThemeNorm).ThenByDescending(x => x.SortOrderEngagementType).ToList();
                        break;
                    case "service":
                        caseReports = sortDir == "asc"
                            ? caseReports.OrderBy(x => x.Service).ThenBy(d => d.SortOrderEngagementType).ToList()
                            : caseReports.OrderByDescending(x => x.Service).ThenByDescending(d => d.SortOrderEngagementType).ToList();
                        break;
                    case "engagementthemenorm":
                        caseReports = sortDir == "asc"
                            ? caseReports.OrderBy(x => x.EngagementThemeNorm).ThenBy(x => x.SortOrderEngagementType).ToList()
                            : caseReports.OrderByDescending(x => x.EngagementThemeNorm).ThenByDescending(x => x.SortOrderEngagementType).ToList();
                        break;
                    case "recommendation":
                        caseReports = sortDir == "asc"
                            ? caseReports.OrderBy(x => x.SortOrderRecommendation).ThenBy(d => d.SortOrderEngagementType).ToList()
                            : caseReports.OrderByDescending(x => x.SortOrderRecommendation).ThenByDescending(d => d.SortOrderEngagementType).ToList();
                        break;
                    case "confirmed":
                        caseReports = sortDir == "asc"
                            ? caseReports.OrderBy(x => x.Confirmed).ThenBy(d => d.SortOrderEngagementType).ToList()
                            : caseReports.OrderByDescending(x => x.Confirmed).ThenByDescending(d => d.SortOrderEngagementType).ToList();
                        break;
                    case "entrydate":
                        caseReports = sortDir == "asc"
                            ? caseReports.OrderBy(x => x.EntryDate).ThenBy(d => d.SortOrderEngagementType).ToList()
                            : caseReports.OrderByDescending(x => x.EntryDate).ThenByDescending(d => d.SortOrderEngagementType).ToList();
                        break;
                    case "progressgrade":
                        caseReports = sortDir == "asc"
                            ? caseReports.OrderBy(x => x.ProgressGrade).ThenBy(d => d.DevelopmentGrade).ToList()
                            : caseReports.OrderByDescending(x => x.ProgressGrade).ThenByDescending(d => d.DevelopmentGrade).ToList();
                        break;
                    case "responsegrade":
                        caseReports = sortDir == "asc"
                            ? caseReports.OrderBy(x => x.ResponseGrade).ThenBy(d => d.DevelopmentGrade).ToList()
                            : caseReports.OrderByDescending(x => x.ResponseGrade).ThenByDescending(d => d.DevelopmentGrade).ToList();
                        break;
                    case "developmentgrade":
                        caseReports = sortDir == "asc"
                            ? caseReports.OrderBy(x => x.DevelopmentGrade).ThenBy(d => d.SortOrderEngagementType).ToList()
                            : caseReports.OrderByDescending(x => x.DevelopmentGrade).ThenByDescending(d => d.SortOrderEngagementType).ToList();
                        break;
                    default:
                        caseReports = caseReports.OrderByDescending(x => x.EntryDate).ThenByDescending(d => d.Created).ToList();
                        break;
                }
            }

            return Json(caseReports.ToPagedList(Logger, jqGridParams.page, jqGridParams.rows), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetCaseReportsByCompanyId(JqGridViewModel jqGridParams, long gesCompanyId, long companyId, bool notshowclosecase, long orgId = 0, long individualId = 0)
        {
            if (orgId == 0 && individualId == 0)
                this.GetIndividualInfo(_gIndividualsService, ref individualId, ref orgId);

            bool cansignUp = CheckClaimSignUp();
            var caseReports = _companiesService.GetCasesDataByCompanyId(gesCompanyId, companyId, orgId, notshowclosecase);
            var clientType = _companiesService.GetClientType(orgId);
            string standforBc = clientType == ClientType.BusinessConduct ? "GS" : "GES";
            
            var caseProfileTemp = new List<CaseReportListViewModel>();
            
            foreach (var caseReportListViewModel in caseReports)
            {
                caseReportListViewModel.ServiceEngagementThemeNorm =
                    caseReportListViewModel.ServiceEngagementThemeNorm.Replace("Global Standards", standforBc)
                        .Replace("Stewardship & Risk", "S&R");

                caseReportListViewModel.CanSignUp = cansignUp;
                
                if (caseReportListViewModel.EngagementTypeId == (long)EngagementTypeEnum.Conventions)
                {
                    if ((ServiceSelectListItemConfiguration.NormIds_Group1 != null &&
                         caseReportListViewModel.NormId != null && ServiceSelectListItemConfiguration.NormIds_Group1.Contains((long)caseReportListViewModel.NormId)) ||
                        (ServiceSelectListItemConfiguration.NormIds_Group2 != null && caseReportListViewModel.NormId != null && ServiceSelectListItemConfiguration.NormIds_Group2.Contains((long)caseReportListViewModel.NormId)))
                    {
                        caseProfileTemp.Add(caseReportListViewModel);
                    }
                    
                }
                else
                {
                    caseProfileTemp.Add(caseReportListViewModel);
                }
            }
            
            caseReports = caseProfileTemp;

            var sortCol = jqGridParams.sidx.ToLower();
            var sortDir = jqGridParams.sord.ToLower();
            if (!(string.IsNullOrEmpty(sortCol) && string.IsNullOrEmpty(sortDir)))
            {
                switch (sortCol)
                {
                    case "reportincident":
                        caseReports = sortDir == "asc"
                            ? caseReports.OrderBy(x => x.IssueName).ThenBy(d => d.SortOrderEngagementType).ToList()
                            : caseReports.OrderByDescending(x => x.IssueName).ThenByDescending(d => d.SortOrderEngagementType).ToList();
                        break;
                    case "location":
                        caseReports = sortDir == "asc"
                            ? caseReports.OrderBy(x => x.Location).ThenBy(d => d.SortOrderEngagementType).ToList()
                            : caseReports.OrderByDescending(x => x.Location).ThenByDescending(d => d.SortOrderEngagementType).ToList();
                        break;
                    case "serviceengagementthemenorm":
                        caseReports = sortDir == "asc"
                            ? caseReports.OrderBy(x => x.ServiceEngagementThemeNorm).ThenBy(x => x.SortOrderEngagementType).ToList()
                            : caseReports.OrderByDescending(x => x.ServiceEngagementThemeNorm).ThenByDescending(x => x.SortOrderEngagementType).ToList();
                        break;
                    case "service":
                        caseReports = sortDir == "asc"
                            ? caseReports.OrderBy(x => x.Service).ThenBy(d => d.SortOrderEngagementType).ToList()
                            : caseReports.OrderByDescending(x => x.Service).ThenByDescending(d => d.SortOrderEngagementType).ToList();
                        break;
                    case "engagementthemenorm":
                        caseReports = sortDir == "asc"
                            ? caseReports.OrderBy(x => x.EngagementThemeNorm).ThenBy(x => x.SortOrderEngagementType).ToList()
                            : caseReports.OrderByDescending(x => x.EngagementThemeNorm).ThenByDescending(x => x.SortOrderEngagementType).ToList();
                        break;
                    case "recommendation":
                        caseReports = sortDir == "asc"
                            ? caseReports.OrderBy(x => x.SortOrderRecommendation).ThenBy(d => d.SortOrderEngagementType).ToList()
                            : caseReports.OrderByDescending(x => x.SortOrderRecommendation).ThenByDescending(d => d.SortOrderEngagementType).ToList();
                        break;
                    case "confirmed":
                        caseReports = sortDir == "asc"
                            ? caseReports.OrderBy(x => x.Confirmed).ThenBy(d => d.SortOrderEngagementType).ToList()
                            : caseReports.OrderByDescending(x => x.Confirmed).ThenByDescending(d => d.SortOrderEngagementType).ToList();
                        break;
                    case "entrydate":
                        caseReports = sortDir == "asc"
                            ? caseReports.OrderBy(x => x.EntryDate).ThenBy(d => d.SortOrderEngagementType).ToList()
                            : caseReports.OrderByDescending(x => x.EntryDate).ThenByDescending(d => d.SortOrderEngagementType).ToList();
                        break;
                    case "progressgrade":
                        caseReports = sortDir == "asc"
                            ? caseReports.OrderBy(x => x.ProgressGrade).ThenBy(d => d.DevelopmentGrade).ToList()
                            : caseReports.OrderByDescending(x => x.ProgressGrade).ThenByDescending(d => d.DevelopmentGrade).ToList();
                        break;
                    case "responsegrade":
                        caseReports = sortDir == "asc"
                            ? caseReports.OrderBy(x => x.ResponseGrade).ThenBy(d => d.DevelopmentGrade).ToList()
                            : caseReports.OrderByDescending(x => x.ResponseGrade).ThenByDescending(d => d.DevelopmentGrade).ToList();
                        break;
                    case "developmentgrade":
                        caseReports = sortDir == "asc"
                            ? caseReports.OrderBy(x => x.DevelopmentGrade).ThenBy(d => d.SortOrderEngagementType).ToList()
                            : caseReports.OrderByDescending(x => x.DevelopmentGrade).ThenByDescending(d => d.SortOrderEngagementType).ToList();
                        break;
                    default:
                        caseReports = caseReports.OrderByDescending(x => x.EntryDate).ThenByDescending(d => d.Created).ToList();
                        break;
                }
            }

            return Json(caseReports.ToPagedList(Logger, jqGridParams.page, jqGridParams.rows), JsonRequestBehavior.AllowGet);
        }

        private static List<long?> ExtractStringToListLong(string recommendationIdString)
        {
            List<long?> recommendationId;
            if (string.IsNullOrEmpty(recommendationIdString))
            {
                recommendationId = new List<long?>();
            }
            else
            {
                recommendationId = recommendationIdString.Split(',')
                                                         .Select(i =>
                                                                 {
                                                                     long result = 0;
                                                                     return (long.TryParse(i, out result) ? result : (long?)null);
                                                                 })
                                                         .ToList();
            }
            return recommendationId;
        }

        private static List<Guid?> ExtractStringToListGuid(string input)
        {
            List<Guid?> results;
            if (string.IsNullOrEmpty(input))
            {
                results = new List<Guid?>();
            }
            else
            {
                results = input.Split(',')
                    .Select(i =>
                    {
                        Guid result = Guid.Empty;
                        return (Guid.TryParse(i, out result) ? result : (Guid?)null);
                    })
                    .ToList();
            }
            return results;
        }

        private long? ConvertStringToLong(string input)
        {
            long result;
            if (string.IsNullOrEmpty(input))
            {
                return null;
            }

            if (long.TryParse(input, out result))
            {
                return result;
            }

            return null;
        }

        [HttpPost]
        public JsonResult GetCompaniesForAutocomplete(string term, int limit, bool searchTags = true, bool searchCases = true, bool searchCompanies = true)
        {
            long orgId = 0, individualId = 0;
            this.GetIndividualInfo(_gIndividualsService, ref individualId, ref orgId);

            var clients = this.SafeExecute(() => _companiesService.GetAutoCompleteCompanyIssueName(orgId, term, limit, searchTags, searchCases, searchCompanies),
                $"Exception when getting the auto complete issue name with term({term}), limit({limit}), searchTags({searchTags}).");

            return Json(new
            {
                total = clients.Count,
                rows = clients
            }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult UpdateGesCaseReportSignUp(bool isSignUp, bool isActive, long gesCaseReportId)
        {
            bool result;
            long orgId = 0, individualId = 0;
            this.GetIndividualInfo(_gIndividualsService, ref individualId, ref orgId);

            //Check permission

            if (!CheckClaimSignUp())
            {
                return Json(new
                {
                    meta = new
                    {
                        success = false,
                        error = "You don't have permission to update Sign Up values"
                    },
                    data = new { }
                }, JsonRequestBehavior.AllowGet);
            }


            var allSignUps = _gesCaseReportSignUpService.GetAll();

            var existedSignup =
                allSignUps.FirstOrDefault(
                    d =>
                        d.I_GesCaseReports_Id != null && d.I_GesCaseReports_Id.Value == gesCaseReportId &&
                         (d.G_Organizations_Id != null && d.G_Organizations_Id.Value == orgId));
            try
            {
                if (isSignUp)
                {
                    if (existedSignup != null)
                    {
                        existedSignup.G_Individuals_Id = individualId;
                        existedSignup.G_Organizations_Id = orgId;
                        existedSignup.Active = isActive;
                        existedSignup.Modified = DateTime.UtcNow;
                        _gesCaseReportSignUpService.Update(existedSignup, true);
                        result = true;
                    }
                    else
                    {
                        _gesCaseReportSignUpService.Add(new GesCaseReportSignUp { I_GesCaseReports_Id = gesCaseReportId, G_Individuals_Id = individualId, G_Organizations_Id = orgId, Active = isActive, Created = DateTime.UtcNow });
                        result = _gesCaseReportSignUpService.Save() > 0;
                    }
                }
                else
                {
                    if (existedSignup != null)
                    {
                        _gesCaseReportSignUpService.Delete(existedSignup, true);
                    }
                    result = true;
                }
                QueryCacheManager.ExpireTag("GesCaseReport");
            }
            catch (GesServiceException e)
            {
                return Json(new
                {
                    meta = new
                    {
                        success = false,
                        error = e.Message
                    },
                    data = new { }
                }, JsonRequestBehavior.AllowGet);
            }

            return Json(new
            {
                meta = new
                {
                    success = result,
                    error = ""
                },
                data = new { }
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UpdateGesCompanyWatcher(bool newValue, long gesCompanyId)
        {
            bool result;
            long orgId = 0, individualId = 0;
            this.GetIndividualInfo(_gIndividualsService, ref individualId, ref orgId);

            try
            {
                if (newValue)
                {
                    _gesCompanyWatcherService.Add(new I_GesCompanyWatcher { I_GesCompanies_Id = gesCompanyId, G_Individuals_Id = individualId });
                    result = _gesCompanyWatcherService.Save() > 0;
                }
                else
                {
                    result = _gesCompanyWatcherService.RemoveGesCompanyWatcher(gesCompanyId, individualId);
                }
                CacheHelper.ClearFocusListRelatedCache(individualId);
            }
            catch (GesServiceException e)
            {
                Logger.Error(e, $"Error when add or remove GesCompanyWatcher");

                return Json(new
                {
                    meta = new
                    {
                        success = false,
                        error = e.Message
                    },
                    data = new { }
                }, JsonRequestBehavior.AllowGet);
            }

            return Json(new
            {
                meta = new
                {
                    success = result,
                    error = ""
                },
                data = new { }
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UpdateGesCaseReportsG_Individuals(bool newValue, long gesCaseReportId)
        {
            bool result;
            long orgId = 0, individualId = 0;
            this.GetIndividualInfo(_gIndividualsService, ref individualId, ref orgId);

            try
            {
                if (newValue)
                {
                    _gesCaseReportsG_IndividualsService.Add(new I_GesCaseReportsG_Individuals { I_GesCaseReports_Id = gesCaseReportId, G_Individuals_Id = individualId });
                    result = _gesCaseReportsG_IndividualsService.Save() > 0;
                }
                else
                {
                    result = _gesCaseReportsG_IndividualsService.RemoveGesCaseReportsG_IndividualsService(gesCaseReportId, individualId);
                }
                CacheHelper.ClearFocusListRelatedCache(individualId);
            }
            catch (GesServiceException e)
            {
                Logger.Error(e, $"Error when add or remove I_GesCaseReportsG_Individuals");

                return Json(new
                {
                    meta = new
                    {
                        success = false,
                        error = e.Message
                    },
                    data = new { }
                }, JsonRequestBehavior.AllowGet);
            }

            return Json(new
            {
                meta = new
                {
                    success = result,
                    error = ""
                },
                data = new { }
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetTimeLines(long gesCaseReportId)
        {
            try
            {
                var timelines = _gesCaseProfilesService.GetTimeLines(gesCaseReportId);

                return Json(timelines, JsonRequestBehavior.AllowGet);
            }
            catch (GesServiceException e)
            {
                Logger.Error(e, $"Error when add or remove I_GesCaseReportsG_Individuals");

                return Json(new
                {
                    meta = new
                    {
                        success = false,
                        error = e.Message
                    },
                    data = new { }
                }, JsonRequestBehavior.AllowGet);
            }
        }



        #region Export Excel

        public ActionResult ExportAllHoldings(List<long> portfolioIds)
        {
            long orgId = 0, individualId = 0;
            this.GetIndividualInfo(_gIndividualsService, ref individualId, ref orgId);

            var result = _exportExcelService.ExportPrescreeningSearchAllCompaniesByPortfolioToExcel(orgId, portfolioIds);
            var companies = _exportExcelService.ExportAllCompaniesByPortfolio(orgId, portfolioIds);

            var templatePath = Server.MapPath(ExcelTemplates.ScreeningReport);
            var template = System.IO.File.ReadAllBytes(templatePath);
            var filename = string.Format(ExcelTemplates.ScreeningReportPrefix + "{0}.xlsx", DateTime.Now.ToString("yyyyMMddHHmmss"));

            using (var ms = new MemoryStream(template))
            {
                ms.Position = 0;
                HttpContext.Response.ClearContent();
                HttpContext.Response.AddHeader("Content-Disposition", "attachment; filename=" + filename);
                HttpContext.Response.ContentType = "application/octet-stream";
                using (var document = NGS.Templater.Configuration.Factory.Open(ms, HttpContext.Response.OutputStream, "xlsx"))
                {
                    document.Process(new { CaseProfiles = result, Companies = companies });
                }
                HttpContext.Response.End();
            }
            return null;
        }

        public ActionResult ExportPrescreeningSearchToExcel(string name, string isin, string portfolioIds, bool? notshowclosecase, bool? onlycompanieswithactivecase, bool? isNew, string companyIds,
            string recommendationId, string conclusionId, string engagementAreaIds, string locationId, string responseId, string progressId, string industryId, bool? onlySearchCompanyName, bool? onlyShowFocusList, string homeCountryIds, string sustainalyticsId)
        {
            var recommendationIds = ExtractStringToListLong(recommendationId);
            var conclusionIds = ExtractStringToListLong(conclusionId);
            var locationIds = ExtractStringToListGuid(locationId);
            var responseIds = ExtractStringToListLong(responseId);
            var progressIds = ExtractStringToListLong(progressId);
            var industryIds = ExtractStringToListLong(industryId);
            var portfoliosId = ExtractStringToListLong(portfolioIds);
            var homeCountriesId = ExtractStringToListGuid(homeCountryIds);
            var SustainalyticsId = ConvertStringToLong(sustainalyticsId);

            var portfolios = portfoliosId.Where(d => d != null).Select(i => (long)i).ToList();

            var companiesIds = new List<long>();
            if (!string.IsNullOrEmpty(companyIds))
            {
                companiesIds = companyIds.Split(',')
                                         .Select(long.Parse)
                                         .ToList();
            }

            var baseUrl = isNew ?? false ? GetBaseUrl() : SiteSettings.OldClientsSiteUrl;

            var serviceId = new List<long?>();
            long? engagementTypeId = null;
            long? normId = null;
            if (!string.IsNullOrEmpty(engagementAreaIds) && engagementAreaIds.Contains("-"))
            {
                var array = engagementAreaIds.Trim().Split('-');
                long temp;

                if (array[0].Trim().Length > 0)
                {
                    serviceId = ExtractStringToListLong(array[0].Trim());
                }

                if (long.TryParse(array[1], out temp))
                {
                    engagementTypeId = temp;
                }

                if (long.TryParse(array[2], out temp))
                {
                    normId = temp;
                }
            }

            onlySearchCompanyName = onlySearchCompanyName ?? false;
            long orgId = 0, individualId = 0;
            this.GetIndividualInfo(_gIndividualsService, ref individualId, ref orgId);

            var result = _exportExcelService.ExportPrescreeningSearchToExcel((long)orgId, (long)individualId, onlyShowFocusList ?? false, name, isin, portfolios, notshowclosecase ?? false, isNew ?? false, baseUrl, onlycompanieswithactivecase ?? false, companiesIds,
                recommendationIds, conclusionIds, serviceId, normId, locationIds, responseIds, progressIds, industryIds, engagementTypeId, true, !onlySearchCompanyName, !onlySearchCompanyName, homeCountriesId, SustainalyticsId)
                                          .ToList();

            var companies = _companiesService.ExportCompanies((long)orgId, (long)individualId, onlyShowFocusList ?? false, name, isin, portfolios, notshowclosecase ?? false, onlycompanieswithactivecase ?? false, isNew ?? false, baseUrl, companiesIds,
                recommendationIds, conclusionIds, serviceId, normId, locationIds, responseIds, progressIds, industryIds, engagementTypeId, true, !onlySearchCompanyName, !onlySearchCompanyName, homeCountriesId, SustainalyticsId);

            var templatePath = Server.MapPath(ExcelTemplates.ScreeningReport);
            var template = System.IO.File.ReadAllBytes(templatePath);
            var filename = string.Format(ExcelTemplates.ScreeningReportPrefix + "{0}.xlsx", DateTime.Now.ToString("yyyyMMddHHmmss"));

            using (var ms = new MemoryStream(template))
            {
                ms.Position = 0;
                HttpContext.Response.ClearContent();
                HttpContext.Response.AddHeader("Content-Disposition", "attachment; filename=" + filename);
                HttpContext.Response.ContentType = "application/octet-stream";
                using (var document = NGS.Templater.Configuration.Factory.Open(ms, HttpContext.Response.OutputStream, "xlsx"))
                {
                    document.Process(new { CaseProfiles = result, Companies = companies });
                }
                HttpContext.Response.End();
            }
            return null;
        }

        [AllowAnonymous]
        [HttpPost]
        public JsonResult ExportStandardCases(long orgId, string portfolioIds = "3282", bool allConclusions = false)
        {
            var portfoliosFormattedIds = new List<long>();
            if (!string.IsNullOrEmpty(portfolioIds))
            {
                portfoliosFormattedIds = portfolioIds.Split(',')
                                                    .Select(long.Parse)
                                                    .ToList();
            }

            var query = _exportExcelService.GetDataForStandardCases(orgId, portfoliosFormattedIds, allConclusions);

            var result = from q in query
                         orderby q.CompanyName
                         select new
                         {
                             isin = q.Isin,
                             company = q.CompanyName,
                             issue = q.Issue,
                             involvement = q.ServiceEngagementThemeNorm,
                             location = q.Location,
                             entrydate = q.EntryDate,
                             conclusion = q.StandardConclusion,
                             bbgid = q.BbgID
                         };

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddSearchResultToFocusList(string name, string isin, string portfolioIds,
            bool? notshowclosecase, bool? onlycompanieswithactivecase, bool? isNew, string companyIds,
            string recommendationId, string conclusionId, string engagementAreaIds, string locationId, string responseId,
            string progressId, string industryId, bool? onlySearchCompanyName, bool? onlyShowFocusList,
            string homeCountryIds, string sustainalyticsId)
        {
            long orgId = 0, individualId = 0;
            this.GetIndividualInfo(_gIndividualsService, ref individualId, ref orgId);
            var recommendationIds = ExtractStringToListLong(recommendationId);
            var conclusionIds = ExtractStringToListLong(conclusionId);
            var locationIds = ExtractStringToListGuid(locationId);
            var responseIds = ExtractStringToListLong(responseId);
            var progressIds = ExtractStringToListLong(progressId);
            var industryIds = ExtractStringToListLong(industryId);
            var portfoliosId = ExtractStringToListLong(portfolioIds);
            var homeCountriesId = ExtractStringToListGuid(homeCountryIds);
            var SustainalyticsId = ConvertStringToLong(sustainalyticsId);
        

            var portfolios = portfoliosId.Where(d => d != null).Select(i => (long)i).ToList();
            var serviceId = new List<long?>();
            long? engagementTypeId = null;
            long? normId = null;
            if (!string.IsNullOrEmpty(engagementAreaIds) && engagementAreaIds.Contains("-"))
            {
                var array = engagementAreaIds.Trim().Split('-');
                long temp;

                if (array[0].Trim().Length > 0)
                {
                    serviceId = ExtractStringToListLong(array[0].Trim());
                }

                if (long.TryParse(array[1], out temp))
                {
                    engagementTypeId = temp;
                }

                if (long.TryParse(array[2], out temp))
                {
                    normId = temp;
                }
            }

            var listGesCompanies = new List<long>();

            try
            {
                listGesCompanies = _companiesService.GetListGescompanyIdsFromFilter(orgId, individualId, false, name, isin, portfolios, notshowclosecase ?? false, string.Empty,
                recommendationIds, conclusionIds, serviceId, normId, locationIds, responseIds, progressIds, industryIds, engagementTypeId, true, !onlySearchCompanyName, !onlySearchCompanyName, homeCountriesId, SustainalyticsId);

                var listCaseReports = _companiesService.GetCaseReportsForFocusList(listGesCompanies, notshowclosecase ?? false, orgId, individualId, false, true, name, recommendationIds, conclusionIds, serviceId, normId, locationIds, responseIds, progressIds, industryIds, engagementTypeId);

                if (listGesCompanies.Count <= 50)
                {
                    _gesCompanyWatcherService.AddListCompanyToFocusList(listGesCompanies, individualId);
                    _gesCaseReportsG_IndividualsService.AddListGescaseReportToFocusList(listCaseReports, individualId);

                    CacheHelper.ClearFocusListRelatedCache(individualId);
                    return Json(new
                    {
                        numCompanies = listGesCompanies.Count,
                        success = true,
                        message = "Your Focus list has been updated successfully."
                    }, JsonRequestBehavior.AllowGet);

                }
            }
            catch (GesServiceException ex)
            {
                Logger.Error(ex, "Error when processing the on the case report.");

                throw;
            }

            return Json(new
            {
                numCompanies = listGesCompanies.Count,
                success = false,
                message = "Too many companies. Kindly narrow down your search conditions."
            }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Private methods

        private IEnumerable<SelectListItem> GetPortfolioIndexDropdown(long orgId, string portfolioIds)
        {
            Guard.AgainstNullArgument(nameof(portfolioIds), portfolioIds);

            var ids = portfolioIds.Split(',').Where(i => !string.IsNullOrEmpty(i)).Select(long.Parse);

            return this.SafeExecute(() =>
            {
                var all = _companiesService.GetPortfolioIndexes(orgId).ToList();
                var items = all.Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name
                });
                var selected = all.Where(i => ids.Contains(i.Id)).Select(i => i.Id);

                return new MultiSelectList(items, "Value", "Text", selected);
            }, $"Exception when getting the portfolio index of Organization: {orgId}.");
        }

        private IEnumerable<SelectListItem> GetRecommendationDropdown(string recommendationIds)
        {
            Guard.AgainstNullArgument(nameof(recommendationIds), recommendationIds);

            var ids = recommendationIds.Split(',').Where(i => !string.IsNullOrEmpty(i)).Select(long.Parse);

            return this.SafeExecute(() =>
            {
                var all = _companiesService.GetRecommendations().ToList();
                var items = all.Select(ToSelectListItem);
                var selected = all.Where(i => ids.Contains(i.Id)).Select(i => i.Id);

                return new MultiSelectList(items, "Value", "Text", selected);
            }, $"Error when getting the recommendations.");
        }

        private IEnumerable<SelectListItem> GetLocationDropdown(string cts)
        {
            Guard.AgainstNullArgument(nameof(cts), cts);

            var countries = cts.Split(',').Where(i => !string.IsNullOrEmpty(i)).Select(i => i.ToLower());

            return this.SafeExecute(() =>
            {
                var all = _companiesService.GetAllCountries().ToList();
                var items = all.Where(d=>d.Name != null) ;//all.Select(ToSelectListItem).Where(i => !string.IsNullOrEmpty(i.Text));
                var selected = all.Where(i => countries.Contains(i.Id.ToString())).Select(i => i.Id);

                return new MultiSelectList(items, "Id", "Name", selected);
            }, $"Error when getting all countries");
        }

        private IEnumerable<SelectListItem> GetHomeCountriesDropdown(string cts)
        {
            Guard.AgainstNullArgument(nameof(cts), cts);

            var countries = cts.Split(',').Where(i => !string.IsNullOrEmpty(i)).Select(i => i.ToLower());

            return this.SafeExecute(() =>
            {
                var all = _companiesService.GetAllCountries().ToList();
                var items = all.Where(d=>d.Name != null) ;//all.Select(ToSelectListItem).Where(i => !string.IsNullOrEmpty(i.Text));
                var selected = all.Where(i => countries.Contains(i.Id.ToString())).Select(i => i.Id);

                return new MultiSelectList(items, "Id", "Name", selected);
            }, $"Error when getting all countries.");
        }

        private IEnumerable<SelectListItem> GetIndustryGroupDropdown(string igs)
        {
            Guard.AgainstNullArgument(nameof(igs), igs);

            var industryGroupIds = igs.Split(',').Where(i => !string.IsNullOrEmpty(i));

            return this.SafeExecute(() =>
            {
                var all = _companiesService.GetAllIndustries().ToList();
                var items = all.Select(ToSelectListItem).Where(i => !string.IsNullOrEmpty(i.Text));
                var selected = all.Where(i => industryGroupIds.Contains(i.Id.ToString())).Select(i => i.Id);

                return new MultiSelectList(items, "Value", "Text", selected);
            }, $"Error when getting all industries.");
        }

        private IEnumerable<SelectListItem> GetEngagementAreaDropdown(string serviceIds, string normAreaIds, long orgId)
        {
            Guard.AgainstNullArgument(nameof(serviceIds), serviceIds);
            Guard.AgainstNullArgument(nameof(normAreaIds), normAreaIds);

            // selected: services
            var sIds = serviceIds.Split(',').Where(i => !string.IsNullOrEmpty(i)).Select(i => i.ToLower());
            var selectedServices = sIds.Select(nid => $"{nid}--").ToList();
            // selected: normAreas
            var nIds = normAreaIds.Split(',').Where(i => !string.IsNullOrEmpty(i)).Select(i => i.ToLower());
            var selectedNormAreas = nIds.Select(nid => $"{BussinessConductServiceId}--{nid}").ToList();

            return this.SafeExecute(() =>
            {
                var items = _companiesService.GetEngagmentThemeNorm(orgId);
                var selected = selectedNormAreas;
                if (selectedServices.Count > 0)
                {
                    selected = selectedServices;
                }

                return new MultiSelectList(items, "Value", "Text", selected);
            }, $"Error when getting the Engagement Areas with organization {orgId}.");
        }

        private bool CheckClaimSignUp()
        {
            var usermager = HttpContext.GetOwinContext()
                   .GetUserManager<GesUserManager>();

            var currentUser = usermager.FindById(User.Identity.GetUserId());

            var activityForm = ClaimEnum.Endorserment.GetEnumDescription();

            return currentUser?.Claims?.Count(d => d.ClaimType == activityForm) > 0;
        }

        private IEnumerable<DocumentViewModel> GetDocuments(long companyId)
        {
            return this.SafeExecute(() => _documentService.GetDocumentsByCompanyId(companyId).Select(x =>
            {
                x.DownloadUrl = CommonHelper.GetDocDownloadUrl(x.FileName);
                return x;
            }), $"Exception when getting the documents of company({companyId})");
        }

        #endregion

        [HttpPost]
        public JsonResult GetAdditionalIncident(JqGridViewModel jqGridParams, long caseProfileId)
        {
            long orgId = 0, individualId = 0;
            this.GetIndividualInfo(_gIndividualsService, ref individualId, ref orgId);
            bool cansignUp = CheckClaimSignUp();
            var caseReports = _gesCaseProfilesService.GetAdditionalCaseReports(caseProfileId, orgId);
            var clientType = _companiesService.GetClientType(orgId);
            string standforBc = clientType == ClientType.BusinessConduct ? "GS" : "GES";
            foreach (var caseReportListViewModel in caseReports)
            {
                caseReportListViewModel.ServiceEngagementThemeNorm =
                    caseReportListViewModel.ServiceEngagementThemeNorm.Replace("Global Standards", standforBc)
                        .Replace("Stewardship & Risk", "S&R");

                caseReportListViewModel.CanSignUp = cansignUp;
            }

            var sortCol = jqGridParams.sidx.ToLower();
            var sortDir = jqGridParams.sord.ToLower();
            if (!(string.IsNullOrEmpty(sortCol) && string.IsNullOrEmpty(sortDir)))
            {
                switch (sortCol)
                {
                    case "issuename":
                        caseReports = sortDir == "asc"
                            ? caseReports.OrderBy(x => x.IssueName).ThenBy(d => d.SortOrderEngagementType).ToList()
                            : caseReports.OrderByDescending(x => x.IssueName).ThenByDescending(d => d.SortOrderEngagementType).ToList();
                        break;
                    case "location":
                        caseReports = sortDir == "asc"
                            ? caseReports.OrderBy(x => x.Location).ThenBy(d => d.SortOrderEngagementType).ToList()
                            : caseReports.OrderByDescending(x => x.Location).ThenByDescending(d => d.SortOrderEngagementType).ToList();
                        break;
                    case "confirmed":
                        caseReports = sortDir == "asc"
                            ? caseReports.OrderBy(x => x.Confirmed).ThenBy(d => d.SortOrderEngagementType).ToList()
                            : caseReports.OrderByDescending(x => x.Confirmed).ThenByDescending(d => d.SortOrderEngagementType).ToList();
                        break;
                    case "entrydate":
                        caseReports = sortDir == "asc"
                            ? caseReports.OrderBy(x => x.EntryDate).ThenBy(d => d.SortOrderEngagementType).ToList()
                            : caseReports.OrderByDescending(x => x.EntryDate).ThenByDescending(d => d.SortOrderEngagementType).ToList();
                        break;
                    case "serviceengagementthemenorm":
                        caseReports = sortDir == "asc"
                            ? caseReports.OrderBy(x => x.ServiceEngagementThemeNorm).ThenBy(x => x.SortOrderEngagementType).ToList()
                            : caseReports.OrderByDescending(x => x.ServiceEngagementThemeNorm).ThenByDescending(x => x.SortOrderEngagementType).ToList();
                        break;

                    default:
                        caseReports = caseReports.OrderByDescending(x => x.EntryDate).ThenByDescending(d => d.Created).ToList();
                        break;
                }
            }

            return Json(caseReports.ToPagedList(Logger, jqGridParams.page, jqGridParams.rows), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ExportPdf(long companyId, long gesCompanyId, bool? showCompanyInfo, bool? showDialogue, bool? showCompanyOverview, bool? showCaseProfiles, bool? showAlerts, bool? showCompanyEvents, bool? showCorporateRatingInformation, bool? showCoverPage, bool? showDocuments, bool? showResolvedAndArchivedCases)
        {
            long orgId = 0, individualId = 0;
            this.GetIndividualInfo(_gIndividualsService, ref individualId, ref orgId);

            var exportUrlCoverPage = $"{Request?.Url?.Scheme}://{Request?.Url?.Authority}/Export/CoverPage?companyId={companyId}&gesCompanyId={gesCompanyId}&orgId={orgId}&individualId={individualId}";
            var exportUrlBodyPage = $"{Request?.Url?.Scheme}://{Request?.Url?.Authority}/Export/CompanyProfile?companyId={companyId}&gesCompanyId={gesCompanyId}&orgId={orgId}&individualId={individualId}";


            exportUrlBodyPage = AddExportOption(showCompanyInfo, nameof(showCompanyInfo), exportUrlBodyPage);
            exportUrlBodyPage = AddExportOption(showDialogue, nameof(showDialogue), exportUrlBodyPage);
            exportUrlBodyPage = AddExportOption(showCompanyOverview, nameof(showCompanyOverview), exportUrlBodyPage);
            exportUrlBodyPage = AddExportOption(showCaseProfiles, nameof(showCaseProfiles), exportUrlBodyPage);
            exportUrlBodyPage = AddExportOption(showAlerts, nameof(showAlerts), exportUrlBodyPage);
            exportUrlBodyPage = AddExportOption(showCompanyEvents, nameof(showCompanyEvents), exportUrlBodyPage);
            exportUrlBodyPage = AddExportOption(showCorporateRatingInformation, nameof(showCorporateRatingInformation), exportUrlBodyPage);
            exportUrlBodyPage = AddExportOption(showCoverPage, nameof(showCoverPage), exportUrlBodyPage);
            exportUrlBodyPage = AddExportOption(showDocuments, nameof(showDocuments), exportUrlBodyPage);
            exportUrlBodyPage = AddExportOption(showResolvedAndArchivedCases, nameof(showResolvedAndArchivedCases), exportUrlBodyPage);

            var exportUrls = showCoverPage.HasValue && showCoverPage.Value ? new List<string> { exportUrlCoverPage, exportUrlBodyPage } : new List<string> { exportUrlBodyPage };
            var output = _phantomJsRunner.Run(exportUrls);

            if (!string.IsNullOrEmpty(output))
            {
                output += ".pdf";
            }

            return new JsonResult
            {
                Data = new { FileName = output }
            };
        }

        [HttpPost]
        public ActionResult ExportCaseProfilePdf(long caseProfileId, bool? showCoverPage, bool? showCompanyInfo, bool? showCaseInfoBusinessConduct, bool? showStatistic, bool? showCompanyEvents, bool? showSummary, bool? showAlerts, bool? showDescription, bool? showConclusion, bool? showGesCommentary, bool? showLatestNews, bool? showEngagementInformation, bool? showDiscussionPoint, bool? showOtherStakeholder, bool? showKPI, bool? showGuidelinesAndConventions, bool? showConfirmationDetails, bool? showReferences, bool? showCompanyDialogue, bool? showSourceDialogue, bool? showCompanyRelatedItems, bool? showGesContactInformation, bool? showAdditionalDocuments,bool? showSummaryMaterialRisk, bool? showClosingDetail)
        {
            long orgId = 0, individualId = 0;
            this.GetIndividualInfo(_gIndividualsService, ref individualId, ref orgId);
            var caseProfileViewModel = _gesCaseProfilesService.GetCaseReportViewModel(caseProfileId, orgId);

            var reportType = _gesCaseProfilesService.GetReportType(caseProfileId, orgId);
            var gesCompanyId = _companiesService.GetGesCompanyId(caseProfileViewModel.CompanyId);

            var exportUrlCoverPage = $"{Request?.Url?.Scheme}://{Request?.Url?.Authority}/Export/CoverPageCaseProfile?reportType={reportType}&caseProfileId={caseProfileId}&orgId={orgId}";
            var exportUrlBodyPage = $"{Request?.Url?.Scheme}://{Request?.Url?.Authority}/Export/CaseProfile?caseReportId={caseProfileId}&orgId={orgId}&reportType={reportType}";

            exportUrlBodyPage = AddExportOption(showCompanyInfo, nameof(showCompanyInfo), exportUrlBodyPage);
            exportUrlBodyPage = AddExportOption(showCaseInfoBusinessConduct, nameof(showCaseInfoBusinessConduct), exportUrlBodyPage);               
            exportUrlBodyPage = AddExportOption(showStatistic, nameof(showStatistic), exportUrlBodyPage);                 
            exportUrlBodyPage = AddExportOption(showCompanyEvents, nameof(showCompanyEvents), exportUrlBodyPage);
            exportUrlBodyPage = AddExportOption(showSummary, nameof(showSummary), exportUrlBodyPage);

            exportUrlBodyPage = AddExportOption(showAlerts, nameof(showAlerts), exportUrlBodyPage);
            exportUrlBodyPage = AddExportOption(showDescription, nameof(showDescription), exportUrlBodyPage);
            exportUrlBodyPage = AddExportOption(showConclusion, nameof(showConclusion), exportUrlBodyPage);
            exportUrlBodyPage = AddExportOption(showGesCommentary, nameof(showGesCommentary), exportUrlBodyPage);
            exportUrlBodyPage = AddExportOption(showLatestNews, nameof(showLatestNews), exportUrlBodyPage);
            exportUrlBodyPage = AddExportOption(showEngagementInformation, nameof(showEngagementInformation), exportUrlBodyPage);
            exportUrlBodyPage = AddExportOption(showDiscussionPoint, nameof(showDiscussionPoint), exportUrlBodyPage);
            exportUrlBodyPage = AddExportOption(showOtherStakeholder, nameof(showOtherStakeholder), exportUrlBodyPage);
            exportUrlBodyPage = AddExportOption(showKPI, nameof(showKPI), exportUrlBodyPage);
            exportUrlBodyPage = AddExportOption(showGuidelinesAndConventions, nameof(showGuidelinesAndConventions), exportUrlBodyPage);
            exportUrlBodyPage = AddExportOption(showConfirmationDetails, nameof(showConfirmationDetails), exportUrlBodyPage);
            exportUrlBodyPage = AddExportOption(showReferences, nameof(showReferences), exportUrlBodyPage);
            exportUrlBodyPage = AddExportOption(showCompanyDialogue, nameof(showCompanyDialogue), exportUrlBodyPage);            
            exportUrlBodyPage = AddExportOption(showSourceDialogue, nameof(showSourceDialogue), exportUrlBodyPage);
            exportUrlBodyPage = AddExportOption(showCompanyRelatedItems, nameof(showCompanyRelatedItems), exportUrlBodyPage);

            exportUrlBodyPage = AddExportOption(showGesContactInformation, nameof(showGesContactInformation), exportUrlBodyPage);
            exportUrlBodyPage = AddExportOption(showAdditionalDocuments, nameof(showAdditionalDocuments), exportUrlBodyPage);

            exportUrlBodyPage = AddExportOption(showSummaryMaterialRisk, nameof(showSummaryMaterialRisk), exportUrlBodyPage);
            exportUrlBodyPage = AddExportOption(showClosingDetail, nameof(showClosingDetail), exportUrlBodyPage);


            var exportUrls = showCoverPage.HasValue && showCoverPage.Value ? new List<string> { exportUrlCoverPage, exportUrlBodyPage } : new List<string> { exportUrlBodyPage };
            var output = _phantomJsRunner.Run(exportUrls);

            if (!string.IsNullOrEmpty(output))
            {
                output += ".pdf";
            }

            return new JsonResult
            {
                Data = new { FileName = output }
            };
        }

        [HttpPost]
        public ActionResult CompanyDialogueDownloadPdf(long caseProfileId, DateTime? fromDate, DateTime? toDate, bool? addTerm)
        {
            var fileNames = new List<string>();
            var fileLocalNames = new List<string>();
            var companyDialogues = new List<DialogueModel>();

            long orgId = 0, individualId = 0;
            this.GetIndividualInfo(_gIndividualsService, ref individualId, ref orgId);

            var reportType = _gesCaseProfilesService.GetReportType(caseProfileId, orgId);

            var caseReport = _gesCaseProfilesService.GetCaseProfileCoreModel(reportType, caseProfileId, orgId);

            var gesCompanyId = _companiesService.GetGesCompanyId(caseReport.BaseComponent.CompanyId);

            var exportUrlCoverPage = $"{Request?.Url?.Scheme}://{Request?.Url?.Authority}/Export/CoverPageDialogue?companyId={caseReport.BaseComponent.CompanyId}&gesCompanyId={gesCompanyId}&orgId={orgId}&individualId={individualId}";
            var exportUrlBodyPage = $"{Request?.Url?.Scheme}://{Request?.Url?.Authority}/Export/CompanyDialogue?caseReportId={caseProfileId}&orgId={orgId}&reportType={reportType}";

            exportUrlBodyPage = AddExportValue(fromDate.HasValue ? fromDate.Value.ToString("yyyy-MM-dd") : "", nameof(fromDate), exportUrlBodyPage);
            exportUrlBodyPage = AddExportValue(toDate.HasValue ? toDate.Value.ToString("yyyy-MM-dd") : "", nameof(toDate), exportUrlBodyPage);

            exportUrlCoverPage = AddExportValue(fromDate.HasValue ? fromDate.Value.ToString("yyyy-MM-dd") : "", nameof(fromDate), exportUrlCoverPage);
            exportUrlCoverPage = AddExportValue(toDate.HasValue ? toDate.Value.ToString("yyyy-MM-dd") : "", nameof(toDate), exportUrlCoverPage);
            exportUrlCoverPage = AddExportValue(reportType.ToString(), "reportType", exportUrlCoverPage);
            exportUrlCoverPage = AddExportValue(caseProfileId.ToString(), "caseProfileId", exportUrlCoverPage);

            companyDialogues = caseReport.IssueComponent?.CompanyDialogues?.ToList();

            if (fromDate.HasValue)
            {
                companyDialogues = companyDialogues.Where(x => x.ContactDate >= fromDate.Value).ToList();
            }

            if (toDate.HasValue)
            {
                companyDialogues = companyDialogues.Where(x => x.ContactDate <= toDate.Value).ToList();
            }

            if (companyDialogues != null && companyDialogues.Any())
            {
                fileNames.AddRange(companyDialogues.Where(x => !string.IsNullOrEmpty(x.FileName))?.Select(x => x.FileName));
            }

            if (addTerm.HasValue && addTerm.Value)
            {
                fileLocalNames.Add(DownLoadCompanyEmail("export_pdf_term.pdf"));
            }

            foreach (var filename in fileNames)
            {
                var downloadCompanyEmailFile = DownLoadCompanyEmail(filename);

                if (!string.IsNullOrEmpty(downloadCompanyEmailFile))
                {
                    fileLocalNames.Add(downloadCompanyEmailFile);
                }
            }

            var exportUrls = new List<string> { exportUrlCoverPage, exportUrlBodyPage };

            var output = _phantomJsRunner.Run(exportUrls, fileLocalNames);

            if (!string.IsNullOrEmpty(output))
            {
                output += ".pdf";
            }

            return new JsonResult
            {
                Data = new { FileName = output }
            };
        }

        private static string AddExportOption(bool? exportOption, string parameterName, string exportUrl)
        {
            if (exportOption.HasValue && exportOption.Value)
            {
                exportUrl += $"&{parameterName}=true";
            }
            return exportUrl;
        }

        public ActionResult GovernanceType()
        {
            //return View("EngagementType", GetEngagementType(engagementTypeId));

            long orgId = 0, individualId = 0;
            this.GetIndividualInfo(_gIndividualsService, ref individualId, ref orgId);
            var engagementType = _engagementTypesService.GetEngagementTypeModel((long)EngagementTypeEnum.Governance, orgId);

            if (!string.IsNullOrWhiteSpace(engagementType.ThemeImagePath))
            {
                var themeBanner = UtilHelper.ImageToByte(
                    GesFileStorageService.GetFilePath(engagementType.ThemeImagePath, "Theme"));
                if (themeBanner != null)
                {
                    engagementType.ThemeImage = Convert.ToBase64String(themeBanner);
                }
            }

            if (engagementType != null)
            {
                engagementType.Name = Resources.CorporateGovernanceLink;
                engagementType.GovernanceServicesIds = $"{engagementType.ServicesId},{(long)GesService.GovernanceOngoing}";
                engagementType = UpdateEngagementContact((long)EngagementTypeEnum.Governance, engagementType);
            }
            return View("EngagementType", engagementType);
        }
        
        public ActionResult BusinessConductEngagementType(long engagementTypeId)
        {
            return View("EngagementType", GetEngagementType(engagementTypeId));
        }
        
        public ActionResult PreviousEngagementType(long engagementTypeId)
        {
            return View("EngagementType", GetEngagementType(engagementTypeId));
        }
        
        public ActionResult StewardshipAndRiskEngagementType(long engagementTypeId)
        {
            return View("EngagementType", GetEngagementType(engagementTypeId));
        }
        
        public ActionResult ScreeningEngagementType(long engagementTypeId)
        {
            return View("EngagementType", GetEngagementType(engagementTypeId));
        }

        public ActionResult BespokeEngagementType(long engagementTypeId)
        {
            return View("EngagementType", GetEngagementType(engagementTypeId));
        }

        private EngagementTypeViewModel GetEngagementType(long engagementTypeId)
        {
            long orgId = 0, individualId = 0;
            this.GetIndividualInfo(_gIndividualsService, ref individualId, ref orgId);
            var engagementType = _engagementTypesService.GetEngagementTypeModel(engagementTypeId, orgId);
            
            if (!string.IsNullOrWhiteSpace(engagementType.ThemeImagePath))
            {
                var themeBanner = UtilHelper.ImageToByte(
                    GesFileStorageService.GetFilePath(engagementType.ThemeImagePath, "Theme"));
                if (themeBanner!= null)
                {
                    engagementType.ThemeImage = Convert.ToBase64String(themeBanner);
                } 
            }

            engagementType = UpdateEngagementContact(engagementTypeId, engagementType);

            return engagementType;
        }

        private static EngagementTypeViewModel UpdateEngagementContact(long engagementTypeId, EngagementTypeViewModel engagementType)
        {
            var formatGesMobile = "<a href='tel: {0}'>{0}</a>";
            var gesMobile = "+46 8 787 99 10";

            if (engagementType == null)
            {
                engagementType = new EngagementTypeViewModel
                {
                    I_EngagementTypes_Id = 0,
                    ServicesId = 0,
                    Name = "There are not engagement theme with Id is " + engagementTypeId + " in the system.",
                    Description = $"Contact Sustainalytics at <a href='mailto:clientsupport@sustainalytics.com'>clientsupport@sustainalytics.com</a> or {string.Format(formatGesMobile, gesMobile)} for questions regarding this engagement."
                };
            }
            if (string.IsNullOrWhiteSpace(engagementType.ContactMobile))
            {
                engagementType.ContactMobile = gesMobile;
            }
            engagementType.Contact = $"Contact {engagementType.ContactFullName} at <a href='mailto:{engagementType.ContactEmail}'> {engagementType.ContactEmail}</a> or {string.Format(formatGesMobile, engagementType.ContactMobile)} for questions regarding this engagement.";
            return engagementType;
        }

        private static string AddExportValue(string parameterValue, string parameterName, string exportUrl)
        {
            if (!string.IsNullOrEmpty(parameterValue))
            {
                exportUrl += $"&{parameterName}=" + parameterValue;
            }
            return exportUrl;
        }

        [AllowAnonymous]
        public ActionResult DownloadFile(string fileName)
        {
            var fileStream = this.SafeExecute(() => this._gesFileStorageService.GetStreamFromOldSystem(fileName), $"Exception when read the stream of document");

            if (fileStream != null && fileStream.CanRead)
            {
                return File(fileStream, System.Web.MimeMapping.GetMimeMapping(fileName), fileName);
            }

            return HttpNotFound();
        }        
        
        [AllowAnonymous]
        [Route("Company/EventExport/{eventId}")]
        public ActionResult EventExport(long? eventId, int timeOffset)
        {
            try
            {
                var eventDetails = _calendarService.GetCalendarEventById(eventId);

                var calendar = GenerateCalendar(eventDetails);
                var serializer = new CalendarSerializer();

                var bytes = Encoding.UTF8.GetBytes(serializer.SerializeToString(calendar));

                var fileName = eventDetails.Heading + ".ics";

                return File(bytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    meta = new
                    {
                        success = false,
                        message = "Error:" + ex.Message
                    },
                    data = new { }
                }, JsonRequestBehavior.AllowGet);
            }
        }
        
        [AllowAnonymous]
        [Route("Company/EventExportSendMail/{eventId}")]
        public ActionResult EventExportSendMail(long? eventId)
        {
            try
            {
                var eventDetails = _calendarService.GetCalendarEventById(eventId);

                var calendar = GenerateCalendar(eventDetails);
                var serializer = new CalendarSerializer();

                var usermager = HttpContext.GetOwinContext()
                    .GetUserManager<GesUserManager>();

                var currentUser = usermager.FindById(User.Identity.GetUserId());

                SendEmail(eventDetails, serializer.SerializeToString(calendar), currentUser);

                return Json(new
                {
                    meta = new
                    {
                        success = true,
                        message = "The event was sent to your email succesfully!"
                    },
                    data = new { }
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    meta = new
                    {
                        success = false,
                        message = "Error:" + ex.Message
                    },
                    data = new { }
                }, JsonRequestBehavior.AllowGet);
            }
        }
        
        [HttpPost]
        [Route("Company/EventExportSendMail2")]
        public ActionResult EventExportSendMail2(long? eventId)
        {
            try
            {
                var eventDetails = _calendarService.GetCalendarEventById(eventId);

                var calendar = GenerateCalendar(eventDetails);
                var serializer = new CalendarSerializer();

                var usermager = HttpContext.GetOwinContext().GetUserManager<GesUserManager>();

                var currentUser = usermager.FindById(User.Identity.GetUserId());

                SaveAttendee(eventDetails, currentUser);

                SendEmail(eventDetails, serializer.SerializeToString(calendar), currentUser);

                return Json(new
                {
                    meta = new
                    { 
                        success = true,
                        message = "The event was sent to your email succesfully!"
                    },
                    data = new { }
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    meta = new
                    {
                        success = false,
                        message = "Error:" + ex.Message
                    },
                    data = new { }
                }, JsonRequestBehavior.AllowGet);
            }
        }
        
        [HttpGet]
        [Route("Company/GetCalendar")]
        public ActionResult GetCalendar(long? eventId, bool companyEvent)
        {
            try
            {
                var eventDetails = _calendarService.GetCalendarEventById(eventId, companyEvent);
                return Json(new
                {
                    meta = new
                    { 
                        success = true,
                        message = "Get event succesfully!"
                    },
                    data = eventDetails
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    meta = new
                    {
                        success = false,
                        message = "Error:" + ex.Message
                    },
                    data = new { }
                }, JsonRequestBehavior.AllowGet);
            }
        }

        private void SendEmail(EventListViewModel eventDetails, string eventMessageContent, GesUser gesUser)
        {
            var credential = new System.Net.NetworkCredential(SiteSettings.SmtpUserName, SiteSettings.SmtpUserPassword);

            var smtpclient = SendMailSmtp.GetSmtpClient(SiteSettings.SmtpHost, SiteSettings.SmtpPort, credential,SiteSettings.SmtpEnableSsl);          

            var oldId = gesUser?.OldUserId ?? -1;
            var individual = _gIndividualsService.GetIndividualByUserId(oldId);

            if (gesUser == null) return;

            var msg = new MailMessage
            {
                From = new MailAddress(SiteSettings.EventSenderEmailAddress, SiteSettings.EventSenderEmailName),
                To = { new MailAddress(gesUser.Email, individual.FirstName + " " + individual.LastName) },
                Subject = eventDetails.Heading,
                Body = eventDetails.Description
            };            

            var contype = new System.Net.Mime.ContentType("text/calendar");
            if (contype.Parameters != null)
            {
                contype.Parameters.Add("method", "REQUEST");
                contype.Parameters.Add("component", "VEVENT");
                contype.Parameters.Add("name", "Meeting.ics");
            }

            try
            {                
                SendMailSmtp.SendEmail(msg, smtpclient, eventMessageContent, contype);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }

        private static Calendar GenerateCalendar(EventListViewModel eventDetails)
        {
            var isAllDay = eventDetails.AllDayEvent ?? false;
            var utcStartTime = new DateTime(eventDetails.EventDate.Year, eventDetails.EventDate.Month,
                eventDetails.EventDate.Day, eventDetails.EventDate.Hour, eventDetails.EventDate.Minute, 0,
                DateTimeKind.Utc);
            
            var utcEndTime = new DateTime((int)eventDetails.EventEndDate?.Year, (int)eventDetails.EventEndDate?.Month,
                (int) eventDetails.EventEndDate?.Day, (int)eventDetails.EventEndDate?.Hour, (int)eventDetails.EventEndDate?.Minute, 0,
                DateTimeKind.Utc);
            
            var rrule = new RecurrencePattern(FrequencyType.Daily, 1) { Count = 5 };

            var calendarEvent = new CalendarEvent
            {
                Start = new CalDateTime(utcStartTime),
                End = new CalDateTime(utcEndTime) , //(isAllDay ? new CalDateTime(utc.AddDays(1)) : new CalDateTime(utc)),
                Description = eventDetails.Description,
                Summary = eventDetails.Heading,
                Location = eventDetails.EventLocation,
                Uid = eventDetails.Id + "@sustainalytics.com",
                IsAllDay = eventDetails.AllDayEvent != null && (bool) eventDetails.AllDayEvent,
                Sequence = 0,
                Priority = 5,
                Transparency = TransparencyType.Transparent,
                Class = "PUBLIC",
                Organizer = new Organizer()
                {
                    CommonName = SiteSettings.EventSenderEmailName,
                    Value = new Uri("mailto:" + SiteSettings.EventSenderEmailAddress)
                }
            };

            var alarm = new Alarm()
            {
                Action = AlarmAction.Display,
                Trigger = new Trigger(TimeSpan.FromDays(-1)),
                Summary = "Inquiry due in 1 day"
            };

            calendarEvent.Alarms.Add(alarm);
            var calendar = new Calendar { Method = "REQUEST" };
            calendar.Events.Add(calendarEvent);

            return calendar;
        }

        public void SaveAttendee(EventListViewModel eventDetails, GesUser user)
        {
            var eventAttendee = _gesEventCalendarUserAcceptService.GetByEventIdAndEmail(eventDetails.Id, user.Email);

            var oldId = user?.OldUserId ?? -1;
            var individual = _gIndividualsService.GetIndividualByUserId(oldId);

            if (user == null) return;

            if (eventAttendee == null)
            {
                eventAttendee = new GesEventCalendarUserAccept();
                eventAttendee.GesEventCalendarUserAcceptId = Guid.NewGuid();
                eventAttendee.FullName = individual?.FirstName + " " + individual?.LastName;
                eventAttendee.Email = user.Email;
                eventAttendee.SendDate = DateTime.Now;
                eventAttendee.I_CalenderEvents_Id = eventDetails.Id;
                _gesEventCalendarUserAcceptService.Add(eventAttendee);
            }
            else
            {
                eventAttendee.Email = user.Email;
                eventAttendee.FullName = individual?.FirstName + " " + individual?.LastName;
                eventAttendee.SendDate = DateTime.Now;
                eventAttendee.IsSentUpdate = true;
                eventAttendee.UpdateSentDate = DateTime.Now;
                eventAttendee.I_CalenderEvents_Id = eventDetails.Id;
                _gesEventCalendarUserAcceptService.Update(eventAttendee);
            }
            _gesEventCalendarUserAcceptService.Save();
        }


        [AllowAnonymous]
        public ActionResult DownloadEmail(string fileName)
        {
            var emailFile = this._gesFileStorageService.GetFilePathFromOldSystem(fileName);

            var term = this._gesFileStorageService.GetFilePathFromOldSystem("export_pdf_term.pdf");

            var mergeredFile = _pdfFilesMerger.MergeTempFile(new List<string> { term, emailFile }, Guid.NewGuid().ToString(), false);

            if (!string.IsNullOrEmpty(mergeredFile))
            {
                mergeredFile += ".pdf";
                var fileStream = _gesFileStorageService.GetFileStream(mergeredFile);

                if (fileStream != null && fileStream.CanRead)
                {
                    return File(fileStream, MimeMapping.GetMimeMapping(mergeredFile), fileName);
                }
            }
            return HttpNotFound();
        }

        [AllowAnonymous]
        [Route("DownloadGesReport/{documentId}")]
        public ActionResult DownloadGesReport(Guid? documentId)
        {
            if (!documentId.HasValue)
                return HttpNotFound();

            var orgId = this.GetOrganizationId(_gIndividualsService);

            var document = this.SafeExecute(() => this._gesDocumentService.GetDocumentById(orgId, documentId.Value), $"Exception when getting the document with criteria organization({orgId}) and document({documentId.Value})");

            if (document != null)
            {
                var fileStream = this.SafeExecute(() => this._gesFileStorageService.GetStream(orgId, documentId.Value), $"Exception when read the stream of document with criteria organization({orgId}) and document({documentId.Value})");

                if (fileStream != null && fileStream.CanRead)
                {
                    return File(fileStream, System.Web.MimeMapping.GetMimeMapping(document.FileName), document.FileName);
                }
            }

            return HttpNotFound();
        }

        public string DownLoadCompanyEmail(string fileName)
        {
            if (!string.IsNullOrEmpty(fileName))
            {
                var emailFile = this._gesFileStorageService.GetFilePathFromOldSystem(fileName);

                var downloadedFile = _pdfFileDownload.DownloadTempFile(emailFile, Guid.NewGuid().ToString(), false);

                if (!string.IsNullOrEmpty(downloadedFile))
                {
                    downloadedFile += ".pdf";
                    return _gesFileStorageService.GetExportFilePath(downloadedFile);
                }
            }

            return string.Empty;
        }


    }
}