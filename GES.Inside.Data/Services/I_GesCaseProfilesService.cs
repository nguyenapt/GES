using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Linq.Dynamic;
using AutoMapper;
using GES.Common.Enumeration;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Repository.Interfaces;
using GES.Inside.Data.Services.Interfaces;
using GES.Common.Logging;
using GES.Common.Models;
using GES.Inside.Data.Extensions;
using GES.Inside.Data.Helpers;
using GES.Inside.Data.Models;
using GES.Inside.Data.Models.Anonymous;

namespace GES.Inside.Data.Services
{
    public class I_GesCaseProfilesService : EntityService<GesEntities, I_GesCaseReports>, II_GesCaseProfilesService
    {
        private readonly GesEntities _dbContext;
        private readonly II_GesCaseReportsRepository _gesCaseProfilesRepository;
        private readonly I_GesCaseReportsExtraService _gesCaseReportsExtraService;
        private readonly II_CompaniesRepository _companiesRepository;
        private readonly IG_ManagedDocumentsRepository _managedDocumentsRepository;
        private readonly IGesCaseReportSignUpRepository _caseReportSignUpRepository;
        private readonly ICalendarService _calendarService;
        private readonly II_CompaniesService _companiesService;

        public I_GesCaseProfilesService(IUnitOfWork<GesEntities> unitOfWork,
            II_GesCaseReportsRepository gesCaseProfilesRepository, II_CompaniesRepository companiesRepository, II_GesCaseReportsExtraRepository extraRepository, IG_ManagedDocumentsRepository managedDocumentsRepository,
            IGesCaseReportSignUpRepository caseReportSignUpRepository, II_CompaniesService companiesService, ICalendarService calendarService, IGesLogger logger)
            : base(unitOfWork, logger, gesCaseProfilesRepository)
        {
            _dbContext = unitOfWork.DbContext;
            _gesCaseProfilesRepository = gesCaseProfilesRepository;
            _companiesRepository = companiesRepository;
            _managedDocumentsRepository = managedDocumentsRepository;
            _caseReportSignUpRepository = caseReportSignUpRepository;
            _calendarService = calendarService;
            _companiesService = companiesService;
            _gesCaseReportsExtraService = new I_GesCaseReportsExtraService(unitOfWork, extraRepository, logger);
        }

        #region DbMigrationTasks

        public int ExtractKeywordsForAllCaseProfiles(int limit)
        {
            var caseReportsExtra = new List<I_GesCaseReportsExtra>();

            var existingGesCaseReportsExtra = _gesCaseReportsExtraService.GetAll().ToList();
            var proceededGesCaseReportIds = existingGesCaseReportsExtra.Select(i => i.I_GesCaseReports_Id).ToList();

            var toBeProceedGesCaseReports = GetAll().Where(i => !proceededGesCaseReportIds.Contains(i.I_GesCaseReports_Id)).Skip(0).Take(limit).ToList();
            foreach (var item in toBeProceedGesCaseReports)
            {
                // prep.
                var content = item.Summary ?? "";
                content += item.Description != null ? (" " + item.Description) : "";

                try
                {
                    // get keywords
                    var kws = PosTagger.ExtractKeywords(content).ToArray();
                    var kwStr = kws.Any() ? string.Join(",", kws) : "";

                    caseReportsExtra.Add(new I_GesCaseReportsExtra
                    {
                        I_GesCaseReports_Id = item.I_GesCaseReports_Id,
                        Keywords = kwStr
                    });
                }
                catch (Exception e)
                {
                    Logger.Error($"Error when insert the case report extra: I_GesCaseReportsId- {item.I_GesCaseReports_Id}");
                }
            }

            // batch update to I_GesCaseReportsExtra tbl
            return _gesCaseReportsExtraService.BatchUpdateCaseReportKeywords(caseReportsExtra);
        }

        public IEnumerable<I_EngagementActivityOptions> GetActivityOptions()
        {
            return this.SafeExecute(_dbContext.I_EngagementActivityOptions.AsEnumerable);
        }

        public I_ActivityForms GetActivityForm(long caseReportId, long orgId)
        {
            return this.SafeExecute(() => _dbContext.I_ActivityForms.FirstOrDefault(i => i.I_GesCaseReports_Id == caseReportId
                                                                  && i.G_Organizations_Id == orgId));
        }

        public I_ActivityForms UpdateEndorsement(I_ActivityForms activityForm)
        {
            if (activityForm.I_ActivityForms_Id == -1)
            {
                this.SafeExecute(() => _dbContext.I_ActivityForms.Add(activityForm));
            }
            else
            {
                this.SafeExecute(() => _dbContext.I_ActivityForms.AddOrUpdate(activityForm));
            }

            this.UnitOfWork.Commit();

            return activityForm;
        }

        #endregion

        public KeyValueObject<string, string> GetCaseReportTitle(long caseReportId)
        {
            return SafeExecute(() => _gesCaseProfilesRepository.GetCaseReportTitle(caseReportId));
        }

        public GesCaseReportType GetReportType(long reportId, long orgId)
        {
            var companyId = _companiesRepository.GetCompanyIdFromCaseProfile(reportId);

            var gesCompanyId = _companiesService.GetGesCompanyId(companyId);

            if (!_gesCaseProfilesRepository.HasSubscribed(orgId, gesCompanyId ?? 0, reportId))
            {
                return GesCaseReportType.Unsubscribed;
            }

            var engagementTypeId = this.SafeExecute(() => _dbContext.I_GesCaseReportsI_EngagementTypes
                .FirstOrDefault(i => i.I_GesCaseReports_Id == reportId)?.I_EngagementTypes_Id);

            var recommendationId = this.SafeExecute(() => _dbContext.I_GesCaseReports
                .FirstOrDefault(i => i.I_GesCaseReports_Id == reportId)?.NewI_GesCaseReportStatuses_Id);


            if (engagementTypeId == null) return GesCaseReportType.NotImplementing;

            var engagementTypeCategoryId = this.SafeExecute(() => _dbContext.I_EngagementTypes
                .FirstOrDefault(i => i.I_EngagementTypes_Id == (long)engagementTypeId)?.I_EngagementTypeCategories_Id);

            if (engagementTypeCategoryId == (long)EngagementTypeCategoryEnum.BusinessConduct)
            {
                if (CheckBussinessConductServices(orgId))
                {
                    if (recommendationId == null) return GesCaseReportType.NotImplementing;

                    return _gesCaseProfilesRepository.GetBCCaseReportType((EngagementTypeEnum)engagementTypeId,
                            (RecommendationType)recommendationId);
                }
                else
                {
                    var conclusionId = this.SafeExecute(() => _dbContext.I_GesCaseReports
                        .FirstOrDefault(i => i.I_GesCaseReports_Id == reportId)?.I_GesCaseReportStatuses_Id);

                    return GetReportTypeByConclusion(conclusionId);
                }
            }
            else if (engagementTypeCategoryId == (long)EngagementTypeCategoryEnum.StewardshipAndRisk)
            {
                if (recommendationId == null) return GesCaseReportType.NotImplementing;

                return _gesCaseProfilesRepository.GetSRCaseReportType((EngagementTypeEnum)engagementTypeId,
                    (RecommendationType)recommendationId);
            }
            else if (engagementTypeCategoryId == (long)EngagementTypeCategoryEnum.Bespoke)
            {
                if (recommendationId == null) return GesCaseReportType.NotImplementing;

                return _gesCaseProfilesRepository.GetBpCaseReportType((RecommendationType)recommendationId);
            }
            else if (engagementTypeCategoryId == (long)EngagementTypeCategoryEnum.Governance)
            {
                if (recommendationId == null) return GesCaseReportType.NotImplementing;

                return _gesCaseProfilesRepository.GetGovCaseReportType((EngagementTypeEnum)engagementTypeId, (RecommendationType)recommendationId);
            }

            return GesCaseReportType.GenerationType;
        }

