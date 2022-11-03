using System.Collections.Generic;

namespace GES.Inside.Data.Models.StoredProcedureParams
{
    public class AdvancedSearchParams : StoredProcedureParams
    {
        public object CompanyIssueName { get; set; }
        public object Isin { get; set; }
        public object IndividualId { get; set; }
        public object OrgId { get; set; }
        public object NormId { get; set; }
        public object EngagementTypeId { get; set; }
        public object RecommendationIds { get; set; }
        public object ConclusionIds { get; set; }
        public object ProgressIds { get; set; }
        public object ResponseIds { get; set; }
        public object ServiceIds { get; set; }
        public object LocationIds { get; set; }
        public object HomeCountryIds { get; set; }
        public object PortfoliosOrganizationIds { get; set; }
        public object IndustryIds { get; set; }
        public object IsHideClosedCases { get; set; }
        public object SustainalyticsId { get; set; }
    }
}
