using System;
using System.Collections.Generic;
using GES.Inside.Data.DataContexts;

namespace GES.Inside.Data.Models
{
    public class CaseReportListViewModel
    {
        public long Id { get; set; }

        public string IssueName { get; set; }

        public string Location { get; set; }

        public long? EngagementTypeId { get; set; }
        public string EngagementType { get; set; }
        public string EngagementThemeNorm { get; set; }

        public bool KeywordMatchedNorm { get; set; }
        public string Norm { get; set; }

        public long? TemplateId { get; set; }
        public long? NormId { get; set; }
        public string Conclusion { get; set; }
        public string Recommendation { get; set; }
        public DateTime? CommentaryModified { get; set; }

        public bool? Confirmed { get; set; }

        public long GesCompanyId { get; set; }
        public long CompanyId { get; set; }
        public string CompanyName { get; set; }
        public int SortOrderEngagementType { get; set; }
        public int SortOrderRecommendation { get; set; }
        public string Description { get; set; }
        public string Service { get; set; }
        public string ServiceEngagementThemeNorm { get; set; }
        public DateTime? EntryDate { get; set; }
        public DateTime? LastModified { get; set; }
        public DateTime? ClosingDate { get; set; }
        public bool KeywordMatched { get; set; }
        public string Keywords { get; set; }
        public long? ProgressGrade { get; set; }
        public long? ResponseGrade { get; set; }
        public long? DevelopmentGrade { get; set; }
        public string MyEndorsement { get; set; }
        public bool IsInFocusList { get; set; }
        public string Response { get; set; }
        public string Progress { get; set; }
        public string Milestone { get; set; }
        public string Development { get; set; }
        public DateTime? MilestoneDate { get; set; }
        public string ChangeObjective { get; set; }
        public DateTime? ProcessGoalModified { get; set; }
        public DateTime? EngagementSince { get; set; }
        public bool CanSignUp { get; set; }
        public int SignUpValue { get; set; }
        public string GesCommentary { get; set; }
        public DateTime? EngagementActivity { get; set; }
        public string EngagementTypeCategory { get; set; }

        public long? EngagementTypeCategoriesId { get; set; }

        public long? ParentForumMessagesId { get; set; }
        public long? ForumMessagesTreeId { get; set; }
        public DateTime? Created { get; set; }
        public long? NewI_GesCaseReportStatuses_Id { get; set; }
        public bool IsResolved { get; set; }
        public bool IsArchived { get; set; }

        public bool IsUnsubscribed { get; set; }

        public IEnumerable<KpiViewModel> KPIs { get; set; }

        public double? TotalScoreForCompanyPreparedness { get; set; }
        public double? SalientHumanRightsPotentialViolationTotalScore { get; set; }
        
        public long? IssueHeadingId { get; set; }
    }
}