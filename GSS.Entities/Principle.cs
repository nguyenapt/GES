using System;
using System.ComponentModel.DataAnnotations;

namespace Sustainalytics.GSS.Entities
{
    public class Principle
    {
        public Guid Id { get; set; }

        public PrincipleType TemplateId { get; set; }

        public PrincipleTemplate Template { get; set; }

        public Guid CompanyProfileId { get; set; }

        public CompanyProfile CompanyProfile { get; set; }

        public AssessmentType Assessment { get; set; }

        public Quarter AssessmentEffectiveSinceQuarterNumerator { get; set; }

        public int AssessmentEffectiveSinceQuarterYear { get; set; }

        public DateTime LastUpdated { get; set; }

        [Required, MaxLength(100)]
        public string LastUpdatedBy { get; set; }
    }
}