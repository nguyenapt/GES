using System;

namespace GES.Inside.Web.Models
{
    public class ScreeningReportQueryPram
    {
        public string ClientId { get; set; }
        public string PortfolioIdsString { get; set; }
        public string FromDate { get; set; }
        public string ToDate{ get; set; }
        public string NormThemeIdsString { get; set; }
    }
}