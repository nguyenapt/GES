using System;

namespace GES.Inside.Data.Models
{
    public class CaseReportSummaryViewModel
    {
        public long Id { get; set; }

        public string CompanyIssueName { get; set; }

        public string Sumary { get; set; }
        public DateTime? EntryDate { get; set; }

    }
}