using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.ModelBinding;
using System.Web.Mvc;

namespace GES.Inside.Web.Models
{
    public class BasicPortfolioViewModel
    {
        [Required]
        [Display(Name = "OrganizationId")]
        public long OrganizationId { get; set; }

        public string OrganizationName { get; set; }

        [Display(Name = "Id")]
        public long PortfolioId { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string PortfolioName { get; set; }

        [Required]
        [Display(Name = "Type")]
        public long PortfolioTypeId { get; set; }
        public IEnumerable<SelectListItem> PortfolioTypes { get; set; }
        
        [Display(Name = "Alerts?")]
        public bool Alerts { get; set; }

        [Display(Name = "ShowInCSC?")]
        public bool ShowInCsc { get; set; }

        [Display(Name = "StandardPortfolio?")]
        public bool StandardPortfolio { get; set; }

        [Display(Name = "GES Standard Universe?")]
        public bool GesStandardUniverse { get; set; }

        public bool EditingPortfolioOnly { get; set; }

        [Display(Name = "Add services in Agreements automatically?")]
        public bool IsAddAgreementServices { get; set; }

        [Display(Name = "Go to details page after creating?")]
        public bool GoToDetailsPage { get; set; }

    }
}