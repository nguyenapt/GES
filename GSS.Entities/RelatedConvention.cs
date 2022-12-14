using System.ComponentModel.DataAnnotations;

namespace Sustainalytics.GSS.Entities
{
    public class RelatedConvention
    {
        public int Id { get; set; }

        [Required, MaxLength(200)]
        public string Name { get; set; }
    }
}
