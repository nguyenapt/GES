using System;

namespace GES.Inside.Data.Models
{
    public class GssResearchPrincipleAssessmentViewModel
    {
        public Guid Id { get; set; }

        public Guid GssId { get; set; }

        public string UNGlobalCompactAssessment { get; set; }

        public string AssessmentEffectiveSice { get; set; }
        public string BasisForNonCompliance { get; set; }
        public string BasisForNonComplianceComment { get; set; }
        public string OECDGuidelineForMultinationalEnterprises { get; set; }
        public string UNGuidingPrincipleOnBusinessAndHumanRights { get; set; }
        public string OtherRelatedConventions { get; set; }
    
    }
}
