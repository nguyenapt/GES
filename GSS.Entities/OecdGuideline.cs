using System.ComponentModel.DataAnnotations;

namespace Sustainalytics.GSS.Entities
{
    public class OecdGuideline
    {
        public int Id { get; set; }

        public int Chapter { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; }
    }
}
