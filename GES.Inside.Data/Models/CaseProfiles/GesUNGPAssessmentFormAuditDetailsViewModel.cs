using System;

namespace GES.Inside.Data.Models.CaseProfiles
{
   
    public class GesUngpAssessmentFormAuditDetailsViewModel
    {
        public Guid GesUngpAssessmentFormAuditDetailsId { get; set; }
        public Guid GesUngpAssessmentFormAuditId { get; set; }
        public string ColumnName { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public string AuditDataState { get; set; }
        public string AuditUser { get; set; }
        public DateTime? AuditDatetime { get; set; }
        public string ColumnNameDescription { get; set; }
    }
}
