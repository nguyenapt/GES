using System;
using System.ComponentModel.DataAnnotations;

namespace Sustainalytics.GSS.Entities
{
    public class Remark
    {
        public Guid Id { get; set; }

        [Required, MaxLength(100)]
        public string Author { get; set; }

        [Required]
        public string Content { get; set; }

        public DateTime Timestamp { get; set; }
    }
}