        private GesCaseReportType GetReportTypeByConclusion(long? conclusionId)
        {
            switch (conclusionId)
            {
                case (long)GesCaseReportStatus.IndicationOfViolation:
                    return GesCaseReportType.StIndicationOfViolation;
                case (long)GesCaseReportStatus.ConfirmedViolation:
                    return GesCaseReportType.StConfirmed;
                case (long)GesCaseReportStatus.Resolved:
                    return GesCaseReportType.StResolved;
                case (long)GesCaseReportStatus.ArchivedOfConclution:
                    return GesCaseReportType.StArchived;
                case (long)GesCaseReportStatus.Alert:
                    return GesCaseReportType.StAlert;
            }

            return GesCaseReportType.NotImplementing;
        }

        private bool CheckBussinessConductServices(long orgId)
        {
            var query = from gs in _dbContext.G_OrganizationsG_Services
                        join s in _dbContext.G_Services on gs.G_Services_Id equals s.G_Services_Id
                        join et in _dbContext.I_EngagementTypes on s.I_EngagementTypes_Id equals et.I_EngagementTypes_Id
                        where gs.G_Organizations_Id == orgId && et.I_EngagementTypeCategories_Id ==
                              (long)EngagementTypeCategoryEnum.BusinessConduct
                        select gs.G_OrganizationsG_Services_Id;

            return this.SafeExecute(() => query.Any());
        }

        public int GetEngagementType(long reportId)
        {
            var engagementTypeId = this.SafeExecute(() => _dbContext.I_GesCaseReportsI_EngagementTypes
                .FirstOrDefault(i => i.I_GesCaseReports_Id == reportId)?.I_EngagementTypes_Id);

            if (engagementTypeId != null) return (int)engagementTypeId;
            return 0;
        }


        public CaseReportViewModel GetCaseReportViewModel(long caseReportId, long orgId)
        {
            var result = from gcr in _dbContext.I_GesCaseReports
                         join gc in _dbContext.I_GesCompanies on gcr.I_GesCompanies_Id equals gc.I_GesCompanies_Id
                         join c in _dbContext.I_Companies on gc.I_Companies_Id equals c.I_Companies_Id
                         join hc in _dbContext.Countries on c.CountryOfIncorporationId equals hc.Id into hcg
                         from hc in hcg.DefaultIfEmpty()
                         join m in _dbContext.SubPeerGroups on c.SubPeerGroupId equals m.Id into mg
                         from m in mg.DefaultIfEmpty()
                         join l in _dbContext.Countries on gcr.CountryId equals l.Id into lg
                         from l in lg.DefaultIfEmpty()
                         join n in _dbContext.I_NormAreas on gcr.I_NormAreas_Id equals n.I_NormAreas_Id into ng
                         from n in ng.DefaultIfEmpty()
                         where gcr.I_GesCaseReports_Id == caseReportId
                         select new CaseReportViewModel
                         {
                             Id = caseReportId,
                             CompanyId = c.I_Companies_Id,
                             CompanyName = c.Name,
                             Isin = c.Isin,
                             Sedol = c.Sedol,
                             Industry = m.Name,
                             MaterialEsgRisk = c.MostMaterialRisk,
                             HomeCountry = hc.Name,
                             UnGlobalCompact = c.IsGlobalCompactMember,
                             IssueName = gcr.ReportIncident,
                             Location = l.Name,
                             NormArea = n.Name,
                             Recommendation = gcr.Recommendation != null ? gcr.Recommendation.Replace("Resolved (Indication of Violation)", "Resolved") : null,
                             ConfirmedViolation = gcr.Confirmed,
                             Summary = gcr.Summary,
                             ChangeObjective = gcr.ProcessGoal,
                             NextStep = gcr.ProcessStep,
                             CompanyDialogue = (gcr.CompanyDialogueNew != null ? gcr.CompanyDialogueNew + "<br/>" : "") + (gcr.CompanyDialogueSummary ?? "")
                         };

            var caseReportViewModel = result.FirstOrDefault();
            if (caseReportViewModel != null)
            {
                caseReportViewModel.RelatedIssues = GetRelatedIssueViewModels(caseReportId);
                caseReportViewModel.GesComment = GetLatestGesComment(caseReportId);
                caseReportViewModel.LatestNews = GetLatestNews(caseReportId);
                caseReportViewModel.Reports = GetRelatedReportList(caseReportId);
                caseReportViewModel.Portfolios = GetPortfoliosByCompanyIdAndOrgId(caseReportViewModel.CompanyId, orgId);
            }

            return caseReportViewModel;
        }

        public CaseProfileBcEvaluateViewModel GetBusinessConductEvaluate(long caseReportId, long orgId)
        {
            var caseReportViewModel = _gesCaseProfilesRepository.GetCaseProfileCoreViewModel<CaseProfileBcEvaluateViewModel>(caseReportId, orgId);
            if (caseReportViewModel != null)
            {
                var companyId = _companiesRepository.GetCompanyIdFromCaseProfile(caseReportId);
                caseReportViewModel.IssueComponent.LatestNews = GetLatestNews(caseReportId);
                caseReportViewModel.AdditionalDocuments = _managedDocumentsRepository.GetAdditionalDocuments(caseReportViewModel.CaseProfileId);
                caseReportViewModel.Endorsement = _caseReportSignUpRepository.GetUserResigned(caseReportViewModel.CaseProfileId, orgId);
                caseReportViewModel.Events = _calendarService.GetCalendarEventsByCompanyId(companyId);
                caseReportViewModel.Alert = _gesCaseProfilesRepository.GetAlert(caseReportId);
                if (caseReportViewModel.CaseComponent.ConfirmedViolation)
                {
                    caseReportViewModel.RevisionCriterials =
                        _gesCaseProfilesRepository.GetRevisionCriterials(caseReportId);
                    //caseReportViewModel.References = _gesCaseProfilesRepository.GetCaseReportReferenceses(caseReportId);  Task - 283
                    caseReportViewModel.ConfirmationInformation =
                        _gesCaseProfilesRepository.GetConfirmationInformation(caseReportId);
                }
            }
            return caseReportViewModel;
        }

