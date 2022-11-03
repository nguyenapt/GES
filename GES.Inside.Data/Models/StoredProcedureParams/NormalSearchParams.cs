namespace GES.Inside.Data.Models.StoredProcedureParams
{
    public class NormalSearchParams : StoredProcedureParams
    {
        public object CompanyIssueName { get; set; }
        public object OrgId { get; set; }
        public object IndividualId { get; set; }
        public object IsHideClosedCases { get; set; }
        public object PortfolioIds { get; set; }
        public object Isin { get; set; }
        public object HomeCountryIds { get; set; }
        public object IndustryIds { get; set; }
    }
}
