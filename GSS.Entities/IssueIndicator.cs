using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sustainalytics.GSS.Entities
{
    public class IssueIndicator
    {
        private const int RelatedConventionsBytes = 10;

        public Guid Id { get; set; }

        public int TemplateId { get; set; }

        public IssueIndicatorTemplate Template { get; set; }

        public Guid PrincipleId { get; set; }

        public Principle Principle { get; set; }

        public AssessmentType Assessment { get; set; }

        public Quarter AssessmentEffectiveSinceQuarterNumerator { get; set; }

        public int AssessmentEffectiveSinceQuarterYear { get; set; }

        public BasisForNonCompliance? BasisForNonComplianceValue { get; set; }

        [Required(AllowEmptyStrings = true), MaxLength(300)]
        public string BasisForNonComplianceText { get; set; } = string.Empty;

        public int OecdGuidelinesFlags { get; set; }

        public int UnGuidingPrinciplesFlags { get; set; }

        [Required, MinLength(RelatedConventionsBytes), MaxLength(RelatedConventionsBytes)]
        public byte[] RelatedConventionsFlags { get; set; } = new byte[RelatedConventionsBytes];

        [Required(AllowEmptyStrings = true), MaxLength(250)]
        public string Headline { get; set; } = string.Empty;

        [Required(AllowEmptyStrings = true), MaxLength(1500)]
        public string CaseSummary { get; set; } = string.Empty;

        [Required(AllowEmptyStrings = true), MaxLength(300)]
        public string Impact { get; set; } = string.Empty;

        public List<ImpactComment> ImpactComments { get; set; }

        [Required(AllowEmptyStrings = true), MaxLength(300)]
        public string Management { get; set; } = string.Empty;

        public List<ManagementComment> ManagementComments { get; set; }

        [Required(AllowEmptyStrings = true), MaxLength(300)]
        public string Conclusion { get; set; } = string.Empty;

        public List<ConclusionComment> ConclusionComments { get; set; }

        #region Upgrade Criteria

        public bool? IncidentConditionStatus { get; set; }

        [Required(AllowEmptyStrings = true), MaxLength(800)]
        public string IncidentConditionComment { get; set; } = string.Empty;

        public bool? CompanyResponseToIncidentConditionStatus { get; set; }

        [Required(AllowEmptyStrings = true), MaxLength(800)]
        public string CompanyResponseToIncidentConditionComment { get; set; } = string.Empty;

        #endregion

        #region Company Contact

        public DateTime? CompanyLastContacted { get; set; }

        public DateTime? CompanyLastResponse { get; set; }

        [Required(AllowEmptyStrings = true), MaxLength(300)]
        public string CompanyContactComments { get; set; } = string.Empty;

        #endregion

        public DateTime LastUpdated { get; set; }

        [Required, MaxLength(100)]
        public string LastUpdatedBy { get; set; }
    }
}