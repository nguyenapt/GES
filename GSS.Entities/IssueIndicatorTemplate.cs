using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sustainalytics.GSS.Entities
{
    public class IssueIndicatorTemplate
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]// otherwise EF will consider this int key as an identity field
        public int Id { get; set; }

        [Required, MaxLength(250)]
        public string Name { get; set; }

        public PrincipleType PrincipleId { get; set; }

        public PrincipleTemplate Principle { get; set; }
    }
}
