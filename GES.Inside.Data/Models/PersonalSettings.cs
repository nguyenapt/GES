using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GES.Inside.Data.Models
{
    public class PersonalSettings
    {
        public PersonalSettings()
        {
            CreatedDate = DateTime.UtcNow;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long PersonalSettingId { get; set; }

        public long IndividualId { get; set; }

        public long PersonalSettingCategoryId { get; set; }

        public PersonalSettingCategories PersonalSettingCategories { get; set; }

        [MaxLength(500)]
        public string SettingValue { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }
    }
}