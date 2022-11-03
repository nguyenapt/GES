using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GES.Inside.Data.DataContexts;

namespace GES.Inside.Data.Models.CaseProfiles
{
    public interface ICaseProfileSdgAndGuidelineConventionComponent
    {
        IList<string> Conventions { get; set; }
        string Guidelines { get; set; }
        IList<Sdg> Sdgs { get; set; }
        double? SalientHumanRightsPotentialViolationTotalScore { get; set; }        
        double? TotalScoreForCompanyPreparedness { get; set; }
    }

    public class CaseProfileSdgAndGuidelineConventionComponent : ICaseProfileSdgAndGuidelineConventionComponent
    {
        [Display(Name = "Conventions")]
        public IList<string> Conventions { get; set; }

        [Display(Name = "Guidelines")]
        public string Guidelines { get; set; }

        public IList<Sdg> Sdgs { get; set; }

        public double? SalientHumanRightsPotentialViolationTotalScore { get; set; }
        public double? TotalScoreForCompanyPreparedness { get; set; }
    }
}