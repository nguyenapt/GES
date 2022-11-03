using System;

namespace GES.Inside.Data.Models
{
    public class CompanyViewModel
    {
        public long Id { get; set; }

        public string MasterCompanyId { get; set; }
        public string Name { get; set; }
        public string Sedol { get; set; }
        public string Isin { get; set; }
        public string Industry { get; set; }
        public string Location { get; set; }
        public string CountryCode { get; set; }
        public string WebsiteUrl { get; set; }
        public DateTime? Modified { get; set; }
        public long? parent { get; set; }
        public string sortPath { get; set; }
        public int? NumberOfCases { get; set; }
        public bool IsMasterCompany { get; set; }
    }
}
