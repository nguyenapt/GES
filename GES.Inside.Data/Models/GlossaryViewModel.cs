using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace GES.Inside.Data.Models
{
    public class GlossaryViewModel
    {
        public Guid Id { get; set; }
        
        public Guid CategoryId { get; set; }

        [Required]
        [RegularExpression("([a-z0-9\\-]+)", ErrorMessage = "Only alphanumeric and dash (-) characters are accepted")]
        public string Slug { get; set; }

        [Required]
        public string Title { get; set; }

        [AllowHtml]
        public string Description { get; set; }

        public int Order { get; set; }

        public int ChildNums { get; set; }

        public List<GlossaryViewModel> Childs { get; set; }
    }
}