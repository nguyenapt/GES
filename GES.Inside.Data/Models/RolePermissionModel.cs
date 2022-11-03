using System;

namespace GES.Inside.Data.Models
{
    public class RolePermissionModel
    {
        public int FormId { get; set; }
        public string FormName { get; set; }
        public string RoleId { get; set; }

        public int AllowedAction { get; set; }
        public bool AllowedRead { get; set; }
    }
}