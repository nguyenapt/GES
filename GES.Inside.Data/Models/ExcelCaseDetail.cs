using System;

namespace GES.Inside.Data.Models
{
    public class ExcelCaseDetail
    {
        public long CompanyId { get; set; }
        public int  SustainalyticsID { get; set; }
        public string Isin { get; set; }
        public string CompanyName { get; set; }
        public string Analyst { get; set; }

        public long IncidentId { get; set; }

        public string Incident { get; set; }

        public string HQLocation { get; set; }

        public string IncidentLocation { get; set; }

        public string NormArea { get; set; }

        public string Conclusion { get; set; }

        public string Recommendation { get; set; }
        
    }
}