        public CaseProfileBcEngageViewModel GetBusinessConductEngage(long caseReportId, long orgId)
        {
            var caseReportViewModel = _gesCaseProfilesRepository.GetCaseProfileCoreViewModel<CaseProfileBcEngageViewModel>(caseReportId, orgId);

            if (caseReportViewModel != null)
            {
                var companyId = _companiesRepository.GetCompanyIdFromCaseProfile(caseReportId);
                caseReportViewModel.IssueComponent.LatestNews = GetLatestNews(caseReportId);
                caseReportViewModel.AdditionalDocuments = _managedDocumentsRepository.GetAdditionalDocuments(caseReportId);
                caseReportViewModel.Endorsement = _caseReportSignUpRepository.GetUserResigned(caseReportId, orgId);
                caseReportViewModel.DiscussionPoints = _companiesRepository.GetEngagementDiscussionPointsByCompanyId(companyId);
                caseReportViewModel.StakeholderViews = _companiesRepository.GetStakeholderViews(companyId);
                caseReportViewModel.InvestorInitiatives = _companiesRepository.GetInvestorInitiatives(companyId);
                caseReportViewModel.Events = _calendarService.GetCalendarEventsByCompanyId(companyId);
                caseReportViewModel.StatisticComponent = _gesCaseProfilesRepository.GetStatisticComponent(orgId, caseReportId);
                caseReportViewModel.EngagementInformationComponent = _gesCaseProfilesRepository.GetEngagementInformationComponent(caseReportId);
                caseReportViewModel.SdgAndGuidelineConventionComponent = _gesCaseProfilesRepository.GetCaseProfileSdgAndGuidelineConventionComponent(caseReportId);

                caseReportViewModel.Alert = _gesCaseProfilesRepository.GetAlert(caseReportId);
                if (caseReportViewModel.CaseComponent.ConfirmedViolation)
                {
                    caseReportViewModel.RevisionCriterials =
                        _gesCaseProfilesRepository.GetRevisionCriterials(caseReportId);
                    caseReportViewModel.References = _gesCaseProfilesRepository.GetCaseReportReferenceses(caseReportId);
                    caseReportViewModel.ConfirmationInformation =
                        _gesCaseProfilesRepository.GetConfirmationInformation(caseReportId);
                }
                if (caseReportViewModel.CaseComponent.IndicationOfViolation)
                {
                    caseReportViewModel.References = _gesCaseProfilesRepository.GetCaseReportReferenceses(caseReportId);
                }
            }
            return caseReportViewModel;
        }

        public CaseProfileBcDisengageViewModel GetBusinessConductDisEngage(long caseReportId, long orgId)
        {
            var caseReportViewModel = _gesCaseProfilesRepository.GetCaseProfileCoreViewModel<CaseProfileBcDisengageViewModel>(caseReportId, orgId);
            if (caseReportViewModel != null)
            {
                var companyId = _companiesRepository.GetCompanyIdFromCaseProfile(caseReportId);
                caseReportViewModel.IssueComponent.LatestNews = GetLatestNews(caseReportId);
                caseReportViewModel.AdditionalDocuments = _managedDocumentsRepository.GetAdditionalDocuments(caseReportViewModel.CaseProfileId);
                caseReportViewModel.StatisticComponent = _gesCaseProfilesRepository.GetStatisticComponent(orgId, caseReportId);
                caseReportViewModel.SdgAndGuidelineConventionComponent = _gesCaseProfilesRepository.GetCaseProfileSdgAndGuidelineConventionComponent(caseReportId);
                caseReportViewModel.Alert = _gesCaseProfilesRepository.GetAlert(caseReportId);
                caseReportViewModel.DiscussionPoints = _companiesRepository.GetEngagementDiscussionPointsByCompanyId(companyId);
                if (caseReportViewModel.CaseComponent.ConfirmedViolation)
                {
                    caseReportViewModel.RevisionCriterials =
                        _gesCaseProfilesRepository.GetRevisionCriterials(caseReportId);
                    caseReportViewModel.References = _gesCaseProfilesRepository.GetCaseReportReferenceses(caseReportId);
                    caseReportViewModel.ConfirmationInformation =
                        _gesCaseProfilesRepository.GetConfirmationInformation(caseReportId);
                }
                if (caseReportViewModel.CaseComponent.IndicationOfViolation)
                {
                    caseReportViewModel.References = _gesCaseProfilesRepository.GetCaseReportReferenceses(caseReportId);
                }
            }
            return caseReportViewModel;

        }

        public CaseProfileBcArchivedViewModel GetBusinessConductArchived(long caseReportId, long orgId)
        {
            var caseReportViewModel = _gesCaseProfilesRepository.GetCaseProfileCoreViewModel<CaseProfileBcArchivedViewModel>(caseReportId, orgId);
            if (caseReportViewModel != null)
            {
                var companyId = _companiesRepository.GetCompanyIdFromCaseProfile(caseReportId);
                caseReportViewModel.IssueComponent.LatestNews = GetLatestNews(caseReportId);
                caseReportViewModel.AdditionalDocuments = _managedDocumentsRepository.GetAdditionalDocuments(caseReportViewModel.CaseProfileId);
                caseReportViewModel.StatisticComponent = _gesCaseProfilesRepository.GetStatisticComponent(orgId, caseReportId);
                caseReportViewModel.SdgAndGuidelineConventionComponent = _gesCaseProfilesRepository.GetCaseProfileSdgAndGuidelineConventionComponent(caseReportId);
                caseReportViewModel.IssueComponent.ClosingIncidentAnalysisComponent = _gesCaseProfilesRepository.GetClosingIncidentAnalysisComponent(caseReportId, RecommendationType.Archived);
                caseReportViewModel.Alert = _gesCaseProfilesRepository.GetAlert(caseReportId);
                caseReportViewModel.DiscussionPoints = _companiesRepository.GetEngagementDiscussionPointsByCompanyId(companyId);
                if (caseReportViewModel.CaseComponent.ConfirmedViolation)
                {
                    caseReportViewModel.RevisionCriterials =
                        _gesCaseProfilesRepository.GetRevisionCriterials(caseReportId);
                    caseReportViewModel.References = _gesCaseProfilesRepository.GetCaseReportReferenceses(caseReportId);
                    caseReportViewModel.ConfirmationInformation =
                        _gesCaseProfilesRepository.GetConfirmationInformation(caseReportId);
                }
                if (caseReportViewModel.CaseComponent.IndicationOfViolation)
                {
                    caseReportViewModel.References = _gesCaseProfilesRepository.GetCaseReportReferenceses(caseReportId);
                }
            }
            return caseReportViewModel;

        }

        public CaseProfileBcResolvedViewModel GetBusinessConductResolved(long caseReportId, long orgId)
        {
            var caseProfile = _gesCaseProfilesRepository.GetCaseProfileCoreViewModel<CaseProfileBcResolvedViewModel>(caseReportId, orgId);
            if (caseProfile != null)
            {
                var companyId = _companiesRepository.GetCompanyIdFromCaseProfile(caseReportId);
                caseProfile.EngagementInformationComponent = _gesCaseProfilesRepository.GetEngagementInformationComponent(caseReportId);
                caseProfile.IssueComponent.LatestNews = GetLatestNews(caseReportId);
                caseProfile.AdditionalDocuments = _managedDocumentsRepository.GetAdditionalDocuments(caseProfile.CaseProfileId);
                caseProfile.StatisticComponent = _gesCaseProfilesRepository.GetStatisticComponent(orgId, caseReportId);
                caseProfile.SdgAndGuidelineConventionComponent = _gesCaseProfilesRepository.GetCaseProfileSdgAndGuidelineConventionComponent(caseReportId);
                caseProfile.IssueComponent.ClosingIncidentAnalysisComponent = _gesCaseProfilesRepository.GetClosingIncidentAnalysisComponent(caseReportId, RecommendationType.Resolved);
                caseProfile.Alert = _gesCaseProfilesRepository.GetAlert(caseReportId);
                caseProfile.DiscussionPoints = _companiesRepository.GetEngagementDiscussionPointsByCompanyId(companyId);
                if (caseProfile.CaseComponent.ConfirmedViolation)
                {
                    caseProfile.RevisionCriterials =
                        _gesCaseProfilesRepository.GetRevisionCriterials(caseReportId);
                    caseProfile.References = _gesCaseProfilesRepository.GetCaseReportReferenceses(caseReportId);
                    caseProfile.ConfirmationInformation =
                        _gesCaseProfilesRepository.GetConfirmationInformation(caseReportId);
                }
                if (caseProfile.CaseComponent.IndicationOfViolation)
                {
                    caseProfile.References = _gesCaseProfilesRepository.GetCaseReportReferenceses(caseReportId);
                }
            }
            return caseProfile;
        }

