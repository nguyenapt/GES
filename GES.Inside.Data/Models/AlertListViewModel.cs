using System;

namespace GES.Inside.Data.Models
{
    public class AlertListViewModel
    {
        public long Id { get; set; }
        public long? CompanyId { get; set; }
        public string Heading { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? LastModified { get; set; }
        public string Summary { get; set; }
        public string Location { get; set; }
        public string Norm { get; set; }
        public long? NormId { get; set; }
        public string Source { get; set; }
        public DateTime? SourceDate { get; set; }
        public bool IsExtended { get; set; }
        public long? NaTypeId { get; set; }
        public string Notices { get; set; }
        public string AlertType { get; set; }
        public CompanyViewModel CompanyViewModel { get; set; }
    }
}