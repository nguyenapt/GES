using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.ModelBinding;
using System.Web.Mvc;
using GES.Inside.Data.Models;

namespace GES.Inside.Web.Models
{
    public class CreateControActivPresetViewModel
    {
        [Required]
        [Display(Name = "Preset Name")]
        public string PresetName { get; set; }

        public string ControvSettings {get; set; } 

        public long PresetId { get; set; }
    }
}