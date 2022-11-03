using System;

namespace GES.Inside.Data.Models
{
    public class GssResearchPrincipleIssueIndicatorViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public int Group { get; set; }
        public GssResearchPrincipleAssessmentViewModel Assessment { get; set; }

        public GssResearchPrincipleGeneralViewModel General { get; set; }

        public GssResearchPrincipleUpgradeCriteriaViewModel UpgradeCriteria { get; set; }
    
        public GssResearchPrincipleCompanyContactViewModel CompanyContact { get; set; }    

    }
}
