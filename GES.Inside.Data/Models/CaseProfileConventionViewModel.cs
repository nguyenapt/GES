using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GES.Inside.Data.Models
{
    public class CaseProfileConventionViewModel
    {
        public long CaseProfileConventionId { get; set; }
        [Display(Name = "Convention short name")]
        public string ConventionShortName { get; set; }
    }
}