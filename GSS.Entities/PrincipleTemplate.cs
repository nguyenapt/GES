using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sustainalytics.GSS.Entities
{
    public class PrincipleTemplate
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]// otherwise EF will consider this int key as an identity field
        public PrincipleType Id { get; set; }

        [Required, MaxLength(3)]
        public string Code { get; set; }

        [Required, MaxLength(50)]
        public string Name { get; set; }

        [Required, MaxLength(300)]
        public string Description { get; set; }

        public NormType NormCode { get; set; }
    }
}
