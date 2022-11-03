using System;
using System.Collections.Generic;

namespace GES.Inside.Data.Models
{
    public class DashboardInfoBoxModel
    {
        public List<long> PortfoliosIndicesId { get; set; }
        public int NumberOfCompanies { get; set; }
        public int NumberOfCases { get; set; }

        public List<DashboardInfoDetail> DashboardInfoDetails { get; set; }
    }

    public class DashboardInfoDetail
    {
        public long PortfolioId { get; set; }
        public string PortfolioName { get; set; }
        public DateTime? LastUpdated { get; set; }
        public int NumHoldings { get; set; }
    }

}
