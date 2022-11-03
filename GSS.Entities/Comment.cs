using System;
using System.ComponentModel.DataAnnotations;

namespace Sustainalytics.GSS.Entities
{
    public class Comment
    {
        public Guid Id { get; set; }

        [Required, MaxLength(1200)]
        public string Text { get; set; }

        public int SortKey { get; set; }

        public Guid IssueIndicatorId { get; set; }

        public IssueIndicator IssueIndicator { get; set; }
    }
}
