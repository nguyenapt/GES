using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AutoMapper;
using GES.Common.Configurations;
using GES.Common.Enumeration;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Models;
using GES.Inside.Data.Repository.Interfaces;
using GES.Common.Logging;
using GES.Common.Resources;
using GES.Inside.Data.Helpers;
using GES.Inside.Data.Models.Anonymous;
using GES.Inside.Data.Models.CaseProfiles;
using GES.Common.Models;
using System.Linq.Dynamic;
using GES.Inside.Data.Extensions;
using Z.EntityFramework.Plus;

namespace GES.Inside.Data.Repository
{
    public class I_GesCaseReportsRepository : GenericRepository<I_GesCaseReports>, II_GesCaseReportsRepository
    {
        private readonly GesEntities _dbContext;
        private readonly II_ConventionsRepository _conventionsRepository;
        private readonly II_CompaniesRepository _companiesRepository;
        private readonly IGesCaseReportSdgRepository _gesCaseReportSdgRepository;
        private readonly ISdgRepository _sdgRepository;
        private DateTime _minimumDateFromAudit = new DateTime(2013, 09, 24);

        public I_GesCaseReportsRepository(GesEntities context, IGesLogger logger, II_ConventionsRepository conventionsRepository, II_CompaniesRepository companiesRepository, IGesCaseReportSdgRepository gesCaseReportSdgRepository, ISdgRepository sdgRepository)
           : base(context, logger)
        {
            _dbContext = context;
            _conventionsRepository = conventionsRepository;
            _companiesRepository = companiesRepository;
            _gesCaseReportSdgRepository = gesCaseReportSdgRepository;
            _sdgRepository = sdgRepository;
        }

        public I_GesCaseReports GetById(long id)
        {
            return this.SafeExecute<I_GesCaseReports>(() => _entities.Set<I_GesCaseReports>().FirstOrDefault(d => d.I_GesCaseReports_Id == id));
        }

        public KeyValueObject<string, string> GetCaseReportTitle(long caseReportId)
        {
            var result = from cr in _dbContext.I_GesCaseReports
                         join gc in _dbContext.I_GesCompanies on cr.I_GesCompanies_Id equals gc.I_GesCompanies_Id
                         join c in _dbContext.I_Companies on gc.I_Companies_Id equals c.I_Companies_Id
                         where cr.I_GesCaseReports_Id == caseReportId
                         select new KeyValueObject<string, string>
                         {
                             Key = c.Name,
                             Value = cr.ReportIncident
                         };

            return this.SafeExecute(() => result.FirstOrDefault());
        }

        public GesCaseReportType GetBCCaseReportType(EngagementTypeEnum engagementType, RecommendationType recommandationType)
        {
            //if (engagementType == EngagementTypeEnum.Conventions || engagementType == EngagementTypeEnum.BusinessConductExtendedTaxation)
            //{
            if (recommandationType == RecommendationType.Evaluate) return GesCaseReportType.BcEvaluate;
            if (recommandationType == RecommendationType.Engage) return GesCaseReportType.BcEngage;
            if (recommandationType == RecommendationType.Disengage) return GesCaseReportType.BcDisengage;
            if (recommandationType == RecommendationType.Resolved || recommandationType == RecommendationType.ResolvedIndicationOfViolation) return GesCaseReportType.BcResolved;
            if (recommandationType == RecommendationType.Archived) return GesCaseReportType.BcArchived;
            //}

            return GesCaseReportType.NotImplementing;
        }

        public GesCaseReportType GetSRCaseReportType(EngagementTypeEnum engagementType, RecommendationType recommandationType)
        {
            if (recommandationType == RecommendationType.Engage)
            {
                switch (engagementType)
                {
                    case EngagementTypeEnum.EmerginMarket:
                        return GesCaseReportType.SrEmeEngage;
                    case EngagementTypeEnum.Governance:
                        return GesCaseReportType.SrGovEngage;
                    case EngagementTypeEnum.CarbonRisk:
                        return GesCaseReportType.SrCarbonRiskEngage;
                }
                return GesCaseReportType.SrEngage;
            }

            if (recommandationType == RecommendationType.Archived)
            {
                switch (engagementType)
                {
                    case EngagementTypeEnum.EmerginMarket:
                        return GesCaseReportType.SrEmeArchived;
                    case EngagementTypeEnum.Governance:
                        return GesCaseReportType.SrGovArchived;
                    case EngagementTypeEnum.CarbonRisk:
                        return GesCaseReportType.SrCarbonRiskArchived;
                }
                return GesCaseReportType.SrArchived;
            }

            if (recommandationType == RecommendationType.Resolved)
            {
                switch (engagementType)
                {
                    case EngagementTypeEnum.Governance:
                        return GesCaseReportType.SrGovResolved;
                    case EngagementTypeEnum.CarbonRisk:
                        return GesCaseReportType.SrCarbonRiskResolved;
                }
            }

            return GesCaseReportType.NotImplementing;
        }

        public GesCaseReportType GetGovCaseReportType(EngagementTypeEnum engagementType, RecommendationType recommandationType)
        {
            switch (recommandationType)
            {
                case RecommendationType.Engage:
                case RecommendationType.Evaluate:
                    return GesCaseReportType.SrGovEngage;
                case RecommendationType.Archived:
                    return GesCaseReportType.SrGovArchived;
                case RecommendationType.Resolved:
                    return GesCaseReportType.SrGovResolved;
            }
            return GesCaseReportType.NotImplementing;
        }


        public GesCaseReportType GetBpCaseReportType(RecommendationType recommandationType)
        {
            switch (recommandationType)
            {
                case RecommendationType.Archived:
                    return GesCaseReportType.BpArchived;
                case RecommendationType.Engage:
                case RecommendationType.Disengage:
                case RecommendationType.Resolved:
                    return GesCaseReportType.BpEngage;
            }

            return GesCaseReportType.NotImplementing;
        }


        public TResult GetCaseProfileCoreViewModel<TResult>(long caseReportId, long orgId) where TResult : ICaseProfileCoreViewModel, new()
        {
            var caseReport = GetCaseReport(caseReportId);
            if (caseReport == null)
                return default(TResult);

            caseReport.RecommendationDate = GetLatestGesCaseReportAudit(caseReportId, "NewI_GesCaseReportStatuses_Id");
            caseReport.ConclusionDate = GetLatestGesCaseReportAudit(caseReportId, "I_GesCaseReportStatuses_Id");
            return new TResult
            {
                CaseProfileId = caseReportId,
                BaseComponent = GetCaseProfileBaseComponent<TResult>(caseReportId, caseReport),
                CaseComponent = GetCaseProfileCaseComponent<TResult>(caseReportId, caseReport),
                IssueComponent = GetCaseProfileIssueComponent<TResult>(caseReportId, caseReport),
                ContactEngagementManager = GetGesContact(caseReportId),
                CaseProfileUNGPAssessmentMethodologyComponent = GetCaseProfileUNGPAssessmentComponent(caseReportId),
                NewI_GesCaseReportStatuses_Id = caseReport.GesCaseReport.NewI_GesCaseReportStatuses_Id
            };
        }

        private ICaseProfileStatusViewModel GetStatusComponent<TResult>(long caseReportId) where TResult : ICaseProfileCoreViewModel
        {
            var caseProfile = (from cr in _dbContext.I_GesCaseReports
                               join r in _dbContext.I_ResponseStatuses on cr.I_ResponseStatuses_Id equals r.I_ResponseStatuses_Id
                                 into rg
                               from r in rg.DefaultIfEmpty()
                               join p in _dbContext.I_ProgressStatuses on cr.I_ProgressStatuses_Id equals p.I_ProgressStatuses_Id
                                 into pg
                               from p in pg.DefaultIfEmpty()
                               where cr.I_GesCaseReports_Id == caseReportId
                               select new
                               {
                                   cr.I_ResponseStatuses_Id,
                                   cr.I_ProgressStatuses_Id,
                                   ReponseName = r == null ? "" : r.ShortName,
                                   ProgressName = p == null ? "" : p.ShortName,
                                   Milestones = cr.I_Milestones
                               }).FirstOrDefault();

            if (caseProfile == null)
                return null;

            if (typeof(TResult) == typeof(CaseProfileFullAttributeViewModel) || typeof(TResult) == typeof(CaseProfileBcEngageViewModel) || typeof(TResult) == typeof(CaseProfileSrViewModel) || typeof(TResult) == typeof(CaseProfileSrEmeViewModel) || typeof(TResult) == typeof(CaseProfileSrGovViewModel))
            {
                var progressStatus = caseProfile.I_ProgressStatuses_Id ?? 0;
                var responseStatus = caseProfile.I_ResponseStatuses_Id ?? 0;
                var latestMilestone = caseProfile.Milestones.OrderByDescending(m => m.MilestoneModified).ThenByDescending(m => m.Created).ToList().FirstOrDefault();

                GesMilestoneTypes milstoneType = null;


                if (latestMilestone != null)
                {
                    milstoneType = _dbContext.GesMilestoneTypes?.Where(t => t.Id == latestMilestone.GesMilestoneTypesId).FirstOrDefault();
                    caseProfile.Milestones.Remove(latestMilestone);
                }

                return new CaseProfileStatusViewModel()
                {
                    Response = responseStatus,
                    ResponseName = caseProfile.ReponseName,
                    Progress = progressStatus,
                    ProgressName = caseProfile.ProgressName,
                    Development = DataHelper.CalcDevelopmentGrade(progressStatus, responseStatus),
                    LatestMilestone = latestMilestone?.Description,
                    LatestMilestoneLevel = milstoneType?.Level ?? 0
                };
            }
            return null;
        }