        public CaseProfileCoreViewModel GetCaseProfileCoreModel(GesCaseReportType reportType, long caseReportId, long orgId)
        {
            //TODO:GetAll
            //return GetFullAttributeModel(reportType, caseReportId, orgId);
            switch (reportType)
            {

                case GesCaseReportType.StConfirmed:
                case GesCaseReportType.StIndicationOfViolation:
                case GesCaseReportType.StAlert:
                    return GetStandardCaseProfile(caseReportId, orgId) ?? new CaseProfileStandardViewModel();
                case GesCaseReportType.StResolved:
                    return GetStResolvedCase(caseReportId, orgId) ?? new CaseProfileStandardViewModel();
                case GesCaseReportType.StArchived:
                    return GetStArchivedCase(caseReportId, orgId) ?? new CaseProfileStandardViewModel();
                default:
                    return GetFullAttributeModel(reportType, caseReportId, orgId) ?? new CaseProfileFullAttributeViewModel();
            }

            //switch (reportType)
            //{
            //    case GesCaseReportType.BcEvaluate:
            //        return GetBusinessConductEvaluate(caseReportId, orgId) ?? new CaseProfileBcEvaluateViewModel();
            //    case GesCaseReportType.BcEngage:
            //        return GetBusinessConductEngage(caseReportId, orgId) ?? new CaseProfileBcEngageViewModel();
            //    case GesCaseReportType.BcResolved:
            //        return GetBusinessConductResolved(caseReportId, orgId) ?? new CaseProfileBcResolvedViewModel();
            //    case GesCaseReportType.BcDisengage:
            //        return GetBusinessConductDisEngage(caseReportId, orgId) ?? new CaseProfileBcDisengageViewModel();
            //    case GesCaseReportType.BcArchived:
            //        return GetBusinessConductArchived(caseReportId, orgId) ?? new CaseProfileBcArchivedViewModel();
            //    case GesCaseReportType.SrEngage:
            //    case GesCaseReportType.SrArchived:
            //    case GesCaseReportType.SrCarbonRiskEngage:
            //    case GesCaseReportType.SrCarbonRiskArchived:
            //    case GesCaseReportType.SrCarbonRiskResolved:
            //        return GetSrEngageOrArchived(caseReportId, orgId) ?? new CaseProfileSrViewModel();
            //    case GesCaseReportType.SrEmeEngage:
            //    case GesCaseReportType.SrEmeArchived:
            //        return GetSrEmeEngageOrArchived(caseReportId, orgId) ?? new CaseProfileSrEmeViewModel();
            //    case GesCaseReportType.SrGovArchived:
            //    case GesCaseReportType.SrGovResolved:
            //    case GesCaseReportType.SrGovEngage:
            //        return GetSrGovEngageOrArchived(caseReportId, orgId) ?? new CaseProfileSrGovViewModel();
            //    case GesCaseReportType.StConfirmed:
            //    case GesCaseReportType.StIndicationOfViolation:
            //    case GesCaseReportType.StAlert:
            //        return GetStandardCaseProfile(caseReportId, orgId) ?? new CaseProfileStandardViewModel();
            //    case GesCaseReportType.StResolved:
            //        return GetStResolvedCase(caseReportId, orgId) ?? new CaseProfileStandardViewModel();
            //    case GesCaseReportType.StArchived:
            //        return GetStArchivedCase(caseReportId, orgId) ?? new CaseProfileStandardViewModel();
            //    case GesCaseReportType.BpEngage:
            //    case GesCaseReportType.BpArchived:
            //        return GetBpCases(caseReportId, orgId) ?? new CaseProfileBespokeViewModel();
            //    case GesCaseReportType.GenerationType:
            //        return GetGenerateCases(caseReportId, orgId) ?? new CaseProfileGenerationViewModel();
            //    default:
            //        return null;
            //}
        }

        public CaseProfileSrEvaluateViewModel GetSrEvaluate(long caseReportId, long orgId)
        {
            var caseReportViewModel = _gesCaseProfilesRepository.GetCaseProfileCoreViewModel<CaseProfileSrEvaluateViewModel>(caseReportId, orgId);
            if (caseReportViewModel != null)
            {
                caseReportViewModel.AdditionalDocuments = _managedDocumentsRepository.GetAdditionalDocuments(caseReportViewModel.CaseProfileId);
            }

            return caseReportViewModel;
        }

        public CaseProfileSrViewModel GetSrEngageOrArchived(long caseReportId, long orgId)
        {
            var caseReportViewModel = _gesCaseProfilesRepository.GetCaseProfileCoreViewModel<CaseProfileSrViewModel>(caseReportId, orgId);
            if (caseReportViewModel != null)
            {
                var companyId = _companiesRepository.GetCompanyIdFromCaseProfile(caseReportId);
                caseReportViewModel.EngagementInformationComponent = _gesCaseProfilesRepository.GetEngagementInformationComponent(caseReportId);
                caseReportViewModel.AdditionalDocuments = _managedDocumentsRepository.GetAdditionalDocuments(caseReportViewModel.CaseProfileId);
                caseReportViewModel.StatisticComponent = _gesCaseProfilesRepository.GetStatisticComponent(orgId, caseReportId);
                caseReportViewModel.Endorsement = _caseReportSignUpRepository.GetUserResigned(caseReportId, orgId);
                caseReportViewModel.Events = _calendarService.GetCalendarEventsByCompanyId(companyId);
                caseReportViewModel.Sdgs = _gesCaseProfilesRepository.GetCaseProfileSdgs(caseReportId).ToList();
                caseReportViewModel.KpiViewModels = _gesCaseProfilesRepository.GetCaseProfileKpis(caseReportId).ToList();
            }
            return caseReportViewModel;
        }

        public CaseProfileBespokeViewModel GetBpCases(long caseReportId, long orgId)
        {
            var caseReportViewModel = _gesCaseProfilesRepository.GetCaseProfileCoreViewModel<CaseProfileBespokeViewModel>(caseReportId, orgId);
            if (caseReportViewModel != null)
            {
                var companyId = _companiesRepository.GetCompanyIdFromCaseProfile(caseReportId);
                caseReportViewModel.EngagementInformationComponent = _gesCaseProfilesRepository.GetEngagementInformationComponent(caseReportId);
                caseReportViewModel.AdditionalDocuments = _managedDocumentsRepository.GetAdditionalDocuments(caseReportViewModel.CaseProfileId);
                caseReportViewModel.StatisticComponent = _gesCaseProfilesRepository.GetStatisticComponent(orgId, caseReportId);
                caseReportViewModel.Endorsement = _caseReportSignUpRepository.GetUserResigned(caseReportId, orgId);
                caseReportViewModel.Events = _calendarService.GetCalendarEventsByCompanyId(companyId);
                caseReportViewModel.Sdgs = _gesCaseProfilesRepository.GetCaseProfileSdgs(caseReportId).ToList();
            }
            return caseReportViewModel;
        }

