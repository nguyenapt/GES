using System;
using System.Collections.Generic;

namespace GES.Inside.Data.Models
{
    public class GssResearchCompanyViewModel
    {
        public long Id { get; set; }

        public string Name { get; set; }
        public string WorkflowStatus { get; set; }
        public string Assessment { get; set; }
        public string Analyst { get; set; }
        public string Reviewer { get; set; }
        public string GssocReview { get; set; }
        public string Flags { get; set; }
        public bool IsParked { get; set; }

        public int EngagementStatus { get; set; }
        public int CompanyStatus { get; set; }
        public string OverallAssesment { get; set; }
        public string CompanyId { get; set; }
        public string ISIN { get; set; }
        public string SubpeerGroupName { get; set; }
        public string Country { get; set; }
        public string Website { get; set; }
        public string BussinessDescription { get; set; }
        public string OutlookEffectiveSince { get; set; }
        public string UNGlobalCompact { get; set; }
        public string UNGuildingPrinceples { get; set; }
        public string OECDGuideline { get; set; }
        public string OtherRelatedConventions { get; set; }
        public string EngagementStatusName { get; set; }
        public string EngagementStatusAsOf { get; set; }


        public List<GssResearchCompanyUNGuidingPrinciplesViewModel> GssResearchCompanyUnGuidingPrinciplesViewModels { get; set; }

        public List<CommentDetailsViewModel> InternalAnalystComments
        {
            get;
            set;
        }

        public List<CommentDetailsViewModel> GssOutlookComments{get;set;}
        public GssResearchPrincipleAssessmentViewModel GssPrincipleAssessmentViewModel { get; set; }

        public GssResearchPrincipleGeneralViewModel GssPrincipleGeneralViewModel { get; set; }
        public GssResearchPrincipleUpgradeCriteriaViewModel GssPrincipleUpgradeCriteriaViewModel { get; set; }
        public GssResearchPrincipleCompanyContactViewModel GssPrincipleCompanyContactViewModel { get; set; }
        public List<GssResearchSourceViewModel> GssSourceViewModels { get; set; }
        public List<GssResearchInternalCommentViewModel> GssInternalCommentViewModels { get; set; }
        public  List<GssResearchPrincipleIssueIndicatorViewModel> GssResearchIssueIndicatorsViewModels { get; set; }
    }
}