        public ICaseProfileEngagementInformationViewModel GetEngagementInformationComponent(long caseReportId)
        {
            var caseProfile = _dbContext.I_GesCaseReports.FirstOrDefault(x => x.I_GesCaseReports_Id == caseReportId);
            if (caseProfile == null)
                return null;
            var milestones = caseProfile.I_Milestones.OrderByDescending(m => m.MilestoneModified).ThenByDescending(m => m.Created).ToList();
            var latestMilestone = milestones.FirstOrDefault();

            GesMilestoneTypes milstoneType = null;

            var engagementTypeId = this.SafeExecute(() => _dbContext.I_GesCaseReportsI_EngagementTypes
                .FirstOrDefault(i => i.I_GesCaseReports_Id == caseReportId)?.I_EngagementTypes_Id);

            if (latestMilestone != null && engagementTypeId != (int)EngagementTypeEnum.EmerginMarket)
            {
                milstoneType = _dbContext.GesMilestoneTypes?.Where(t => t.Id == latestMilestone.GesMilestoneTypesId).FirstOrDefault();
                milestones.Remove(latestMilestone);

            }

            return new CaseProfileEngagementInformationViewModel
            {
                ChangeObjective = caseProfile.ProcessGoal,
                LatestMilestone = latestMilestone?.Description,
                LatestMilestoneLevel = milstoneType?.Level ?? 0,
                NextStep = caseProfile.ProcessStep,
                Milestones = milestones,
                ChangeObjectiveDateTime = caseProfile.ProcessGoalModified ?? new DateTime(),
                LatestMilestoneDateTime = latestMilestone?.MilestoneModified ?? new DateTime(),
                NextStepDateTime = caseProfile.ProcessStepUpdated ?? new DateTime(),
                GapAnalysis = caseProfile.GAPAnalysis
            };
        }

        public ICaseProfileIncidentAnalysisComponent GetOpeningIncidentAnalysisComponent(long caseProfileId)
        {
            var caseProfile = GetById(caseProfileId);
            if (caseProfile == null || (caseProfile.IncidentAnalysisSummary == null && caseProfile.IncidentAnalysisConclusion == null && caseProfile.IncidentAnalysisDialogueAndAnalysis == null && caseProfile.IncidentAnalysisGuidelines == null && caseProfile.IncidentAnalysisSources == null))
                return null;

            return new CaseProfileIncidentAnalysisComponent
            {
                IncidentAnalysisSummary = caseProfile.IncidentAnalysisSummary,
                IncidentAnalysisConclusion = caseProfile.IncidentAnalysisConclusion,
                IncidentAnalysisDialogueAndAnalysis = caseProfile.IncidentAnalysisDialogueAndAnalysis,
                IncidentAnalysisGuidelines = caseProfile.IncidentAnalysisGuidelines,
                IncidentAnalysisSources = caseProfile.IncidentAnalysisSources
            };
        }

        public ICaseProfileIncidentAnalysisComponent GetClosingIncidentAnalysisComponent(long caseProfileId, RecommendationType recommendationType)
        {
            var caseProfile = GetById(caseProfileId);
            if (caseProfile == null || (caseProfile.ClosingIncidentAnalysisSummary == null && caseProfile.ClosingIncidentAnalysisConclusion == null && caseProfile.ClosingIncidentAnalysisDialogueAndAnalysis == null))
                return null;

            return new CaseProfileIncidentAnalysisComponent
            {
                IncidentAnalysisSummary = caseProfile.ClosingIncidentAnalysisSummary,
                IncidentAnalysisConclusion = caseProfile.ClosingIncidentAnalysisConclusion,
                IncidentAnalysisDialogueAndAnalysis = caseProfile.ClosingIncidentAnalysisDialogueAndAnalysis,
                RecommendationType = recommendationType
            };
        }

        //TODO truong.pham Refactor code
        public IEnumerable<I_GesCaseReportStatuses> GetRecommendations()
        {
            var listId = new List<long> { 6, 7, 8, 3, 9 };

            return _dbContext.I_GesCaseReportStatuses.Where(d => listId.Contains(d.I_GesCaseReportStatuses_Id)).OrderByDescending(d=>d.SortOrder).FromCache();
        }

        public IEnumerable<I_GesCaseReportStatuses> GetConclusions()
        {
            var listId = new List<long> { 11, 1, 2, 3, 5};

            return _dbContext.I_GesCaseReportStatuses.Where(d => listId.Contains(d.I_GesCaseReportStatuses_Id)).OrderByDescending(d=>d.SortOrder).FromCache();
        }

        public IEnumerable<IssueHeading> GetIssueHeadings()
        {
            return _dbContext.IssueHeading.OrderBy(d => d.Name).FromCache();
        }

        public IEnumerable<IdNameModel> GetEngagementTypes()
        {
            return from e in _dbContext.I_EngagementTypes
                   join ec in _dbContext.I_EngagementTypeCategories on e.I_EngagementTypeCategories_Id equals ec
                       .I_EngagementTypeCategories_Id
                   where e.IsShowInCaseProfileTemplate == true
                   orderby e.SortOrder descending
                   select new IdNameModel
                   {
                       Id = e.I_EngagementTypes_Id,
                       Name = ec.Name + " - " + e.Name
                   };
        }

        public ICaseProfileSdgAndGuidelineConventionComponent GetCaseProfileSdgAndGuidelineConventionComponent(long caseProfileId)
        {
            var caseProfile = GetById(caseProfileId);

            if (caseProfile == null) return null;

            var component = new CaseProfileSdgAndGuidelineConventionComponent
            {
                Conventions = _conventionsRepository.GetConvertionsByCaseReportId(caseProfileId).Select(x => x.Name).ToList(),
                Guidelines = caseProfile.Guidelines,
                Sdgs = GetCaseProfileSdgs(caseProfileId).ToList()
            };

            var ungpAssessment = GetCaseProfileUNGPAssessmentComponent(caseProfileId);

            if (ungpAssessment != null)
            {
                component.SalientHumanRightsPotentialViolationTotalScore = ungpAssessment.SalientHumanRightsPotentialViolationTotalScore;
                component.TotalScoreForCompanyPreparedness = ungpAssessment.TotalScoreForCompanyPreparedness;
            }

            return component;
        }

        public IEnumerable<Sdg> GetCaseProfileSdgs(long caseProfileId)
        {
            var sdgIds = _gesCaseReportSdgRepository.GetSdgIdsByCaseProfile(caseProfileId);
            return _sdgRepository.GetSdgsByOrder(sdgIds);
        }

        public IEnumerable<I_NormAreas> GetNormAreas()
        {
            return _dbContext.I_NormAreas.FromCache();
        }

        public IEnumerable<GesContact> GetUsers()
        {
            var contacts = from u in _dbContext.G_Users
                           join i in _dbContext.G_Individuals on u.G_Individuals_Id equals i.G_Individuals_Id
                           join s in _dbContext.G_UsersG_SecurityGroups on u.G_Users_Id equals s.G_Users_Id
                           where s.G_SecurityGroups_Id == (int)SecurityGroups.Analyst
                           select new GesContact
                           {
                               UserId = u.G_Users_Id,
                               FirstName = i.FirstName,
                               LastName = i.LastName,
                               Email = i.Email,
                               JobTitle = i.JobTitle
                           };

            return contacts.FromCache();
        }

        public IEnumerable<G_DocumentManagementTaxonomies> GetDocumentManagementTaxonomies()
        {
            return _dbContext.G_DocumentManagementTaxonomies.Where(x => x.ParentG_DocumentManagementTaxonomies_Id != null);
        }

        public IEnumerable<G_ManagedDocumentServices> GetManagedDocumentServices()
        {
            return _dbContext.G_ManagedDocumentServices;
        }

        public IEnumerable<I_ResponseStatuses> GetResponseStatuses()
        {
            return _dbContext.I_ResponseStatuses.FromCache();
        }

        public IEnumerable<I_ProgressStatuses> GetProgressStatuses()
        {
            return _dbContext.I_ProgressStatuses.FromCache();
        }

        public ICaseProfileStatisticViewModel GetStatisticComponent(long orgId, long caseProfileId)
        {
            return new CaseProfileStatisticViewModel
            {
                ConferenceCount = _companiesRepository.CountCompanyDialogueByCaseProfile(orgId, caseProfileId, EngagementStatisticType.ConferenceCalls),
                MeetingCount = _companiesRepository.CountCompanyDialogueByCaseProfile(orgId, caseProfileId, EngagementStatisticType.Meetings),
                LatestMeeting = _companiesRepository.GetLatestConferenceAndMeetingTime(orgId, caseProfileId),
                CorrespondenceCount = _companiesRepository.CountCompanyDialogueByCaseProfile(orgId, caseProfileId, EngagementStatisticType.Emails),
                Contacts = _companiesRepository.CountCompanyDialogueByCaseProfile(orgId, caseProfileId, EngagementStatisticType.Contacts)
            };
        }

