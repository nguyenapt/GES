using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sustainalytics.GSS.Entities
{
    public class CompanyProfile
    {
        public Guid Id { get; set; }

        public int CompanyId { get; set; }

        public Company Company { get; set; }

        public int Version { get; set; }

        public WorkflowStatus Status { get; set; }

        public DateTime LastUpdated { get; set; }

        [Required, MaxLength(100)]
        public string LastUpdatedBy { get; set; }

        public AssessmentType Assessment { get; set; }

        public bool HasSignificantAssessmentChange { get; set; }

        public Quarter AssessmentEffectiveSinceQuarterNumerator { get; set; }

        public int AssessmentEffectiveSinceQuarterYear { get; set; }

        public List<Principle> Principles { get; set; }

        [Required(AllowEmptyStrings = true), MaxLength(300)]
        public string PreviouslyNonCompliant { get; set; } = string.Empty;

        [Required(AllowEmptyStrings = true), MaxLength(500)]
        public string OverallConclusion { get; set; } = string.Empty;

        public FeedbackType? OutlookAssessment { get; set; }

        public Quarter? OutlookAssessmentEffectiveSinceQuarterNumerator { get; set; }

        public int? OutlookAssessmentEffectiveSinceQuarterYear { get; set; }

        [Required(AllowEmptyStrings = true), MaxLength(300)]
        public string OutlookAssessmentComments { get; set; } = string.Empty;

        public EngagementType? Engagement { get; set; }

        public DateTime? EngagementAsOf { get; set; }

        public List<Source> Sources { get; set; }
    }
}
