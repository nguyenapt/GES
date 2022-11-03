using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GES.Inside.Data.Models.CaseProfiles
{
    public interface ICaseProfileUNGPAssessmentComponent
    {
        Guid UNGPAssessmentFormId { get; set; }
        double? SalientHumanRightsPotentialViolationTotalScore { get; set; }
        string GesCommentSalientHumanRight { get; set; }
        double? TotalScoreForCompanyPreparedness { get; set; }
        double? HumanRightsPolicyTotalScore { get; set; }
        double? TotalScoreForHumanRightsDueDiligence { get; set; }
        double? TotalScoreForRemediationOfAdverseHumanRightsImpacts { get; set; }
        string GesCommentCompanyPreparedness { get; set; }
        IList<UNGPAssessmentResource> Sources { get; set; }
        DateTime? Created { get; set; }
        DateTime? Modified { get; set; }

    }

    public class CaseProfileUNGPAssessmentComponent : ICaseProfileUNGPAssessmentComponent
    {
        public Guid UNGPAssessmentFormId { get; set; }
        public double? SalientHumanRightsPotentialViolationTotalScore { get; set; }
        public string GesCommentSalientHumanRight { get; set; }
        public double? TotalScoreForCompanyPreparedness { get; set; }
        public double? HumanRightsPolicyTotalScore { get; set; }
        public double? TotalScoreForHumanRightsDueDiligence { get; set; }
        public double? TotalScoreForRemediationOfAdverseHumanRightsImpacts { get; set; }
        public string GesCommentCompanyPreparedness { get; set; }
        public IList<UNGPAssessmentResource> Sources { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Modified { get; set; }
    }

    public class UNGPAssessmentResource
    {
        public string SourcesName { get; set; }
        public string SourcesLink { get; set; }
        public DateTime? SourceDate { get; set; }
    }
}