        public GesContact GetGesContact(long caseProfileId)
        {
            var contacts = from gcr in _dbContext.I_GesCaseReports
                           join u in _dbContext.G_Users on gcr.AnalystG_Users_Id equals u.G_Users_Id
                           join i in _dbContext.G_Individuals on u.G_Individuals_Id equals i.G_Individuals_Id
                           where gcr.I_GesCaseReports_Id == caseProfileId
                           select new GesContact
                           {
                               FirstName = i.FirstName,
                               LastName = i.LastName,
                               Email = (i.Email ?? string.Empty).Replace(Configurations.OldEmailDomain, Configurations.EmailDomain),
                               JobTitle = i.JobTitle
                           };

            return SafeExecute(() => contacts.FirstOrDefault());
        }

        private ICaseProfileIssueComponent GetCaseProfileIssueComponent<TResult>(long caseProfileId, ReportQueryResult q) where TResult : ICaseProfileCoreViewModel
        {
            var latestGesCommentary = q.GesCaseReport.I_GesCommentary.OrderByDescending(d => d.CommentaryModified).FirstOrDefault();
            var latestGSSLink = q.GesCaseReport.I_GSSLink.OrderByDescending(d => d.GSSLinkModified).FirstOrDefault();
            var latestNews = q.GesCaseReport.I_GesLatestNews.OrderByDescending(d => d.LatestNewsModified).FirstOrDefault();
            var alert = GetAlert(caseProfileId);
            var caseProfileIssueComponent = new CaseProfileIssueComponent
            {
                CaseProfileId = caseProfileId,
                //Summary = RemoveLastSentence(q.GesCaseReport.Summary),
                Summary = q.GesCaseReport.Summary,
                Guidelines = q.GesCaseReport.Guidelines,
                CompanyDialogueNew = q.GesCaseReport.CompanyDialogueNew,
                CompanyDialogueSummary = q.GesCaseReport.CompanyDialogueSummary,
                SourceDialogueNew = q.GesCaseReport.SourceDialogueNew,
                SourceDialogueSummary = q.GesCaseReport.SourceDialogueSummary,
                GesCommentary = latestGesCommentary?.Description,
                GesCommentaryModified = latestGesCommentary?.CommentaryModified ?? new DateTime(),
                GSSLink = latestGSSLink?.Description,
                GSSLinkModified = latestGSSLink?.GSSLinkModified ?? new DateTime(),
                LatestNews = latestNews?.Description,
                LatestNewsModified = latestNews?.LatestNewsModified ?? new DateTime(),
                LatestNewsArchive = q.GesCaseReport.I_GesLatestNews.OrderByDescending(d => d.LatestNewsModified).ToList(),
                CompanyPreparedness = q.GesCaseReport.CompanyPreparedness,
                Conclusion = q.GesCaseReport.Conclusion,
                AlertText = alert?.Text,
                AlertDate = alert?.AlertDate,
                AlertSource = GetAlertSource(alert),
                CompanyDialogues = GetCompanyDialogueByCaseProfileId(caseProfileId),
                SourceDialogues = GetSourceDialogueByCaseProfileId(caseProfileId),
                CompanyDialogueNewReviewed = q.GesCaseReport.CompanyDialogueNewReviewed
            };

            if (typeof(TResult) == typeof(CaseProfileBcEvaluateViewModel))
            {
                caseProfileIssueComponent.Conclusion = q.GesCaseReport.Conclusion;
                caseProfileIssueComponent.Summary = RemoveLastSentence(q.GesCaseReport.Summary);
                return caseProfileIssueComponent;
            }

            if (typeof(TResult) == typeof(CaseProfileBcEngageViewModel) || typeof(TResult) == typeof(CaseProfileBcDisengageViewModel))
            {
                var bcEngageOrDisEngageCaseProfileIssueComponent = Mapper.Map<BusinessConductEngageOrDisEngageOrResolveCaseProfileIssueComponent>(caseProfileIssueComponent);
                bcEngageOrDisEngageCaseProfileIssueComponent.Description = q.GesCaseReport.Description;
                bcEngageOrDisEngageCaseProfileIssueComponent.Conclusion = q.GesCaseReport.Conclusion;
                bcEngageOrDisEngageCaseProfileIssueComponent.Summary = RemoveLastSentence(q.GesCaseReport.Summary);
                return bcEngageOrDisEngageCaseProfileIssueComponent;
            }
            if (typeof(TResult) == typeof(CaseProfileBcResolvedViewModel) || typeof(TResult) == typeof(CaseProfileBcArchivedViewModel))
            {
                var bcArchivedResolvedCaseProfileIssueComponent = Mapper.Map<BusinessConductEngageOrDisEngageOrResolveCaseProfileIssueComponent>(caseProfileIssueComponent);
                bcArchivedResolvedCaseProfileIssueComponent.Description = q.GesCaseReport.Description;
                bcArchivedResolvedCaseProfileIssueComponent.Summary = RemoveLastSentence(q.GesCaseReport.Summary);
                return bcArchivedResolvedCaseProfileIssueComponent;
            }
            if (typeof(TResult) == typeof(CaseProfileSrEmeViewModel))
            {
                var srEmeCaseProfileIssueComponent = Mapper.Map<SrEmeCaseProfileIssueComponent>(caseProfileIssueComponent);
                srEmeCaseProfileIssueComponent.MostMaterialRisk = q.Company.MostMaterialRisk;
                srEmeCaseProfileIssueComponent.Summary = RemoveLastSentence(q.GesCaseReport.Summary);
                return srEmeCaseProfileIssueComponent;
            }

            if (typeof(TResult) == typeof(CaseProfileStandardViewModel))
            {
                caseProfileIssueComponent.ConclusionObs = q.GesCaseReport.ConclusionObs;
                caseProfileIssueComponent.Conclusion = q.GesCaseReport.Conclusion;
                caseProfileIssueComponent.Summary = RemoveLastSentence(q.GesCaseReport.Summary);
                caseProfileIssueComponent.Description = q.GesCaseReport.Description;
                return caseProfileIssueComponent;
            }

            if (typeof(TResult) == typeof(CaseProfileFullAttributeViewModel))
            {
                var fullAttributeCaseProfileIssueComponent = Mapper.Map<FullAttributeCaseProfileIssueComponent>(caseProfileIssueComponent);
                fullAttributeCaseProfileIssueComponent.MostMaterialRisk = q.Company.MostMaterialRisk;
                fullAttributeCaseProfileIssueComponent.Description = q.GesCaseReport.Description;
                fullAttributeCaseProfileIssueComponent.ConclusionObs = q.GesCaseReport.ConclusionObs;
                fullAttributeCaseProfileIssueComponent.Conclusion = q.GesCaseReport.Conclusion;
                fullAttributeCaseProfileIssueComponent.Summary = RemoveLastSentence(q.GesCaseReport.Summary);
                return fullAttributeCaseProfileIssueComponent;
            }
            return caseProfileIssueComponent;
        }

        private string RemoveLastSentence(string source)
        {
            if (string.IsNullOrEmpty(source))
            {
                return source;
            }
            var result = source.Trim();
            if (result.Length > 2 && result.Substring(result.Length - 1) == ".")
            {
                result = result.Substring(0, result.Length - 2);
            }
            if (result.Length > 0 && result.Contains("."))
            {
                var lastSentence = result.Substring(result.LastIndexOf(".", StringComparison.Ordinal + 1));
                if (lastSentence.Contains("The reported practices"))
                {
                    result = result.Substring(0, result.LastIndexOf(".", StringComparison.Ordinal) + 1);
                }
                else
                {
                    result = source.Trim();
                }
            }
            else
            {
                return source;
            }
            return result;
        }

        private string GetAlertSource(AlertViewModel alert)
        {
            var alertSource = "";
            if (alert != null)
            {
                alertSource = alert.Source;
                if (alert.SourceDate != null)
                {
                    alertSource += ", " + alert.SourceDate.Value.ToString(Configurations.DateFormat);
                }
            }
            return alertSource;
        }

