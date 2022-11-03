using System;

namespace GES.Inside.Data.Models.Portfolio
{
    public class GesStandardUniverseViewModel
    {
        public long CompanyId { get; set; }
        public string CompanyName { get; set; }
        public int SustainalyticsID { get; set; }
        public string Isin { get; set; }
        public string Sedol { get; set; }
        public bool IsInThisPortfolio { get; set; }
        public string Country { get; set; }
        public long? MasterCompanyId { get; set; }

        public string Sector { get; set; }
        public string IndustryGroup { get; set; }
        public string Industry { get; set; }
        public string SectorCode { get; set; }
        public long? MsciId { get; set; }

        public string SortOrder { get; set; }
        public string ClientName { get; set; }
        public string PortfolioName { get; set; }
        public string Screened { get; set; }
    }
}
