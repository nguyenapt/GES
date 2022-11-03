using System;
using System.Collections.Generic;

namespace GES.Inside.Data.Models.Auth
{
    public class GesAccountRoleViewModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string OldUserName { get; set; }
        public string Email { get; set; }
        public string OrgName { get; set; }
        public long? OldUserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string RoleId { get; set; }
    }
}
