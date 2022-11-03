using System;
using GES.Common.Configurations;

namespace GES.Inside.Web.Models
{
    public class GesUNGPAssessmentFormResourcesViewModel
    {
        public System.Guid GesUNGPAssessmentFormResourcesId { get; set; }
        public Guid? GesUNGPAssessmentFormId { get; set; }
        public string SourcesName { get; set; }
        public string SourcesLink { get; set; }
        public DateTime? SourceDate { get; set; }
        public string SourceDateString => this.SourceDate?.ToString(Configurations.DateFormat);

        public DateTime? Modified { get; set; }
        public DateTime? Created { get; set; }
        public string ModifiedBy { get; set; }
    }
}