        public CaseProfileGenerationViewModel GetGenerateCases(long caseReportId, long orgId)
        {
            var caseReportViewModel = _gesCaseProfilesRepository.GetCaseProfileCoreViewModel<CaseProfileGenerationViewModel>(caseReportId, orgId);
            if (caseReportViewModel != null)
            {
                var companyId = _companiesRepository.GetCompanyIdFromCaseProfile(caseReportId);
                caseReportViewModel.EngagementInformationComponent = _gesCaseProfilesRepository.GetEngagementInformationComponent(caseReportId);
                caseReportViewModel.AdditionalDocuments = _managedDocumentsRepository.GetAdditionalDocuments(caseReportViewModel.CaseProfileId);
                caseReportViewModel.StatisticComponent = _gesCaseProfilesRepository.GetStatisticComponent(orgId, caseReportId);
                caseReportViewModel.Endorsement = _caseReportSignUpRepository.GetUserResigned(caseReportId, orgId);
                caseReportViewModel.Events = _calendarService.GetCalendarEventsByCompanyId(companyId);
                caseReportViewModel.Sdgs = _gesCaseProfilesRepository.GetCaseProfileSdgs(caseReportId).ToList();
            }
            return caseReportViewModel;
        }


        public CaseProfileSrEmeViewModel GetSrEmeEngageOrArchived(long caseReportId, long orgId)
        {
            var caseReportViewModel = _gesCaseProfilesRepository.GetCaseProfileCoreViewModel<CaseProfileSrEmeViewModel>(caseReportId, orgId);
            if (caseReportViewModel != null)
            {
                var companyId = _companiesRepository.GetCompanyIdFromCaseProfile(caseReportId);
                caseReportViewModel.EngagementInformationComponent = _gesCaseProfilesRepository.GetEngagementInformationComponent(caseReportId);
                caseReportViewModel.AdditionalDocuments = _managedDocumentsRepository.GetAdditionalDocuments(caseReportViewModel.CaseProfileId);
                caseReportViewModel.StatisticComponent = _gesCaseProfilesRepository.GetStatisticComponent(orgId, caseReportId);
                caseReportViewModel.Endorsement = _caseReportSignUpRepository.GetUserResigned(caseReportId, orgId);
                caseReportViewModel.Events = _calendarService.GetCalendarEventsByCompanyId(companyId);
                caseReportViewModel.Sdgs = _gesCaseProfilesRepository.GetCaseProfileSdgs(caseReportId).ToList();
            }
            return caseReportViewModel;
        }

        public CaseProfileSrGovViewModel GetSrGovEngageOrArchived(long caseReportId, long orgId)
        {
            var caseReportViewModel = _gesCaseProfilesRepository.GetCaseProfileCoreViewModel<CaseProfileSrGovViewModel>(caseReportId, orgId);
            if (caseReportViewModel != null)
            {
                var companyId = _companiesRepository.GetCompanyIdFromCaseProfile(caseReportId);
                caseReportViewModel.EngagementInformationComponent = _gesCaseProfilesRepository.GetEngagementInformationComponent(caseReportId);
                caseReportViewModel.AdditionalDocuments = _managedDocumentsRepository.GetAdditionalDocuments(caseReportViewModel.CaseProfileId);
                caseReportViewModel.StatisticComponent = _gesCaseProfilesRepository.GetStatisticComponent(orgId, caseReportId);
                caseReportViewModel.Endorsement = _caseReportSignUpRepository.GetUserResigned(caseReportId, orgId);
                caseReportViewModel.Events = _calendarService.GetCalendarEventsByCompanyId(companyId);
                caseReportViewModel.Sdgs = _gesCaseProfilesRepository.GetCaseProfileSdgs(caseReportId).ToList();
                if (caseReportViewModel.IssueComponent != null)
                {
                    caseReportViewModel.IssueComponent.ClosingIncidentAnalysisComponent = _gesCaseProfilesRepository.GetClosingIncidentAnalysisComponent(caseReportId, RecommendationType.Resolved);
                }
            }
            return caseReportViewModel;
        }

        public CaseProfileSrEngageEmeViewModel GetSrEngageEme(long caseReportId, long orgId)
        {
            var caseReportViewModel = _gesCaseProfilesRepository.GetCaseProfileCoreViewModel<CaseProfileSrEngageEmeViewModel>(caseReportId, orgId);
            if (caseReportViewModel != null)
            {
                caseReportViewModel.AdditionalDocuments = _managedDocumentsRepository.GetAdditionalDocuments(caseReportViewModel.CaseProfileId);
            }
            return caseReportViewModel;
        }

        public CaseProfileSrEngageCropGovernance GetSrEngageCropGovernance(long caseReportId, long orgId)
        {
            var caseReportViewModel = _gesCaseProfilesRepository.GetCaseProfileCoreViewModel<CaseProfileSrEngageCropGovernance>(caseReportId, orgId);
            if (caseReportViewModel != null)
            {
                caseReportViewModel.AdditionalDocuments = _managedDocumentsRepository.GetAdditionalDocuments(caseReportViewModel.CaseProfileId);
            }
            return caseReportViewModel;
        }

        public CaseProfileStandardViewModel GetStandardCaseProfile(long caseReportId, long orgId)
        {
            var caseReportViewModel = _gesCaseProfilesRepository.GetCaseProfileCoreViewModel<CaseProfileStandardViewModel>(caseReportId, orgId);
            if (caseReportViewModel != null)
            {
                var companyId = _companiesRepository.GetCompanyIdFromCaseProfile(caseReportId);
                caseReportViewModel.EngagementInformationComponent = _gesCaseProfilesRepository.GetEngagementInformationComponent(caseReportId);
                caseReportViewModel.StatisticComponent = _gesCaseProfilesRepository.GetStatisticComponent(orgId, caseReportId);
                caseReportViewModel.Events = _calendarService.GetCalendarEventsByCompanyId(companyId);
                caseReportViewModel.GuidelineConventionComponent = _gesCaseProfilesRepository.GetCaseProfileSdgAndGuidelineConventionComponent(caseReportId);
                caseReportViewModel.References = _gesCaseProfilesRepository.GetCaseReportReferenceses(caseReportId);
                if (caseReportViewModel.CaseComponent.ConfirmedViolation)
                {
                    caseReportViewModel.RevisionCriterials = _gesCaseProfilesRepository.GetRevisionCriterials(caseReportId);
                    caseReportViewModel.ConfirmationInformation = _gesCaseProfilesRepository.GetConfirmationInformation(caseReportId);
                }
            }
            return caseReportViewModel;
        }

        public CaseProfileStandardViewModel GetStArchivedCase(long caseReportId, long orgId)
        {
            var caseReportViewModel = _gesCaseProfilesRepository.GetCaseProfileCoreViewModel<CaseProfileStandardViewModel>(caseReportId, orgId);
            if (caseReportViewModel != null)
            {
                var companyId = _companiesRepository.GetCompanyIdFromCaseProfile(caseReportId);
                caseReportViewModel.EngagementInformationComponent = _gesCaseProfilesRepository.GetEngagementInformationComponent(caseReportId);
                caseReportViewModel.StatisticComponent = _gesCaseProfilesRepository.GetStatisticComponent(orgId, caseReportId);
                caseReportViewModel.Events = _calendarService.GetCalendarEventsByCompanyId(companyId);
                caseReportViewModel.GuidelineConventionComponent = _gesCaseProfilesRepository.GetCaseProfileSdgAndGuidelineConventionComponent(caseReportId);
                caseReportViewModel.References = _gesCaseProfilesRepository.GetCaseReportReferenceses(caseReportId);
                caseReportViewModel.IssueComponent.ClosingIncidentAnalysisComponent = _gesCaseProfilesRepository.GetClosingIncidentAnalysisComponent(caseReportId, RecommendationType.Resolved);

                if (caseReportViewModel.CaseComponent.ConfirmedViolation)
                {
                    var revisionCriterials = _gesCaseProfilesRepository.GetRevisionCriterials(caseReportId);

                    if (revisionCriterials.Any(x => x.Checked))
                    {
                        caseReportViewModel.RevisionCriterials = revisionCriterials;
                        caseReportViewModel.ConfirmationInformation = _gesCaseProfilesRepository.GetConfirmationInformation(caseReportId);
                    }
                }
            }
            return caseReportViewModel;
        }

