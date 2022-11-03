using System.ComponentModel.DataAnnotations;

namespace Sustainalytics.GSS.Entities
{
    public class UnGuidingPrinciple
    {
        public int Id { get; set; }

        public int Code { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; }
    }
}
