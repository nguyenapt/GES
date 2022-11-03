namespace GES.Inside.Data.Models.StoredProcedureParams
{
    public class CheckMasterCompanyParams : StoredProcedureParams
    {
        public object CompanyIssueName { get; set; }
        public object OrgId { get; set; }
        public object PortfolioIds { get; set; }
        public object Isin { get; set; }
    }
}