        public CaseProfileStandardViewModel GetStResolvedCase(long caseReportId, long orgId)
        {
            var caseReportViewModel = _gesCaseProfilesRepository.GetCaseProfileCoreViewModel<CaseProfileStandardViewModel>(caseReportId, orgId);
            if (caseReportViewModel != null)
            {
                var companyId = _companiesRepository.GetCompanyIdFromCaseProfile(caseReportId);
                caseReportViewModel.EngagementInformationComponent = _gesCaseProfilesRepository.GetEngagementInformationComponent(caseReportId);
                caseReportViewModel.StatisticComponent = _gesCaseProfilesRepository.GetStatisticComponent(orgId, caseReportId);
                caseReportViewModel.Events = _calendarService.GetCalendarEventsByCompanyId(companyId);
                caseReportViewModel.GuidelineConventionComponent = _gesCaseProfilesRepository.GetCaseProfileSdgAndGuidelineConventionComponent(caseReportId);
                caseReportViewModel.IssueComponent.ClosingIncidentAnalysisComponent = _gesCaseProfilesRepository.GetClosingIncidentAnalysisComponent(caseReportId, RecommendationType.Resolved);

                caseReportViewModel.References = _gesCaseProfilesRepository.GetCaseReportReferenceses(caseReportId);
                caseReportViewModel.RevisionCriterials = _gesCaseProfilesRepository.GetRevisionCriterials(caseReportId);
                caseReportViewModel.ConfirmationInformation = _gesCaseProfilesRepository.GetConfirmationInformation(caseReportId);
            }
            return caseReportViewModel;
        }


        private string GetLatestGesComment(long caseReportId)
        {
            var gesComment = _dbContext.I_GesCommentary.Where(d => d.I_GesCaseReports_Id == caseReportId).OrderByDescending(d => d.CommentaryModified).FirstOrDefault();
            return gesComment?.Description;
        }

        private List<RelatedIssueViewModel> GetRelatedIssueViewModels(long caseReportId)
        {
            var relatedIssues = (from rl in _dbContext.I_RelatedCases
                                 join gcr in _dbContext.I_GesCaseReports on rl.Related_ID equals gcr.I_GesCaseReports_Id
                                 join gc in _dbContext.I_GesCompanies on gcr.I_GesCompanies_Id equals gc.I_GesCompanies_Id
                                 join c in _dbContext.I_Companies on gc.I_Companies_Id equals c.I_Companies_Id
                                 where rl.Issue_ID == caseReportId && gcr.ShowInClient
                                 select new RelatedIssueViewModel
                                 {
                                     Id = rl.ID,
                                     RelatedId = rl.Related_ID,
                                     IssueId = rl.Issue_ID,
                                     CompanyId = c.I_Companies_Id,
                                     RelatedCompanyName = c.Name,
                                     RelatedIssueName = gcr.ReportIncident
                                 }).ToList();
            return relatedIssues;
        }

        private string GetLatestNews(long caseReportId)
        {
            var latestNews = _dbContext.I_GesLatestNews.Where(d => d.I_GesCaseReports_Id == caseReportId).OrderByDescending(d => d.LatestNewsModified).FirstOrDefault();
            return latestNews?.Description;
        }

        private List<IdNameModel> GetPortfoliosByCompanyIdAndOrgId(long companyId, long orgId)
        {
            var results = (from p in _dbContext.I_Portfolios
                           join pc in _dbContext.I_PortfoliosI_Companies on p.I_Portfolios_Id equals pc.I_Portfolios_Id
                           join po in _dbContext.I_PortfoliosG_Organizations on p.I_Portfolios_Id equals po.I_Portfolios_Id
                           where pc.I_Companies_Id == companyId && po.G_Organizations_Id == orgId
                           select new IdNameModel
                           {
                               Id = p.I_Portfolios_Id,
                               Name = p.Name
                           });

            return results.Distinct().ToList();
        }

        private List<RelatedReportViewModel> GetRelatedReportList(long caseReportId)
        {
            var result = from rps in _dbContext.I_GesCaseReportsI_ProcessStatuses
                         join ps in _dbContext.I_ProcessStatuses on rps.I_ProcessStatuses_Id equals ps.I_ProcessStatuses_Id
                         into psg
                         from ps in psg.DefaultIfEmpty()
                         join md in _dbContext.G_ManagedDocuments on rps.G_ManagedDocuments_Id equals md.G_ManagedDocuments_Id into mdg
                         from md in mdg.DefaultIfEmpty()
                         join u in _dbContext.G_Uploads on md.G_ManagedDocuments_Id equals (int)(u.G_ManagedDocuments_Id ?? default(int)) into ug
                         from u in ug.DefaultIfEmpty()
                         join gcr in _dbContext.I_GesCaseReports on rps.I_GesCaseReports_Id equals gcr.I_GesCaseReports_Id into gcrg
                         from gcr in gcrg.DefaultIfEmpty()
                         join gc in _dbContext.I_GesCompanies on gcr.I_GesCompanies_Id equals gc.I_GesCompanies_Id into gcg
                         from gc in gcg.DefaultIfEmpty()
                         join rc in _dbContext.I_RiskCompanies on gc.I_Companies_Id equals rc.I_Companies_Id into rcg
                         from rc in rcg.DefaultIfEmpty()
                         where rps.I_GesCaseReports_Id == caseReportId && ps.I_ProcessStatuses_Id != (long)ProcessStatus.Exit && gcr.ShowInClient
                         select new RelatedReportViewModel
                         {
                             CaseReportProcessStatusId = rps.I_GesCaseReportsI_ProcessStatuses_Id,
                             ProcessStatusId = ps.I_ProcessStatuses_Id,
                             Done = rps.Done,
                             DateChanged = rps.DateChanged,
                             ProcessStatusName = ps.Name,
                             ProcessStatusDescription = ps.Description,
                             ReportingTemplateId = ps.G_ReportingTemplates_Id,
                             ManagedDocumentName = md.Name,
                             FileName = u.FileName,
                             GesCaseReportGesCompaniesId = gcr.I_GesCompanies_Id,
                             NewGesCaseReportStatusId = gcr.NewI_GesCaseReportStatuses_Id,
                             GesCaseReportNaArticleId = gcr.I_NaArticles_Id,
                             RiskCompaniesId = rc.I_RiskCompanies_Id,
                             SortOrder = ps.SortOrder
                         };

            return result.OrderBy(d => d.SortOrder).ToList();
        }

        public List<CaseReportListViewModel> GetAdditionalCaseReports(long caseProfileId, long orgId)
        {
            var caseReport = _gesCaseProfilesRepository.GetById(caseProfileId);

            var gesCompanyId = caseReport?.I_GesCompanies_Id ?? 0;

            return GetAdditionalReportsById(orgId, gesCompanyId, caseProfileId);
        }

