using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using GES.Common.Enumeration;

namespace GES.Clients.Web.Models
{
    public class CompanySearchBoxViewModel
    {
        [Display(Name = "Search")]
        public string Name { get; set; }

        [Display(Name = "ISIN")]
        public string ISIN { get; set; }

        [Display(Name = "Engagement status")]
        public List<long> Recommendation { get; set; }

        [Display(Name = "Conclusion")]
        public long? Conclusion { get; set; }

        [Display(Name = "Norm/Theme")]
        public long? EngagementArea { get; set; }

        [Display(Name = "Location (Incident country)")]
        public long? Location { get; set; }
        
        [Display(Name = "SEDOL")]
        public string SEDOL { get; set; }

        [Display(Name = "Peer group")]
        public long? Industry { get; set; }

        [Display(Name = "Norm")]
        public long? Norm { get; set; }        

        [Display(Name = "Progress")]
        public long? Progress { get; set; }

        [Display(Name = "Response")]
        public long? Response { get; set; }        

        [Display(Name = "Domicile")]
        public long? HomeCountry { get; set; }

        [Display(Name = "Engagement Type")]
        public long? EngagementType { get; set; }

        [Display(Name = "Portfolio/Index")]
        public List<long> PortfolioIndex { get; set; }

        [Display(Name = "Show resolved or archived cases")]
        public bool ShowClosedCases { get; set; } // Closed = Archived + Resolved

        [Display(Name = "Show only companies with active cases")]
        public bool OnlyCompaniesWithActiveCases { get; set; }
        public bool onlySearchCompany { get; set; }
        public bool onlyShowFocusList { get; set; }
        public string CompanyId { get; set; }
        
        [Display(Name = "Sustainalytics ID")]
        public string SustainalyticsId { get; set; }

        public IEnumerable<SelectListItem> Industries { get; set; }
        public IEnumerable<SelectListItem> Recommendations { get; set; }
        public IEnumerable<SelectListItem> Norms { get; set; }
        public IEnumerable<SelectListItem> Conclusions { get; set; }
        public IEnumerable<SelectListItem> Progresses { get; set; }
        public IEnumerable<SelectListItem> Responses { get; set; }
        public IEnumerable<SelectListItem> Locations { get; set; }
        public IEnumerable<SelectListItem> HomeCountries { get; set; }
        public IEnumerable<SelectListItem> EngagementTypes { get; set; }
        public IEnumerable<SelectListItem> PortfolioIndexes { get; set; }
        public IEnumerable<SelectListItem> EngagementAreas { get; set; }

        public ClientType ClientType { get; set; }
    }
}