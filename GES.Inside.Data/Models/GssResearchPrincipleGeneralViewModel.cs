using System;
using System.Collections.Generic;

namespace GES.Inside.Data.Models
{
    public class GssResearchPrincipleGeneralViewModel
    {
        public List<GssResearchPrincipleGeneralItemViewModel> HeadLineViewModels { get; set; }
        public List<GssResearchPrincipleGeneralItemViewModel> CaseSummaryViewModels { get; set; }
        public List<GssResearchPrincipleGeneralItemViewModel> ImpactViewModels { get; set; }
        public List<GssResearchPrincipleGeneralItemViewModel> ImpactAssessmentViewModels { get; set; }
        
    }
}