        private List<CaseReportListViewModel> GetAdditionalReportsById(long orgId, long gesCompanyId, long gesCaseReportId)
        {
            var addtionalCaseReports = (from rp in _dbContext.I_GesCaseReports
                                        join l in _dbContext.Countries on rp.CountryId equals l.Id into lg
                                        from l in lg.DefaultIfEmpty()
                                        join n in _dbContext.I_NormAreas on rp.I_NormAreas_Id equals n.I_NormAreas_Id into ng
                                        from n in ng.DefaultIfEmpty()
                                        join gc in _dbContext.I_GesCompanies on rp.I_GesCompanies_Id equals gc.I_GesCompanies_Id
                                        join rd in _dbContext.I_GesCaseReportStatuses on rp.NewI_GesCaseReportStatuses_Id equals
                                        rd.I_GesCaseReportStatuses_Id into rdg
                                        from rd in rdg.DefaultIfEmpty()
                                        join rx in _dbContext.I_GesCaseReportStatuses on rp.I_GesCaseReportStatuses_Id equals
                                        rx.I_GesCaseReportStatuses_Id into rdx
                                        from rx in rdx.DefaultIfEmpty()

                                        join cret in _dbContext.I_GesCaseReportsI_EngagementTypes on rp.I_GesCaseReports_Id equals
                                        cret.I_GesCaseReports_Id
                                        join et in _dbContext.I_EngagementTypes on cret.I_EngagementTypes_Id equals
                                        et.I_EngagementTypes_Id into etg
                                        from et in etg.DefaultIfEmpty()
                                        join etc in _dbContext.I_EngagementTypeCategories on et.I_EngagementTypeCategories_Id equals etc.I_EngagementTypeCategories_Id into etcg
                                        from etc in etcg.DefaultIfEmpty()

                                        join gp in (from gp in _dbContext.I_GesCaseReportsI_ProcessStatuses where gp.I_ProcessStatuses_Id == (long)ProcessStatus.Closed select gp) on rp.I_GesCaseReports_Id equals gp.I_GesCaseReports_Id into ggp
                                        from gp in ggp.DefaultIfEmpty()

                                        where rp.I_GesCompanies_Id == gesCompanyId && rp.I_GesCaseReports_Id != gesCaseReportId && rp.ShowInClient

                                        select new CaseReportListViewModel
                                        {
                                            Id = rp.I_GesCaseReports_Id,
                                            GesCompanyId = rp.I_GesCompanies_Id,
                                            IssueName = rp.ReportIncident,
                                            Confirmed = et.I_EngagementTypes_Id == (int)EngagementTypeEnum.Conventions ? rp.I_GesCaseReportStatuses_Id == (long)GesCaseReportStatus.ConfirmedViolation : new bool?(),
                                            Location = l.Name,
                                            TemplateId = (rp.I_GesCaseReportStatuses_Id == (long)GesCaseReportStatus.IndicationOfViolation || rp.I_GesCaseReportStatuses_Id == (long)GesCaseReportStatus.Alert) ? (long)ReportingTemplates.StandardIndicationViolations : (long)ReportingTemplates.StandardConfirmedViolations,
                                            Conclusion = rx.Name,
                                            Recommendation = rd.Name,
                                            IsResolved = rp.NewI_GesCaseReportStatuses_Id == (long)GesCaseReportStatus.Resolved || rp.NewI_GesCaseReportStatuses_Id == (long)GesCaseReportStatus.ResolvedIndicationOfViolation,
                                            IsArchived = rp.NewI_GesCaseReportStatuses_Id == (long)GesCaseReportStatus.ArchivedOfRecommendation,
                                            EngagementTypeId = et.I_EngagementTypes_Id,
                                            SortOrderEngagementType = et.SortOrder ?? 1,
                                            SortOrderRecommendation = rd.SortOrder ?? 1,
                                            Description = rp.Summary,
                                            EntryDate = rp.EntryDate ?? rp.Created,
                                            LastModified = rp.Modified,
                                            Norm = n.Name,
                                            EngagementType = et.Name,
                                            EngagementTypeCategory = etc.Name,
                                            ProgressGrade = rp.I_ProgressStatuses_Id != null && rd.Name.Equals("engage", StringComparison.InvariantCultureIgnoreCase) ? rp.I_ProgressStatuses_Id : 0,
                                            ResponseGrade = rp.I_ResponseStatuses_Id != null && rd.Name.Equals("engage", StringComparison.InvariantCultureIgnoreCase) ? rp.I_ResponseStatuses_Id : 0,
                                            ClosingDate = gp.DateChanged,
                                            CompanyId = gc.I_Companies_Id
                                        }
            ).ToList();

            var result = from r in addtionalCaseReports
                         group r by new
                         {
                             r.Id,
                             r.GesCompanyId,
                             r.IssueName,
                             r.Confirmed,
                             r.Location,
                             r.TemplateId,
                             r.Conclusion,
                             r.Recommendation,
                             r.IsResolved,
                             r.IsArchived,
                             r.EngagementTypeId,
                             r.SortOrderEngagementType,
                             r.SortOrderRecommendation,
                             r.Description,
                             r.EntryDate,
                             r.LastModified,
                             r.Norm,
                             r.EngagementType,
                             r.EngagementTypeCategory,
                             r.ProgressGrade,
                             r.ResponseGrade,
                             r.ClosingDate,
                             r.IsInFocusList,
                             r.CompanyId
                         }
                into g
                         select new CaseReportListViewModel
                         {
                             Id = g.Key.Id,
                             GesCompanyId = g.Key.GesCompanyId,
                             IssueName = g.Key.IssueName,
                             Confirmed = g.Key.Confirmed,
                             Location = g.Key.Location,
                             TemplateId = g.Key.TemplateId,
                             Conclusion = g.Key.Conclusion,
                             Recommendation = g.Key.Recommendation != null ? g.Key.Recommendation.Replace("Resolved (Indication of Violation)", "Resolved") : null,
                             IsResolved = g.Key.IsResolved,
                             IsArchived = g.Key.IsArchived,
                             EngagementTypeId = g.Key.EngagementTypeId,
                             SortOrderEngagementType = g.Key.SortOrderEngagementType,
                             SortOrderRecommendation = g.Key.SortOrderRecommendation,
                             Description = g.Key.Description,
                             EntryDate = g.Key.EntryDate,
                             LastModified = g.Key.LastModified,
                             ServiceEngagementThemeNorm = DataHelper.GetEngagementThemeNorm(g.Key.EngagementTypeCategory, g.Key.Norm, g.Key.EngagementType, g.Key.EngagementTypeId ?? 0),
                             ProgressGrade = g.Key.ProgressGrade,
                             ResponseGrade = g.Key.ResponseGrade,
                             ClosingDate = g.Key.ClosingDate,
                             IsInFocusList = g.Key.IsInFocusList,
                             DevelopmentGrade = DataHelper.CalcDevelopmentGrade(g.Key.ProgressGrade ?? 0, g.Key.ResponseGrade ?? 0),
                             CompanyId = g.Key.CompanyId
                         };

            var caseReports = result.ToList();
            _gesCaseProfilesRepository.UpdateCaseReportInformation(caseReports, orgId, gesCompanyId);
            return caseReports;
        }



