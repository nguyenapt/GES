using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GES.Inside.Data.Models.Auth
{
    public class GesForm
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        /// <summary>
        /// = controller
        /// </summary>
        public string Name { get; set; }
        public bool IsInClientSite { get; set; }
        public DateTime? Created { get; set; }
        public string FormKey { get; set; }
        public int SortOrder { get; set; }
    }
}
