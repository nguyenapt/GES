using System;

namespace GES.Inside.Data.Models.Portfolio
{
    public class CompanyPortfolioViewModel
    {
        public long CompanyId { get; set; }
        public string CompanyName { get; set; }
        public bool IsMasterCompany { get; set; }
        public string Isin { get; set; }
        public string Sedol { get; set; }
        public bool IsInThisPortfolio { get; set; }
        public string Country { get; set; }
        public long? MasterCompanyId { get; set; }
        public long? SustainalyticsID { get; set; }
        public bool ShowInClient { get; set; }

    }
}
