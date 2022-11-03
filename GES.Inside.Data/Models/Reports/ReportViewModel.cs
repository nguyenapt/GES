using System;

namespace GES.Inside.Data.Models.Reports
{
    public class ReportViewModel
    {
        public System.Guid DocumentId { get; set; }
        public string Name { get; set; }
        public string FileName { get; set; }
        public string HashCodeDocument { get; set; }
        public long? GesDocumentServiceId { get; set; }
        public string Metadata01 { get; set; }
        public Nullable<System.DateTime> Created { get; set; }
        public Nullable<System.DateTime> Modified { get; set; }
    }
}
