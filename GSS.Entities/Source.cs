using System;
using System.ComponentModel.DataAnnotations;

namespace Sustainalytics.GSS.Entities
{
    public class Source
    {
        public Guid Id { get; set; }

        [Required]
        public string Headline { get; set; }

        [Required, MaxLength(450)]
        public string Publisher { get; set; }

        public DateTime PublicationDate { get; set; }

        public Guid CompanyProfileId { get; set; }

        public CompanyProfile CompanyProfile { get; set; }
    }
}
