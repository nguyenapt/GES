using System;

namespace GES.Inside.Data.Models
{
    public class ReferenceViewModel
    {
        public long I_GesCaseReportReferences_Id { get; set; }
        public long G_GesCaseReport_Id { get; set; }
        public string Source { get; set; }
        public short? PublicationYear { get; set; }
        public string DocumentName { get; set; }
        public string AvailableFrom { get; set; }
        public DateTime AccessedDate { get; set; }
        public string GesCaseReportAvailabilityStatusesText { get; set; }
    }
}
