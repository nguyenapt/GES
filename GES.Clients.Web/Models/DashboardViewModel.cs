using GES.Inside.Data.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace GES.Clients.Web.Models
{
    public class DashboardViewModel
    {
        [Display(Name = "Portfolio")]
        public List<long> Portfolio { get; set; }
        [Display(Name = "Index")]
        public List<long> Index { get; set; }

        public IEnumerable<SelectListItem> Portfolios { get; set; }
        public IEnumerable<SelectListItem> Indices { get; set; }

        public CompanySearchBoxViewModel CompanySearchBox { get; set; }

        public IList<GesAnnouncementModel> AnnouncementModels { get; set; }

        public IList<GesBlogModel> BlogModels { get; set; }
    }
}