        public PaginatedResults<CaseReportListViewModel> GetCaseProfiles(JqGridViewModel jqGridParams, long? companyId)
        {
            var cases = _gesCaseProfilesRepository.GetCaseProfiles(companyId);

            if (jqGridParams._search)
            {
                var finalRules = JqGridHelper.GetFilterRules<CaseReportListViewModel>(jqGridParams);
                cases = string.IsNullOrEmpty(finalRules) ? cases : cases.Where(finalRules);
            }

            //SORT
            var sortCol = jqGridParams.sidx.ToLower();
            var sortDir = jqGridParams.sord.ToLower();
            if (!(string.IsNullOrEmpty(sortCol) && string.IsNullOrEmpty(sortDir)))
            {
                switch (sortCol)
                {
                    case "issuename":
                        cases = sortDir == "asc"
                            ? cases.OrderBy(x => x.IssueName)
                            : cases.OrderByDescending(x => x.IssueName);
                        break;
                    case "companyname":
                        cases = sortDir == "asc"
                            ? cases.OrderBy(x => x.CompanyName).ThenBy(d => d.IssueName)
                            : cases.OrderByDescending(x => x.CompanyName).ThenBy(d => d.IssueName);
                        break;
                    case "location":
                        cases = sortDir == "asc"
                            ? cases.OrderBy(x => x.Location).ThenBy(d => d.IssueName)
                            : cases.OrderByDescending(x => x.Location).ThenBy(d => d.IssueName);
                        break;
                    case "engagementthemenorm":
                        cases = sortDir == "asc"
                            ? cases.OrderBy(x => x.EngagementThemeNorm).ThenBy(d => d.IssueName)
                            : cases.OrderByDescending(x => x.EngagementThemeNorm).ThenBy(d => d.IssueName);
                        break;
                    case "recommendation":
                        cases = sortDir == "asc"
                            ? cases.OrderBy(x => x.Recommendation).ThenBy(d => d.IssueName)
                            : cases.OrderByDescending(x => x.Recommendation).ThenBy(d => d.IssueName);
                        break;
                    case "entrydate":
                        cases = sortDir == "asc"
                            ? cases.OrderBy(x => x.EntryDate).ThenBy(d => d.IssueName)
                            : cases.OrderByDescending(x => x.EntryDate).ThenBy(d => d.IssueName);
                        break;
                }
            }

            return cases.ToPagedList(Logger, jqGridParams.page, jqGridParams.rows);
        }

        public IEnumerable<TimeLineModel> GetTimeLines(long caseProfileId)
        {
            return _gesCaseProfilesRepository.GetTimeLines(caseProfileId).OrderBy(d => d.DateTime);
        }

        public List<GesCaseAuditLogsViewModel> GetRecommendationHistory(long caseReportId)
        {
            return _gesCaseProfilesRepository.GetRecommendationsHistory(caseReportId);
        }
        public List<GesCaseAuditLogsViewModel> GetConclusionHistory(long caseReportId)
        {
            return _gesCaseProfilesRepository.GetConclusionsHistory(caseReportId);
        }

        public CaseProfileFullAttributeViewModel GetFullAttributeModel(GesCaseReportType reportType, long caseReportId, long orgId)
        {
            var caseReportViewModel = _gesCaseProfilesRepository.GetCaseProfileCoreViewModel<CaseProfileFullAttributeViewModel>(caseReportId, orgId);

            if (caseReportViewModel != null)
            {
                var companyId = _companiesRepository.GetCompanyIdFromCaseProfile(caseReportId);
                //BC-Engage
                caseReportViewModel.IssueComponent.LatestNews = GetLatestNews(caseReportId);
                caseReportViewModel.AdditionalDocuments = _managedDocumentsRepository.GetAdditionalDocuments(caseReportId);
                caseReportViewModel.Endorsement = _caseReportSignUpRepository.GetUserResigned(caseReportId, orgId);
                caseReportViewModel.DiscussionPoints = _companiesRepository.GetEngagementDiscussionPointsByCompanyId(companyId);
                caseReportViewModel.StakeholderViews = _companiesRepository.GetStakeholderViews(companyId);
                caseReportViewModel.InvestorInitiatives = _companiesRepository.GetInvestorInitiatives(companyId);
                caseReportViewModel.Events = _calendarService.GetCalendarEventsByCompanyId(companyId);
                caseReportViewModel.StatisticComponent = _gesCaseProfilesRepository.GetStatisticComponent(orgId, caseReportId);
                caseReportViewModel.EngagementInformationComponent = _gesCaseProfilesRepository.GetEngagementInformationComponent(caseReportId);
                caseReportViewModel.SdgAndGuidelineConventionComponent = _gesCaseProfilesRepository.GetCaseProfileSdgAndGuidelineConventionComponent(caseReportId);
                caseReportViewModel.Alert = _gesCaseProfilesRepository.GetAlert(caseReportId);
                caseReportViewModel.References = _gesCaseProfilesRepository.GetCaseReportReferenceses(caseReportId);

                //StArchived
                if (caseReportViewModel.CaseComponent.ConfirmedViolation)
                {
                    if (reportType == GesCaseReportType.StArchived)
                    {
                        var revisionCriterials = _gesCaseProfilesRepository.GetRevisionCriterials(caseReportId);

                        if (revisionCriterials.Any(x => x.Checked))
                        {
                            caseReportViewModel.RevisionCriterials = revisionCriterials;
                        }
                    }
                    else
                    {
                        caseReportViewModel.RevisionCriterials = _gesCaseProfilesRepository.GetRevisionCriterials(caseReportId);
                    }
                    caseReportViewModel.ConfirmationInformation = _gesCaseProfilesRepository.GetConfirmationInformation(caseReportId);
                }
                //Bc-Evaluate
                //Bc-Resolved
                //caseReportViewModel.IssueComponent.ClosingIncidentAnalysisComponent = _gesCaseProfilesRepository.GetClosingIncidentAnalysisComponent(caseReportId, RecommendationType.Resolved);
                //Bc-Disengage
                //Bc-Archived                

                //SrEngageOrArchived                                
                caseReportViewModel.Sdgs = _gesCaseProfilesRepository.GetCaseProfileSdgs(caseReportId).ToList();
                caseReportViewModel.KpiViewModels = _gesCaseProfilesRepository.GetCaseProfileKpis(caseReportId).ToList();
                //SrEmeEngageOrArchived
                //SrGovEngageOrArchived                
                //if (caseReportViewModel.IssueComponent != null)
                //{
                //    caseReportViewModel.IssueComponent.ClosingIncidentAnalysisComponent = _gesCaseProfilesRepository.GetClosingIncidentAnalysisComponent(caseReportId, RecommendationType.Resolved);
                //}
                //StAlert                
                caseReportViewModel.GuidelineConventionComponent = _gesCaseProfilesRepository.GetCaseProfileSdgAndGuidelineConventionComponent(caseReportId);
                //StResolved                


                //BpEngage & BpArchived  
                //GenerationType
                if (caseReportViewModel.IssueComponent != null)
                {
                    switch (reportType)
                    {
                        case GesCaseReportType.BcResolved:
                        case GesCaseReportType.SrGovEngage:
                        case GesCaseReportType.SrGovArchived:
                        case GesCaseReportType.SrGovResolved:
                        case GesCaseReportType.StResolved:
                        case GesCaseReportType.StArchived:
                            caseReportViewModel.IssueComponent.ClosingIncidentAnalysisComponent = _gesCaseProfilesRepository.GetClosingIncidentAnalysisComponent(caseReportId, RecommendationType.Resolved);
                            break;
                        case GesCaseReportType.BcArchived:
                            caseReportViewModel.IssueComponent.ClosingIncidentAnalysisComponent = _gesCaseProfilesRepository.GetClosingIncidentAnalysisComponent(caseReportId, RecommendationType.Archived);
                            break;
                    }
                }
            }
            return caseReportViewModel;
        }

    }
}
