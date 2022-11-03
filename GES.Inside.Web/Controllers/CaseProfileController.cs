using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using GES.Common.Configurations;
using GES.Common.Enumeration;
using GES.Common.Helpers;
using GES.Common.Models;
using GES.Inside.Data.Services.Interfaces;
using GES.Inside.Web.Configs;
using GES.Inside.Web.Helpers;
using GES.Common.Logging;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Helpers;
using GES.Inside.Data.Models;
using GES.Inside.Data.Repository.Interfaces;
using GES.Inside.Data.Services;
using GES.Inside.Web.Models;
using Microsoft.AspNet.Identity;
using GES.Inside.Web.Extensions;

namespace GES.Inside.Web.Controllers
{
    public class CaseProfileController : GesControllerBase
    {
        #region Declaration
        private readonly II_GesCaseProfilesService _caseProfilesService;
        private readonly IGesCaseReportSignUpService _caseReportSignupService;
        private readonly II_GesCaseReportsRepository _gesCaseProfilesRepository;
        private readonly II_GesCaseProfilesService _gesCaseProfilesService;
        private readonly II_CompaniesService _companiesService;
        private readonly II_CompaniesRepository _companiesRepository;
        private readonly IDocumentService _documentService;
        private readonly IUploadService _uploadService;
        private readonly II_GesCaseReportsI_EngagementTypesRepository _caseReportsIEngagementTypesRepository;
        private readonly ISdgRepository _sdgRepository;
        private readonly IGesCaseReportSdgRepository _gesCaseReportSdgRepository;
        private readonly II_GesCaseReportsKpisRepository _gesCaseReportsKpisRepository;
        private readonly II_KpisRepository _kpisRepository;
        private readonly II_KpiPerformanceRepository _kpiPerformanceRepository;
        private readonly II_GesCaseReportsI_KpisService _gesCaseReportsIKpisService;
        private readonly II_MilestonesRepository _milestonesRepository;
        private readonly IGesUngpAssessmentScoresRepository _assessmentScoresRepository;
        private readonly IGesUngpAssessmentScoresService _gesUngpAssessmentScoresService;
        private readonly IGesUngpAssessmentFormRepository _gesUngpAssessmentFormRepository;
        private readonly II_GesCommentaryRepository _commentaryRepository;
        private readonly II_GSSLinkRepository _gsslinkRepository;
        private readonly II_GesLatestNewsRepository _gesLatestNewsRepository;
        private readonly IGesUNGPAssessmentFormResourcesRepository _gesUngpAssessmentFormResourcesRepository;
        private readonly IGesUserService _gesUserService;
        private readonly IG_IndividualsService _gIndividualsService;
        private readonly II_GesCompanyDialogueService _gesCompanyDialogueService;
        private readonly IGesCaseReports_Audit_DetailService _gesCaseReportsAuditDetailService;
        private readonly II_GesSourceDialogueService _gesSourceDialogueService;
        private readonly II_ConventionsService _conventionsService;
        private readonly II_ConventionsRepository _conventionsRepository;
        private readonly II_GesCaseReportsI_ConventionsRepository _caseReportsIConventionsRepository;
        private readonly II_GesCaseReportsI_NormsRepository _caseReportsINormsRepository;
        private readonly II_NormsService _normService;
        private readonly II_EngagementDiscussionPointsRepository _engagementDiscussionPointsRepository;
        private readonly IG_IndividualsRepository _gIndividualsRepository;
        private readonly II_EngagementOtherStakeholderViewsRepository _engagementOtherStakeholderViewsRepository;
        private readonly II_GesAssociatedCorporationsRepository _gesAssociatedCorporationsRepository;
        private readonly II_GesCaseReportSourcesRepository _gesCaseReportSourcesRepository;
        private readonly II_GesCaseReportReferencesRepository _gesCaseReportReferencesRepository;
        private readonly II_GesCaseReportSupplementaryReadingRepository _gesCaseReportSupplementaryReadingRepository;
        private readonly IGesCaseProfileTemplatesRepository _caseProfileTemplatesRepository;
        private readonly IOrganizationService _organizationService;
        private readonly IGesFileStorageService _gesFileStorageService;
        private readonly IG_ManagedDocumentsRepository _documentsRepository;
        #endregion

        #region Constructor

        public CaseProfileController(IGesLogger logger, II_GesCaseProfilesService caseProfilesService,
            IGesCaseReportSignUpService caseReportSignupService, II_GesCaseReportsRepository gesCaseProfilesRepository,
            II_GesCaseProfilesService gesCaseProfilesService,
            II_CompaniesService companiesService, II_CompaniesRepository companiesRepository,
            IDocumentService documentService, IUploadService uploadService,
            II_GesCaseReportsI_EngagementTypesRepository caseReportsIEngagementTypesRepository,
            ISdgRepository sdgRepository, IGesCaseReportSdgRepository gesCaseReportSdgRepository,
            II_GesCaseReportsKpisRepository gesCaseReportsKpisRepository,
            II_KpisRepository kpisRepository,
            II_KpiPerformanceRepository kpiPerformanceRepository,
            II_GesCaseReportsI_KpisService gesCaseReportsIKpisService,
            II_MilestonesRepository milestonesRepository,
            IGesUngpAssessmentScoresRepository assessmentScoresRepository,
            IGesUngpAssessmentScoresService gesUngpAssessmentScoresService,
            IGesUngpAssessmentFormRepository gesUngpAssessmentFormRepository,
            IGesUserService gesUserService,
            II_GesCommentaryRepository commentaryRepository,
            II_GSSLinkRepository gsslinkRepository,
            IG_IndividualsService gIndividualsService,
            II_GesCompanyDialogueService gesCompanyDialogueService,
            II_GesLatestNewsRepository gesLatestNewsRepository,
            IGesUNGPAssessmentFormResourcesRepository gesUngpAssessmentFormResourcesRepository,
            II_GesSourceDialogueService gesSourceDialogueService,
            II_ConventionsService conventionsService,
            II_ConventionsRepository conventionsRepository,
            II_GesCaseReportsI_ConventionsRepository caseReportsIConventionsRepository,
            II_NormsService normService,
            II_GesCaseReportsI_NormsRepository caseReportsINormsRepository,
            IGesCaseReports_Audit_DetailService gesCaseReportsAuditDetailService,
            II_EngagementDiscussionPointsRepository engagementDiscussionPointsRepository,
            IG_IndividualsRepository gIndividualsRepository,
            II_EngagementOtherStakeholderViewsRepository engagementOtherStakeholderViewsRepository,
            II_GesAssociatedCorporationsRepository gesAssociatedCorporationsRepository,
            II_GesCaseReportSourcesRepository gesCaseReportSourcesRepository,
            II_GesCaseReportReferencesRepository gesCaseReportReferencesRepository,
            II_GesCaseReportSupplementaryReadingRepository gesCaseReportSupplementaryReadingRepository,
            IGesCaseProfileTemplatesRepository caseProfileTemplatesRepository,
            IGesFileStorageService gesFileStorageService,
            IG_ManagedDocumentsRepository documentsRepository,
        IOrganizationService organizationService) : base(logger)
        {
            _caseProfilesService = caseProfilesService;
            _caseReportSignupService = caseReportSignupService;
            _gesCaseProfilesRepository = gesCaseProfilesRepository;
            _gesCaseProfilesService = gesCaseProfilesService;
            _companiesService = companiesService;
            _companiesRepository = companiesRepository;
            _documentService = documentService;
            _uploadService = uploadService;
            _caseReportsIEngagementTypesRepository = caseReportsIEngagementTypesRepository;
            _sdgRepository = sdgRepository;
            _gesCaseReportSdgRepository = gesCaseReportSdgRepository;
            _gesCaseReportsKpisRepository = gesCaseReportsKpisRepository;
            _kpisRepository = kpisRepository;
            _kpiPerformanceRepository = kpiPerformanceRepository;
            _gesCaseReportsIKpisService = gesCaseReportsIKpisService;
            _milestonesRepository = milestonesRepository;
            _assessmentScoresRepository = assessmentScoresRepository;
            _gesUngpAssessmentScoresService = gesUngpAssessmentScoresService;
            _gesUngpAssessmentFormRepository = gesUngpAssessmentFormRepository;
            _gesUserService = gesUserService;
            _commentaryRepository = commentaryRepository;
            _gsslinkRepository = gsslinkRepository;
            _gesLatestNewsRepository = gesLatestNewsRepository;
            _gesSourceDialogueService = gesSourceDialogueService;
            _gesUngpAssessmentFormResourcesRepository = gesUngpAssessmentFormResourcesRepository;
            _gIndividualsService = gIndividualsService;
            _gesCompanyDialogueService = gesCompanyDialogueService;
            _conventionsService = conventionsService;
            _conventionsRepository = conventionsRepository;
            _caseReportsIConventionsRepository = caseReportsIConventionsRepository;
            _normService = normService;
            _caseReportsINormsRepository = caseReportsINormsRepository;
            _engagementDiscussionPointsRepository = engagementDiscussionPointsRepository;
            _gIndividualsRepository = gIndividualsRepository;
            _engagementOtherStakeholderViewsRepository = engagementOtherStakeholderViewsRepository;
            _gesAssociatedCorporationsRepository = gesAssociatedCorporationsRepository;
            _gesCaseReportsAuditDetailService = gesCaseReportsAuditDetailService;
            _gesCaseReportSourcesRepository = gesCaseReportSourcesRepository;
            _gesCaseReportReferencesRepository = gesCaseReportReferencesRepository;
            _gesCaseReportSupplementaryReadingRepository = gesCaseReportSupplementaryReadingRepository;
            _caseProfileTemplatesRepository = caseProfileTemplatesRepository;
            _gesFileStorageService = gesFileStorageService;
            _organizationService = organizationService;
            _documentsRepository = documentsRepository;
        }

        #endregion

        [CustomAuthorize(FormKey = "Case", Action = ActionEnum.Read)]
        public ActionResult List(long? companyId)
        {
            ViewBag.CompanyId = companyId ?? -1;
            ViewBag.Countries = _organizationService.GetCountries();
            ViewBag.NgController = "CaseProfileTemplateController";
            ViewBag.Title = "Case profiles";
            return View();
        }

        [CustomAuthorize(FormKey = "Case", Action = ActionEnum.Read)]
        public ActionResult Edit(long id)
        {
            this.SafeExecute(() =>
            {
                ViewBag.ClientsStatuses = _organizationService.GetClientStatuses();
                ViewBag.ClientProgressStatuses = _organizationService.GetClientProgressStatuses();
                ViewBag.Industries = _organizationService.GetIndustries();
                ViewBag.Countries = _organizationService.GetCountries();
            }, $"Error when getting the viewbag information. Please check inner exception for detail.");

            var caseReportTitle = _gesCaseProfilesService.GetCaseReportTitle(id);
            ViewBag.Title = caseReportTitle?.Key;
            ViewBag.SubTitle = caseReportTitle?.Value;
            ViewBag.NgController = "CaseProfileController";
            ViewBag.CaseProfileId = id;
            return View();
        }

        public ActionResult Add(long companyId)
        {
            ViewBag.CompanyId = companyId;
            ViewBag.NgController = "CaseProfileController";
            ViewBag.Title = "Add case profile";
            return View("Edit");
        }
        
        public ActionResult AddWithTemplate(long companyId, long engagementTypeId, long recommendationId)
        {
            ViewBag.CompanyId = companyId;
            ViewBag.NgController = "CaseProfileController";
            ViewBag.Title = "Add case profile";
            return View("Edit");
        }

        [CustomAuthorize(FormKey = "Endorsement", Action = ActionEnum.Read)]
        public ActionResult SignedUpCaseProfiles()
        {
            return View();
        }

        public ActionResult SignedUpUsers()
        {
            var idStr = Request.QueryString["id"];
            long id;
            var result = Int64.TryParse(idStr, out id);
            ViewBag.Id = -1;
            if (result)
                ViewBag.Id = id;

            return View();
        }

        #region Angularjs's API

