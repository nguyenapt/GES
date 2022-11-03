using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using GES.Inside.Data.DataContexts;

namespace GES.Inside.Data.Models.Auth
{
    public class GesAccountViewModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string OldUserName { get; set; }
        public string Email { get; set; }
        public string OrgName { get; set; }
        public long? OrgId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string Title { get; set; }
        public string Phone { get; set; }
        public string MobilePhone { get; set; }
        public string WorkPhone { get; set; }
        public string Fax { get; set; }
        public string Comments { get; set; }

        public long? OldUserId { get; set; }
        public bool IsLocked { get; set; }
        public string Status { get; set; }

        public IEnumerable<string> RoleList { get; set; }
        public IEnumerable<string> Claims { get; set; }
        public string ClaimsString { get; set; }
        public DateTime? LastLogIn { get; set; }

    }
}
