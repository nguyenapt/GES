using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sustainalytics.GSS.Entities
{
    public class Company
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]// otherwise EF will consider this int key as an identity field
        public int Id { get; set; }

        public Guid InternalId { get; set; }

        public int? ResearchEntityId { get; set; }

        public Company ResearchEntity { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = true), MaxLength(50)]
        public string Isin { get; set; } = string.Empty;

        [Required(AllowEmptyStrings = true)]
        public string Description { get; set; } = string.Empty;

        [Required(AllowEmptyStrings = true), MaxLength(50)]
        public string Country { get; set; } = string.Empty;

        [Required(AllowEmptyStrings = true), MaxLength(100)]
        public string SubPeerGroup { get; set; } = string.Empty;

        [Required(AllowEmptyStrings = true), MaxLength(255)]
        public string Website { get; set; } = string.Empty;

        public DateTime? IsParkedForResearchSince { get; set; }

        public DateTime? UnGlobalCompactSignatorySince { get; set; }

        /// <summary>
        /// Applicable and directly settable only for coverage entities
        /// </summary>
        public bool IsResearchEntityCandidate { get; set; }

        [Required(AllowEmptyStrings = true), MaxLength(100)]
        public string Analyst { get; set; } = string.Empty;

        [Required(AllowEmptyStrings = true), MaxLength(100)]
        public string Reviewer { get; set; } = string.Empty;

        [NotMapped]
        public CompanyProfile LatestProfile { get; set; }

        [NotMapped]
        public Company CorporateTreeResearch { get; set; }

        [NotMapped]
        public List<Company> CorporateTreeCoverages { get; set; }
    }
}