        [HttpGet]
        public JsonResult GetCaseProfileById(long id)
        {
            var caseProfile = _gesCaseProfilesRepository.GetById(id);

            foreach (var propertyInfo in caseProfile.GetType().GetProperties())
            {
                if (propertyInfo.GetValue(caseProfile) != null && propertyInfo.GetValue(caseProfile).Equals("NULL"))
                {
                    propertyInfo.SetValue(caseProfile, "");
                }
            }

            var basicCaseProfile = Mapper.Map<BasicCaseProfileViewModel>(caseProfile);
            

            if (basicCaseProfile == null)
                return Json(new { Status = "The case profile id: " + id.ToString() + "do not exist" }, JsonRequestBehavior.AllowGet);

            basicCaseProfile.EntryDate = caseProfile.EntryDate?.ToString(Configurations.DateFormat);
            var companyId = _companiesRepository.GetCompanyIdFromCaseProfile(id);
            GetCompanyInfo(companyId, ref basicCaseProfile);
            basicCaseProfile.I_Engagement_Type_Id = _caseReportsIEngagementTypesRepository.GetByCaseProfileId(id).FirstOrDefault()?.I_EngagementTypes_Id;
            basicCaseProfile.SdgIds = _gesCaseReportSdgRepository.GetSdgIdsByCaseProfile(id);
            basicCaseProfile.MileStoneModel = _gesCaseProfilesRepository.GetLatestMilestoneModelByCaseProfileId(id);
            basicCaseProfile.CompanyDialogueLogs = _gesCaseProfilesRepository.GetCompanyDialogueLogsByCaseProfileId(id);
            basicCaseProfile.SourceDialogueLogs = _gesCaseProfilesRepository.GetSourceDialogueLogsByCaseProfileId(id);
            basicCaseProfile.DiscussionPoints = _engagementDiscussionPointsRepository.GetEngagementDiscussionPointsByCompanyId(companyId);
            basicCaseProfile.StakeholderViews = _engagementOtherStakeholderViewsRepository.GetStakeholderViews(companyId);
            basicCaseProfile.AssociatedCorporations = _gesAssociatedCorporationsRepository.GetAssociatedCorporationsByCaseProfile(id);
            basicCaseProfile.SourcesViewModels = _gesCaseReportSourcesRepository.GetSourcesByCaseReportId(id);
            basicCaseProfile.ReferencesViewModels = _gesCaseReportReferencesRepository.GetReferencesByCaseReportId(id);
            basicCaseProfile.SupplementaryReadingViewModels = _gesCaseReportSupplementaryReadingRepository.GetSupplementaryReadingsByCaseReportId(id);

            var documentViewModels = _documentsRepository.GetAdditionalDocuments(id).ToList();

            foreach (var doc in documentViewModels)
            {
                doc.DownloadUrl = "/documentmgmt/CompanyDocDownload/?documentId=" + doc.G_ManagedDocuments_Id;
            }

            basicCaseProfile.Documents = documentViewModels;

            if (caseProfile.I_GesCommentary != null)
            {
                basicCaseProfile.CommentaryViewModels = Mapper.Map<List<I_GesCommentary>, List<GesCommentaryViewModel>>(caseProfile.I_GesCommentary.ToList()).OrderByDescending(x => x.Created).ThenBy(n => n.CommentaryModified);
            }

            if (caseProfile.I_GSSLink != null)
            {
                basicCaseProfile.GSSLinkViewModels = Mapper.Map<List<I_GSSLink>, List<GSSLinkViewModel>>(caseProfile.I_GSSLink.ToList()).OrderByDescending(x => x.Created).ThenBy(n => n.GSSLinkModified);
            }

            if (caseProfile.I_GesLatestNews != null)
            {
                basicCaseProfile.GesLatestNewsViewModels = Mapper.Map<List<I_GesLatestNews>, List<GesLatestNewsViewModel>>(caseProfile.I_GesLatestNews.ToList()).OrderByDescending(x => x.Created).ThenBy(n => n.LatestNewsModified);
            }

            if (caseProfile.I_GesCaseReportsI_Conventions != null)
            {
                basicCaseProfile.ConventionsViewModels = Mapper.Map<List<I_GesCaseReportsI_Conventions>, List<ConventionsViewModel>>(caseProfile.I_GesCaseReportsI_Conventions.ToList());

            }

            if (caseProfile.I_GesCaseReportsI_Norms != null)
            {
                basicCaseProfile.NormViewModels = Mapper.Map<List<I_GesCaseReportsI_Norms>, List<NormViewModel>>(caseProfile.I_GesCaseReportsI_Norms.ToList());

            }

            return Json(basicCaseProfile, JsonRequestBehavior.AllowGet);
        }
        
