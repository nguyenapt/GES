using System.Collections.Generic;
using GES.Inside.Data.DataContexts;

namespace GES.Inside.Data.Models
{
    public class CompanyListViewModel
    {
        public long Id { get; set; }

        public string CompanyIssueName { get; set; }
        
        public string HomeCountry { get; set; }

        public string CompanyName { get; set; }
        public string CompanyLink { get; set; }
        public long CompanyId { get; set; }
        public int NumCases { get; set; }
        public int NumCasesActive { get; set; }
        public int NumAlerts { get; set; }

        public string Isin { get; set; }
        public string Sedol { get; set; }
        public string MsciIndustry { get; set; }
        public int IsInFocusList { get; set; }
        
        public decimal? MarketCap { get; set; }
        public int SortOrder { get; set; }
        
        public string Conclusion { get; set; }
        public string Recommendation { get; set; }
        public string ChangeSince { get; set; }
        
        public int SustainalyticsID { get; set; }
    }
}