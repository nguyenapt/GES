using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.ModelBinding;
using System.Web.Mvc;

namespace GES.Inside.Web.Models
{
    public class ApplyControActivPresetViewModel
    {
        [Required]
        public long PortfolioOrganizationId { get; set; }

        [Required]
        [Display(Name = "Select a Preset")]
        public long? PresetId { get; set; }
        public IEnumerable<SelectListItem> Presets { get; set; }

        [Display(Name = "Selected Preset Name")]
        public string PresetName { get; set; }

        [Display(Name = "Overwrite existing values?")]
        public bool OverwriteExistingValues { get; set; }

        [Display(Name = "Preview changes (of values)")]
        public string PreviewValues { get; set; }
    }
}