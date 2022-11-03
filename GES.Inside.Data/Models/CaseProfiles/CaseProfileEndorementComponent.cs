using System.ComponentModel.DataAnnotations;

namespace GES.Inside.Data.Models.CaseProfiles
{
    public class CaseProfileEndorementComponent : CaseProfileComponent
    {
        [Display(Name = "The sign up function but also show who has endorsed*CS")]
        public string EndorsedBy { get; set; }
    }
}
