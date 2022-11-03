using System;
using GES.Inside.Data.DataContexts;
using java.sql;

namespace GES.Inside.Data.Models
{
    public class ReportQueryResult
    {
        public I_GesCaseReports GesCaseReport { get; set; }
        public I_Companies Company { get; set; }
        public SubPeerGroups Msci { get; set; }
        public Countries Country { get; set; }
        public Countries Location { get; set; }
        public I_NormAreas NormArea { get; set; }
        public I_GesCommentary GesCommentary { get; set; }
        public I_GesCaseReportStatuses Recommendation { get; set; }
        public I_GesCaseReportStatuses Conclusion { get; set; }
        public DateTime? RecommendationDate { get; set; }
        public DateTime? ConclusionDate { get; set; }
        public string EngagementTypeCategoryName { get; set; }
        public string EngagementTypeName { get; set; }
        public long EngagementTypeId { get; set; }
    }
}