using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using GES.Inside.Data.Models;

namespace GES.Inside.Web.Models
{
    public class GlossaryViewModel
    {
        public Guid Id { get; set; }
        
        public Guid CategoryId { get; set; }

        [Required]
        [RegularExpression("([A-Za-z0-9\\-]+)", ErrorMessage = "Slug can't contain space and special character")]
        public string Slug { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        [AllowHtml]
        public string Description { get; set; }

        public int Order { get; set; }

        public int ChildNums { get; set; }
    }
}