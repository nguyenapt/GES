using System;

namespace GES.Inside.Data.Models
{
    public class GssResearchPrincipleUpgradeCriteriaViewModel
    {
        public Guid Id { get; set; }
        public Guid GssId { get; set; }
        public Boolean TheViolationHasCeased { get; set; }
        public string TheViolationHasCeasedDescription { get; set; }

        public Boolean TheCompanyHasAdoptedaResponsibleCourseOfAction { get; set; }
        public string TheCompanyHasAdoptedaResponsibleCourseOfActionDescription { get; set; }
    }
}
