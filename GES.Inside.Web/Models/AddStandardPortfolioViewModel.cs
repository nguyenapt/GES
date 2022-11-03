using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.ModelBinding;
using System.Web.Mvc;

namespace GES.Inside.Web.Models
{
    public class AddStandardPortfolioViewModel
    {
        [Required]
        [Display(Name = "OrganizationId")]
        public long OrganizationId { get; set; }

        public string OrganizationName { get; set; }

        [Display(Name = "Alerts?")]
        public bool Alerts { get; set; }

        [Display(Name = "ShowInCsc?")]
        public bool ShowInCsc { get; set; }

        [Required]
        [Display(Name = "Select a Standard Portfolio")]
        public long? PortfolioId { get; set; }
        public IEnumerable<SelectListItem> StandardPortfolios { get; set; }

        [Display(Name = "Selected Portfolio Name")]
        public string PortfolioName { get; set; }

        [Display(Name = "Add services in Agreements automatically?")]
        public bool IsAddAgreementServices { get; set; }

    }
}