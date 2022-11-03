using System;

namespace GES.Inside.Data.Models
{
    public class GssResearchPrincipleCompanyContactViewModel
    {
        public Guid Id { get; set; }
        public Guid GssId { get; set; }
        public DateTime CompanyLastContactModified { get; set; }
        public DateTime MostRecentCompanyResponse { get; set; }
        public string CompanyContactComment { get; set; }

    }
}
