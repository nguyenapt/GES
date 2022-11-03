using System.ComponentModel.DataAnnotations;

namespace GES.Clients.Web.Models
{
    public class ConfirmedViolationModel
    {
        [Display(Name = "Confirmed")]
        public bool ConfirmedViolation { get; set; }

        [Display(Name = "Confirmed Date")]
        public string ConfirmedViolationDate { get; set; }
    }
}