        private ICaseProfileCaseComponent GetCaseProfileCaseComponent<TResult>(long caseProfileId, ReportQueryResult q) where TResult : ICaseProfileCoreViewModel
        {
            var recommendationDate = q.RecommendationDate?.ToString(Configurations.DateFormat) ?? string.Empty;
            var conclusionDate = q.ConclusionDate?.ToString(Configurations.DateFormat) ?? "";
            var conclusionHistory = GetConclusionHistory(caseProfileId);
            var caseProfileCaseComponent = new CaseProfileCaseComponent
            {
                CaseProfileId = caseProfileId,
                AlertEntryDate = q.GesCaseReport.EntryDate ?? q.GesCaseReport.Created,
                Heading = q.GesCaseReport.ReportIncident,
                EngagementTheme = DataHelper.GetEngagementTheme(q.EngagementTypeCategoryName, q.EngagementTypeName),
                Norm = DataHelper.GetEngagementNorm(q.NormArea?.Name, q.EngagementTypeName, q.EngagementTypeId),
                NormId = q.GesCaseReport.I_NormAreas_Id ?? 0,
                Location = q.Location?.Name,
                CountryCode = q.Location?.Alpha3Code?.ToLower()?.Substring(0,2),
                Recommendation = !string.IsNullOrEmpty(recommendationDate) && q.Recommendation != null ? $"{q.Recommendation.Name.Replace("Resolved (Indication of Violation)", "Resolved")} - {recommendationDate}" : q.Recommendation?.Name.Replace("Resolved (Indication of Violation)", "Resolved")??"",
                Conclusion = !string.IsNullOrEmpty(conclusionDate) ? $"{q.Conclusion?.Name} - {conclusionDate}" : q.Conclusion?.Name,
                RecommendationArchive = GetRecommendationHistory(caseProfileId),
                ConclusionArchive = conclusionHistory,
                ConclusionId = q.Conclusion?.I_GesCaseReportStatuses_Id
            };

            var confirmed = conclusionHistory.Any(d => d.Id == (long)GesCaseReportStatus.ConfirmedViolation);

            var confirmViolation = q.GesCaseReport.I_GesCaseReportStatuses_Id == (long)GesCaseReportStatus.ConfirmedViolation;

            caseProfileCaseComponent.ConfirmedViolation = confirmViolation || confirmed;

            caseProfileCaseComponent.IndicationOfViolation = q.GesCaseReport.I_GesCaseReportStatuses_Id == (long)GesCaseReportStatus.IndicationOfViolation;

            if (typeof(TResult) == typeof(CaseProfileBcEvaluateViewModel))
            {
                var bcEvaluateCaseProfileCaseComponent = Mapper.Map<BcEvaluateCaseProfileCaseComponent>(caseProfileCaseComponent);
                return bcEvaluateCaseProfileCaseComponent;
            }
            if (typeof(TResult) == typeof(CaseProfileBcEngageViewModel))
            {
                var bcEngageCaseProfileCaseComponent = Mapper.Map<BcEngageCaseProfileCaseComponent>(caseProfileCaseComponent);
                bcEngageCaseProfileCaseComponent.ConfirmedViolationDate = confirmViolation ? q.GesCaseReport.I_GesCaseReportStatuses_IdReviewed?.ToString(Configurations.DateFormat) : "";
                bcEngageCaseProfileCaseComponent.StatusComponent = GetStatusComponent<TResult>(caseProfileId);
                return bcEngageCaseProfileCaseComponent;
            }
            if (typeof(TResult) == typeof(CaseProfileBcDisengageViewModel))
            {
                var bcDisEngageCaseProfileCaseComponent = Mapper.Map<BcDisEngageCaseProfileCaseComponent>(caseProfileCaseComponent);
                bcDisEngageCaseProfileCaseComponent.ConfirmedViolationDate = confirmViolation ? q.GesCaseReport.I_GesCaseReportStatuses_IdReviewed?.ToString(Configurations.DateFormat) : "";
                return bcDisEngageCaseProfileCaseComponent;
            }
            if (typeof(TResult) == typeof(CaseProfileBcArchivedViewModel))
            {
                var bcDisEngageCaseProfileCaseComponent = Mapper.Map<BcArchivedCaseProfileCaseComponent>(caseProfileCaseComponent);
                bcDisEngageCaseProfileCaseComponent.ConfirmedViolationDate = confirmViolation ? q.GesCaseReport.I_GesCaseReportStatuses_IdReviewed?.ToString(Configurations.DateFormat) : "";
                return bcDisEngageCaseProfileCaseComponent;
            }
            if (typeof(TResult) == typeof(CaseProfileSrViewModel) || typeof(TResult) == typeof(CaseProfileSrEmeViewModel) || typeof(TResult) == typeof(CaseProfileSrGovViewModel) || typeof(TResult) == typeof(CaseProfileBespokeViewModel) || typeof(TResult) == typeof(CaseProfileGenerationViewModel))
            {
                var srCaseProfileCaseComponent = Mapper.Map<SrCaseProfileCaseComponent>(caseProfileCaseComponent);

                srCaseProfileCaseComponent.NormArea = q.NormArea?.Name;
                srCaseProfileCaseComponent.StatusComponent = GetStatusComponent<TResult>(caseProfileId);

                if (typeof(TResult) == typeof(CaseProfileSrGovViewModel))
                {
                    srCaseProfileCaseComponent.Theme = GetGovernanceCaseThemeByIssueName(q.GesCaseReport.ReportIncident);
                    srCaseProfileCaseComponent.EngagementTheme = Resources.CorporateGovernanceType;
                }
                else
                {
                    srCaseProfileCaseComponent.Theme = q.EngagementTypeName?.Replace("Engagement", "").Trim();
                }
                return srCaseProfileCaseComponent;
            }

            if (typeof(TResult) == typeof(CaseProfileStandardViewModel))
            {
                caseProfileCaseComponent.ConfirmedViolationDate = confirmViolation ? q.GesCaseReport.I_GesCaseReportStatuses_IdReviewed?.ToString(Configurations.DateFormat) : "";
            }

            if (typeof(TResult) == typeof(CaseProfileFullAttributeViewModel))
            {
                var caseProfileFullAttributeViewModel = Mapper.Map<FullAttributeCaseProfileCaseComponent>(caseProfileCaseComponent);
                caseProfileFullAttributeViewModel.ConfirmedViolationDate = confirmViolation ? q.GesCaseReport.I_GesCaseReportStatuses_IdReviewed?.ToString(Configurations.DateFormat) : "";
                caseProfileFullAttributeViewModel.StatusComponent = GetStatusComponent<TResult>(caseProfileId);
                caseProfileFullAttributeViewModel.NormArea = q.NormArea?.Name;
                caseProfileFullAttributeViewModel.Theme = q.EngagementTypeName?.Replace("Engagement", "")?.Replace("engagement", "").Trim();
                return caseProfileFullAttributeViewModel;
            }

            return caseProfileCaseComponent;
        }

        private string GetGovernanceCaseThemeByIssueName(string issueName)
        {
            const string ongoingEngagement = "Ongoing engagement";
            const string preAgmRelated = "Pre-AGM engagement";
            const string agmRelated = "AGM-related";
            const string agm = "AGM";

            if (issueName.ToLower().Trim().StartsWith(ongoingEngagement.ToLower()))
            {
                return ongoingEngagement;
            }
            if (issueName.ToLower().Trim().StartsWith(preAgmRelated.ToLower()) || issueName.ToLower().Trim().StartsWith(agm.ToLower()))
            {
                return agmRelated;
            }
            return string.Empty;
        }

        private static ICaseProfileBaseComponent GetCaseProfileBaseComponent<TResult>(long caseReportId, ReportQueryResult q) where TResult : ICaseProfileCoreViewModel
        {
            var baseComponent = new CaseProfileBaseComponent
            {
                CaseProfileId = caseReportId,
                CompanyId = q.Company.I_Companies_Id,
                CompanyName = q.Company.Name,
                CompanyIsin = q.Company.Isin,
                CompanyIndustry = q.Msci?.Name,
                CompanyHomeCountry = q.Country?.Name,
                CompanyHomeCountryCode = q.Country?.Alpha3Code?.ToLower()
            };            

            if (typeof(TResult) == typeof(CaseProfileSrEmeViewModel) || typeof(TResult) == typeof(CaseProfileFullAttributeViewModel))
            {
                var srEmebaseComponent = Mapper.Map<FullAttributeCaseProfileBaseComponent>(baseComponent);
                srEmebaseComponent.Gri = q.Company.Gri.HasValue && q.Company.Gri.Value;
                srEmebaseComponent.GlobalCompactMember = q.Company.IsGlobalCompactMember;
                return srEmebaseComponent;
            }

            return baseComponent;
        }

        private IList<DialogueModel> GetCompanyDialogueByCaseProfileId(long caseProfileId)
        {
            var result = from dialogue in _dbContext.I_GesCompanyDialogues
                         from document in _dbContext.G_ManagedDocuments.Where(document => document.G_ManagedDocuments_Id == dialogue.G_ManagedDocuments_Id).DefaultIfEmpty()
                         from contactType in _dbContext.I_ContactTypes.Where(contactType => contactType.I_ContactTypes_Id == dialogue.I_ContactTypes_Id).DefaultIfEmpty()
                         from individual in _dbContext.G_Individuals.Where(individual => individual.G_Individuals_Id == dialogue.G_Individuals_Id).DefaultIfEmpty()
                         from upload in _dbContext.G_Uploads.Where(upload => upload.G_ManagedDocuments_Id != null && upload.G_ManagedDocuments_Id == document.G_ManagedDocuments_Id).DefaultIfEmpty()
                         where dialogue.ShowInCsc && dialogue.I_GesCaseReports_Id == caseProfileId
                         orderby dialogue.ContactDate descending
                         select new DialogueModel
                         {
                             ContactDate = dialogue.ContactDate,
                             ContactDirectionId = dialogue.I_ContactDirections_Id,
                             ContactTypeId = contactType.I_ContactTypes_Id,
                             ContactTypeName = contactType.Name,
                             JobTitle = individual.JobTitle,
                             FirstName = individual.FirstName,
                             LastName = individual.LastName,
                             FileName = upload.FileName,
                             ClassA = dialogue.ClassA
                         };
            return SafeExecute(() => result.ToList());
        }

