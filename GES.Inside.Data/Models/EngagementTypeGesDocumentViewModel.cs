using System;

namespace GES.Inside.Data.Models
{
    public class EngagementTypeGesDocumentViewModel
    {
        public Guid Id { get; set; }
        public Guid? DocumentId { get; set; }
        public string Name { get; set; }
        public string FileName { get; set; }
        public DateTime? Created { get; set; }
        public string HashCodeDocument { get; set; }
        public string ReportSection { get; set; }
        public Guid? DocumentTypeId { get; set; }
    }
}