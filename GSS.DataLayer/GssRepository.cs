using Sustainalytics.GSS.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Sustainalytics.GSS.DataLayer
{
    public class GssRepository
    {
        private readonly string connectionString;
        private readonly string caller;
        private static readonly Dictionary<WorkflowStatus, List<WorkflowStatus>> workflowAllowedTransitions = new Dictionary<WorkflowStatus, List<WorkflowStatus>>
        {
            { WorkflowStatus.Published, new List<WorkflowStatus> { WorkflowStatus.Draft } },
            { WorkflowStatus.Draft, new List<WorkflowStatus> { WorkflowStatus.Review, WorkflowStatus.Final } },
            { WorkflowStatus.Review, new List<WorkflowStatus> { WorkflowStatus.Editing } },
            { WorkflowStatus.Editing, new List<WorkflowStatus> { WorkflowStatus.Final } },
            { WorkflowStatus.Final, new List<WorkflowStatus> { WorkflowStatus.Draft, WorkflowStatus.Review, WorkflowStatus.Published } }
        };

        public GssRepository(string connectionString, string caller)
        {
            this.connectionString = string.IsNullOrWhiteSpace(connectionString) ? throw new ArgumentException($"No connection string provided for {this} instantiation!") : connectionString;
            this.caller = string.IsNullOrWhiteSpace(caller) ? throw new ArgumentException($"No user provided for {this} instantiation!") : caller;
        }

        public Tuple<List<Company>, int> GetFilteredCompaniesPageWithCount(int skip, int take, 
            string companyNameFilter = null, List<WorkflowStatus> workflowStatusFilter = null, List<AssessmentType> assessmentFilter = null, 
            string analystFilter = null, bool? flaggedFilter = null, string reviewerFilter = null, bool? gssocReviewFilter = null)
        {
            using (var ctx = new GssDbContext(connectionString))
            {
                var companies = ctx.Companies
                                    .GroupJoin(ctx.CompanyProfiles, c => c.Id, cp => cp.CompanyId, (c, cp) => new
                                    {
                                        c.Id,
                                        c.Name,
                                        c.IsParkedForResearchSince,
                                        Status = cp.Max(p => (WorkflowStatus?)p.Status),
                                        Assessment = cp.OrderByDescending(p => p.Status).Select(p => (AssessmentType?)p.Assessment).FirstOrDefault(),
                                        c.Analyst,
                                        c.Reviewer,
                                        HasSignificantAssessmentChange = cp.OrderByDescending(p => p.Status).Select(p => (bool?)p.HasSignificantAssessmentChange).FirstOrDefault(), 
                                        c.IsResearchEntityCandidate
                                    })
                                    .Where(c => c.IsParkedForResearchSince == null || c.Status.HasValue)// keep only companies which are either active or do have a profile attached
                                    .GroupJoin(ctx.Companies, c => c.Id, ce => ce.ResearchEntityId, (c, ces) => new
                                    {
                                        c.Id,
                                        c.Name,
                                        c.IsParkedForResearchSince,
                                        c.Status, 
                                        c.Assessment, 
                                        c.Analyst,
                                        c.Reviewer,
                                        IsResearchEntityCandidate = ces.Any(ce => ce.IsResearchEntityCandidate) || c.IsResearchEntityCandidate, 
                                        c.HasSignificantAssessmentChange
                                    });

                var companyNameFilterLowered = companyNameFilter?.Trim().ToLower();

                if (!string.IsNullOrEmpty(companyNameFilterLowered))
                {
                    companies = companies.Where(c => c.Name.ToLower().Contains(companyNameFilterLowered));
                }

                if (workflowStatusFilter?.Count > 0)
                {
                    companies = companies.Where(c => c.Status.HasValue && workflowStatusFilter.Contains(c.Status.Value));
                }

                if (assessmentFilter?.Count > 0)
                {
                    companies = companies.Where(c => c.Assessment.HasValue && assessmentFilter.Contains(c.Assessment.Value));
                }

                var analystFilterLowered = analystFilter?.Trim().ToLower();

                if (!string.IsNullOrEmpty(analystFilterLowered))
                {
                    companies = companies.Where(c => c.Analyst.ToLower().Contains(analystFilterLowered));
                }

                if (flaggedFilter.HasValue)
                {
                    companies = companies.Where(c => c.IsResearchEntityCandidate == flaggedFilter.Value);
                }

                var reviewerFilterLowered = reviewerFilter?.Trim().ToLower();

                if (!string.IsNullOrEmpty(reviewerFilterLowered))
                {
                    companies = companies.Where(c => c.Reviewer.ToLower().Contains(reviewerFilterLowered));
                }

                if (gssocReviewFilter.HasValue)
                {
                    companies = companies.Where(c => c.HasSignificantAssessmentChange == gssocReviewFilter);
                }

                var filteredCompaniesCount = companies.Count();

                var filteredCompaniesPage = 
                    skip >= filteredCompaniesCount ? new List<Company>() 
                                                    : companies.OrderBy(c => c.Name)
                                                                .Skip(skip)
                                                                .Take(take)
                                                                .ToList()
                                                                .Select(c => new Company
                                                                {
                                                                    Id = c.Id,
                                                                    Name = c.Name,
                                                                    IsParkedForResearchSince = c.IsParkedForResearchSince,
                                                                    Analyst = c.Analyst,
                                                                    Reviewer = c.Reviewer,
                                                                    IsResearchEntityCandidate = c.IsResearchEntityCandidate,
                                                                    LatestProfile = c.Status == null ? null :
                                                                        new CompanyProfile
                                                                        {
                                                                            Assessment = c.Assessment.Value,
                                                                            Status = c.Status.Value,
                                                                            HasSignificantAssessmentChange = c.HasSignificantAssessmentChange.Value
                                                                        }
                                                                })
                                                                .ToList();

                return Tuple.Create(filteredCompaniesPage, filteredCompaniesCount);
            }
        }

        public Company GetCompany(int companyId)
        {
            using (var ctx = new GssDbContext(connectionString))
            {
                ctx.Configuration.ProxyCreationEnabled = false;

                var company = ctx.Companies.SingleOrDefault(c => c.Id == companyId) ?? throw new ArgumentException($"No company found for ID {companyId}!");

                if (company.ResearchEntityId.HasValue)
                {
                    var researchEntity = ctx.Companies
                                            .Where(c => c.Id == company.ResearchEntityId)
                                            .Select(c => new { c.Name, c.IsResearchEntityCandidate })
                                            .Single();

                    company.CorporateTreeResearch = new Company
                    {
                        Id = company.ResearchEntityId.Value,
                        Name = researchEntity.Name,
                        IsResearchEntityCandidate = researchEntity.IsResearchEntityCandidate
                    };

                    company.CorporateTreeCoverages = ListCoverages(ctx, company.ResearchEntityId.Value);
                }
                else
                {

                    company.CorporateTreeResearch = new Company { Id = company.Id, Name = company.Name, IsResearchEntityCandidate = company.IsResearchEntityCandidate };

                    company.CorporateTreeCoverages = ListCoverages(ctx, company.Id);
                }

                company.LatestProfile = ctx.CompanyProfiles
                                            .Where(cp => cp.CompanyId == companyId)
                                            .OrderByDescending(cp => cp.Status)
                                            .Include(cp => cp.Principles.Select(p => p.Template))
                                            .Include(cp => cp.Sources)
                                            .FirstOrDefault();
                
                if (company.LatestProfile != null)
                {
                    company.LatestProfile.Principles = company.LatestProfile.Principles.OrderBy(p => p.TemplateId).ToList();

                    company.LatestProfile.Sources = company.LatestProfile.Sources.OrderByDescending(s => s.PublicationDate).ToList();
                }

                return company;
            }
        }

        private List<Company> ListCoverages(GssDbContext ctx, int researchEntityId)
        {
            return ctx.Companies
                        .Where(c => c.ResearchEntityId == researchEntityId)
                        .Select(c => new { c.Id, c.Name, c.IsResearchEntityCandidate })
                        .ToList()
                        .Select(c => new Company { Id = c.Id, Name = c.Name, IsResearchEntityCandidate = c.IsResearchEntityCandidate })
                        .OrderBy(c => c.Name)
                        .ToList();
        }

        public List<WorkflowStatus> ListPermittedTransitions(int companyId)
        {
            using (var ctx = new GssDbContext(connectionString))
            {
                if (!ctx.Companies.Any(c => c.Id == companyId))
                {
                    throw new ArgumentException($"No company found for ID {companyId}!");
                }

                var companyStatus = ctx.CompanyProfiles
                                        .Where(cp => cp.CompanyId == companyId)
                                        .OrderByDescending(cp => cp.Status)
                                        .Select(cp => (WorkflowStatus?)cp.Status)
                                        .FirstOrDefault();

                if (!workflowAllowedTransitions.TryGetValue(companyStatus ?? WorkflowStatus.Published, out var permittedTransitions))
                {
                    permittedTransitions = new List<WorkflowStatus>();
                }

                return permittedTransitions;
            }
        }

        public void SetCompanyProfileStatus(int companyId, WorkflowStatus newStatus)
        {
            using (var ctx = new GssDbContext(connectionString))
            {
                var company = ctx.Companies.Where(c => c.Id == companyId).Select(c => new { c.Name, c.IsParkedForResearchSince }).SingleOrDefault();

                if (company == null)
                {
                    throw new ArgumentException($"No company found for ID {companyId}!");
                }
                else if (company.IsParkedForResearchSince.HasValue)
                {
                    throw new ArgumentException($"Company \"{company.Name}\" is parked!");
                }

                var companyStatus = ctx.CompanyProfiles
                                        .Where(cp => cp.CompanyId == companyId)
                                        .OrderByDescending(cp => cp.Status)
                                        .Select(cp => (WorkflowStatus?)cp.Status)
                                        .FirstOrDefault() ?? WorkflowStatus.Published;// assimilate the missing profile with a published status

                if (!workflowAllowedTransitions.TryGetValue(companyStatus, out var permittedTransitions) || 
                    !permittedTransitions.Contains(newStatus))
                {
                    throw new ArgumentException($"Cannot set '{company.Name}' on {newStatus}! Allowed new statuses: {string.Join(", ", permittedTransitions)}.");
                }

                if (companyStatus == WorkflowStatus.Published)
                {
                    AddWorkingCompanyProfile(ctx, companyId, newStatus);
                }

                ctx.SaveChanges();
            }
        }

        private void AddWorkingCompanyProfile(GssDbContext ctx, int companyId, WorkflowStatus status)
        {
            var profile = ctx.CompanyProfiles
                                .AsNoTracking()// in order to be able to clone the entities
                                .SingleOrDefault(cp => cp.CompanyId == companyId && cp.Status == WorkflowStatus.Published);

            var currentTimestamp = DateTime.UtcNow;

            if (profile == null)
            {
                ProcessBasicCompanyProfile(ctx, companyId, currentTimestamp, out profile);
            }
            else// clone the published profile for a new research cycle
            {
                profile.Version++;
            }

            profile.Id = Guid.NewGuid();
            profile.Status = status;
            profile.LastUpdated = currentTimestamp;
            profile.LastUpdatedBy = caller;

            ctx.CompanyProfiles.Add(profile);
        }

        private void ProcessBasicCompanyProfile(GssDbContext ctx, int companyId, DateTime timestamp, out CompanyProfile companyProfile)
        {
            var currentQuarter = timestamp.GetGssQuarter();

            var principles = Enum.GetValues(typeof(PrincipleType))
                                    .Cast<PrincipleType>()
                                    .ToDictionary(pt => pt, pt => new Principle
                                    {
                                        Id = Guid.NewGuid(),
                                        TemplateId = pt,
                                        AssessmentEffectiveSinceQuarterNumerator = currentQuarter.Numerator,
                                        AssessmentEffectiveSinceQuarterYear = currentQuarter.Year,
                                        LastUpdated = timestamp,
                                        LastUpdatedBy = caller
                                    });

            ctx.IssueIndicators.AddRange(ctx.IssueIndicatorTemplates
                                            .Select(iit => new { iit.Id, iit.PrincipleId })
                                            .ToList()
                                            .Select(iit => new IssueIndicator
                                            {
                                                Id = Guid.NewGuid(),
                                                TemplateId = iit.Id,
                                                PrincipleId = principles[iit.PrincipleId].Id,
                                                AssessmentEffectiveSinceQuarterNumerator = currentQuarter.Numerator,
                                                AssessmentEffectiveSinceQuarterYear = currentQuarter.Year,
                                                LastUpdated = timestamp,
                                                LastUpdatedBy = caller
                                            }));

            companyProfile = new CompanyProfile
            {
                CompanyId = companyId,
                AssessmentEffectiveSinceQuarterNumerator = currentQuarter.Numerator,
                AssessmentEffectiveSinceQuarterYear = currentQuarter.Year,
                Principles = principles.Values.ToList()
            };
        }

        public void UpdateGlobalCompactSignatorySince(int companyId, DateTime? globalCompactSignatorySince)
        {
            using (var ctx = new GssDbContext(connectionString))
            {
                var company = ctx.Companies.SingleOrDefault(c => c.Id == companyId);

                if (company == null)
                {
                    throw new ArgumentException($"No company found for ID '{companyId}'!");
                }
                if (company.IsParkedForResearchSince.HasValue)
                {
                    throw new ArgumentException($"Cannot edit the company '{company.Name}' as long as it is parked!");
                }
                if (globalCompactSignatorySince > DateTime.UtcNow)
                {
                    throw new ArgumentException($"Future dates are not acceptable for 'global compact signatory since' for company '{company.Name}'!");
                }

                company.UnGlobalCompactSignatorySince = globalCompactSignatorySince;

                ctx.SaveChanges();
            }
        }

        public void UpdateOutlook(Guid companyProfileId, FeedbackType? outlookAssessment, SpecificQuarter outlookAssessmentEffectiveSince, string outlookAssessmentComments)
        {
            if (outlookAssessment == null && (outlookAssessmentEffectiveSince != null || !string.IsNullOrWhiteSpace(outlookAssessmentComments)) ||
                outlookAssessmentEffectiveSince == null && (outlookAssessment != null || !string.IsNullOrWhiteSpace(outlookAssessmentComments)) ||
                string.IsNullOrWhiteSpace(outlookAssessmentComments) && (outlookAssessment != null || outlookAssessmentEffectiveSince != null))
            {
                throw new ArgumentException($"Inconsistent GSS outlook data provided for companyProfileId '{companyProfileId}'! Either none or all outlook data should have value.");
            }

            using (var ctx = new GssDbContext(connectionString))
            {
                var companyProfile = ctx.CompanyProfiles.Include(cp => cp.Company).SingleOrDefault(cp => cp.Id == companyProfileId);

                if (companyProfile == null)
                {
                    throw new ArgumentException($"No company profile found for ID '{companyProfileId}'!");
                }
                if (companyProfile.Company.IsParkedForResearchSince.HasValue)
                {
                    throw new ArgumentException($"Cannot edit the profile for company '{companyProfile.Company.Name}' as long as it is parked!");
                }

                outlookAssessmentComments = outlookAssessmentComments?.Trim() ?? string.Empty;

                if (outlookAssessment != companyProfile.OutlookAssessment ||
                    outlookAssessmentEffectiveSince?.Numerator != companyProfile.OutlookAssessmentEffectiveSinceQuarterNumerator ||
                    outlookAssessmentEffectiveSince?.Year != companyProfile.OutlookAssessmentEffectiveSinceQuarterYear ||
                    outlookAssessmentComments != companyProfile.OutlookAssessmentComments)
                {
                    companyProfile.OutlookAssessment = outlookAssessment;
                    companyProfile.OutlookAssessmentEffectiveSinceQuarterNumerator = outlookAssessmentEffectiveSince?.Numerator;
                    companyProfile.OutlookAssessmentEffectiveSinceQuarterYear = outlookAssessmentEffectiveSince?.Year;
                    companyProfile.OutlookAssessmentComments = outlookAssessmentComments;
                    companyProfile.LastUpdated = DateTime.UtcNow;
                    companyProfile.LastUpdatedBy = caller;

                    ctx.SaveChanges();
                }
            }
        }

        public void UpdatePreviouslyNonCompliantComment(Guid companyProfileId, string previouslyNonCompliantComment)
        {
            using (var ctx = new GssDbContext(connectionString))
            {
                var companyProfile = ctx.CompanyProfiles.Include(cp => cp.Company).SingleOrDefault(cp => cp.Id == companyProfileId);

                if (companyProfile == null)
                {
                    throw new ArgumentException($"No company profile found for ID '{companyProfileId}'!");
                }
                if (companyProfile.Company.IsParkedForResearchSince.HasValue)
                {
                    throw new ArgumentException($"Cannot edit the profile for company '{companyProfile.Company.Name}' as long as it is parked!");
                }
                if (companyProfile.Assessment != AssessmentType.Watchlist)
                {
                    throw new ArgumentException("The previously non-compliant attributes are applicable only for watchlist profiles!");
                }

                previouslyNonCompliantComment = previouslyNonCompliantComment?.Trim() ?? string.Empty;

                if (previouslyNonCompliantComment != companyProfile.PreviouslyNonCompliant)
                {
                    companyProfile.PreviouslyNonCompliant = previouslyNonCompliantComment;
                    companyProfile.LastUpdated = DateTime.UtcNow;
                    companyProfile.LastUpdatedBy = caller;

                    ctx.SaveChanges();
                }
            }
        }

        public void UpdateOverallConclusion(Guid companyProfileId, string overallConclusion)
        {
            using (var ctx = new GssDbContext(connectionString))
            {
                var companyProfile = ctx.CompanyProfiles.Include(cp => cp.Company).SingleOrDefault(cp => cp.Id == companyProfileId);

                if (companyProfile == null)
                {
                    throw new ArgumentException($"No company profile found for ID '{companyProfileId}'!");
                }
                if (companyProfile.Company.IsParkedForResearchSince.HasValue)
                {
                    throw new ArgumentException($"Cannot edit the profile for company '{companyProfile.Company.Name}' as long as it is parked!");
                }

                overallConclusion = overallConclusion?.Trim() ?? string.Empty;

                if (overallConclusion != companyProfile.OverallConclusion)
                {
                    companyProfile.OverallConclusion = overallConclusion;
                    companyProfile.LastUpdated = DateTime.UtcNow;
                    companyProfile.LastUpdatedBy = caller;

                    ctx.SaveChanges();
                }
            }
        }

        public void UpdateEngagement(Guid companyProfileId, EngagementType engagement, DateTime? engagementAsOf)
        {
            if (engagement != EngagementType.None && engagementAsOf == null)
            {
                throw new ArgumentException($"'engagement status as of' required for the {engagement} engagement type for companyProfileId '{companyProfileId}'!");
            }
            if (engagementAsOf > DateTime.UtcNow)
            {
                throw new ArgumentException($"Future dates are not acceptable for 'engagement status as of' for companyProfileId '{companyProfileId}'!");
            }

            using (var ctx = new GssDbContext(connectionString))
            {
                var companyProfile = ctx.CompanyProfiles.Include(cp => cp.Company).SingleOrDefault(cp => cp.Id == companyProfileId);

                if (companyProfile == null)
                {
                    throw new ArgumentException($"No company profile found for ID '{companyProfileId}'!");
                }
                if (companyProfile.Company.IsParkedForResearchSince.HasValue)
                {
                    throw new ArgumentException($"Cannot edit the profile for company '{companyProfile.Company.Name}' as long as it is parked!");
                }

                if (engagement != companyProfile.Engagement || engagementAsOf != companyProfile.EngagementAsOf)
                {
                    companyProfile.Engagement = engagement;
                    companyProfile.EngagementAsOf = engagementAsOf;
                    companyProfile.LastUpdated = DateTime.UtcNow;
                    companyProfile.LastUpdatedBy = caller;

                    ctx.SaveChanges();
                }
            }
        }

        public Dictionary<Guid, string> ListIssueIndicators(Guid principleId)
        {
            using (var ctx = new GssDbContext(connectionString))
            {
                return ctx.IssueIndicators
                            .Where(ii => ii.PrincipleId == principleId)
                            .OrderBy(ii => ii.TemplateId)
                            .Include(ii => ii.Template)
                            .ToDictionary(ii => ii.Id, ii => ii.Template.Name);
            }
        }

        public IssueIndicator GetIssueIndicator(Guid issueIndicatorId)
        {
            using (var ctx = new GssDbContext(connectionString))
            {
                var issueIndictor = ctx.IssueIndicators
                                        .Include(ii => ii.ImpactComments)
                                        .Include(ii => ii.ManagementComments)
                                        .Include(ii => ii.ConclusionComments)
                                        .SingleOrDefault(ii => ii.Id == issueIndicatorId) ?? throw new ArgumentException($"No issue indicator found for ID '{issueIndicatorId}'!");

                issueIndictor.ImpactComments = issueIndictor.ImpactComments.OrderBy(ic => ic.SortKey).ToList();
                issueIndictor.ManagementComments = issueIndictor.ManagementComments.OrderBy(mc => mc.SortKey).ToList();
                issueIndictor.ConclusionComments = issueIndictor.ConclusionComments.OrderBy(cc => cc.SortKey).ToList();

                return issueIndictor;
            }
        }
    }
}
