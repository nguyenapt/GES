using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace GES.Inside.Data.Models
{
    public class GesDocumentViewModel
    {
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Display(Name = "Select file")]
        public string FileName { get; set; }
        public string HashCodeDocument { get; set; }
        public long? ServiceId { get; set; }
        public string ServiceName { get; set; }
        public string FileExtension { get; set; }
        public string DownloadUrl { get; set; }
        public string Comment { get; set; }
        public string Source { get; set; }
        public int SortOrder { get; set; }
        public DateTime? Created { get; set; }
        public string CreatedString { get; set; }

        [Display(Name = "Select organization(s)")]
        public string[] SelectedOrganizations { get; set; }
        public IEnumerable<SelectListItem> Organizations { get; set; }

        [Display(Name = "Report section")]
        public string Metadata01 { get; set; }
        public string Metadata02 { get; set; }
        public string Metadata03 { get; set; }

    }
}
