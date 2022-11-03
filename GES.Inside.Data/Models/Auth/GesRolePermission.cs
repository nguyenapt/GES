using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GES.Inside.Data.Models.Auth
{
    public class GesRolePermission
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public int AllowedAction { get; set; }
        public int GesFormId { get; set; }
        public GesForm GesForm { get; set; }
        public string GesRoleId { get; set; }
        public GesRole GesRole { get; set; }
        public DateTime? Created { get; set; }
    }
}