        private IList<DialogueModel> GetSourceDialogueByCaseProfileId(long caseProfileId)
        {
            var result = from dialogue in _dbContext.I_GesSourceDialogues
                         from individual in _dbContext.G_Individuals.Where(individual => individual.G_Individuals_Id == dialogue.G_Individuals_Id)
                         from organization in _dbContext.G_Organizations.Where(organization => organization.G_Organizations_Id == individual.G_Organizations_Id)
                         from contactType in _dbContext.I_ContactTypes.Where(contactType => contactType.I_ContactTypes_Id == dialogue.I_ContactTypes_Id).DefaultIfEmpty()
                         from document in _dbContext.G_ManagedDocuments.Where(document => document.G_ManagedDocuments_Id == dialogue.G_ManagedDocuments_Id).DefaultIfEmpty()
                         from upload in _dbContext.G_Uploads.Where(upload => upload.G_ManagedDocuments_Id != null && upload.G_ManagedDocuments_Id == document.G_ManagedDocuments_Id).DefaultIfEmpty()
                         where dialogue.ShowInCsc && dialogue.I_GesCaseReports_Id == caseProfileId
                         orderby dialogue.ContactDate descending
                         select new DialogueModel
                         {
                             ContactDate = dialogue.ContactDate,
                             ContactDirectionId = dialogue.I_ContactDirections_Id,
                             ContactTypeName = contactType.Name,
                             JobTitle = individual.JobTitle,
                             FirstName = individual.FirstName,
                             LastName = individual.LastName,
                             FileName = upload.FileName,
                             ClassA = dialogue.ClassA
                         };
            return SafeExecute(() => result.ToList());
        }

        public IList<DialogueEditModel> GetCompanyDialogueLogsByCaseProfileId(long caseProfileId)
        {
            var result = from dialogue in _dbContext.I_GesCompanyDialogues
                         from document in _dbContext.G_ManagedDocuments.Where(document => document.G_ManagedDocuments_Id == dialogue.G_ManagedDocuments_Id).DefaultIfEmpty()
                         from contactType in _dbContext.I_ContactTypes.Where(contactType => contactType.I_ContactTypes_Id == dialogue.I_ContactTypes_Id).DefaultIfEmpty()
                         from contactDirection in _dbContext.I_ContactDirections.Where(contactDirection => contactDirection.I_ContactDirections_Id == dialogue.I_ContactDirections_Id).DefaultIfEmpty()
                         from individual in _dbContext.G_Individuals.Where(individual => individual.G_Individuals_Id == dialogue.G_Individuals_Id).DefaultIfEmpty()
                         from upload in _dbContext.G_Uploads.Where(upload => upload.G_ManagedDocuments_Id != null && upload.G_ManagedDocuments_Id == document.G_ManagedDocuments_Id).DefaultIfEmpty()
                         where dialogue.ShowInCsc && dialogue.I_GesCaseReports_Id == caseProfileId
                         orderby dialogue.ContactDate descending
                         select new DialogueEditModel
                         {
                             I_GesCompanySourceDialogues_Id = dialogue.I_GesCompanyDialogues_Id,
                             ContactDate = dialogue.ContactDate,
                             ContactDirectionId = dialogue.I_ContactDirections_Id, 
                             ContactDirectionName = contactDirection.Name,
                             //SendNotifications = dialogue.SendNotifications,
                             ContactTypeId = contactType.I_ContactTypes_Id,
                             ContactTypeName = contactType.Name,
                             JobTitle = individual.JobTitle,
                             FirstName = individual.FirstName,
                             LastName = individual.LastName,
                             ContactFullName = individual.FirstName + " " + individual.LastName,
                             FileName = upload.FileName,
                             ClassA = dialogue.ClassA,
                             G_Individuals_Id = individual.G_Individuals_Id,
                             Action = dialogue.Action,
                             Notes = dialogue.Notes,
                             Text = dialogue.Text,
                             G_ManagedDocuments_Id = dialogue.G_ManagedDocuments_Id,
                             ShowInCsc = dialogue.ShowInCsc,
                             ShowInReport = dialogue.ShowInReport,
                             DialogueType="Company"
                         };
            return SafeExecute(() => result.ToList());
        }

        public IList<DialogueEditModel> GetSourceDialogueLogsByCaseProfileId(long caseProfileId)
        {
            var result = from dialogue in _dbContext.I_GesSourceDialogues
                         from individual in _dbContext.G_Individuals.Where(individual => individual.G_Individuals_Id == dialogue.G_Individuals_Id)
                         from organization in _dbContext.G_Organizations.Where(organization => organization.G_Organizations_Id == individual.G_Organizations_Id)
                         from contactType in _dbContext.I_ContactTypes.Where(contactType => contactType.I_ContactTypes_Id == dialogue.I_ContactTypes_Id).DefaultIfEmpty()
                         from contactDirection in _dbContext.I_ContactDirections.Where(contactDirection => contactDirection.I_ContactDirections_Id == dialogue.I_ContactDirections_Id).DefaultIfEmpty()
                         from document in _dbContext.G_ManagedDocuments.Where(document => document.G_ManagedDocuments_Id == dialogue.G_ManagedDocuments_Id).DefaultIfEmpty()
                         from upload in _dbContext.G_Uploads.Where(upload => upload.G_ManagedDocuments_Id != null && upload.G_ManagedDocuments_Id == document.G_ManagedDocuments_Id).DefaultIfEmpty()
                         where dialogue.ShowInCsc && dialogue.I_GesCaseReports_Id == caseProfileId
                         orderby dialogue.ContactDate descending
                         select new DialogueEditModel
                         {
                             I_GesCompanySourceDialogues_Id = dialogue.I_GesSourceDialogues_Id,
                             I_GesCaseReports_Id = dialogue.I_GesCaseReports_Id,
                             ContactDate = dialogue.ContactDate,
                             ContactDirectionId = dialogue.I_ContactDirections_Id,
                             ContactDirectionName = contactDirection.Name,
                             ContactTypeId = contactType.I_ContactTypes_Id,
                             ContactTypeName = contactType.Name,
                             JobTitle = individual.JobTitle,
                             FirstName = individual.FirstName,
                             LastName = individual.LastName,
                             ContactFullName = individual.FirstName + " " + individual.LastName,
                             FileName = upload.FileName,
                             ClassA = dialogue.ClassA,
                             G_Individuals_Id = individual.G_Individuals_Id,
                             Action = dialogue.Action,
                             Notes = dialogue.Notes,
                             Text = dialogue.Text,
                             G_ManagedDocuments_Id = dialogue.G_ManagedDocuments_Id,
                             ShowInCsc = dialogue.ShowInCsc,
                             ShowInReport = dialogue.ShowInReport,
                             DialogueType = "Source"
                         };
            return SafeExecute(() => result.ToList());
        }

        private ReportQueryResult GetCaseReport(long caseReportId)
        {
            return (from gcr in _dbContext.I_GesCaseReports
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
                    join gcommentary in _dbContext.I_GesCommentary on gcr.I_GesCaseReports_Id equals gcommentary.I_GesCaseReports_Id into gcg
                    from gcommentary in gcg.DefaultIfEmpty()
                    join recommendation in _dbContext.I_GesCaseReportStatuses on gcr.NewI_GesCaseReportStatuses_Id equals recommendation.I_GesCaseReportStatuses_Id into rdm
                    from recommendation in rdm.DefaultIfEmpty()

                    join conclusion in _dbContext.I_GesCaseReportStatuses on gcr.I_GesCaseReportStatuses_Id equals conclusion.I_GesCaseReportStatuses_Id into clsg
                    from conclusion in clsg.DefaultIfEmpty()

                    join cret in _dbContext.I_GesCaseReportsI_EngagementTypes on gcr.I_GesCaseReports_Id equals cret.I_GesCaseReports_Id
                    join et in _dbContext.I_EngagementTypes on cret.I_EngagementTypes_Id equals et.I_EngagementTypes_Id into etg
                    from et in etg.DefaultIfEmpty()
                    join etc in _dbContext.I_EngagementTypeCategories on et.I_EngagementTypeCategories_Id equals etc.I_EngagementTypeCategories_Id into etcg
                    from etc in etcg.DefaultIfEmpty()
                    where gcr.I_GesCaseReports_Id == caseReportId
                    select new ReportQueryResult()
                    {
                        GesCaseReport = gcr,
                        Company = c,
                        Msci = m,
                        Country = hc,
                        Location = l,
                        NormArea = n,
                        GesCommentary = gcommentary,
                        Recommendation = recommendation,
                        ConclusionDate = gcr.I_GesCaseReportStatuses_IdReviewed,
                        EngagementTypeCategoryName = etc.Name,
                        EngagementTypeName = et.Name,
                        EngagementTypeId = et.I_EngagementTypes_Id,
                        Conclusion = conclusion
                    }).FirstOrDefault();
        }

