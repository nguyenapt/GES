using System;
using System.ComponentModel.DataAnnotations;

namespace Sustainalytics.GSS.Entities
{
    public class ChangeRecord
    {
        public Guid Id { get; set; }

        public int CompanyId { get; set; }

        public int CompanyVersion { get; set; }

        public int? PrincipleTemplateId { get; set; }

        public int? IssueIndicatorTemplateId { get; set; }

        public int Type { get; set; }

        public DateTime Timestamp { get; set; }

        public string PreviousValue { get; set; } // the previous value upon changing

        [Required, MaxLength(100)]
        public string Author { get; set; }
    }
}
