using System;
using System.Collections.Generic;

namespace GES.Inside.Data.Models
{
    public class RelatedReportViewModel
    {
        public long CaseReportProcessStatusId { get; set; }
        public long? ProcessStatusId { get; set; }
        public bool Done { get; set; }
        public DateTime? DateChanged { get; set; }
        public string ProcessStatusName { get; set; }
        public string ProcessStatusDescription { get; set; }
        public long? ReportingTemplateId { get; set; }
        public string ManagedDocumentName { get; set; }
        public string FileName { get; set; }
        public long? GesCaseReportGesCompaniesId { get; set; }
        public long? NewGesCaseReportStatusId { get; set; }
        public long? GesCaseReportNaArticleId { get; set; }
        public long? RiskCompaniesId { get; set; }
        public int? SortOrder { get; set; }
    }
}