        public IQueryable<CaseReportListViewModel> GetCaseProfiles(long? companyId)
        {
            var companies = from c in _dbContext.I_Companies
                            join gc in _dbContext.I_GesCompanies on c.I_Companies_Id equals gc.I_Companies_Id
                            join rp in _dbContext.I_GesCaseReports on gc.I_GesCompanies_Id equals rp.I_GesCompanies_Id
                            join l in _dbContext.Countries on rp.CountryId equals l.Id into lg
                            from l in lg.DefaultIfEmpty()
                            join r in _dbContext.I_GesCaseReportStatuses on rp.NewI_GesCaseReportStatuses_Id equals r.I_GesCaseReportStatuses_Id into rdm
                            from r in rdm.DefaultIfEmpty()
                            join cret in _dbContext.I_GesCaseReportsI_EngagementTypes on rp.I_GesCaseReports_Id equals cret.I_GesCaseReports_Id
                            join et in _dbContext.I_EngagementTypes on cret.I_EngagementTypes_Id equals et.I_EngagementTypes_Id into etg
                            from et in etg.DefaultIfEmpty()
                            join etc in _dbContext.I_EngagementTypeCategories on et.I_EngagementTypeCategories_Id equals etc.I_EngagementTypeCategories_Id into etcg
                            from etc in etcg.DefaultIfEmpty()
                            join n in _dbContext.I_NormAreas on rp.I_NormAreas_Id equals n.I_NormAreas_Id into ng
                            from n in ng.DefaultIfEmpty()

                            where (companyId == null || companyId == -1 || c.I_Companies_Id == companyId) && (rp.I_NormAreas_Id == null || rp.I_NormAreas_Id != 6)
                            select new CaseReportListViewModel
                            {
                                Id = rp.I_GesCaseReports_Id,
                                IssueName = rp.ReportIncident,
                                CompanyId = c.I_Companies_Id,
                                CompanyName = c.Name,
                                EntryDate = rp.EntryDate,
                                Location = l.Name,
                                Recommendation = r.Name.Replace("Resolved (Indication of Violation)", "Resolved"),
                                IssueHeadingId = rp.IssueHeadingId,
                                EngagementThemeNorm = (!etc.Name.ToLower().Contains("conduct")
                                    ? etc.Name.Replace("Engagement", "")
                                    : et.Name.ToLower().Contains("extended") ? et.Name : "Business Conduct").Trim()
                                    + " - " + (cret.I_EngagementTypes_Id == (int)EngagementTypeEnum.Conventions ? n.Name : et.Name.Replace("Engagement", "")).Trim()
                            };
            return companies;
        }


        private long? GetCaseProfileEngagementTypeId(long caseProfileId)
        {
            var caseProfileEngagementTypes = from rp in _dbContext.I_GesCaseReports
                                             join cret in _dbContext.I_GesCaseReportsI_EngagementTypes on rp.I_GesCaseReports_Id equals cret.I_GesCaseReports_Id
                                             join et in _dbContext.I_EngagementTypes on cret.I_EngagementTypes_Id equals et.I_EngagementTypes_Id
                                             where rp.I_GesCaseReports_Id == caseProfileId
                                             select et.I_EngagementTypes_Id;

            if (caseProfileEngagementTypes.Any())
            {
                return caseProfileEngagementTypes.FirstOrDefault();
            }
            return null;
        }

        public List<PublishedComponent> GetRecommendationHistory(long caseProfileId)
        {
            var recommendations = GetRecommendationsHistory(caseProfileId);

            var result = from r in recommendations
                select new PublishedComponent
                {
                    Content = r.NewValueName,
                    PublishedDate = r.AuditDatetime
                };
            return result.OrderByDescending(d => d.PublishedDate).ToList();
        }

        public bool HasConfirmedConclusion(long caseProfileId)
        {
            var conclusions = GetConclusionHistory(caseProfileId);
            return conclusions != null && conclusions.Any(x => x.Id == (long)ConclusionType.ConfirmedViolation);
        }

        public void UpdateCaseReportConfirmed(IList<CaseReportListViewModel> caseProfiles)
        {
            if (caseProfiles != null && caseProfiles.Count > 0)
            {
                foreach (var caseReport in caseProfiles)
                {
                    if (!(caseReport.Confirmed.HasValue && caseReport.Confirmed.Value))
                    {
                        caseReport.Confirmed = HasConfirmedConclusion(caseReport.Id);
                    }
                }
            }
        }

        public void UpdateCaseReportKPI(IList<CaseReportListViewModel> caseProfiles)
        {
            if (caseProfiles != null && caseProfiles.Count > 0)
            {
                foreach (var caseReport in caseProfiles)
                {
                    if (caseReport.EngagementTypeCategoriesId != null && caseReport.EngagementTypeCategoriesId.Value == (long)EngagementTypeCategoryEnum.StewardshipAndRisk
                        && caseReport.EngagementTypeId != null && caseReport.EngagementTypeId.Value != (long)EngagementTypeEnum.EmerginMarket
                        && caseReport.EngagementTypeId.Value != (long)EngagementTypeEnum.Governance)
                    {
                        caseReport.KPIs = GetCaseProfileKpis(caseReport.Id);
                    }
                }
            }
        }

        public IEnumerable<GesMilestoneTypes> AllMilestoneTypes()
        {
            return _dbContext.GesMilestoneTypes.FromCache();
        }

        public MilestoneModel GetLatestMilestoneModelByCaseProfileId(long id)
        {
            var milestone = _dbContext.I_Milestones.Where(x => x.I_GesCaseReports_Id == id)
                .OrderByDescending(x => x.Created)
                .FirstOrDefault();

            if (milestone == null) return null;

            var milestoneModel = new MilestoneModel()
            {
                MilestoneId = milestone.I_Milestones_Id,
                MilestoneDescription = milestone.Description,
                MilestoneModified = milestone.MilestoneModified,
                MileStoneCreated = milestone.Created,
                GesMilestoneTypesId = milestone.GesMilestoneTypesId
            };

            return milestoneModel;

        }


        public void UpdateCaseReportInformation(IList<CaseReportListViewModel> caseProfiles, long orgId, long gesCompanyId)
        {
            if (caseProfiles != null && caseProfiles.Count > 0)
            {
                foreach (var caseReport in caseProfiles)
                {
                    if (!(caseReport.Confirmed.HasValue && caseReport.Confirmed.Value))
                    {
                        caseReport.Confirmed = HasConfirmedConclusion(caseReport.Id);
                    }
                    caseReport.IsUnsubscribed = !HasSubscribed(orgId, gesCompanyId, caseReport.Id);
                }
            }
        }

        public void UpdateCaseReportSubscribed(IList<CaseReportListViewModel> caseProfiles, long orgId, long gesCompanyId)
        {
            foreach (var caseReport in caseProfiles)
            {
                caseReport.IsUnsubscribed = !HasSubscribed(orgId, gesCompanyId, caseReport.Id);
            }
        }

        public bool HasSubscribed(long orgId, long gesCompanyId, long caseProfileId)
        {
            var engagementTypeId = GetCaseProfileEngagementTypeId(caseProfileId);
            return HasSubscribed(orgId, gesCompanyId, engagementTypeId, caseProfileId);
        }

        private bool HasSubscribed(long orgId, long gesCompanyId, long? engagementTypeId, long caseProfileId)
        {
            var filteredCaseProfile = from po in _dbContext.I_PortfoliosG_Organizations //check services in portfolio-services
                                      join pos in _dbContext.I_PortfoliosG_OrganizationsG_Services on po.I_PortfoliosG_Organizations_Id equals pos
                                          .I_PortfoliosG_Organizations_Id
                                      join g in _dbContext.G_Services on pos.G_Services_Id equals g.G_Services_Id
                                      join et in _dbContext.I_EngagementTypes on g.I_EngagementTypes_Id equals et.I_EngagementTypes_Id
                                      join pc in _dbContext.I_PortfoliosI_Companies on po.I_Portfolios_Id equals pc.I_Portfolios_Id
                                      join c in _dbContext.I_Companies on pc.I_Companies_Id equals c.I_Companies_Id
                                      join gc in _dbContext.I_GesCompanies on c.MasterI_Companies_Id != null
                                          ? c.MasterI_Companies_Id.Value
                                          : c.I_Companies_Id equals gc.I_Companies_Id
                                      join rp in _dbContext.I_GesCaseReports on gc.I_GesCompanies_Id equals rp.I_GesCompanies_Id
                                      where po.G_Organizations_Id == orgId && gc.I_GesCompanies_Id == gesCompanyId && rp.ShowInClient
                                      group rp by new
                                      {
                                          rp.I_GesCaseReports_Id,
                                          et.I_EngagementTypes_Id
                                      }
                into g
                                      select new
                                      {
                                          g.Key.I_GesCaseReports_Id,
                                          g.Key.I_EngagementTypes_Id
                                      };

            var availableEngagementType = from s in _dbContext.G_Services
                                          join os in _dbContext.G_OrganizationsG_Services on s.G_Services_Id equals os.G_Services_Id
                                          where os.G_Organizations_Id == orgId
                                          select s.I_EngagementTypes_Id;

            var filterBCOrSRServices = availableEngagementType.Any(x => x == engagementTypeId)
                   && filteredCaseProfile.Any(x => x.I_GesCaseReports_Id == caseProfileId && x.I_EngagementTypes_Id == engagementTypeId);

            if (filterBCOrSRServices) return true;

            //Check for Global ethical standard
            var filterGlobalStandardServices = from gs in _dbContext.G_OrganizationsG_Services
                                               where gs.G_Organizations_Id == orgId
                                               select gs;

            return filterGlobalStandardServices.Any(x => x.G_Services_Id == (long)GesService.GesGlobalEthicalStandard)
                && (engagementTypeId == (long)EngagementTypeEnum.Conventions);
        }

        public IEnumerable<KpiViewModel> GetCaseProfileKpis(long caseProfileId)
        {
            return SafeExecute(() =>
            {
                var kpis = from ck in _dbContext.I_GesCaseReportsI_Kpis
                           join k in _dbContext.I_Kpis on ck.I_Kpis_Id equals k.I_Kpis_Id
                           join kp in _dbContext.I_KpiPerformance on ck.I_KpiPerformance_Id equals kp.I_KpiPerformance_Id
                           where ck.I_GesCaseReports_Id == caseProfileId
                           select new KpiViewModel
                           {
                               KpiId = ck.I_Kpis_Id,
                               KpiDescription = k.Description,
                               KpiName = k.Name,
                               KpiPerformance = kp.Name
                           };

                return kpis;
            });
        }