        [HttpGet]
        public JsonResult GetCaseProfileInvisibleEntities(long engagementTypesId, long gesCaseReportStatusesId)
        {
            var caseProfileVisiableEntities = _caseProfileTemplatesRepository.GetGesCaseProfileInvisibleEntitiesViews(engagementTypesId, gesCaseReportStatusesId);

            return Json(caseProfileVisiableEntities, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetContactsJqGrid(JqGridViewModel jqGridParams)
        {
            long orgId = 0, individualId = 0;
            this.GetIndividualInfo(_gIndividualsService, ref individualId, ref orgId);

            var contacts = SafeExecute(() => _gesCaseProfilesRepository.GetContacts(jqGridParams, orgId), "Error when getting the Contacts {@JqGridViewModel}", jqGridParams);

            return Json(contacts);
        }

        private void GetCompanyInfo(long companyId, ref BasicCaseProfileViewModel basicCaseProfile)
        {
            var company = _companiesRepository.GetCompanyById(companyId);
            basicCaseProfile.I_Companies_Id = companyId;
            basicCaseProfile.CompanyName = company.Name;
            basicCaseProfile.CompanyIsin = company.Isin;
            basicCaseProfile.CompanyIndustry = company.SubPeerGroups?.Name;
            basicCaseProfile.CompanyHomeCountry = company.Countries?.Name;
            basicCaseProfile.UnGlobalCompact = company.UnGlobalCompact;
            basicCaseProfile.GriAlignedDisclosure = company.GriAlignedDisclosure;
        }

        private CaseReportGesUngpAssessmentFormViewModel GetGesUngpAssessmentFormViewModel(
            GesUNGPAssessmentForm gesUngpAssessmentForm)
        {
            var gesUngpAssessmentFormViewModel =
                Mapper.Map<CaseReportGesUngpAssessmentFormViewModel>(gesUngpAssessmentForm);

            if (gesUngpAssessmentFormViewModel != null)
            {
                GetUngpScoreValue(gesUngpAssessmentFormViewModel);
            }

            if (gesUngpAssessmentForm?.GesUNGPAssessmentFormResources != null & gesUngpAssessmentFormViewModel != null)
            {
                gesUngpAssessmentFormViewModel.AssessmentFormResourcesViewModels = Mapper
                    .Map<List<GesUNGPAssessmentFormResources>, List<GesUNGPAssessmentFormResourcesViewModel>>(
                        gesUngpAssessmentForm.GesUNGPAssessmentFormResources.ToList()).OrderByDescending(x => x.Created)
                    .ThenBy(n => n.Modified);

            }

            if (gesUngpAssessmentFormViewModel?.ModifiedBy != null)
            {
                var oldUserId = _gesUserService.GetById(gesUngpAssessmentFormViewModel?.ModifiedBy).OldUserId;

                if (oldUserId != null)
                {
                    var individual = _gIndividualsRepository.GetIndividualByUser((long)oldUserId);

                    if (individual != null)
                    {
                        gesUngpAssessmentFormViewModel.ModifiedByString =
                            $"{individual.FirstName} {individual.LastName} at {gesUngpAssessmentFormViewModel.Modified.ToString()}";
                    }
                }
            }

            if (gesUngpAssessmentFormViewModel != null)
            {
                var gesUngpAuditViewModel =
                    _gesUngpAssessmentFormRepository.GetGesUngpAssessmentFormHistoryByUngpId(
                        gesUngpAssessmentFormViewModel.I_GesCaseReports_Id);

                gesUngpAssessmentFormViewModel.GesUngpAuditViewModel = gesUngpAuditViewModel;
            }

            return gesUngpAssessmentFormViewModel;

        }

        private void GetUngpScoreValue(CaseReportGesUngpAssessmentFormViewModel gesUngpAssessmentFormViewModel)
        {
            gesUngpAssessmentFormViewModel.TheExtentOfHarmesScoreValue =
               GetUngpScoreDescription(gesUngpAssessmentFormViewModel.TheExtentOfHarmesScoreId);
            gesUngpAssessmentFormViewModel.TheNumberOfPeopleAffectedScoreValue =
                GetUngpScoreDescription(gesUngpAssessmentFormViewModel.TheNumberOfPeopleAffectedScoreId);
            gesUngpAssessmentFormViewModel.OverSeveralYearsScoreValue =
                GetUngpScoreDescription(gesUngpAssessmentFormViewModel.OverSeveralYearsScoreId);
            gesUngpAssessmentFormViewModel.SeveralLocationsScoreValue =
                GetUngpScoreDescription(gesUngpAssessmentFormViewModel.SeveralLocationsScoreId);
            gesUngpAssessmentFormViewModel.IsViolationScoreValue =
                GetUngpScoreDescription(gesUngpAssessmentFormViewModel.IsViolationScoreId);
            gesUngpAssessmentFormViewModel.GesConfirmedViolationScoreValue =
                GetUngpScoreDescription(gesUngpAssessmentFormViewModel.GesConfirmedViolationScoreId);
            gesUngpAssessmentFormViewModel.HumanRightsPolicyPubliclyDisclosedAddScoreValue =
                GetUngpScoreDescription(gesUngpAssessmentFormViewModel.HumanRightsPolicyPubliclyDisclosedAddScoreId);
            gesUngpAssessmentFormViewModel.HumanRightsPolicyCommunicatedScoreValue =
                GetUngpScoreDescription(gesUngpAssessmentFormViewModel.HumanRightsPolicyCommunicatedScoreId);
            gesUngpAssessmentFormViewModel.HumanRightsPolicyStipulatesScoreValue =
                GetUngpScoreDescription(gesUngpAssessmentFormViewModel.HumanRightsPolicyStipulatesScoreId);
            gesUngpAssessmentFormViewModel.HumanRightsPolicyApprovedScoreValue =
                GetUngpScoreDescription(gesUngpAssessmentFormViewModel.HumanRightsPolicyApprovedScoreId);
            gesUngpAssessmentFormViewModel.GovernanceCommitmentScoreValue =
                GetUngpScoreDescription(gesUngpAssessmentFormViewModel.GovernanceCommitmentScoreId);
            gesUngpAssessmentFormViewModel.GovernanceExamplesScoreValue =
                GetUngpScoreDescription(gesUngpAssessmentFormViewModel.GovernanceExamplesScoreId);
            gesUngpAssessmentFormViewModel.GovernanceClearDivisionScoreValue =
                GetUngpScoreDescription(gesUngpAssessmentFormViewModel.GovernanceClearDivisionScoreId);
            gesUngpAssessmentFormViewModel.BusinessPartnersAddScoreValue =
                GetUngpScoreDescription(gesUngpAssessmentFormViewModel.BusinessPartnersAddScoreId);
            gesUngpAssessmentFormViewModel.IdentificationAndCommitmentScoreValue =
                GetUngpScoreDescription(gesUngpAssessmentFormViewModel.IdentificationAndCommitmentScoreId);
            gesUngpAssessmentFormViewModel.StakeholderEngagementAddScoreValue =
                GetUngpScoreDescription(gesUngpAssessmentFormViewModel.StakeholderEngagementAddScoreId);
            gesUngpAssessmentFormViewModel.HumanRightsTrainingScoreValue =
                GetUngpScoreDescription(gesUngpAssessmentFormViewModel.HumanRightsTrainingScoreId);
            gesUngpAssessmentFormViewModel.RemedyProcessInPlaceScoreValue =
                GetUngpScoreDescription(gesUngpAssessmentFormViewModel.RemedyProcessInPlaceScoreId);
            gesUngpAssessmentFormViewModel.GrievanceMechanismHasOperationalLevelScoreValue =
                GetUngpScoreDescription(gesUngpAssessmentFormViewModel.GrievanceMechanismHasOperationalLevelScoreId);
            gesUngpAssessmentFormViewModel.GrievanceMechanismExistenceOfOperationalLevelScoreValue =
                GetUngpScoreDescription(gesUngpAssessmentFormViewModel
                    .GrievanceMechanismExistenceOfOperationalLevelScoreId);
            gesUngpAssessmentFormViewModel.GrievanceMechanismClearProcessScoreValue =
                GetUngpScoreDescription(gesUngpAssessmentFormViewModel.GrievanceMechanismClearProcessScoreId);
            gesUngpAssessmentFormViewModel.GrievanceMechanismRightsNormsScoreValue =
                GetUngpScoreDescription(gesUngpAssessmentFormViewModel.GrievanceMechanismRightsNormsScoreId);
            gesUngpAssessmentFormViewModel.GrievanceMechanismFilingGrievanceScoreValue =
                GetUngpScoreDescription(gesUngpAssessmentFormViewModel.GrievanceMechanismFilingGrievanceScoreId);
            gesUngpAssessmentFormViewModel.GrievanceMechanismReoccurringGrievancesScoreValue =
                GetUngpScoreDescription(gesUngpAssessmentFormViewModel.GrievanceMechanismReoccurringGrievancesScoreId);
            gesUngpAssessmentFormViewModel.GrievanceMechanismFormatAndProcesseScoreValue =
                GetUngpScoreDescription(gesUngpAssessmentFormViewModel.GrievanceMechanismFormatAndProcesseScoreId);
        }

        private string GetUngpScoreDescription(Guid? scoreId)
        {
            return _gesUngpAssessmentScoresService.GetScoreDescription(scoreId);
        }

        [HttpGet]
        public JsonResult GetNewCaseProfile(long? companyId)
        {
            long? gesCompanyId = null;
            if (companyId != null)
            {
                gesCompanyId = _companiesService.GetGesCompanyId(companyId.Value);
            }

            if (gesCompanyId == null)
            {
                return Json(new { Status = "Failure" }, JsonRequestBehavior.AllowGet);
            }

            var caseProfile = new I_GesCaseReports { I_GesCompanies_Id = gesCompanyId.Value };

            var basicCaseProfile = Mapper.Map<BasicCaseProfileViewModel>(caseProfile);

            if (basicCaseProfile == null)
                return Json(new { Status = "Failure" },  JsonRequestBehavior.AllowGet);

            basicCaseProfile.EntryDate = caseProfile.EntryDate?.ToString(Configurations.DateFormat);
            GetCompanyInfo(companyId.Value, ref basicCaseProfile);
            return Json(basicCaseProfile, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetGesUngpAssessment(long? gesCaseReportId)
        {
            CaseReportGesUngpAssessmentFormViewModel gesUngpAssessmentForm;
            try
            {
                gesUngpAssessmentForm = GetGesUngpAssessmentFormViewModel(_gesCaseReportSdgRepository.GesUngpAssessmentByCaseProfile(gesCaseReportId));
                if (gesUngpAssessmentForm == null)
                {
                    gesUngpAssessmentForm = new CaseReportGesUngpAssessmentFormViewModel();
                    var assessmentFormResourcesViewModels = new List<GesUNGPAssessmentFormResourcesViewModel>();
                    gesUngpAssessmentForm.AssessmentFormResourcesViewModels = assessmentFormResourcesViewModels;
                }
            }
            catch (Exception e)
            {
                return Json(new { Status = "Load UNGP failed, caused: " + e.Message }, JsonRequestBehavior.AllowGet);
            }

            return Json(gesUngpAssessmentForm, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult GetCountries()
        {
            //TODO truong.pham check to show empty name country
            var countries = _companiesService.AllCountries().Select(x => new { x.Id, FullName = x.Name ?? string.Empty, CountryCode = x.Alpha3Code?.ToLower()?.Substring(0,2) }).OrderBy(d=>d.FullName).ToList();
            //countries.Insert(0, new {Id = Guid.Parse("00000000-0000-0000-0000-000000000001"), FullName  = "N/A", CountryCode = "N/A"});

            return Json(countries, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetRecommendations()
        {
            var recoumendations = _gesCaseProfilesRepository.GetRecommendations().Select(x => new { x.I_GesCaseReportStatuses_Id, x.Name });
            return Json(recoumendations, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetIssueHeadings()
        {
            var recoumendations = _gesCaseProfilesRepository.GetIssueHeadings().Select(x => new { x.Id, x.Name });
            return Json(recoumendations, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetConclusions()
        {
            var recoumendations = _gesCaseProfilesRepository.GetConclusions().Select(x => new { x.I_GesCaseReportStatuses_Id, x.Name });
            return Json(recoumendations, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetEngagementTypes()
        {
            var engagementTypes = _gesCaseProfilesRepository.GetEngagementTypes().Select(x => new { x.Id, x.Name }).OrderBy(d=>d.Name);
            return Json(engagementTypes, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetNormAreas()
        {
            var normAreas = _gesCaseProfilesRepository.GetNormAreas().Select(x => new { x.I_NormAreas_Id, x.Name });
            return Json(normAreas, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetContactDirections()
        {
            var contactDirections = ((ContactDirection[])Enum.GetValues(typeof(ContactDirection))).Select(c => new I_ContactDirections() { I_ContactDirections_Id = (int)c, Name = c.ToString() }).ToList();
            return Json(contactDirections, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetContactTypes()
        {
            var contactTypes = _companiesRepository.GetContactTypes().ToList();
            return Json(contactTypes, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetUsers()
        {
            var users = _gesCaseProfilesRepository.GetUsers().Select(x => new { x.UserId, FullName = $"{x.FirstName} {x.LastName}" }).OrderBy(x => x.FullName);
            return Json(users, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetResponseStatuses()
        {
            var responseStatuses = _gesCaseProfilesRepository.GetResponseStatuses().Select(x => new { x.I_ResponseStatuses_Id, x.ShortName });
            return Json(responseStatuses, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetDocumentManagementTaxonomies()
        {
            var documentManagementTaxonomies = _gesCaseProfilesRepository.GetDocumentManagementTaxonomies().Select(x => new { x.G_DocumentManagementTaxonomies_Id, x.Name }).OrderBy(x => x.Name);
            return Json(documentManagementTaxonomies, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetManagedDocumentServices()
        {
            var documentServices = _gesCaseProfilesRepository.GetManagedDocumentServices().Select(x => new { x.G_ManagedDocumentServices_Id, x.Name }).OrderBy(x => x.Name);
            return Json(documentServices, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetProgressStatuses()
        {
            var progressStatuses = _gesCaseProfilesRepository.GetProgressStatuses().Select(x => new { x.I_ProgressStatuses_Id, x.ShortName });
            return Json(progressStatuses, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetDevelopmentGradeText(int progressId, int responseId)
        {
            //Can be calculated in front-end, but call from back-end for consistency
            var developmentGrade = DataHelper.CalcDevelopmentGrade(progressId, responseId);
            var developmentGradeText = DataHelper.ConvertDevelopmentGradeToText(developmentGrade);
            return Json(developmentGradeText, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetAdditionalDocuments(long companyId)
        {
            if (companyId == 0)
                return null;

            var documents = _documentService.GetDocumentsByCompanyId(companyId).Select(x =>
            {
                x.DownloadUrl = CommonHelper.GetDocDownloadUrl(x.FileName);
                return x;
            });
            return Json(documents, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetUploadedDocumentById(long documentId)
        {
            var document = _documentService.GetUploadedDocumentById(documentId);
            document.DownloadUrl = CommonHelper.GetDocDownloadUrl(document.FileName);
            return Json(document, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetUserById(long id)
        {
            var user = _gesCaseProfilesRepository.GetUsers().FirstOrDefault(x => x.UserId == id);
            return Json(user, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetSdgs()
        {
            var sdgs = _sdgRepository.GetSdgs().Select(x => new SdgViewModel { Sdg_Id = x.Sdg_Id, Sdg_Link = x.Sdg_Link, Sdg_Name = x.Sdg_Name, DocumentId = x.DocumentId ?? new Guid() });
            return Json(sdgs, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetCaseReportKpisByCaseReportId(long gesCaseReportId)
        {
            var caseReportKpIs = _gesCaseReportsKpisRepository.GetGesCaseReportsKpisByCaseReportID(gesCaseReportId);
            return Json(caseReportKpIs, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetKpisByEngagementTypeId(long engagementTypeId)
        {
            var caseReportKpIs = _kpisRepository.GetKpisByEngagementTypeId(engagementTypeId);
            return Json(caseReportKpIs, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetKpiPerformances()
        {
            var caseReportKpIs = _kpiPerformanceRepository.GetAllKpiPerformances();
            return Json(caseReportKpIs, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetUngpAssessmentScores()
        {
            var ungPerformancesScores = _gesUngpAssessmentScoresService.GetAllGesUngpAssessmentScores();

            var gesUngpAssessmentScoresViewModels = ungPerformancesScores as GesUngpAssessmentScoresViewModel[] ?? ungPerformancesScores.ToArray();
            var assessmentScoresViewModels = new GesUngpAssessmentScoresViewModels
            {
                ScaleExtentOfHarmScore =
                        GetGesUngpAssessmentScoresByScoreType(gesUngpAssessmentScoresViewModels, UngpScoreType.SXOH.ToString()),
                ScaleNumberOfPeopleAffectedScore = GetGesUngpAssessmentScoresByScoreType(gesUngpAssessmentScoresViewModels, UngpScoreType.SNPA.ToString()),
                SystematicOverServeralYearsScore = GetGesUngpAssessmentScoresByScoreType(gesUngpAssessmentScoresViewModels, UngpScoreType.SOSY.ToString()),
                SystematicOverServeralLocationScore = GetGesUngpAssessmentScoresByScoreType(gesUngpAssessmentScoresViewModels, UngpScoreType.SSLO.ToString()),
                OngoingViolationOccurringScore = GetGesUngpAssessmentScoresByScoreType(gesUngpAssessmentScoresViewModels, UngpScoreType.OVSO.ToString()),
                ConfirmedViolationOfInternationalNormsScore = GetGesUngpAssessmentScoresByScoreType(gesUngpAssessmentScoresViewModels, UngpScoreType.CVOI.ToString()),
                HumanRightsPubliclyDisclosedAdditionalScore = GetGesUngpAssessmentScoresByScoreType(gesUngpAssessmentScoresViewModels, UngpScoreType.HNMR.ToString()),
                HumanRightsBusinessPartnersAdditionalScore = GetGesUngpAssessmentScoresByScoreType(gesUngpAssessmentScoresViewModels, UngpScoreType.HBPR.ToString()),
                HumanRightsStakeholderEngagementAdditionalScore = GetGesUngpAssessmentScoresByScoreType(gesUngpAssessmentScoresViewModels, UngpScoreType.HBSE.ToString()),
                HumanRightsTrainningAdditionalScore = GetGesUngpAssessmentScoresByScoreType(gesUngpAssessmentScoresViewModels, UngpScoreType.HBTC.ToString()),

                HumanRightsPubliclyDisclosedCommunicatedScore = GetGesUngpAssessmentScoresByScoreType(gesUngpAssessmentScoresViewModels, UngpScoreType.HSPC.ToString()),

                HumanRightsPubliclyDisclosedExpectationsPersonnelScore = GetGesUngpAssessmentScoresByScoreType(gesUngpAssessmentScoresViewModels, UngpScoreType.HPSE.ToString()),
                HumanRightsPubliclyDisclosedExpectationsPolicyApproved = GetGesUngpAssessmentScoresByScoreType(gesUngpAssessmentScoresViewModels, UngpScoreType.HPAP.ToString()),
                HumanRightsGovernanceCommitment = GetGesUngpAssessmentScoresByScoreType(gesUngpAssessmentScoresViewModels, UngpScoreType.HGWC.ToString()),
                HumanRightsGovernanceProvidesExamples = GetGesUngpAssessmentScoresByScoreType(gesUngpAssessmentScoresViewModels, UngpScoreType.HGPE.ToString()),
                HumanRightsGovernanceClearDivision = GetGesUngpAssessmentScoresByScoreType(gesUngpAssessmentScoresViewModels, UngpScoreType.HGCD.ToString()),
                HumanRightsIdentificationCommitment = GetGesUngpAssessmentScoresByScoreType(gesUngpAssessmentScoresViewModels, UngpScoreType.HICD.ToString()),
                RemediationAdverseHumanRightsImpactsRemedyProcess = GetGesUngpAssessmentScoresByScoreType(gesUngpAssessmentScoresViewModels, UngpScoreType.RRCP.ToString()),
                RemediationAdverseHumanRightsImpactsGrievancMechanismOperationalLevel = GetGesUngpAssessmentScoresByScoreType(gesUngpAssessmentScoresViewModels, UngpScoreType.RGOM.ToString()),
                RemediationAdverseHumanRightsImpactsGrievancMechanismExistenceOperationalLevel = GetGesUngpAssessmentScoresByScoreType(gesUngpAssessmentScoresViewModels, UngpScoreType.RGEO.ToString()),
                RemediationAdverseHumanRightsImpactsGrievancMechanismDisclosesClearProcess = GetGesUngpAssessmentScoresByScoreType(gesUngpAssessmentScoresViewModels, UngpScoreType.RGDC.ToString()),
                RemediationAdverseHumanRightsImpactsGrievancMechanismAddressesGrievance = GetGesUngpAssessmentScoresByScoreType(gesUngpAssessmentScoresViewModels, UngpScoreType.RGGA.ToString()),
                RemediationAdverseHumanRightsImpactsGrievancMechanismFilingGrievance = GetGesUngpAssessmentScoresByScoreType(gesUngpAssessmentScoresViewModels, UngpScoreType.RGFG.ToString()),
                RemediationAdverseHumanRightsImpactsGrievancMechanismReoccurringGrievances = GetGesUngpAssessmentScoresByScoreType(gesUngpAssessmentScoresViewModels, UngpScoreType.RGRG.ToString()),
                RemediationAdverseHumanRightsImpactsGrievancMechanismFormatProcesses = GetGesUngpAssessmentScoresByScoreType(gesUngpAssessmentScoresViewModels, UngpScoreType.RGFP.ToString())

            };

            return Json(assessmentScoresViewModels, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetMilestoneTypes()
        {
            //TODO truong.pham check to show empty name country
            var milestones = _gesCaseProfilesRepository.AllMilestoneTypes().Select(x => new { x.Id, Name = x.Name ?? string.Empty, MilestoneCode = x.MilestoneCode?.ToLower(), x.Description, x.Level, x.Order }).OrderBy(x => x.Order);
            return Json(milestones, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetMilestoneByCaseReportId(long caseId)
        {
            var milestones = _gesCaseProfilesRepository.GetMilestonesByCasereportId(caseId).OrderByDescending(x => x.MilestoneModified);
            return Json(milestones, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetManagedDocumentByDocumentId(long companyDialogId, long? documentId)
        {
            if (documentId != null)
            {
                var document = _documentService.GetUploadedDocumentById(documentId.Value);
                if (document != null)
                {
                    document.DownloadUrl = CommonHelper.GetDocDownloadUrl(document.FileName);
                }
                else
                {
                    document = new DocumentViewModel()
                    {
                        I_GesCompanyDialogues_Id = companyDialogId,
                        Name = "",
                        FileName = ""
                    };
                }

                return Json(document, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var document = new DocumentViewModel()
                {
                    I_GesCompanyDialogues_Id = companyDialogId,
                    Name = "",
                    FileName = ""
                };
                return Json(document, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult GetManagedDocumentByCompanySourceDialogId(long companySourceDialogId, string dialogType)
        {
                var document = _documentService.GetDocumentByCompanySourceDialogId(companySourceDialogId, dialogType);
                if (document != null)
                {
                    document.DownloadUrl = CommonHelper.GetDocDownloadUrl(document.FileName);
                }
                else
                {
                    document = new DocumentViewModel()
                    {
                        I_GesCompanyDialogues_Id = companySourceDialogId,
                        Name = "",
                        FileName = ""
                    };
                }

                return Json(document, JsonRequestBehavior.AllowGet);
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

        [HttpGet]
        public JsonResult GetConventions()
        {
            var conventions = _conventionsService.GetAllConventions();
            return Json(conventions, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetAllConventions()
        {
            var conventions = _caseReportsIConventionsRepository.GetAllConventions();
            return Json(conventions, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetNorms()
        {
            var norms = _normService.GetAllNorms();
            return Json(norms, JsonRequestBehavior.AllowGet);
        }

        private IEnumerable<GesUngpAssessmentScoresViewModel> GetGesUngpAssessmentScoresByScoreType(IEnumerable<GesUngpAssessmentScoresViewModel> assessmentScoresViewModels, string scoreType)
        {
            var assessmentScoresViewModelsByScoreType = assessmentScoresViewModels.Where(assessmentScoresViewModel => scoreType.Equals(assessmentScoresViewModel.ScoreType)).ToList().OrderBy(x => x.Order);

            return assessmentScoresViewModelsByScoreType;
        }

        [HttpPost]
        public JsonResult UpdateCaseProfile(BasicCaseProfileViewModel caseProfile)
        {
            I_GesCaseReports updatingCaseProfile;
            if (caseProfile.I_GesCaseReports_Id == 0)
            {
                updatingCaseProfile = new I_GesCaseReports();
            }
            else
            {
                updatingCaseProfile = _gesCaseProfilesRepository.GetById(caseProfile.I_GesCaseReports_Id);
                if (updatingCaseProfile == null)
                    return null;
            }

            DateTime entryDate;
            if (DateTime.TryParse(caseProfile.EntryDate, out entryDate))
            {
                updatingCaseProfile.EntryDate = entryDate;
            }

            updatingCaseProfile.Summary = caseProfile.Summary;
            updatingCaseProfile.ReportIncident = caseProfile.ReportIncident;
            updatingCaseProfile.IssueHeadingId = caseProfile.IssueHeadingId;
            updatingCaseProfile.Guidelines = caseProfile.Guidelines;
            updatingCaseProfile.Description = caseProfile.Description;
            updatingCaseProfile.DescriptionNew = caseProfile.DescriptionNew;
            updatingCaseProfile.Conclusion = caseProfile.Conclusion;
            updatingCaseProfile.CompanyDialogueSummary = caseProfile.CompanyDialogueSummary;
            updatingCaseProfile.CompanyDialogueNew = caseProfile.CompanyDialogueNew;
            updatingCaseProfile.SourceDialogueSummary = caseProfile.SourceDialogueSummary;
            updatingCaseProfile.SourceDialogueNew = caseProfile.SourceDialogueNew;
            updatingCaseProfile.ProcessGoal = caseProfile.ProcessGoal;
            updatingCaseProfile.ProcessStep = caseProfile.ProcessStep;
            updatingCaseProfile.CountryId = caseProfile.CountryId;
            updatingCaseProfile.NewI_GesCaseReportStatuses_Id = caseProfile.NewI_GesCaseReportStatuses_Id;
            updatingCaseProfile.I_GesCaseReportStatuses_Id = caseProfile.I_GesCaseReportStatuses_Id;
            updatingCaseProfile.I_NormAreas_Id = caseProfile.I_NormAreas_Id;
            updatingCaseProfile.AnalystG_Users_Id = caseProfile.AnalystG_Users_Id;
            updatingCaseProfile.I_ResponseStatuses_Id = caseProfile.I_ResponseStatuses_Id;
            updatingCaseProfile.I_ProgressStatuses_Id = caseProfile.I_ProgressStatuses_Id;
            updatingCaseProfile.I_GesCompanies_Id = caseProfile.I_GesCompanies_Id;
            updatingCaseProfile.ShowInClient = caseProfile.ShowInClient;
            updatingCaseProfile.Comment = caseProfile.Comment;

            updatingCaseProfile.IncidentAnalysisSummary = caseProfile.IncidentAnalysisSummary;
            updatingCaseProfile.IncidentAnalysisDialogueAndAnalysis = caseProfile.IncidentAnalysisDialogueAndAnalysis;
            updatingCaseProfile.IncidentAnalysisConclusion = caseProfile.IncidentAnalysisConclusion;
            updatingCaseProfile.IncidentAnalysisGuidelines = caseProfile.IncidentAnalysisGuidelines;
            updatingCaseProfile.IncidentAnalysisSources = caseProfile.IncidentAnalysisSources;
            updatingCaseProfile.ClosingIncidentAnalysisSummary = caseProfile.ClosingIncidentAnalysisSummary;
            updatingCaseProfile.ClosingIncidentAnalysisDialogueAndAnalysis = caseProfile.ClosingIncidentAnalysisDialogueAndAnalysis;
            updatingCaseProfile.ClosingIncidentAnalysisConclusion = caseProfile.ClosingIncidentAnalysisConclusion;
            
            if (updatingCaseProfile.I_GesCaseReports_Id == 0)
            {
                updatingCaseProfile.Created = DateTime.UtcNow;
                _gesCaseProfilesRepository.Add(updatingCaseProfile);
            }
            else
            {
                _gesCaseProfilesRepository.Edit(updatingCaseProfile);
            }

            _gesCaseProfilesRepository.Save();

            UpdateCaseReportEngagementTypes(updatingCaseProfile.I_GesCaseReports_Id, caseProfile.I_Engagement_Type_Id);

            //Update Company-Source-Dialogue
            //UpdateCompanyDialogues(caseProfile.CompanyDialogueLogs, caseProfile.I_GesCaseReports_Id);

            //UpdateSourceDialogues(caseProfile.SourceDialogueLogs, caseProfile.I_GesCaseReports_Id);

            //if (caseProfile.MileStoneModel != null)
            //{
            //    UpdateCaseReportMilestone(updatingCaseProfile.I_GesCaseReports_Id, caseProfile.MileStoneModel);
            //}

            //_gesCaseReportSdgRepository.TryUpdateSdgsForCaseProfile(updatingCaseProfile.I_GesCaseReports_Id,
            //    caseProfile.SdgIds);

            //var updateCaseReportKpisResult = UpdateGesCaseReportsKpis(
            //    (List<CaseReportKpiViewModel>)caseProfile.CaseReportKpiViewModels, updatingCaseProfile.I_GesCaseReports_Id);

            //if (!updateCaseReportKpisResult)
            //{
            //    return Json(new
            //    {
            //        success = false,
            //        message = "Failed updating CaseReport Kpis."
            //    });
            //}

            //            if (caseProfile.GesUngpAssessmentFormViewModel != null)
            //            {
            //                var updateGesUngpAssessmentFormResult = UpdateGesUngpAssessmentForm(updatingCaseProfile.I_GesCaseReports_Id,
            //                    caseProfile.GesUngpAssessmentFormViewModel);
            //
            //                if (!updateGesUngpAssessmentFormResult)
            //                {
            //                    return Json(new
            //                    {
            //                        success = false,
            //                        message = "Failed updating CaseReport Ges UNGP Assessment."
            //                    });
            //                }
            //            }

            var updateCommentaryResult = UpdateCommentaries((List<GesCommentaryViewModel>)caseProfile.CommentaryViewModels, updatingCaseProfile.I_GesCaseReports_Id);

            if (!updateCommentaryResult)
            {
                return Json(new
                {
                    success = false,
                    message = "Failed updating Sustainalytics Commentary."
                });
            }            

            var updateLatestNewsesResult = UpdateLatestNews((List<GesLatestNewsViewModel>)caseProfile.GesLatestNewsViewModels,
                updatingCaseProfile.I_GesCaseReports_Id);

            if (!updateLatestNewsesResult)
            {
                return Json(new
                {
                    success = false,
                    message = "Failed updating Ges Latest News."
                });
            }

            return Json(new { caseProfileId = updatingCaseProfile.I_GesCaseReports_Id, Status = "Success" });
        }

        //private void UpdateCaseReportMilestone(long caseProfileId, MilestoneModel milestoneModel)
        //{
        //    var oldMilestone = _milestonesRepository.GetMilestone(caseProfileId, milestoneModel.MilestoneId);

        //    //Update description
        //    if (oldMilestone != null)
        //    {
        //        if (oldMilestone.Description != null && oldMilestone.Description.Equals(milestoneModel.MilestoneDescription) && oldMilestone.GesMilestoneTypesId == milestoneModel.GesMilestoneTypesId) return;
        //        oldMilestone.Description = milestoneModel.MilestoneDescription;
        //        oldMilestone.MilestoneModified = DateTime.UtcNow;
        //        oldMilestone.GesMilestoneTypesId = milestoneModel.GesMilestoneTypesId;
        //        _milestonesRepository.Edit(oldMilestone);

        //    }
        //    else
        //    {
        //        //Add new Milestone
        //        var newMilestone = new I_Milestones
        //        {
        //            I_GesCaseReports_Id = caseProfileId,
        //            GesMilestoneTypesId = milestoneModel.GesMilestoneTypesId,
        //            Description = milestoneModel.MilestoneDescription,
        //            MilestoneModified = DateTime.UtcNow,
        //            Created = DateTime.UtcNow
        //        };

        //        _milestonesRepository.Add(newMilestone);
        //    }

        //    _milestonesRepository.Save();
        //}

        [HttpPost]
        public JsonResult SaveMilestone(MilestoneModel milestone)
        {
            var x = milestone;

            if (milestone.MilestoneId == 0)
            {
                var newMilestone = new I_Milestones
                {
                    I_GesCaseReports_Id = milestone.CaseReportId,
                    Description = milestone.MilestoneDescription,
                    MilestoneModified = milestone.MilestoneModified != null ? milestone.MilestoneModified.Value.Date : (DateTime?)null,
                    Created = DateTime.UtcNow,
                    GesMilestoneTypesId = milestone.GesMilestoneTypesId
                };
                _milestonesRepository.Add(newMilestone);
            }
            else
            {
                var oldMilestone = _milestonesRepository.GetById(milestone.MilestoneId);

                if (oldMilestone != null)
                {
                    oldMilestone.GesMilestoneTypesId = milestone.GesMilestoneTypesId;
                    oldMilestone.MilestoneModified = milestone.MilestoneModified != null ? milestone.MilestoneModified.Value.Date : (DateTime?)null;                    
                    oldMilestone.Description = milestone.MilestoneDescription;
                    _milestonesRepository.Edit(oldMilestone);
                }
            }

            _milestonesRepository.Save();

            return Json(new { Status = "Success", Id = milestone.CaseReportId });
        }

        [HttpPost]
        public JsonResult DeleteMilestone(long milestoneId)
        {
            try
            {
                var milestone = _milestonesRepository.GetById(milestoneId);

                if (milestone != null)
                {
                    _milestonesRepository.Delete(milestone);

                    _milestonesRepository.Save();
                }
            }
            catch
            {
                return Json(new { Status = "Error" });
            }
            return Json(new { Status = "Success" });
        }

        private void UpdateCaseReportEngagementTypes(long caseProfileId, long? engagementTypeId)
        {
            if (engagementTypeId == null)
            {
                return;
            }
            var engagementType = _caseReportsIEngagementTypesRepository.GetByCaseProfileId(caseProfileId).FirstOrDefault();
            if (engagementType == null)
            {
                engagementType = new I_GesCaseReportsI_EngagementTypes
                {
                    Created = DateTime.UtcNow,
                    I_EngagementTypes_Id = engagementTypeId.Value,
                    I_GesCaseReports_Id = caseProfileId
                };

                _caseReportsIEngagementTypesRepository.Add(engagementType);
            }
            else if (engagementType.I_EngagementTypes_Id != engagementTypeId.Value)
            {
                engagementType.I_EngagementTypes_Id = engagementTypeId.Value;
                _caseReportsIEngagementTypesRepository.Edit(engagementType);
            }
            _caseReportsIEngagementTypesRepository.Save();
        }

        [HttpPost]
        public JsonResult SaveKpi(CaseReportKpiViewModel kpi)
        {

            I_GesCaseReportsI_Kpis m_Kpi = null;
            try
            {
                if (kpi.I_GesCaseReportsI_Kpis_Id == 0)
                {
                    m_Kpi = new I_GesCaseReportsI_Kpis()
                    {
                        I_GesCaseReports_Id = kpi.I_GesCaseReports_Id,
                        I_KpiPerformance_Id = kpi.I_KpiPerformance_Id,
                        I_Kpis_Id = kpi.I_Kpis_Id,
                        Created = DateTime.UtcNow
                    };
                    _gesCaseReportsIKpisService.Add(m_Kpi);

                }
                else
                {
                    m_Kpi = _gesCaseReportsIKpisService.GetById(kpi.I_GesCaseReportsI_Kpis_Id);

                    if (m_Kpi != null)
                    {
                        m_Kpi.I_KpiPerformance_Id = kpi.I_KpiPerformance_Id;
                        m_Kpi.I_Kpis_Id = kpi.I_Kpis_Id;
                        _gesCaseReportsIKpisService.Update(m_Kpi);
                    }
                }
                _gesCaseReportsIKpisService.Save();
            }
            catch
            {
                return Json(new { Status = "Error" });
            }

            return Json(new { Status = "Success", Id = m_Kpi.I_GesCaseReportsI_Kpis_Id });
        }

        [HttpPost]
        public JsonResult DeleteKpi(CaseReportKpiViewModel kpi)
        {
            try
            {
                var m_Kpi = _gesCaseReportsIKpisService.GetById(kpi.I_GesCaseReportsI_Kpis_Id);

                if (m_Kpi != null)
                {
                    _gesCaseReportsIKpisService.Delete(m_Kpi);

                    _gesCaseReportsIKpisService.Save();
                }
            }
            catch
            {
                return Json(new { Status = "Error" });
            }
            return Json(new { Status = "Success" });
        }

        [HttpPost]
        public JsonResult SaveUngp(long caseProfileId, CaseReportGesUngpAssessmentFormViewModel ungp)
        {

            try
            {
                if (ungp != null && caseProfileId != 0)
                {
                    var updateGesUngpAssessmentFormResult = UpdateGesUngpAssessmentForm(
                        caseProfileId,
                        ungp);
                }
            }
            catch (Exception e)
            {
                return Json(new { Status = "Failed updating CaseReport Ges UNGP Assessment. Caused:" + e.Message });
            }

            return Json(new { Status = "Success", ungp });
        }

        [HttpPost]
        public JsonResult ShowHideUngp(long caseProfileId, bool isPublished)
        {
            try
            {
                if (caseProfileId != 0)
                {
                    var assessmentForm = _gesUngpAssessmentFormRepository.GetGesUngpAssessmentFormByCaseProfileId(caseProfileId);

                    if (assessmentForm != null)
                    {
                        assessmentForm.IsPublished = isPublished;
                        assessmentForm.ModifiedBy = _gesUserService.GetById(User.Identity.GetUserId()).Id;
                        assessmentForm.Modified = DateTime.UtcNow;
                        _gesUngpAssessmentFormRepository.Edit(assessmentForm);
                        _gesUngpAssessmentFormRepository.Save();
                    }
                }
            }
            catch (Exception e)
            {
                return Json(new { Status = "Failed updating CaseReport Ges UNGP Assessment. Caused:" + e.Message });
            }

            return Json(new { Status = "Success" });
        }

        private bool UpdateGesUngpAssessmentForm(long caseProfileId,
            CaseReportGesUngpAssessmentFormViewModel assessmentFormViewModel)
        {

            try
            {
                GesUNGPAssessmentForm assessmentForm;

                if (assessmentFormViewModel.GesUNGPAssessmentFormId == Guid.Empty)
                {
                    assessmentForm = new GesUNGPAssessmentForm()
                    {
                        GesUNGPAssessmentFormId = Guid.NewGuid(),
                        I_GesCaseReports_Id = caseProfileId,
                        Created = DateTime.UtcNow

                    };
                }
                else
                {
                    assessmentForm =
                        _gesUngpAssessmentFormRepository.GetGesUngpAssessmentFormByCaseProfileId(caseProfileId);
                }

                assessmentForm.TheExtentOfHarmesScoreId = assessmentFormViewModel.TheExtentOfHarmesScoreId;
                assessmentForm.TheExtentOfHarmesScoreComment = assessmentFormViewModel.TheExtentOfHarmesScoreComment;
                assessmentForm.TheNumberOfPeopleAffectedScoreId = assessmentFormViewModel.TheNumberOfPeopleAffectedScoreId;
                assessmentForm.TheNumberOfPeopleAffectedScoreComment = assessmentFormViewModel.TheNumberOfPeopleAffectedScoreComment;
                assessmentForm.OverSeveralYearsScoreId = assessmentFormViewModel.OverSeveralYearsScoreId;
                assessmentForm.OverSeveralYearsScoreComment = assessmentFormViewModel.OverSeveralYearsScoreComment;
                assessmentForm.SeveralLocationsScoreId = assessmentFormViewModel.SeveralLocationsScoreId;
                assessmentForm.SeveralLocationsScoreComment = assessmentFormViewModel.SeveralLocationsScoreComment;
                assessmentForm.IsViolationScoreId = assessmentFormViewModel.IsViolationScoreId;
                assessmentForm.IsViolationScoreComment = assessmentFormViewModel.IsViolationScoreComment;
                assessmentForm.GesConfirmedViolationScoreId = assessmentFormViewModel.GesConfirmedViolationScoreId;
                assessmentForm.GesConfirmedViolationScoreComment = assessmentFormViewModel.GesConfirmedViolationScoreComment;
                assessmentForm.SalientHumanRightsPotentialViolationTotalScore = assessmentFormViewModel.SalientHumanRightsPotentialViolationTotalScore;
                assessmentForm.GesCommentSalientHumanRight = assessmentFormViewModel.GesCommentSalientHumanRight;
                assessmentForm.HumanRightsPolicyPubliclyDisclosedAddScoreId = assessmentFormViewModel.HumanRightsPolicyPubliclyDisclosedAddScoreId;
                assessmentForm.HumanRightsPolicyPubliclyDisclosed = assessmentFormViewModel.HumanRightsPolicyPubliclyDisclosed;
                assessmentForm.HumanRightsPolicyCommunicatedScoreId = assessmentFormViewModel.HumanRightsPolicyCommunicatedScoreId;
                assessmentForm.HumanRightsPolicyCommunicated = assessmentFormViewModel.HumanRightsPolicyCommunicated;
                assessmentForm.HumanRightsPolicyStipulatesScoreId = assessmentFormViewModel.HumanRightsPolicyStipulatesScoreId;
                assessmentForm.HumanRightsPolicyStipulates = assessmentFormViewModel.HumanRightsPolicyStipulates;
                assessmentForm.HumanRightsPolicyApprovedScoreId = assessmentFormViewModel.HumanRightsPolicyApprovedScoreId;
                assessmentForm.HumanRightsPolicyApproved = assessmentFormViewModel.HumanRightsPolicyApproved;
                assessmentForm.HumanRightsPolicyTotalScore = assessmentFormViewModel.HumanRightsPolicyTotalScore;
                assessmentForm.GovernanceCommitmentScoreId = assessmentFormViewModel.GovernanceCommitmentScoreId;
                assessmentForm.GovernanceCommitment = assessmentFormViewModel.GovernanceCommitment;
                assessmentForm.GovernanceExamplesScoreId = assessmentFormViewModel.GovernanceExamplesScoreId;
                assessmentForm.GovernanceExamples = assessmentFormViewModel.GovernanceExamples;
                assessmentForm.GovernanceClearDivisionScoreId = assessmentFormViewModel.GovernanceClearDivisionScoreId;
                assessmentForm.GovernanceClearDivision = assessmentFormViewModel.GovernanceClearDivision;
                assessmentForm.BusinessPartners = assessmentFormViewModel.BusinessPartners;
                assessmentForm.BusinessPartnersAddScoreId = assessmentFormViewModel.BusinessPartnersAddScoreId;
                assessmentForm.IdentificationAndCommitmentScoreId = assessmentFormViewModel.IdentificationAndCommitmentScoreId;
                assessmentForm.IdentificationAndCommitment = assessmentFormViewModel.IdentificationAndCommitment;
                assessmentForm.StakeholderEngagement = assessmentFormViewModel.StakeholderEngagement;
                assessmentForm.StakeholderEngagementAddScoreId = assessmentFormViewModel.StakeholderEngagementAddScoreId;
                assessmentForm.HumanRightsTraining = assessmentFormViewModel.HumanRightsTraining;
                assessmentForm.HumanRightsTrainingScoreId = assessmentFormViewModel.HumanRightsTrainingScoreId;
                assessmentForm.TotalScoreForHumanRightsDueDiligence = assessmentFormViewModel.TotalScoreForHumanRightsDueDiligence;
                assessmentForm.RemedyProcessInPlaceScoreId = assessmentFormViewModel.RemedyProcessInPlaceScoreId;
                assessmentForm.RemedyProcessInPlace = assessmentFormViewModel.RemedyProcessInPlace;
                assessmentForm.GrievanceMechanismHasOperationalLevelScoreId = assessmentFormViewModel.GrievanceMechanismHasOperationalLevelScoreId;
                assessmentForm.GrievanceMechanismHasOperationalLevel = assessmentFormViewModel.GrievanceMechanismHasOperationalLevel;
                assessmentForm.GrievanceMechanismExistenceOfOperationalLevelScoreId = assessmentFormViewModel.GrievanceMechanismExistenceOfOperationalLevelScoreId;
                assessmentForm.GrievanceMechanismExistenceOfOperationalLevel = assessmentFormViewModel.GrievanceMechanismExistenceOfOperationalLevel;
                assessmentForm.GrievanceMechanismClearProcessScoreId = assessmentFormViewModel.GrievanceMechanismClearProcessScoreId;
                assessmentForm.GrievanceMechanismClearProcess = assessmentFormViewModel.GrievanceMechanismClearProcess;
                assessmentForm.GrievanceMechanismRightsNormsScoreId = assessmentFormViewModel.GrievanceMechanismRightsNormsScoreId;
                assessmentForm.GrievanceMechanismRightsNorms = assessmentFormViewModel.GrievanceMechanismRightsNorms;
                assessmentForm.GrievanceMechanismFilingGrievanceScoreId = assessmentFormViewModel.GrievanceMechanismFilingGrievanceScoreId;
                assessmentForm.GrievanceMechanismFilingGrievance = assessmentFormViewModel.GrievanceMechanismFilingGrievance;
                assessmentForm.GrievanceMechanismReoccurringGrievancesScoreId = assessmentFormViewModel.GrievanceMechanismReoccurringGrievancesScoreId;
                assessmentForm.GrievanceMechanismReoccurringGrievances = assessmentFormViewModel.GrievanceMechanismReoccurringGrievances;
                assessmentForm.GrievanceMechanismFormatAndProcesseScoreId = assessmentFormViewModel.GrievanceMechanismFormatAndProcesseScoreId;
                assessmentForm.GrievanceMechanismFormatAndProcesse = assessmentFormViewModel.GrievanceMechanismFormatAndProcesse;
                assessmentForm.TotalScoreForRemediationOfAdverseHumanRightsImpacts = assessmentFormViewModel.TotalScoreForRemediationOfAdverseHumanRightsImpacts;
                assessmentForm.GesCommentCompanyPreparedness = assessmentFormViewModel.GesCommentCompanyPreparedness;
                assessmentForm.TotalScoreForCompanyPreparedness = assessmentFormViewModel.TotalScoreForCompanyPreparedness;
                assessmentForm.IsPublished = assessmentFormViewModel.IsPublished;
                assessmentForm.ModifiedBy = _gesUserService.GetById(User.Identity.GetUserId()).Id;

                assessmentForm.Modified = DateTime.UtcNow;

                if (assessmentFormViewModel.GesUNGPAssessmentFormId == Guid.Empty)
                {
                    _gesUngpAssessmentFormRepository.Add(assessmentForm);
                }
                else
                {
                    _gesUngpAssessmentFormRepository.Edit(assessmentForm);
                }

                _assessmentScoresRepository.Save();

                UpdateGesUngpAssessmentFormResources((List<GesUNGPAssessmentFormResourcesViewModel>)assessmentFormViewModel.AssessmentFormResourcesViewModels,
                    assessmentForm.GesUNGPAssessmentFormId);

            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        private bool UpdateGesUngpAssessmentFormResources(IReadOnlyCollection<GesUNGPAssessmentFormResourcesViewModel> newResourcesList, Guid gesUngpAssessmentFormId)
        {
            var oldResourcesList = _gesUngpAssessmentFormResourcesRepository.GetGesUNGPAssessmentFormResourcesByFormId(gesUngpAssessmentFormId).ToList();

            if (newResourcesList == null)
            {
                //Remove all news
                foreach (var deleteItem in oldResourcesList)
                {
                    _gesUngpAssessmentFormResourcesRepository.Delete(deleteItem);
                }
                _gesUngpAssessmentFormResourcesRepository.Save();
                return true;
            }

            var listResourceIds = newResourcesList.ToList().Select(d => d.GesUNGPAssessmentFormResourcesId);

            try
            {
                //Delete Item
                var listDelete = oldResourcesList.Where(d => !listResourceIds.Contains(d.GesUNGPAssessmentFormResourcesId));

                foreach (var deleteItem in listDelete)
                {
                    _gesUngpAssessmentFormResourcesRepository.Delete(deleteItem);
                }

                //Add newItem
                foreach (var sourceItem in newResourcesList)
                {
                    if (sourceItem.GesUNGPAssessmentFormResourcesId == Guid.Empty)
                    {
                        _gesUngpAssessmentFormResourcesRepository.Add(new GesUNGPAssessmentFormResources()
                        {
                            GesUNGPAssessmentFormResourcesId = Guid.NewGuid(),
                            GesUNGPAssessmentFormId = gesUngpAssessmentFormId,
                            SourcesName = sourceItem.SourcesName,
                            SourcesLink = sourceItem.SourcesLink,
                            SourceDate = sourceItem.SourceDate,
                            Created = DateTime.UtcNow,
                            //ModifiedBy = _gesUserService.GetById(User.Identity.GetUserId()).OldUserId;

                        });
                    }
                    else
                    {
                        var updateResource = _gesUngpAssessmentFormResourcesRepository.GetById(sourceItem.GesUNGPAssessmentFormResourcesId);
                        if (updateResource.SourcesName != null &&
                            updateResource.SourcesName.Equals(sourceItem.SourcesName) &&
                            updateResource.SourcesLink != null &&
                            updateResource.SourcesLink.Equals(sourceItem.SourcesLink) &&
                            updateResource.SourceDate != null &&
                            updateResource.SourceDate.Equals(sourceItem.SourceDate))
                        {
                            continue;
                        }



                        updateResource.SourcesName = sourceItem.SourcesName;
                        updateResource.SourcesLink = sourceItem.SourcesLink;
                        updateResource.SourceDate = sourceItem.SourceDate;
                        updateResource.Modified = DateTime.UtcNow;

                        _gesUngpAssessmentFormResourcesRepository.Edit(updateResource);
                    }
                }

                _gesUngpAssessmentFormResourcesRepository.Save();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        [HttpPost]
        public JsonResult SaveManagedDocument(DocumentViewModel document, HttpPostedFileBase file)
        {
            var result = SaveManagedDocument(document, file, RelatedDocumentMng.Company);
            if (result > 0)
            {
                return Json(new { Status = "Success", Id = result });
            }
            else
            {
                return Json(new { Status = "Failed", Id = -1 });
            }
        }

        [HttpPost]
        public JsonResult SaveCompanyDialogAttachmentFile(DocumentViewModel document, HttpPostedFileBase file, string dialogType)
        {
            var relatedType = dialogType == "Source"
                ? RelatedDocumentMng.SourceDialog
                : RelatedDocumentMng.CompanyDialog;

            var result = SaveManagedDocument(document, file, relatedType);

            if (result > 0)
            {
                return Json(new {Status = "Success", Id= result });
            }
            else
            {
                return Json(new { Status = "Failed", Ïd = -1 });
            }
        }

        [HttpPost]
        public JsonResult DeleteManagedDocument(long documentId)
        {
            if (_documentService.TryDeleteManagedDocument(documentId))
                return Json(new { Status = "Success" });
            return Json(new { Status = "Failed" });
        }

        private long SaveManagedDocument(DocumentViewModel document, HttpPostedFileBase file, RelatedDocumentMng relatedType)
        {
            if (document.G_ManagedDocuments_Id == 0 || string.IsNullOrEmpty(document.DownloadUrl))
            {
                var newFileName = UploadFile(file);
                document.FileName = newFileName;
                if (string.IsNullOrEmpty(newFileName))
                    return -1;
            }
            return _documentService.TrySaveManagedDocument(document, relatedType);
        }

        private string UploadFile(HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
            {
                var fileExtension = Path.GetExtension(file.FileName);
                var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(file.FileName);

                var fileName = UtilHelper.CreateSafeFileName(fileNameWithoutExtension, fileExtension);

                var folderPath = SiteSettings.FilePathOnOldSystem;

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                var path = Path.Combine(folderPath, fileName);
                file.SaveAs(path);
                return fileName;
            }
            return string.Empty;
        }

        private bool UpdateCommentaries(IReadOnlyCollection<GesCommentaryViewModel> newCommentaries, long gesCaseReportsId)
        {
            var oldCommentary = _commentaryRepository.GetGesCommentariesByCaseProfileId(gesCaseReportsId).ToList();

            if (newCommentaries == null)
            {
                //Remove all news
                foreach (var deleteItem in oldCommentary)
                {
                    _commentaryRepository.Delete(deleteItem);
                }
                _commentaryRepository.Save();

                _commentaryRepository.Save();

                return true;
            }

            var listCommentaryIds = newCommentaries.ToList().Select(d => d.I_GesCommentary_Id);

            try
            {
                //Delete Item
                var listDelete = oldCommentary.Where(d => !listCommentaryIds.Contains(d.I_GesCommentary_Id));

                foreach (var deleteItem in listDelete)
                {
                    _commentaryRepository.Delete(deleteItem);
                }

                //Add newItem
                foreach (var newCommentaryItem in newCommentaries)
                {
                    if (newCommentaryItem.I_GesCommentary_Id == 0)
                    {
                        _commentaryRepository.Add(new I_GesCommentary()
                        {
                            I_GesCaseReports_Id = gesCaseReportsId,
                            Description = newCommentaryItem.Description,
                            Created = DateTime.UtcNow
                        });
                    }
                    else
                    {
                        var updateCommentary = _commentaryRepository.GetById(newCommentaryItem.I_GesCommentary_Id);
                        if (updateCommentary.Description != null && updateCommentary.Description.Equals(newCommentaryItem.Description)) continue;

                        updateCommentary.Description = newCommentaryItem.Description;
                        updateCommentary.CommentaryModified = DateTime.UtcNow;
                        _commentaryRepository.Edit(updateCommentary);
                    }
                }

                _commentaryRepository.Save();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        private bool UpdateLatestNews(IReadOnlyCollection<GesLatestNewsViewModel> newLatestNewsList,
            long gesCaseReportsId)
        {
            var oldLatestNewsList = _gesLatestNewsRepository.GetGesLatestNewsByCaseProfileId(gesCaseReportsId).ToList();

            if (newLatestNewsList == null)
            {
                //Remove all news
                foreach (var deleteItem in oldLatestNewsList)
                {
                    _gesLatestNewsRepository.Delete(deleteItem);
                }
                _gesLatestNewsRepository.Save();
                return true;
            }

            var listLatestNewsIds = newLatestNewsList.ToList().Select(d => d.I_GesLatestNews_Id);

            try
            {
                //Delete Item
                var listDelete = oldLatestNewsList.Where(d => !listLatestNewsIds.Contains(d.I_GesLatestNews_Id));

                foreach (var deleteItem in listDelete)
                {
                    _gesLatestNewsRepository.Delete(deleteItem);
                }

                //Add newItem
                foreach (var newLatestNewsItem in newLatestNewsList)
                {
                    if (newLatestNewsItem.I_GesLatestNews_Id == 0)
                    {
                        _gesLatestNewsRepository.Add(new I_GesLatestNews()
                        {
                            I_GesCaseReports_Id = gesCaseReportsId,
                            Description = newLatestNewsItem.Description,
                            Created = DateTime.UtcNow
                        });
                    }
                    else
                    {
                        var updateLatestNews = _gesLatestNewsRepository.GetById(newLatestNewsItem.I_GesLatestNews_Id);
                        if (updateLatestNews.Description != null &&
                            updateLatestNews.Description.Equals(newLatestNewsItem.Description)) continue;

                        updateLatestNews.Description = newLatestNewsItem.Description;
                        updateLatestNews.LatestNewsModified = DateTime.UtcNow;

                        _gesLatestNewsRepository.Edit(updateLatestNews);
                    }
                }

                _gesLatestNewsRepository.Save();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        [HttpPost]
        public JsonResult SaveCompanyDialogue(DialogueEditModel dialogue)
        {
            I_GesCompanyDialogues m_CompanyDialogue = null;
            try
            {
                if (dialogue.I_GesCompanySourceDialogues_Id == 0)
                {
                    m_CompanyDialogue = new I_GesCompanyDialogues()
                    {
                        I_GesCaseReports_Id = dialogue.I_GesCaseReports_Id,
                        Action = dialogue.Action,
                        ClassA = dialogue.ClassA,
                        ContactDate = dialogue.ContactDate != null ? dialogue.ContactDate.Value.Date : (DateTime?)null,
                        G_Individuals_Id = dialogue.G_Individuals_Id,
                        //G_ManagedDocuments_Id = dialogue.G_ManagedDocuments_Id,
                        I_ContactDirections_Id = dialogue.ContactDirectionId,
                        I_ContactTypes_Id = dialogue.ContactTypeId,
                        Notes = dialogue.Notes,
                        //SendNotifications = dialogue.SendNotifications,
                        ShowInCsc = dialogue.ShowInCsc,
                        ShowInReport = dialogue.ShowInReport,
                        Text = dialogue.Text
                    };
                    _gesCompanyDialogueService.Add(m_CompanyDialogue);
                }
                else
                {
                    m_CompanyDialogue = _gesCompanyDialogueService.GetById(dialogue.I_GesCompanySourceDialogues_Id);

                    if (m_CompanyDialogue != null)
                    {
                        m_CompanyDialogue.Action = dialogue.Action;
                        m_CompanyDialogue.ClassA = dialogue.ClassA;
                        m_CompanyDialogue.ContactDate = dialogue.ContactDate != null ? dialogue.ContactDate.Value.Date : (DateTime?)null;                        
                        m_CompanyDialogue.G_Individuals_Id = dialogue.G_Individuals_Id;
                        //m_CompanyDialogue.G_ManagedDocuments_Id = dialogue.G_ManagedDocuments_Id;
                        m_CompanyDialogue.I_ContactDirections_Id = dialogue.ContactDirectionId;
                        m_CompanyDialogue.I_ContactTypes_Id = dialogue.ContactTypeId;
                        m_CompanyDialogue.Notes = dialogue.Notes;
                        //m_CompanyDialogue.SendNotifications = dialogue.SendNotifications;
                        m_CompanyDialogue.ShowInCsc = dialogue.ShowInCsc;
                        m_CompanyDialogue.ShowInReport = dialogue.ShowInReport;
                        m_CompanyDialogue.Text = dialogue.Text;

                        _gesCompanyDialogueService.Update(m_CompanyDialogue);
                    }
                }
                _gesCompanyDialogueService.Save();
            }
            catch (Exception)
            {
                return Json(new { Status = "Error" });
            }

            return Json(new { Status = "Success", Id = m_CompanyDialogue.I_GesCompanyDialogues_Id });
        }

        [HttpPost]
        public JsonResult DeleteCompanyDialogue(DialogueEditModel dialogue)
        {
            try
            {
                var m_CompanyDialogue = _gesCompanyDialogueService.GetById(dialogue.I_GesCompanySourceDialogues_Id);

                if (m_CompanyDialogue != null)
                {
                    _gesCompanyDialogueService.Delete(m_CompanyDialogue);
                    _gesCompanyDialogueService.Save();
                }
            }
            catch
            {
                return Json(new { Status = "Error" });
            }
            return Json(new { Status = "Success" });
        }

        [HttpPost]
        public JsonResult SaveSourceDialogue(DialogueEditModel dialogue)
        {
            I_GesSourceDialogues m_SourceDialogue = null;
            try
            {
                if (dialogue.I_GesCompanySourceDialogues_Id == 0)
                {
                    m_SourceDialogue = new I_GesSourceDialogues()
                    {
                        I_GesCaseReports_Id = dialogue.I_GesCaseReports_Id,
                        Action = dialogue.Action,
                        ClassA = dialogue.ClassA,
                        ContactDate = dialogue.ContactDate != null ? dialogue.ContactDate.Value.Date : (DateTime?)null,
                        G_Individuals_Id = dialogue.G_Individuals_Id,
                        //G_ManagedDocuments_Id = dialogue.G_ManagedDocuments_Id,
                        I_ContactDirections_Id = dialogue.ContactDirectionId,
                        I_ContactTypes_Id = dialogue.ContactTypeId,
                        Notes = dialogue.Notes,
                        ShowInCsc = dialogue.ShowInCsc,
                        ShowInReport = dialogue.ShowInReport,
                        Text = dialogue.Text
                    };
                    _gesSourceDialogueService.Add(m_SourceDialogue);
                }
                else
                {
                    m_SourceDialogue = _gesSourceDialogueService.GetById(dialogue.I_GesCompanySourceDialogues_Id);

                    if (m_SourceDialogue != null)
                    {
                        m_SourceDialogue.Action = dialogue.Action;
                        m_SourceDialogue.ClassA = dialogue.ClassA;
                        m_SourceDialogue.ContactDate = dialogue.ContactDate != null ? dialogue.ContactDate.Value.Date : (DateTime?)null;
                        m_SourceDialogue.G_Individuals_Id = dialogue.G_Individuals_Id;
                        //m_SourceDialogue.G_ManagedDocuments_Id = dialogue.G_ManagedDocuments_Id;
                        m_SourceDialogue.I_ContactDirections_Id = dialogue.ContactDirectionId;
                        m_SourceDialogue.I_ContactTypes_Id = dialogue.ContactTypeId;
                        m_SourceDialogue.Notes = dialogue.Notes;
                        m_SourceDialogue.ShowInCsc = dialogue.ShowInCsc;
                        m_SourceDialogue.ShowInReport = dialogue.ShowInReport;
                        m_SourceDialogue.Text = dialogue.Text;
                        _gesSourceDialogueService.Update(m_SourceDialogue);
                    }
                }
                _gesSourceDialogueService.Save();
            }
            catch (Exception)
            {
                return Json(new { Status = "Error" });
            }

            return Json(new { Status = "Success", Id = m_SourceDialogue.I_GesSourceDialogues_Id });
        }

        [HttpPost]
        public JsonResult DeleteSourceDialogue(DialogueEditModel dialogue)
        {
            try
            {
                var m_SourceDialogue = _gesSourceDialogueService.GetById(dialogue.I_GesCompanySourceDialogues_Id);

                if (m_SourceDialogue != null)
                {
                    _gesSourceDialogueService.Delete(m_SourceDialogue);
                    _gesSourceDialogueService.Save();
                }
            }
            catch
            {
                return Json(new { Status = "Error" });
            }
            return Json(new { Status = "Success" });
        }

        private bool UpdateConventions(List<ConventionsViewModel> newConventions, long gesCaseReportsId)
        {
            var oldConventions = _caseReportsIConventionsRepository.GetGesCaseReportsConventionsByCaseReportId(gesCaseReportsId).ToList();

            if (newConventions == null)
            {
                //Remove all news
                foreach (var deleteItem in oldConventions)
                {
                    _caseReportsIConventionsRepository.Delete(deleteItem);
                }
                _caseReportsIConventionsRepository.Save();
                return true;
            }

            var listConventionsIds = newConventions.ToList().Select(d => d.I_GesCaseReportsI_Conventions_Id);

            try
            {
                //Delete Item
                var listDelete = oldConventions.Where(d => !listConventionsIds.Contains(d.I_GesCaseReportsI_Conventions_Id));

                foreach (var deleteItem in listDelete)
                {
                    _caseReportsIConventionsRepository.Delete(deleteItem);
                }

                //Add newItem
                foreach (var newConventionItem in newConventions)
                {
                    if (newConventionItem.I_GesCaseReportsI_Conventions_Id == 0)
                    {
                        _caseReportsIConventionsRepository.Add(new I_GesCaseReportsI_Conventions()
                        {
                            I_GesCaseReports_Id = gesCaseReportsId,
                            I_Conventions_Id = newConventionItem.I_Conventions_Id
                        });
                    }
                    else
                    {
                        var updateConventionItem = _caseReportsIConventionsRepository.GetById(newConventionItem.I_GesCaseReportsI_Conventions_Id);
                        if (updateConventionItem.I_Conventions_Id.Equals(newConventionItem.I_Conventions_Id)) continue;

                        updateConventionItem.I_Conventions_Id = newConventionItem.I_Conventions_Id;

                        _caseReportsIConventionsRepository.Edit(updateConventionItem);
                    }
                }

                _caseReportsIConventionsRepository.Save();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        private bool UpdateNorms(List<NormViewModel> newNorms, long gesCaseReportsId)
        {
            var oldNorms = _caseReportsINormsRepository.GetGesCaseReportsNormByCaseReportId(gesCaseReportsId).ToList();

            if (newNorms == null)
            {
                //Remove all news
                foreach (var deleteItem in oldNorms)
                {
                    _caseReportsINormsRepository.Delete(deleteItem);
                }
                _caseReportsINormsRepository.Save();
                return true;
            }

            var listNormsIds = newNorms.ToList().Select(d => d.I_GesCaseReportsI_Norms_Id);

            try
            {
                //Delete Item
                var listDelete = oldNorms.Where(d => !listNormsIds.Contains(d.I_GesCaseReportsI_Norms_Id));

                foreach (var deleteItem in listDelete)
                {
                    _caseReportsINormsRepository.Delete(deleteItem);
                }

                //Add newItem
                foreach (var newNormItem in newNorms)
                {
                    if (newNormItem.I_GesCaseReportsI_Norms_Id == 0)
                    {
                        _caseReportsINormsRepository.Add(new I_GesCaseReportsI_Norms()
                        {
                            I_GesCaseReports_Id = gesCaseReportsId,
                            I_Norms_Id = newNormItem.I_Norms_Id
                        });
                    }
                    else
                    {
                        var updateNormItem = _caseReportsINormsRepository.GetById(newNormItem.I_GesCaseReportsI_Norms_Id);
                        if (updateNormItem.I_Norms_Id.Equals(newNormItem.I_Norms_Id)) continue;

                        updateNormItem.I_Norms_Id = newNormItem.I_Norms_Id;

                        _caseReportsINormsRepository.Edit(updateNormItem);
                    }
                }

                _caseReportsINormsRepository.Save();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        #endregion

        #region JsonResult
        [HttpPost]
        public JsonResult GetDataForSignedUpCaseProfileListJqGrid(JqGridViewModel jqGridParams)
        {
            var listClient = this.SafeExecute(() => _caseReportSignupService.GetCaseReportSignUpList(jqGridParams), "Error when get the case report {@JqGridViewModel}", jqGridParams);

            return Json(listClient, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetDataForSignedUpCaseProfileUserListJqGrid(JqGridViewModel jqGridParams, long id)
        {
            var listClient = this.SafeExecute(() => _caseReportSignupService.GetCaseReportSignUpUserList(jqGridParams, id), "Error when get case report {@JqGridViewModel}", jqGridParams);

            return Json(listClient);
        }

        public JsonResult GenerateCaseProfileKeywords(int limit = 1000) // by default, only proceed max 1000 items each run
        {
            int counter;
            try
            {
                counter = _caseProfilesService.ExtractKeywordsForAllCaseProfiles(limit);
            }
            catch (Exception e)
            {
                return Json(new
                {
                    success = false,
                    error = e.Message
                }, JsonRequestBehavior.AllowGet);
            }

            return Json(new
            {
                success = true,
                count = counter
            }, JsonRequestBehavior.AllowGet);
        }

        [HandleJsonException]
        public ActionResult ExportEndorsements()
        {
            var result = _caseReportSignupService.ExportEndorsements();

            var templatePath = Server.MapPath(ExcelTemplates.Endorsement);
            var template = System.IO.File.ReadAllBytes(templatePath);
            var filename = string.Format(ExcelTemplates.EndorsementPrefix + "{0}.xlsx", DateTime.Now.ToString("yyyyMMddHHmmss"));

            using (var ms = new MemoryStream(template))
            {
                ms.Position = 0;
                HttpContext.Response.ClearContent();
                HttpContext.Response.AddHeader("Content-Disposition", "attachment; filename=" + filename);
                HttpContext.Response.ContentType = "application/octet-stream";
                using (var document = MvcApplication.TemplaterFactory.Open(ms, HttpContext.Response.OutputStream, "xlsx"))
                {
                    document.Process(new { Endorsement = result });
                }
                HttpContext.Response.End();
            }
            return null;
        }

        [HttpPost]
        public JsonResult GetDataForCasesJqGrid(JqGridViewModel jqGridParams, long? companyId = null)
        {
            var caseProfiles = this.SafeExecute(() => _caseProfilesService.GetCaseProfiles(jqGridParams, companyId), "Error when getting the cases {@JqGridViewModel}", jqGridParams);

            return Json(caseProfiles);
        }

        [HttpGet]
        public JsonResult CheckExistHeading(long issueHeadingId,long caseprofileId,long? companyId = null)
        {
            var caseProfiles = _gesCaseProfilesRepository.GetCaseProfiles(companyId);
            if (caseProfiles != null && caseProfiles.Any()){
                var rtVal = Json(caseProfiles.Where(x => x.IssueHeadingId == issueHeadingId && x.Id != caseprofileId), JsonRequestBehavior.AllowGet);
                return rtVal;
            }
            return null;
        }

        [HttpPost]
        public JsonResult SaveDiscussionPoint(DiscussionPointsViewModel discussionPoint)
        {
            I_EngagementDiscussionPoints m_DiscussionPoint = null;
            try
            {
                if (discussionPoint.DiscussionPointsId == 0)
                {
                    m_DiscussionPoint = new I_EngagementDiscussionPoints()
                    {
                        Name = discussionPoint.Name,
                        Description = discussionPoint.Description,
                        I_Companies_Id = discussionPoint.CompanyId,
                        Created = DateTime.Now
                    };
                    _engagementDiscussionPointsRepository.Add(m_DiscussionPoint);

                }
                else
                {
                    m_DiscussionPoint = _engagementDiscussionPointsRepository.GetById(discussionPoint.DiscussionPointsId);

                    if (m_DiscussionPoint != null)
                    {
                        m_DiscussionPoint.Name = discussionPoint.Name;
                        m_DiscussionPoint.Description = discussionPoint.Description;

                        _engagementDiscussionPointsRepository.Edit(m_DiscussionPoint);
                    }
                }
                _engagementDiscussionPointsRepository.Save();
            }
            catch
            {
                return Json(new { Status = "Error" });
            }

            return Json(new { Status = "Success", Id = m_DiscussionPoint.I_EngagementDiscussionPoints_Id });
        }

        [HttpPost]
        public JsonResult DeleteDiscussionPoint(DiscussionPointsViewModel discussionPoint)
        {
            try
            {
                var m_discussionPoint = _engagementDiscussionPointsRepository.GetById(discussionPoint.DiscussionPointsId);

                if (m_discussionPoint != null)
                {
                    _engagementDiscussionPointsRepository.Delete(m_discussionPoint);

                    _engagementDiscussionPointsRepository.Save();
                }
            }
            catch
            {
                return Json(new { Status = "Error" });
            }
            return Json(new { Status = "Success" });
        }


        [HttpPost]
        public JsonResult SaveStakeholderView(OtherStakeholderViewModel stakeholderView)
        {
            I_EngagementOtherStakeholderViews m_OtherStakeholderView;
            try
            {
                if (stakeholderView.OtherStakeholderViewsId == 0)
                {
                    m_OtherStakeholderView = new I_EngagementOtherStakeholderViews()
                    {
                        Name = stakeholderView.Name,
                        Description = stakeholderView.Description,
                        I_Companies_Id = stakeholderView.CompanyId,
                        Url = stakeholderView.Url,
                        Created = DateTime.Now
                    };

                    _engagementOtherStakeholderViewsRepository.Add(m_OtherStakeholderView);
                }
                else
                {
                    m_OtherStakeholderView = _engagementOtherStakeholderViewsRepository.GetById(stakeholderView.OtherStakeholderViewsId);

                    if (m_OtherStakeholderView != null)
                    {
                        m_OtherStakeholderView.Name = stakeholderView.Name;
                        m_OtherStakeholderView.Description = stakeholderView.Description;
                        m_OtherStakeholderView.Url = stakeholderView.Url;

                        _engagementOtherStakeholderViewsRepository.Edit(m_OtherStakeholderView);
                    }
                }
                _engagementOtherStakeholderViewsRepository.Save();
            }
            catch
            {
                return Json(new { Status = "Error" });
            }

            return Json(new { Status = "Success", Id = m_OtherStakeholderView.I_EngagementOtherStakeholderViews_Id });
        }

        [HttpPost]
        public JsonResult DeleteStakeholderView(OtherStakeholderViewModel stakeholderView)
        {
            try
            {
                var m_stakeholderView = _engagementOtherStakeholderViewsRepository.GetById(stakeholderView.OtherStakeholderViewsId);

                if (m_stakeholderView != null)
                {
                    _engagementOtherStakeholderViewsRepository.Delete(m_stakeholderView);

                    _engagementOtherStakeholderViewsRepository.Save();
                }
            }
            catch
            {
                return Json(new { Status = "Error" });
            }
            return Json(new { Status = "Success" });
        }

        [HttpPost]
        public JsonResult SaveCommentary(GesCommentaryViewModel commentary)
        {
            I_GesCommentary m_Commentary = null;
            try
            {
                if (commentary.I_GesCommentary_Id == 0)
                {
                    m_Commentary = new I_GesCommentary()
                    {
                        Description = commentary.Description,
                        I_GesCaseReports_Id = commentary.I_GesCaseReports_Id,
                        Created = DateTime.Now,
                        CommentaryModified = DateTime.Now
                    };
                    _commentaryRepository.Add(m_Commentary);

                }
                else
                {
                    m_Commentary = _commentaryRepository.GetById(commentary.I_GesCommentary_Id);

                    if (m_Commentary != null)
                    {
                        m_Commentary.Description = commentary.Description;
                        m_Commentary.CommentaryModified = DateTime.Now;

                        _commentaryRepository.Edit(m_Commentary);
                    }
                }
                _commentaryRepository.Save();
            }
            catch
            {
                return Json(new { Status = "Error" });
            }

            return Json(new { Status = "Success", Id = m_Commentary.I_GesCommentary_Id });
        }

        [HttpPost]
        public JsonResult DeleteCommentary(GesCommentaryViewModel commentary)
        {
            try
            {
                var m_Commentary = _commentaryRepository.GetById(commentary.I_GesCommentary_Id);

                if (m_Commentary != null)
                {
                    _commentaryRepository.Delete(m_Commentary);

                    _commentaryRepository.Save();
                }
            }
            catch
            {
                return Json(new { Status = "Error" });
            }
            return Json(new { Status = "Success" });
        }

        [HttpPost]
        public JsonResult SaveGSSLink(GSSLinkViewModel gsslink)
        {
            I_GSSLink m_GSSLink = null;
            try
            {
                if (gsslink.I_GSSLink_Id == Guid.Empty)
                {
                    m_GSSLink = new I_GSSLink()
                    {
                        I_GSSLink_Id = Guid.NewGuid(),
                        Description = gsslink.Description,
                        I_GesCaseReports_Id = gsslink.I_GesCaseReports_Id,
                        Created = DateTime.Now,
                        GSSLinkModified = DateTime.Now
                    };
                    _gsslinkRepository.Add(m_GSSLink);

                }
                else
                {
                    m_GSSLink = _gsslinkRepository.GetById(gsslink.I_GSSLink_Id);

                    if (m_GSSLink != null)
                    {
                        m_GSSLink.Description = gsslink.Description;
                        m_GSSLink.GSSLinkModified = DateTime.Now;

                        _gsslinkRepository.Edit(m_GSSLink);
                    }
                }
                _gsslinkRepository.Save();
            }
            catch
            {
                return Json(new { Status = "Error" });
            }

            return Json(new { Status = "Success", Id = m_GSSLink.I_GSSLink_Id });
        }

        [HttpPost]
        public JsonResult DeleteGSSLink(GSSLinkViewModel gsslink)
        {
            try
            {
                var m_GSSLink = _gsslinkRepository.GetById(gsslink.I_GSSLink_Id);

                if (m_GSSLink != null)
                {
                    _gsslinkRepository.Delete(m_GSSLink);

                    _gsslinkRepository.Save();
                }
            }
            catch
            {
                return Json(new { Status = "Error" });
            }
            return Json(new { Status = "Success" });
        }

        [HttpPost]
        public JsonResult SaveNews(GesLatestNewsViewModel news)
        {
            I_GesLatestNews m_News = null;
            try
            {
                if (news.I_GesLatestNews_Id == 0)
                {
                    m_News = new I_GesLatestNews()
                    {
                        Description = news.Description,
                        I_GesCaseReports_Id = news.I_GesCaseReports_Id,
                        Created = DateTime.Now,
                        LatestNewsModified = news.LatestNewsModified
                    };
                    _gesLatestNewsRepository.Add(m_News);

                }
                else
                {
                    m_News = _gesLatestNewsRepository.GetById(news.I_GesLatestNews_Id);

                    if (m_News != null)
                    {
                        m_News.Description = news.Description;
                        m_News.LatestNewsModified = news.LatestNewsModified;

                        _gesLatestNewsRepository.Edit(m_News);
                    }
                }
                _gesLatestNewsRepository.Save();
            }
            catch
            {
                return Json(new { Status = "Error" });
            }

            return Json(new { Status = "Success", Id = m_News.I_GesLatestNews_Id });
        }

        [HttpPost]
        public JsonResult DeleteNews(GesLatestNewsViewModel news)
        {
            try
            {
                var m_News = _gesLatestNewsRepository.GetById(news.I_GesLatestNews_Id);

                if (m_News != null)
                {
                    _gesLatestNewsRepository.Delete(m_News);

                    _gesLatestNewsRepository.Save();
                }
            }
            catch
            {
                return Json(new { Status = "Error" });
            }
            return Json(new { Status = "Success" });
        }

        [HttpPost]
        public JsonResult SaveSdgs(long I_GesCaseReports_Id, List<SdgViewModel> sdgs)
        {
            try
            {
                if (I_GesCaseReports_Id != 0)
                {
                    var SdgIds = sdgs?.Select(x => x.Sdg_Id).ToList();
                    _gesCaseReportSdgRepository.TryUpdateSdgsForCaseProfile(I_GesCaseReports_Id, SdgIds);
                }
            }
            catch
            {
                return Json(new { Status = "Error" });
            }
            return Json(new { Status = "Success" });

        }
        [HttpPost]
        public JsonResult RemoveSdg(long I_GesCaseReports_Id, List<SdgViewModel> sdgs)
        {
            try
            {
                if (I_GesCaseReports_Id != 0)
                {
                    var SdgIds = sdgs?.Select(x => x.Sdg_Id).ToList();
                    _gesCaseReportSdgRepository.TryUpdateSdgsForCaseProfile(I_GesCaseReports_Id, SdgIds);
                }
            }
            catch
            {
                return Json(new { Status = "Error" });
            }
            return Json(new { Status = "Success" });

        }

        [HttpPost]
        public JsonResult SaveGuideline(NormViewModel guideline)
        {

            I_GesCaseReportsI_Norms m_Guideline = null;
            try
            {
                if (guideline.I_GesCaseReportsI_Norms_Id == 0)
                {
                    m_Guideline = new I_GesCaseReportsI_Norms()
                    {
                        I_GesCaseReports_Id = guideline.I_GesCaseReports_Id,
                        I_Norms_Id = guideline.I_Norms_Id
                    };
                    _caseReportsINormsRepository.Add(m_Guideline);

                }
                else
                {
                    m_Guideline = _caseReportsINormsRepository.GetById(guideline.I_GesCaseReportsI_Norms_Id);

                    if (m_Guideline != null)
                    {
                        m_Guideline.I_Norms_Id = guideline.I_Norms_Id;
                        _caseReportsINormsRepository.Edit(m_Guideline);
                    }
                }
                _caseReportsINormsRepository.Save();
            }
            catch
            {
                return Json(new { Status = "Error" });
            }

            return Json(new { Status = "Success", Id = m_Guideline.I_GesCaseReportsI_Norms_Id });
        }

        [HttpPost]
        public JsonResult DeleteGuideline(NormViewModel guideline)
        {
            try
            {
                var m_Guideline = _caseReportsINormsRepository.GetById(guideline.I_GesCaseReportsI_Norms_Id);

                if (m_Guideline != null)
                {
                    _caseReportsINormsRepository.Delete(m_Guideline);

                    _caseReportsINormsRepository.Save();
                }
            }
            catch
            {
                return Json(new { Status = "Error" });
            }
            return Json(new { Status = "Success" });
        }

        [HttpPost]
        public JsonResult SaveConvention(ConventionsViewModel convention)
        {

            I_GesCaseReportsI_Conventions m_Convention = null;
            try
            {
                if (convention.I_GesCaseReportsI_Conventions_Id == 0)
                {
                    m_Convention = new I_GesCaseReportsI_Conventions()
                    {
                        I_GesCaseReports_Id = convention.I_GesCaseReports_Id,
                        I_Conventions_Id = convention.I_Conventions_Id
                    };
                    _caseReportsIConventionsRepository.Add(m_Convention);

                }
                else
                {
                    m_Convention = _caseReportsIConventionsRepository.GetById(convention.I_GesCaseReportsI_Conventions_Id);

                    if (m_Convention != null)
                    {
                        m_Convention.I_Conventions_Id = convention.I_Conventions_Id;
                        _caseReportsIConventionsRepository.Edit(m_Convention);
                    }
                }
                _caseReportsIConventionsRepository.Save();
            }
            catch
            {
                return Json(new { Status = "Error" });
            }

            return Json(new { Status = "Success", Id = m_Convention.I_GesCaseReportsI_Conventions_Id });
        }

        [HttpPost]
        public JsonResult DeleteConvention(ConventionsViewModel convention)
        {
            try
            {
                var m_Convention = _caseReportsIConventionsRepository.GetById(convention.I_GesCaseReportsI_Conventions_Id);

                if (m_Convention != null)
                {
                    _caseReportsIConventionsRepository.Delete(m_Convention);

                    _caseReportsIConventionsRepository.Save();
                }
            }
            catch
            {
                return Json(new { Status = "Error" });
            }
            return Json(new { Status = "Success" });
        }

        [HttpPost]
        public JsonResult SaveAssociatedCorporation(AssociatedCorporationsViewModel associated)
        {

            I_GesAssociatedCorporations m_Associated = null;
            try
            {
                if (associated.AssociatedCorporationId == 0)
                {
                    m_Associated = new I_GesAssociatedCorporations()
                    {
                        I_GesCaseReports_Id = associated.CaseReportId,
                        Name = associated.Name,
                        Ownership = associated.Ownership,
                        Comment = associated.Comment,
                        Traded = associated.Traded,
                        ShowInReport = associated.ShowInReport == true ? 1 : 0
                    };
                    _gesAssociatedCorporationsRepository.Add(m_Associated);

                }
                else
                {
                    m_Associated = _gesAssociatedCorporationsRepository.GetById(associated.AssociatedCorporationId);

                    if (m_Associated != null)
                    {
                        m_Associated.Name = associated.Name;
                        m_Associated.Ownership = associated.Ownership;
                        m_Associated.Comment = associated.Comment;
                        m_Associated.Traded = associated.Traded;
                        m_Associated.ShowInReport = associated.ShowInReport == true ? 1 : 0;
                        _gesAssociatedCorporationsRepository.Edit(m_Associated);
                    }
                }
                _gesAssociatedCorporationsRepository.Save();
            }
            catch
            {
                return Json(new { Status = "Error" });
            }

            return Json(new { Status = "Success", Id = m_Associated.I_GesAssociatedCorporations_Id });
        }

        [HttpPost]
        public JsonResult DeleteAssociatedCorporation(AssociatedCorporationsViewModel associated)
        {
            try
            {
                var m_Associated = _gesAssociatedCorporationsRepository.GetById(associated.AssociatedCorporationId);

                if (m_Associated != null)
                {
                    _gesAssociatedCorporationsRepository.Delete(m_Associated);

                    _gesAssociatedCorporationsRepository.Save();
                }
            }
            catch
            {
                return Json(new { Status = "Error" });
            }
            return Json(new { Status = "Success" });
        }

        [HttpPost]
        public JsonResult SaveSource(CaseReportSourceReferenceViewModel source)
        {
            short? s = null;
            I_GesCaseReportSources m_Source = null;
            try
            {
                if (source.Id == 0)
                {
                    m_Source = new I_GesCaseReportSources()
                    {
                        I_GesCaseReports_Id = source.CaseReportId,
                        Accessed = source.Accessed,
                        AvailableFrom = source.AvailableFrom,
                        G_ManagedDocuments_Id = source.ManagedDocumentId,
                        I_GesCaseReportAvailabilityStatuses_Id = source.Status,
                        MainSource = source.MainSource,
                        PublicationYear = source.PublicationYear != null ? Convert.ToInt16(source.PublicationYear) : s,
                        ShowInReport = source.ShowInReport,
                        Source = source.Source
                        //TODO: document
                    };
                    _gesCaseReportSourcesRepository.Add(m_Source);

                }
                else
                {
                    m_Source = _gesCaseReportSourcesRepository.GetById(source.Id);

                    if (m_Source != null)
                    {
                        m_Source.Accessed = source.Accessed;
                        m_Source.AvailableFrom = source.AvailableFrom;
                        m_Source.G_ManagedDocuments_Id = source.ManagedDocumentId;
                        m_Source.I_GesCaseReportAvailabilityStatuses_Id = source.Status;
                        m_Source.MainSource = source.MainSource;
                        m_Source.PublicationYear = source.PublicationYear != null ? Convert.ToInt16(source.PublicationYear) : s;
                        m_Source.ShowInReport = source.ShowInReport;
                        m_Source.Source = source.Source;
                        _gesCaseReportSourcesRepository.Edit(m_Source);
                    }
                }
                _gesCaseReportSourcesRepository.Save();
            }
            catch
            {
                return Json(new { Status = "Error" });
            }

            return Json(new { Status = "Success", Id = m_Source.I_GesCaseReportSources_Id });
        }

        [HttpPost]
        public JsonResult DeleteSource(CaseReportSourceReferenceViewModel source)
        {
            try
            {
                var m_Source = _gesCaseReportSourcesRepository.GetById(source.Id);

                if (m_Source != null)
                {
                    _gesCaseReportSourcesRepository.Delete(m_Source);

                    _gesCaseReportSourcesRepository.Save();
                }
            }
            catch
            {
                return Json(new { Status = "Error" });
            }
            return Json(new { Status = "Success" });
        }
        [HttpPost]
        public JsonResult SaveReference(CaseReportSourceReferenceViewModel reference)
        {
            short? s = null;
            I_GesCaseReportReferences m_Reference = null;
            try
            {
                if (reference.Id == 0)
                {
                    m_Reference = new I_GesCaseReportReferences()
                    {
                        I_GesCaseReports_Id = reference.CaseReportId,
                        Accessed = reference.Accessed.Value,
                        AvailableFrom = reference.AvailableFrom,
                        G_ManagedDocuments_Id = reference.ManagedDocumentId,
                        I_GesCaseReportAvailabilityStatuses_Id = reference.Status.Value,                        
                        PublicationYear = reference.PublicationYear != null ? Convert.ToInt16(reference.PublicationYear) : s,
                        ShowInReport = reference.ShowInReport,
                        Source = reference.Source
                        //TODO: document
                    };
                    _gesCaseReportReferencesRepository.Add(m_Reference);

                }
                else
                {
                    m_Reference = _gesCaseReportReferencesRepository.GetById(reference.Id);

                    if (m_Reference != null)
                    {
                        m_Reference.Accessed = reference.Accessed.Value;
                        m_Reference.AvailableFrom = reference.AvailableFrom;
                        m_Reference.G_ManagedDocuments_Id = reference.ManagedDocumentId;
                        m_Reference.I_GesCaseReportAvailabilityStatuses_Id = reference.Status.Value;
                        m_Reference.PublicationYear = reference.PublicationYear != null ? Convert.ToInt16(reference.PublicationYear) : s;
                        m_Reference.ShowInReport = reference.ShowInReport;
                        m_Reference.Source = reference.Source;
                        _gesCaseReportReferencesRepository.Edit(m_Reference);
                    }
                }
                _gesCaseReportReferencesRepository.Save();
            }
            catch
            {
                return Json(new { Status = "Error" });
            }

            return Json(new { Status = "Success", Id = m_Reference.I_GesCaseReportReferences_Id });
        }

        [HttpPost]
        public JsonResult DeleteReference(CaseReportSourceReferenceViewModel reference)
        {
            try
            {
                var m_Reference = _gesCaseReportReferencesRepository.GetById(reference.Id);

                if (m_Reference != null)
                {
                    _gesCaseReportReferencesRepository.Delete(m_Reference);

                    _gesCaseReportReferencesRepository.Save();
                }
            }
            catch
            {
                return Json(new { Status = "Error" });
            }
            return Json(new { Status = "Success" });
        }

        [HttpPost]
        public JsonResult SaveSupplementaryReading(CaseReportSupplementaryViewModel supplementary)
        {
            short? s = null;
            I_GesCaseReportSupplementaryReading m_Supplementary = null;
            try
            {
                if (supplementary.Id == 0)
                {
                    m_Supplementary = new I_GesCaseReportSupplementaryReading()
                    {
                        I_GesCaseReports_Id = supplementary.CaseReportId,
                        Accessed = supplementary.Accessed.Value,
                        AvailableFrom = supplementary.AvailableFrom,
                        G_ManagedDocuments_Id = supplementary.ManagedDocumentId,
                        I_GesCaseReportAvailabilityStatuses_Id = supplementary.Status.Value,
                        PublicationYear = supplementary.PublicationYear != null ? Convert.ToInt16(supplementary.PublicationYear) : s,
                        ShowInReport = supplementary.ShowInReport,
                        Source = supplementary.Source
                        //TODO: document
                    };
                    _gesCaseReportSupplementaryReadingRepository.Add(m_Supplementary);

                }
                else
                {
                    m_Supplementary = _gesCaseReportSupplementaryReadingRepository.GetById(supplementary.Id);

                    if (m_Supplementary != null)
                    {
                        m_Supplementary.Accessed = supplementary.Accessed.Value;
                        m_Supplementary.AvailableFrom = supplementary.AvailableFrom;
                        m_Supplementary.G_ManagedDocuments_Id = supplementary.ManagedDocumentId;
                        m_Supplementary.I_GesCaseReportAvailabilityStatuses_Id = supplementary.Status.Value;
                        m_Supplementary.PublicationYear = supplementary.PublicationYear != null ? Convert.ToInt16(supplementary.PublicationYear) : s;
                        m_Supplementary.ShowInReport = supplementary.ShowInReport;
                        m_Supplementary.Source = supplementary.Source;
                        _gesCaseReportSupplementaryReadingRepository.Edit(m_Supplementary);
                    }
                }
                _gesCaseReportSupplementaryReadingRepository.Save();
            }
            catch
            {
                return Json(new { Status = "Error" });
            }

            return Json(new { Status = "Success", Id = m_Supplementary.I_GesCaseReportSupplementaryReading_Id });
        }

        [HttpPost]
        public JsonResult DeleteSupplementaryReading(CaseReportSupplementaryViewModel supplementary)
        {
            try
            {
                var m_Supplementary = _gesCaseReportSupplementaryReadingRepository.GetById(supplementary.Id);

                if (m_Supplementary != null)
                {
                    _gesCaseReportSupplementaryReadingRepository.Delete(m_Supplementary);

                    _gesCaseReportSupplementaryReadingRepository.Save();
                }
            }
            catch
            {
                return Json(new { Status = "Error" });
            }
            return Json(new { Status = "Success" });
        }

        [HttpGet]
        public JsonResult GetRecommendationHistory(long gesCaseReportId)
        {
            var recommendationHistory = _caseProfilesService.GetRecommendationHistory(gesCaseReportId);
            return Json(recommendationHistory, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetConclusionHistory(long gesCaseReportId)
        {
            var recommendationHistory = _caseProfilesService.GetConclusionHistory(gesCaseReportId);
            return Json(recommendationHistory, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult DeleteGesCaseAuditLogs(GesCaseAuditLogsViewModel recommendationLog)
        {
            try
            {
                var recommendation = _gesCaseReportsAuditDetailService.GetById(recommendationLog.Id);

                if (recommendation != null)
                {
                    _gesCaseReportsAuditDetailService.Delete(recommendation);
                    _gesCaseReportsAuditDetailService.Save();
                }
            }
            catch
            {
                return Json(new { Status = "Error" });
            }
            return Json(new { Status = "Success" });
        }

        [HttpPost]
        public JsonResult SaveGesCaseAuditLogs(long caseReportId, List<GesCaseAuditLogsViewModel> recommendationLogs, string statusType)
        {
            try
            {
                if (recommendationLogs != null)
                {
                    var columnName = statusType == "recommendation" ? "NewI_GesCaseReportStatuses_Id" : "I_GesCaseReportStatuses_Id";

                    var recomendationLogsOriginal = _gesCaseReportsAuditDetailService
                        .GetGesCaseProfileAuditByCaseReportId(caseReportId, columnName);

                    if (recomendationLogsOriginal.Any())
                    {
                        foreach (var gesCaseReportsAuditDetailse in recomendationLogsOriginal)
                        {
                            var audit = recommendationLogs.FirstOrDefault(
                                d => d.Id == gesCaseReportsAuditDetailse.GesCaseReports_Audit_Details1);

                            if (audit != null && audit.AuditDatetime != gesCaseReportsAuditDetailse.AuditDatetime)
                            {
                                gesCaseReportsAuditDetailse.AuditDatetime = audit.AuditDatetime.Date;
                                _gesCaseReportsAuditDetailService.Update(gesCaseReportsAuditDetailse);
                            }
                        }


                        _gesCaseReportsAuditDetailService.Save();
                    }
                }
            }
            catch (Exception e)
            {
                return Json(new { Status = "Failed updating CaseReport Ges UNGP Assessment. Caused:" + e.Message });
            }

            return Json(new { Status = "Success"});
        }

        #endregion
    }
}