        public List<PublishedComponent> GetConclusionHistory(long caseProfileId)
        {
            var recommendations = GetConclusionsHistory(caseProfileId);

            var result = from r in recommendations
                select new PublishedComponent
                {
                    Content = r.NewValueName,
                    PublishedDate = r.AuditDatetime,
                    Id = ConvertStringToLong(r.NewValue)
                };
            return result.OrderByDescending(d => d.PublishedDate).ToList();
        }

        private long? ConvertStringToLong(string input)
        {
            if (!string.IsNullOrEmpty(input))
            {
                long result;
                if (long.TryParse(input, out result))
                {
                    return result;
                }
            }
            return null;
        }

        public List<ReferenceViewModel> GetCaseReportReferenceses(long caseProfileId)
        {
            var references = from r in _dbContext.I_GesCaseReportReferences
                             join s in _dbContext.I_GesCaseReportAvailabilityStatuses on r.I_GesCaseReportAvailabilityStatuses_Id equals s.I_GesCaseReportAvailabilityStatuses_Id
                             where r.I_GesCaseReports_Id == caseProfileId && r.ShowInReport
                             select new ReferenceViewModel
                             {
                                 AccessedDate = r.Accessed,
                                 AvailableFrom = r.AvailableFrom,
                                 G_GesCaseReport_Id = r.I_GesCaseReports_Id,
                                 GesCaseReportAvailabilityStatusesText = s.Name,
                                 I_GesCaseReportReferences_Id = r.I_GesCaseReportReferences_Id,
                                 Source = r.Source,
                                 DocumentName = r.G_ManagedDocuments != null ? r.G_ManagedDocuments.Name : string.Empty,
                                 PublicationYear = r.PublicationYear
                             };

            return references.OrderByDescending(x => x.PublicationYear).ThenByDescending(x => x.AccessedDate).ToList();
        }

        public List<RevisionViewModel> GetRevisionCriterials(long caseProfileId)
        {
            var revisionIds = new List<long> { 1, 2, 4 };
            var references = from r in _dbContext.I_GesCaseReportsI_GesRevisionTexts
                             join t in _dbContext.I_GesRevisionTexts on r.I_GesRevisionTexts_Id equals t.I_GesRevisionTexts_Id
                             where r.I_GesCaseReports_Id == caseProfileId && revisionIds.Contains(t.I_GesRevisionTexts_Id)
                             select new RevisionViewModel
                             {
                                 I_GesCaseReportsI_GesRevisionTexts_Id = r.I_GesCaseReportsI_GesRevisionTexts_Id,
                                 I_GesRevisionTexts_Id = r.I_GesRevisionTexts_Id,
                                 GesRevisionText = t.Text,
                                 Checked = r.Checked,
                                 DateText = r.DateText,
                                 Description = r.Description,
                                 G_GesCaseReport_Id = r.I_GesCaseReports_Id
                             };

            return references.OrderBy(d => d.I_GesRevisionTexts_Id).ToList();
        }

        public ConfirmationInformationViewModel GetConfirmationInformation(long caseProfileId)
        {

            return new ConfirmationInformationViewModel
            {
                SourceType = GetSourceType(caseProfileId),
                MainSource = GetMainSources(caseProfileId),
                Document = GetDocuments(caseProfileId)
            };
        }

        private string GetSourceType(long caseProfileId)
        {
            return (from r in _dbContext.I_GesCaseReports
                    join s in _dbContext.I_GesCaseReportSourceTypes on r.I_GesCaseReportSourceTypes_Id equals s
                        .I_GesCaseReportSourceTypes_Id
                    into sg
                    from s in sg.DefaultIfEmpty()
                    where r.I_GesCaseReports_Id == caseProfileId
                    select s.Name
            ).FirstOrDefault();
        }

        private string GetMainSources(long caseProfileId)
        {
            var source = (from r in _dbContext.I_GesCaseReportSources
                          where r.I_GesCaseReports_Id == caseProfileId && r.MainSource == true
                          select r.PublicationYear != null ? r.Source + " (" + r.PublicationYear.ToString() + ") " : r.Source
            ).ToList();

            return string.Join("; ", source);
        }

        private string GetDocuments(long caseProfileId)
        {
            var documents = (from r in _dbContext.I_GesCaseReportSources
                             join d in _dbContext.G_ManagedDocuments on r.G_ManagedDocuments_Id equals d.G_ManagedDocuments_Id
                             where r.I_GesCaseReports_Id == caseProfileId && r.MainSource == true && d.Name != null
                             select d.Name
            ).ToList();

            return string.Join("; ", documents);
        }

        public IEnumerable<TimeLineModel> GetTimeLines(long caseProfileId)
        {
            var timeLines = GetTimeLineRecommendation(caseProfileId);

            var timeLineConclution = GetTimeLineConclusion(caseProfileId);
            if (timeLineConclution != null)
            {
                timeLines.Add(timeLineConclution);
            }

            var timeLineMileStone = GetTimeLineMilestone(caseProfileId);
            if (timeLineMileStone.Any())
            {
                timeLines.AddRange(timeLineMileStone);
            }

            return timeLines;
        }

        private List<TimeLineModel> GetTimeLineRecommendation(long caseProfileId)
        {
            var recommendationIds = new List<long> { 6, 7, 8, 3, 9 };
            var recommentdateion = GetRecommendationHistory(caseProfileId).Where(d => d.Id != null && recommendationIds.Contains(d.Id.Value)).ToList();

            var timelines = (from r in recommentdateion
                             select new TimeLineModel
                             {
                                 DateTime = r.PublishedDate,
                                 PointProperty = new TimeLinePointModel
                                 {
                                     CommonType = (int)TimeLineCommonType.Recommendation,
                                     TypeName = r.Content,
                                     SpecificType = CastRecomentdation((int)r.Id)
                                 }
                             }).ToList();

            return timelines;
        }

        private TimeLineModel GetTimeLineConclusion(long caseProfileId)
        {
            var conclusions = GetConclusionHistory(caseProfileId);
            var conclution = conclusions.FirstOrDefault(d => d.Id == (int)ConclusionType.ConfirmedViolation);

            if (conclution != null)
            {
                return new TimeLineModel
                {
                    DateTime = conclution.PublishedDate,
                    PointProperty = new TimeLinePointModel
                    {
                        CommonType = (int)TimeLineCommonType.Conclusion,
                        TypeName = conclution.Content,
                        SpecificType = 0
                    }
                };
            }
            return null;
        }

        private List<TimeLineModel> GetTimeLineMilestone(long caseProfileId)
        {
            var timeLines = new List<TimeLineModel>();
            var milestones = GetMileStones(caseProfileId);
            if (milestones.Count > 0)
            {
                foreach (var milestone in milestones)
                {
                    var milestoneNumber = SplitNumberFromMileStoneText(milestone.Description.Substring(0, 18));

                    if (milestoneNumber != null && milestone.MilestoneModified != null)
                    {
                        timeLines.Add(new TimeLineModel
                        {
                            DateTime = milestone.MilestoneModified.Value,
                            PointProperty = new TimeLinePointModel
                            {
                                CommonType = (int)TimeLineCommonType.MileStone,
                                TypeName = "Milestone " + milestoneNumber,
                                SpecificType = milestoneNumber.Value,
                                Tooltip = $"milestone-{milestoneNumber}-hint"
                            }
                        });
                    }
                }
            }
            return timeLines;
        }

        private int? SplitNumberFromMileStoneText(string mileStone)
        {
            Regex re = new Regex(@"([a-zA-Z\s]+)(\d+)([\s\:a-zA-Z]+)");
            Match result = re.Match(mileStone);
            if (result.Success)
            {
                string numberPart = result.Groups[2].Value;
                return int.Parse(numberPart);
            }
            return null;
        }

        private int CastRecomentdation(int caseStatusId)
        {
            switch (caseStatusId)
            {
                case (int)RecommendationType.Evaluate:
                    return 0;
                case (int)RecommendationType.Engage:
                    return 1;
                case (int)RecommendationType.Disengage:
                    return 2;
                case (int)RecommendationType.Resolved:
                    return 3;
                case (int)RecommendationType.Archived:
                    return 4;
            }
            return 0;
        }

        public List<I_Milestones> GetMileStones(long caseProfileId)
        {
            var milestones = from m in _dbContext.I_Milestones
                             where m.I_GesCaseReports_Id == caseProfileId && m.Description.StartsWith("Milestone")
                             select m;
            return milestones.ToList();
        }

        public AlertViewModel GetAlert(long caseProfileId)
        {
            return (from r in _dbContext.I_GesCaseReports
                    join a in _dbContext.I_NaArticles on r.I_NaArticles_Id equals a.I_NaArticles_Id
                    where r.I_GesCaseReports_Id == caseProfileId && a.Publishable
                    select new AlertViewModel
                    {
                        I_NaArticles_Id = a.I_NaArticles_Id,
                        AlertDate = a.Created,
                        Heading = a.Heading,
                        Text = a.Text,
                        Source = a.Source,
                        SourceDate = a.SourceDate
                    }).FirstOrDefault();
        }

        public ICaseProfileUNGPAssessmentComponent GetCaseProfileUNGPAssessmentComponent(long caseProfileId)
        {
            var ungpAssessmentComponent =  (from r in _dbContext.GesUNGPAssessmentForm                    
                    where r.I_GesCaseReports_Id == caseProfileId && r.IsPublished == true
                    select new CaseProfileUNGPAssessmentComponent
                    {
                        UNGPAssessmentFormId = r.GesUNGPAssessmentFormId,
                        GesCommentCompanyPreparedness = r.GesCommentCompanyPreparedness,
                        HumanRightsPolicyTotalScore = r.HumanRightsPolicyTotalScore,
                        GesCommentSalientHumanRight = r.GesCommentSalientHumanRight,
                        SalientHumanRightsPotentialViolationTotalScore = r.SalientHumanRightsPotentialViolationTotalScore,                        
                        TotalScoreForHumanRightsDueDiligence = r.TotalScoreForHumanRightsDueDiligence,
                        TotalScoreForRemediationOfAdverseHumanRightsImpacts = r.TotalScoreForRemediationOfAdverseHumanRightsImpacts,
                        TotalScoreForCompanyPreparedness = r.TotalScoreForCompanyPreparedness,
                        Created = r.Created,
                        Modified = r.Modified
                    }).FirstOrDefault();
            if (ungpAssessmentComponent != null)
            {
                ungpAssessmentComponent.Sources = GetUNGPAssessmentResources(ungpAssessmentComponent.UNGPAssessmentFormId);
            }
            return ungpAssessmentComponent;
        }

        private IList<UNGPAssessmentResource> GetUNGPAssessmentResources(Guid? ungpAssessmentFormId)
        {
            return (from s in _dbContext.GesUNGPAssessmentFormResources
                    where s.GesUNGPAssessmentFormId == ungpAssessmentFormId
                    select new UNGPAssessmentResource
                    {
                        SourceDate = s.SourceDate,
                        SourcesLink = s.SourcesLink,
                        SourcesName = s.SourcesName
                    }).ToList();
        }

        public PaginatedResults<GesContact> GetContacts(JqGridViewModel jqGridParams, long orgId)
        {
            var contacts = from i in _dbContext.G_Individuals
                           join o in _dbContext.G_Organizations on i.G_Organizations_Id equals o.G_Organizations_Id
                           let cdCount =
                            (
                              from cd in _dbContext.I_GesCompanyDialogues
                              where i.G_Individuals_Id == cd.G_Individuals_Id
                              select cd.G_Individuals_Id
                            ).Count()
                           let sdCount =
                           (
                             from sd in _dbContext.I_GesSourceDialogues
                             where i.G_Individuals_Id == sd.G_Individuals_Id
                             select sd.G_Individuals_Id
                           ).Count()
                           select new GesContact
                           {
                               UserId = i.G_Individuals_Id,
                               FirstName = i.FirstName,
                               LastName = i.LastName,
                               Email = i.Email,
                               JobTitle = i.JobTitle,
                               OrganizationName = o.Name,
                               OrganizationId = i.G_Organizations_Id,
                               Phone = i.Phone,
                               Comment = i.Comment,
                               Organization_Address = o.Address1,
                               Organization_PostalCode = o.PostalCode,
                               Organization_City = o.City,
                               Organization_G_Countries_Id = o.G_Countries_Id,
                               Organization_Phone = o.Phone,
                               Organization_WebsiteUrl = o.WebsiteUrl,
                               Organization_Comment = o.Comment,
                               Organization_Customer = o.Customer,
                               NumberCompanyDialogue = cdCount,
                               NumberSourceDialogue = sdCount
                           };

            if (jqGridParams._search)
            {
                var finalRules = JqGridHelper.GetFilterRules<GesContact>(jqGridParams);
                contacts = string.IsNullOrEmpty(finalRules) ? contacts : contacts.Where(finalRules);
            }

            //SORT
            var sortCol = jqGridParams.sidx.ToLower();
            var sortDir = jqGridParams.sord.ToLower();
            if (!(string.IsNullOrEmpty(sortCol) && string.IsNullOrEmpty(sortDir)))
            {
                switch (sortCol)
                {
                    case "userid":
                        contacts = sortDir == "asc"
                            ? contacts.OrderBy(x => x.UserId)
                            : contacts.OrderByDescending(x => x.UserId);
                        break;
                    case "firstname":
                        contacts = sortDir == "asc"
                            ? contacts.OrderBy(x => x.FirstName).ThenBy(d => d.UserId)
                            : contacts.OrderByDescending(x => x.FirstName).ThenBy(d => d.UserId);
                        break;
                    case "lastname":
                        contacts = sortDir == "asc"
                            ? contacts.OrderBy(x => x.LastName).ThenBy(d => d.UserId)
                            : contacts.OrderByDescending(x => x.LastName).ThenBy(d => d.UserId);
                        break;
                    case "email":
                        contacts = sortDir == "asc"
                            ? contacts.OrderBy(x => x.Email).ThenBy(d => d.UserId)
                            : contacts.OrderByDescending(x => x.Email).ThenBy(d => d.UserId);
                        break;
                    case "jobtitle":
                        contacts = sortDir == "asc"
                            ? contacts.OrderBy(x => x.JobTitle).ThenBy(d => d.UserId)
                            : contacts.OrderByDescending(x => x.JobTitle).ThenBy(d => d.UserId);
                        break;
                }
            }

            return contacts.ToPagedList(Logger, jqGridParams.page, jqGridParams.rows);
        }

        public List<GesCaseAuditLogsViewModel> GetConclusionsHistory(long caseReportId)
        {
            return GetRecommendationsHistory(caseReportId, "I_GesCaseReportStatuses_Id");
        }

        public List<GesCaseAuditLogsViewModel> GetRecommendationsHistory(long caseReportId)
        {
            return GetRecommendationsHistory(caseReportId, "NewI_GesCaseReportStatuses_Id");
        }

        private List<GesCaseAuditLogsViewModel> GetRecommendationsHistory(long caseReportId, string columnName)
        {
            var result = (from a in _dbContext.GesCaseReports_Audit
                join d in _dbContext.GesCaseReports_Audit_Details on a.GesCaseReports_Audit_Id equals d
                    .GesCaseReports_Audit_GesCaseReports_Audit_Id
                where a.I_GesCaseReports_Id == caseReportId && d.TableName == "I_GesCaseReports" &&
                      d.ColumnName == columnName
                orderby d.AuditDatetime descending
                select new GesCaseAuditLogsViewModel
                {
                    Id = d.GesCaseReports_Audit_Details1,
                    I_GesCaseReportsId = caseReportId,
                    OldValue = d.OldValue,
                    NewValue = d.NewValue,
                    AuditDatetime = d.AuditDatetime
                }).ToList();

            UpdateRecommendationName(result);

            foreach (var gesCaseAuditLogsViewModel in result)
            {
                gesCaseAuditLogsViewModel.AuditDatetime = gesCaseAuditLogsViewModel.AuditDatetime.Date;
            }

            return result;
        }

        private void UpdateRecommendationName(List<GesCaseAuditLogsViewModel> auditLogs)
        {
            if (auditLogs.Any())
            {
                var status = _dbContext.I_GesCaseReportStatuses.ToList();
                long tempId;
                foreach (var audit in auditLogs)
                {
                    if (!string.IsNullOrEmpty(audit.OldValue) && long.TryParse(audit.OldValue, out tempId))
                    {
                        audit.OldValueName = status
                            .FirstOrDefault(d => d.I_GesCaseReportStatuses_Id == tempId)?.Name;
                    }

                    if (!string.IsNullOrEmpty(audit.NewValue) && long.TryParse(audit.NewValue, out tempId))
                    {
                        audit.NewValueName = status
                            .FirstOrDefault(d => d.I_GesCaseReportStatuses_Id == tempId)?.Name;
                    }
                }

            }
        }

        private DateTime? GetLatestGesCaseReportAudit(long caseReportId, string fieldName)
        {
            var result = (from a in _dbContext.GesCaseReports_Audit
                join d in _dbContext.GesCaseReports_Audit_Details on a.GesCaseReports_Audit_Id equals d
                    .GesCaseReports_Audit_GesCaseReports_Audit_Id
                where a.I_GesCaseReports_Id == caseReportId && d.TableName == "I_GesCaseReports" &&
                      d.ColumnName == fieldName
                orderby d.AuditDatetime descending
                select d.AuditDatetime).FirstOrDefault();

            if (result > DateTime.MinValue)
            {
                return result;
            }

            return null;
        }

        public List<MilestoneModel> GetMilestonesByCasereportId(long caseReportId)
        {
            var result = (from a in _dbContext.I_Milestones
                          join t in _dbContext.GesMilestoneTypes on a.GesMilestoneTypesId equals t.Id into gt from t in gt.DefaultIfEmpty()
                where a.I_GesCaseReports_Id == caseReportId
                select new MilestoneModel
                {
                    MilestoneId = a.I_Milestones_Id,
                    MilestoneDescription = a.Description,
                    MilestoneModified = a.MilestoneModified,
                    GesMilestoneTypesId = a.GesMilestoneTypesId,
                    GesMilestoneTypeName = t != null?t.Name:""
                }).ToList();

            return result;
        }
    }
}
